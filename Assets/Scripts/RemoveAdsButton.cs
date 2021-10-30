using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsButton : MonoBehaviour
{
    void Start()
    {
        if (!IAPManager.Instance.IsInit || !IAPManager.Instance.ShowAds)
        {
            gameObject.SetActive(false);
        }
    }

    public void BuyRemoveAds()
    {
        IAPManager.Instance.OnPurchaseResolved += DisableButton;
        IAPManager.Instance.BuyProductID("remove_ads");
    }

    private void DisableButton()
    {
        IAPManager.Instance.ShowAds = false;
        IAPManager.Instance.OnPurchaseResolved -= DisableButton;
        gameObject.SetActive(false);
    }
}
