using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    public enum SceneToLoad {Menu, Selector, Game }
    public enum DirectionToHide {Up, Down, Right, Left}

    [SerializeField]
    private bool isDefault = true;
    [SerializeField]
    private GameObject panelToMove;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private SceneToLoad sceneToLoad;
    [SerializeField]
    private DirectionToHide directionToHide;


    private void OnEnable()
    {
        if (!isDefault)
            return;

        panelToMove.transform.DOMove(GetPositionToMove() * -1, 0.5f).From().SetEase(Ease.InOutBack);
        playButton.interactable = true;
    }

    public async Task AnimatePanel()
    {
        await panelToMove.transform.DOMove(GetPositionToMove(), 0.5f).SetEase(Ease.InOutBack).AsyncWaitForCompletion();
    }

    public async Task AnimatePaneIn()
    {
        await panelToMove.transform.DOMove(Vector2.zero, 0.5f).SetEase(Ease.InOutBack).AsyncWaitForCompletion();
    }

    public void Load()
    {
        playButton.interactable = false;

        switch (sceneToLoad)
        {
            case SceneToLoad.Game:
                panelToMove.transform.DOMove(GetPositionToMove(), 0.5f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.Instance.LoadGame());
                break;
            case SceneToLoad.Menu:
                panelToMove.transform.DOMove(GetPositionToMove(), 0.5f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.Instance.LoadMenu());
                break;
            case SceneToLoad.Selector:
                panelToMove.transform.DOMove(GetPositionToMove(), 0.5f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.Instance .LoadLobby());
                break;
        }
    }

    private Vector2 GetPositionToMove()
    {
        switch (directionToHide)
        {
            case DirectionToHide.Up:
                return new Vector2(0, Screen.height * 2);
            case DirectionToHide.Down:
                return new Vector2(0, - Screen.height * 2);
            case DirectionToHide.Left:
                return new Vector2(-Screen.width, 0);
            case DirectionToHide.Right:
                return new Vector2(Screen.width, 0);
            default:
                return Vector2.one;
        }
    }
}
