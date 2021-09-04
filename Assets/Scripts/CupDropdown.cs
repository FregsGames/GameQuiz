using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupDropdown : MonoBehaviour
{
    [SerializeField]
    private RectTransform innerPanel;

    private RectTransform rectTransform;

    private bool shown = false;

    private bool updatingSize;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Toggle()
    {
        updatingSize = true;
        if (!shown)
        {
            rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, 800), 0.2f);
            innerPanel.DOSizeDelta(new Vector2(innerPanel.sizeDelta.x, 600), 0.2f).onComplete = () => updatingSize = false;
        }
        else
        {
            rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, 200), 0.2f);
            innerPanel.DOSizeDelta(new Vector2(innerPanel.sizeDelta.x, 0), 0.2f).onComplete = () => updatingSize = false;
        }
        shown = !shown;
    }

    private void Update()
    {
        if (updatingSize)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
    }
}
