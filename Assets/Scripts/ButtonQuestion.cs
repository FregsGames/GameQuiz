using Questions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using SuperMaxim.Messaging;
using UnityEngine.UI;

public class ButtonQuestion : MonoBehaviour
{
    public string AssignedAnswer { get; private set; }
    [SerializeField]
    Button button;

    [SerializeField]
    TextMeshProUGUI buttonText;


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
