using Assets.Scripts.Payloads;
using DG.Tweening;
using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float Timer { get; set; }
    public bool UsesTime { get; set; }

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Image timerImage;

    public void Setup(Sequence sequence, float time = 0)
    {
        timerImage.enabled = time != 0;
        Timer = time;

        GetComponent<Image>().DOFade(0, 0f);

        if (Timer > 0)
        {
            UsesTime = true;
            sequence.Append(GetComponent<Image>().DOFade(1, 0.5f));
        }
    }

    public void TimeQuestion()
    {
        StartCoroutine("StartTimer");
    }

    public void StopTimer()
    {
        StopCoroutine("StartTimer");
    }

    private IEnumerator StartTimer()
    {
        var elapsedTime = 0.0f;

        while(elapsedTime < Timer)
        {
            timerImage.fillAmount = 1 - (elapsedTime / Timer);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Messenger.Default.Publish(new TimeOut());
    }

    public void FadeOut()
    {
        GetComponent<Image>().DOFade(0, 0.25f);
    }
}
