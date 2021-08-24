using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Cups;

public class CupSection : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cupTitle;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;

    public void Setup(CupScriptable cup)
    {
        this.cupTitle.text = cup.title;
        image.sprite = cup.cupImage;
    }

    public void EnableButton(bool state)
    {
        button.interactable = state;
    }

}
