using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSettings : Singleton<ColorSettings>
{
    [Header("Normal state question")]
    [SerializeField]
    private Color n_normal;
    [SerializeField]
    private Color n_highlighted;
    [SerializeField]
    private Color n_pressed;
    [SerializeField]
    private Color n_selected;
    [SerializeField]
    private Color n_disabled;

    [Header("Plain state question")]
    [SerializeField]
    private Color p_normal;
    [SerializeField]
    private Color p_highlighted;
    [SerializeField]
    private Color p_pressed;
    [SerializeField]
    private Color p_selected;
    [SerializeField]
    private Color p_disabled;

    [Header("Correct state question")]
    [SerializeField]
    private Color c_normal;
    [SerializeField]
    private Color c_highlighted;
    [SerializeField]
    private Color c_pressed;
    [SerializeField]
    private Color c_selected;
    [SerializeField]
    private Color c_disabled;

    [Header("Wrong state question")]
    [SerializeField]
    private Color w_normal;
    [SerializeField]
    private Color w_highlighted;
    [SerializeField]
    private Color w_pressed;
    [SerializeField]
    private Color w_selected;
    [SerializeField]
    private Color w_disabled;

    [SerializeField]
    private Color w_textColor;
    [SerializeField]
    private Color p_textColor;
    [SerializeField]
    private Color c_textColor;
    [SerializeField]
    private Color n_textColor;

    public Color W_textColor { get => w_textColor; set => w_textColor = value; }
    public Color C_textColor { get => c_textColor; set => c_textColor = value; }
    public Color P_textColor { get => p_textColor; set => p_textColor = value; }
    public Color N_textColor { get => n_textColor; set => n_textColor = value; }

    public ColorBlock Normal { get; set; }
    public ColorBlock Plain { get; set; }
    public ColorBlock Correct { get; set; }
    public ColorBlock Wrong { get; set; }

    protected override void Awake()
    {
        base.Awake();

        Normal = new ColorBlock() {
            normalColor = n_normal,
            highlightedColor = n_highlighted,
            pressedColor = n_pressed,
            selectedColor = n_selected,
            disabledColor = n_disabled,
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        Plain = new ColorBlock() {
            normalColor = p_normal,
            highlightedColor = p_highlighted,
            pressedColor = p_pressed,
            selectedColor = p_selected,
            disabledColor = p_disabled,
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        Correct = new ColorBlock() {
            normalColor = c_normal,
            highlightedColor = c_highlighted,
            pressedColor = c_pressed,
            selectedColor = c_selected,
            disabledColor = c_disabled,
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };

        Wrong = new ColorBlock() {
            normalColor = w_normal,
            highlightedColor = w_highlighted,
            pressedColor = w_pressed,
            selectedColor = w_selected,
            disabledColor = w_disabled,
            colorMultiplier = 1,
            fadeDuration = 0.1f
        };
    }

}
