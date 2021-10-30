using DG.Tweening;
using UnityEngine;

public class UpdatePanel : MonoBehaviour
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private MenuManager menuManager;
    [SerializeField]
    private int id;

    private void Start()
    {
        if (PlayerPrefs.GetInt($"update_{id}") == 0)
        {
            PlayerPrefs.SetInt($"update_{id}", 1);
            ShowPanel();
        }
    }

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
}
