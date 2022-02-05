using Assets.Scripts.Payloads;
using Questions;
using SuperMaxim.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CupsSelection : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private CupDropdown cupDropdownPrefab;
    private List<CupScriptable> cups;

    private CupScriptable currentCup;
    private CupDropdown currentCupDropdown;

    [SerializeField]
    private MenuManager menuManagerToGame;
    [SerializeField]
    private MenuManager menuManagerToMenu;

    private bool loadingGame = false;

    private List<string> alreadyInstantiatedPremiumCups = new List<string>();

    public List<CupDropdown> CupDropdowns { get; set; } = new List<CupDropdown>();

    private Vector2 firstElementPos;

    [SerializeField]
    private ScrollRect scrollRect;

    private void Start()
    {
        InstatiateCups();

        Messenger.Default.Subscribe<CupSelectedPayload>(OnCupSelected);
        Messenger.Default.Subscribe<CupDropdown>(OnCupToggle);
    }

    private void InstatiateCups()
    {
        cups = Cups.Instance.GetAllCups();

        foreach (var cup in cups)
        {
            if(cup.state == Cups.CupType.premium && !IAPManager.Instance.HasBought(cup.id))
            {
                if (!alreadyInstantiatedPremiumCups.Contains(cup.id))
                {
                    alreadyInstantiatedPremiumCups.Add(cup.id);
                }
                else
                {
                    continue;
                }
            }

            var dropdown = Instantiate(cupDropdownPrefab, content);
            CupDropdowns.Add(dropdown);

            dropdown.Setup(cup);
        }

        firstElementPos = CupDropdowns[0].gameObject.GetComponent<RectTransform>().position;
    }

    private void OnEnable()
    {
        IAPManager.Instance.OnPurchaseResolved += Refresh;
    }

    private void Refresh(bool purchaseSuccessful)
    {
        if (!purchaseSuccessful)
            return;

        alreadyInstantiatedPremiumCups = new List<string>();

        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }

        InstatiateCups();
    }

    private void OnDisable()
    {
        IAPManager.Instance.OnPurchaseResolved -= Refresh;
    }

    private void OnCupToggle(CupDropdown obj)
    {
        currentCupDropdown = obj;
        StartCoroutine(RecolocateScroll(obj));
        //dropdownRect anchorPos = 
    }

    IEnumerator RecolocateScroll(CupDropdown obj)
    {
        yield return new WaitForSeconds(0.2f);
        RectTransform contentRect = content.GetComponent<RectTransform>();
        RectTransform dropdownRect = obj.gameObject.GetComponent<RectTransform>();
        Vector3 originalPos = contentRect.position;
        float diffToPos = firstElementPos.y - dropdownRect.position.y;

        var elapsedTime = 0.0f;
        while(elapsedTime < 0.1f)
        {
            contentRect.position = Vector3.Lerp(originalPos, originalPos + Vector3.up * diffToPos, elapsedTime / 0.1f);
            scrollRect.velocity = Vector2.zero;
            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        contentRect.position = originalPos + Vector3.up * diffToPos;
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<CupSelectedPayload>(OnCupSelected);
        Messenger.Default.Unsubscribe<CupDropdown>(OnCupToggle);
    }

    private async void OnCupSelected(CupSelectedPayload obj)
    {
        if (obj == null)
        {
            content.gameObject.SetActive(true);
            await menuManagerToGame.AnimatePanelIn();
            if(currentCupDropdown != null)
            {
                currentCupDropdown.Toggle();
            }
        }
        else if(obj.Cup != null && !obj.endless)
        {
            await menuManagerToGame.AnimatePanel();
            currentCup = obj.Cup;
            currentCupDropdown = obj.CupDropdown;
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

        currentCupDropdown = null;

        loadingGame = true;

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

    public void ReturnButton()
    {
        if(currentCupDropdown != null)
        {
            currentCupDropdown.Close();
        }

        if (content.gameObject.activeSelf)
        {
            StartCoroutine(menuManagerToMenu.Load());
        }
    }

}
