using DG.Tweening;
using UnityEngine;
using TMPro;
using System;
using SuperMaxim.Messaging;
using UnityEngine.UI;

public class ButtonQuestion : MonoBehaviour
{
    public enum State
    {
        Correct,
        Wrong,
        Plain,
        Normal
    }

    public string AssignedAnswer { get; private set; }
    [SerializeField]
    Button button;

    [SerializeField]
    TextMeshProUGUI buttonText;

    public void SetColor(State state)
    {
        switch (state)
        {
            case State.Correct:
                button.colors = ColorSettings.instance.Correct;
                buttonText.color = ColorSettings.instance.C_textColor;
                break;
            case State.Plain:
                button.colors = ColorSettings.instance.Plain;
                buttonText.color = ColorSettings.instance.P_textColor;
                break;
            case State.Wrong:
                button.colors = ColorSettings.instance.Wrong;
                buttonText.color = ColorSettings.instance.W_textColor;
                break;
            case State.Normal:
                button.colors = ColorSettings.instance.Normal;
                buttonText.color = ColorSettings.instance.N_textColor;
                break;
        }
    }

    public void SetQuestion(string answer)
    {
        buttonText.text = answer;
        AssignedAnswer = answer;
    }

    public void OnClick()
    {
       Messenger.Default.Publish(this);
    }

    public void SetInteractable(bool state)
    {
        button.interactable = state;
    }
}
