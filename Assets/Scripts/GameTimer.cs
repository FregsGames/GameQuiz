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

        rectTransform.DOAnchorPosY(Screen.height * 2, 0);

        if (Timer > 0)
        {
            UsesTime = true;
            sequence.Append(rectTransform.DOAnchorPosY(0, 1f).SetEase(Ease.OutBack));
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
}
