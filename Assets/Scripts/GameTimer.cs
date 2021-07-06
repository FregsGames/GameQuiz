using DG.Tweening;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public float Time { get; set; }

    [SerializeField]
    private RectTransform rectTransform;

    public void Setup(Sequence sequence, float time = 0)
    {
        gameObject.SetActive(time != 0);
        Time = time;

        rectTransform.DOAnchorPosY(Screen.height * 2, 0);

        if (Time > 0)
        {
            sequence.Append(rectTransform.DOAnchorPosY(0, 1f).SetEase(Ease.OutBack));
        }
    }
}
