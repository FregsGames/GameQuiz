using System;
using UnityEngine;
using UnityEngine.UI;
using static Levels;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    public LevelScriptableC Level { get; set; }

    public Action<LevelButton> OnClick;

    public Button Button { get; set; }

    private Image image;

    private void OnEnable()
    {
        Button = GetComponent<Button>();
        image = GetComponent<Image>();
        Button.onClick.AddListener(() => { OnClick?.Invoke(this); });
    }

    public void SetSprite(Sprite sprite, int number = -1)
    {
        image.sprite = sprite;
        if(number != -1)
        {
            text.text = number.ToString();
        }
    }
}
