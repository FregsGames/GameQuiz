using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainPanel;
    [SerializeField]
    private Button playButton;


    private void OnEnable()
    {
        playButton.interactable = true;
    }

    public void LoadGame()
    {
        playButton.interactable = false;

        mainPanel.transform.DOMoveX(Screen.width * 2, 1f).SetEase(Ease.InOutBack).OnComplete(() => SceneLoader.instance.LoadGame());
    }
}
