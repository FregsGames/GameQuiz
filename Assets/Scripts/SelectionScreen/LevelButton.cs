using System;
using UnityEngine;
using UnityEngine.UI;
using static Levels;

public class LevelButton : MonoBehaviour
{
    public Level Level { get; set; }

    public Action<LevelButton> OnClick;

    public Button Button { get; set; }

    private void OnEnable()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(() => { OnClick?.Invoke(this); });
    }

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
