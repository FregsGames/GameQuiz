using Assets.Scripts.Payloads;
using Questions;
using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CupsSelection : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private CupDropdown cupDropdownPrefab;
    private List<CupScriptable> cups;

    private CupScriptable currentCup;

    [SerializeField]
    private MenuManager menuManagerToGame;

    private bool loadingGame = false;

    private void Start()
    {
        cups = Cups.Instance.GetAllCups();

        foreach (var cup in cups)
        {
            var dropdown = Instantiate(cupDropdownPrefab, content);
            dropdown.Setup(cup);
        }

        Messenger.Default.Subscribe<CupSelectedPayload>(OnCupSelected);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<CupSelectedPayload>(OnCupSelected);
    }

    private void OnCupSelected(CupSelectedPayload obj)
    {
        if (obj == null)
        {
            content.gameObject.SetActive(true);
        }
        else if(obj.Cup != null && !obj.endless)
        {
            currentCup = obj.Cup;
            content.gameObject.SetActive(false);
        }
        else if(obj.Cup != null && obj.endless)
        {
            currentCup = obj.Cup;
            PlayEndless();
        }
    }

    private async void PlayEndless()
    {
        if (loadingGame)
            return;

        loadingGame = true;
        QuestionGenerator.Instance.CurrentGamesContainer = currentCup.gamesContainer;

        await menuManagerToGame.AnimatePanel();
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return null;

        FindObjectOfType<GameLogic>().StartEndless(currentCup);

        SceneManager.UnloadSceneAsync("CupSelection");
    }

    public void Back()
    {

    }

}
