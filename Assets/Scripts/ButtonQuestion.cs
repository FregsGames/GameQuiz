using Questions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using SuperMaxim.Messaging;

public class ButtonQuestion : MonoBehaviour
{
    public string AssignedQuestion { get; private set; }

    [SerializeField]
    TextMeshProUGUI buttonText;


    public void SetQuestion(string answer)
    {
        buttonText.text = answer;
        AssignedQuestion = answer;
    }

    public void OnClick()
    {
        Messenger.Default.Publish(AssignedQuestion);
    }
}
