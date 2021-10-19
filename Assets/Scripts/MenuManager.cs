using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public enum SceneToLoad {Menu, CupSelection, Game }
    public enum DirectionToHide {Up, Down, Right, Left}

    [SerializeField]
    private GameObject extra;
    [SerializeField]
    private bool hideExtraOnShow;
    [SerializeField]
    private bool hideExtraOnHide;

    [SerializeField]
    private bool isDefault = true;
    [SerializeField]
    private GameObject panelToMove;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private SceneToLoad thisScene;
    [SerializeField]
    private SceneToLoad sceneToLoad;
    [SerializeField]
    private DirectionToHide directionToShow;

    [SerializeField]
    private bool differentHideDir;
    [SerializeField]
    private DirectionToHide directionToHide;

    [SerializeField]
    private Ease ease = Ease.InOutBack;

    public Ease MenuEase { get => ease;}


    private void OnEnable()
    {
        if (!isDefault)
            return;

        if(hideExtraOnShow && extra != null)
        {
            extra.SetActive(false);
            panelToMove.transform.DOMove(GetPositionToMove() * -1, 0.5f).From().SetEase(ease).OnComplete(() => extra.SetActive(true));
        }
        else
        {
            panelToMove.transform.DOMove(GetPositionToMove() * -1, 0.5f).From().SetEase(ease);
        }
        playButton.interactable = true;
    }

    public async Task AnimatePanel()
    {
        await panelToMove.transform.DOMove(GetPositionToMove(), 0.5f).SetEase(ease).AsyncWaitForCompletion();
    }

    public async Task AnimatePanelIn()
    {
        await panelToMove.transform.DOMove(Vector2.zero, 0.5f).SetEase(ease).AsyncWaitForCompletion();
    }

    public void LoadButton()
    {
        StartCoroutine(Load());
    }

    public IEnumerator Load()
    {
        playButton.interactable = false;

        if(hideExtraOnHide && extra != null)
        {
            extra.SetActive(false);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad.ToString(), LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        panelToMove.transform.DOMove(GetPositionToMove(true), 0.5f).SetEase(ease).OnComplete(() => SceneManager.UnloadSceneAsync(thisScene.ToString()));

    }

    private Vector2 GetPositionToMove(bool hide = false)
    {
        if (hide && differentHideDir)
        {
            switch (directionToHide)
            {
                case DirectionToHide.Up:
                    return new Vector2(0, Screen.height * 2);
                case DirectionToHide.Down:
                    return new Vector2(0, -Screen.height * 2);
                case DirectionToHide.Left:
                    return new Vector2(-Screen.width, 0);
                case DirectionToHide.Right:
                    return new Vector2(Screen.width, 0);
                default:
                    return Vector2.one;
            }
        }
        else
        {

            switch (directionToShow)
            {
                case DirectionToHide.Up:
                    return new Vector2(0, Screen.height * 2);
                case DirectionToHide.Down:
                    return new Vector2(0, -Screen.height * 2);
                case DirectionToHide.Left:
                    return new Vector2(-Screen.width, 0);
                case DirectionToHide.Right:
                    return new Vector2(Screen.width, 0);
                default:
                    return Vector2.one;
            }
        }

    }
}
