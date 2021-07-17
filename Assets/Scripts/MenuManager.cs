using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

        panelToMove.transform.DOMove(GetPositionToMove() * -1, 1f).From().SetEase(Ease.InOutBack);
        playButton.interactable = true;
    }

    public void Load()
    {
        playButton.interactable = false;

        switch (sceneToLoad)
        {
            case SceneToLoad.Game:
                panelToMove.transform.DOMove(GetPositionToMove(), 1f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.instance.LoadGame());
                break;
            case SceneToLoad.Menu:
                panelToMove.transform.DOMove(GetPositionToMove(), 1f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.instance.LoadMenu());
                break;
            case SceneToLoad.Selector:
                panelToMove.transform.DOMove(GetPositionToMove(), 1f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.instance.LoadLobby());
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
                return new Vector2(-Screen.width * 2, 0);
            case DirectionToHide.Right:
                return new Vector2(Screen.width * 2, 0);
            default:
                return Vector2.one;
        }
    }
}
