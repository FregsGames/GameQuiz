﻿using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SuperMaxim.Messaging;
using System;
using Assets.Scripts.Payloads;

public class CupDropdown : MonoBehaviour
{
    [SerializeField]
    private RectTransform innerPanel;

    private RectTransform rectTransform;

    private bool shown = false;

    private bool updatingSize;

    public CupScriptable Cup { get; set; }
    [SerializeField]
    private TextMeshProUGUI cupName;
    [SerializeField]
    private TextMeshProUGUI levelsText;
    [SerializeField]
    private Button infiniteButton;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Messenger.Default.Subscribe<CupDropdown>(OnCupToggle);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<CupDropdown>(OnCupToggle);
    }

    private void OnCupToggle(CupDropdown obj)
    {
        if(obj != this)
        {
            Close();
        }
    }

    public void Setup(CupScriptable cup)
    {
        Cup = cup;
        cupName.text = cup.name;
        levelsText.text = $"{cup.GetCompletedLevelsCount()}/{cup.levels.Count}";
        infiniteButton.interactable = cup.GetCompletedLevelsCount() == cup.levels.Count;
    }

    public void PlayLevel()
    {
        Close();
        Messenger.Default.Publish(new CupSelectedPayload() {
            Cup = Cup,
            endless = false,
            CupDropdown = this
        });
    }

    public void PlayEndless()
    {
        Close();
        Messenger.Default.Publish(new CupSelectedPayload()
        {
            Cup = Cup,
            endless = true,
            CupDropdown = this
        });
    }

    public void Close()
    {
        if (!shown)
            return;

        rectTransform.DOSizeDelta(new Vector2(rectTransform.sizeDelta.x, 200), 0.2f);
        innerPanel.DOSizeDelta(new Vector2(innerPanel.sizeDelta.x, 0), 0.2f).onComplete = () => updatingSize = false;
        shown = !shown;
    }

    public void Toggle()
    {
        updatingSize = true;
        if (!shown)
        {
            Messenger.Default.Publish(this);
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
