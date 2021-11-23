using System;
using UnityEngine;
using UnityEngine.UI;
using static Levels;

public class LevelButton : MonoBehaviour
{
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

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
