using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardVideoPanel : MonoBehaviour
{
    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private Transform content;

    public void ShowPanel()
    {
        content.gameObject.SetActive(true);
        content.DOMoveX(-Screen.width, 0);
        content.DOMoveX(0, 0.5f).SetEase(menuManager.MenuEase);
        background.SetActive(true);
        gameObject.SetActive(true);
    }
    public void HideOptions()
    {
        content.DOMoveX(-Screen.width, 0.5f).SetEase(menuManager.MenuEase).OnComplete(() => gameObject.SetActive(false));
    }

    public void NoButton()
    {
        AdsManager.Instance.NoAdsPanelDiscarded = true;
        HideOptions();
    }

    public void YesButton()
    {
        AdsManager.Instance.ShowRewarded();
        HideOptions();
    }
}
