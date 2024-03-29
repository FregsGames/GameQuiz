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
    private TextMeshProUGUI levelsTitleText;
    [SerializeField]
    private TextMeshProUGUI descText;
    [SerializeField]
    private TextMeshProUGUI levelsText;
    [SerializeField]
    private TextMeshProUGUI levelsTitle;
    [SerializeField]
    private Button infiniteButton;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button buyButton;
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Image cupImage;
    [SerializeField]
    private GameObject infiniteInfo;
    [SerializeField]
    private RectTransform contentDescRect;

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

        if(cup.levels == null || cup.levels.Count == 0)
        {
            levelsText.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            levelsTitle.gameObject.SetActive(false);

            RectTransform playButtonRect = playButton.GetComponent<RectTransform>();

            infiniteButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, playButtonRect.anchorMin.y);

            cupName.GetComponent<RectTransform>().anchorMax = new Vector2(1f, 1f);
        }

        levelsText.text = $"{cup.GetCompletedLevelsCount()}/{cup.levels.Count}";
        infiniteButton.interactable = cup.GetCompletedLevelsCount() == cup.levels.Count;

        if (infiniteButton.interactable)
        {
            infiniteInfo.SetActive(false);
            contentDescRect.anchorMin = new Vector2(0, 0.37f);
        }

        if(cup.state == Cups.CupType.free || IAPManager.Instance.HasBought(cup.id))
        {
            cupImage.sprite = cup.cupImage;
            cupName.text = Translations.instance.GetText(cup.title);
            descText.text = Translations.instance.GetText(cup.desc);
        }
        else
        {
            descText.text = Translations.instance.GetText(cup.packDesc);
            cupName.text = Translations.instance.GetText(cup.packTitle);

            playButton.gameObject.SetActive(false);
            infiniteButton.gameObject.SetActive(false);

            buyButton.gameObject.SetActive(true);
            priceText.gameObject.SetActive(true);
            levelsText.gameObject.SetActive(false);
            levelsTitleText.gameObject.SetActive(false);
        }
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

    public void Buy()
    {
        IAPManager.Instance.BuyProductID(Cup.id);
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
        if (updatingSize)
            return;

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
