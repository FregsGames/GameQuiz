using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CustomDebug : Singleton<CustomDebug>
{
    [SerializeField]
    private TextMeshProUGUI log;

    public void Log(string toLog)
    {
        log.text = log.text + "\n" + toLog;
    }
}
