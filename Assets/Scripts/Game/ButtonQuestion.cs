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
                button.colors = ColorSettings.Instance.Correct;
                buttonText.color = ColorSettings.Instance.C_textColor;
                break;
            case State.Plain:
                button.colors = ColorSettings.Instance.Plain;
                buttonText.color = ColorSettings.Instance.P_textColor;
                break;
            case State.Wrong:
                button.colors = ColorSettings.Instance.Wrong;
                buttonText.color = ColorSettings.Instance.W_textColor;
                break;
            case State.Normal:
                button.colors = ColorSettings.Instance.Normal;
                buttonText.color = ColorSettings.Instance.N_textColor;
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
