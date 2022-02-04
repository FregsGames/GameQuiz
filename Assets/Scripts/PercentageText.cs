using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PercentageText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private float timeToFill = 1.4f;

    private IEnumerator Start()
    {
        yield return 0;

        Cups.Instance.CheckCups();

        float percentageCompleted = Cups.Instance.percentageCompleted;

        float completed = 0;

        var elapsedTime = 0.0f;

        while(elapsedTime < timeToFill)
        {
            completed = Mathf.Lerp(0, percentageCompleted, elapsedTime / timeToFill) * 100;

            text.text = Translations.instance.GetText("progress") + ": " + completed.ToString("F0") + "%";

            elapsedTime += Time.deltaTime;
            yield return 0;
        }

        completed = percentageCompleted * 100;
        text.text = Translations.instance.GetText("progress") + ": " + completed.ToString("F0") + "%";
    }
}
