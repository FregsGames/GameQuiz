using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CupSection : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cupTitle;
    [SerializeField]
    private Image image;
    [SerializeField]
    private Button button;

    public void Setup(Cup cup)
    {
        this.cupTitle.text = cup.title;
        image.sprite = cup.cupImage;
    }

    public void EnableButton(bool state)
    {
        button.interactable = state;
    }

}
