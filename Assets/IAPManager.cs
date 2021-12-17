using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : Singleton<IAPManager>, IStoreListener
{

    private IStoreController controller;
    private IExtensionProvider extensions;

    public Action<bool> OnPurchaseResolved;

    private int initTries = 0;

    private ConfigurationBuilder configurationBuilder;

    public bool IsInit { get {return IsInitialized(); } }

    public bool showAds = true;
    public bool ShowAds { get => showAds; set => showAds = value; }

    public List<string> BoughtPacks { get; set; } = new List<string>();

    private void Start()
    {
        configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        configurationBuilder.AddProduct("remove_ads", ProductType.NonConsumable);

        configurationBuilder.AddProduct("cup_shuffle", ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, configurationBuilder);

        // NOTE: REMEMBER TO ADD THE NEW PACKS TO THE BOUGHT PACK LIST:

        /*if (HasBought("pack"))
        {
            BoughtPacks.Add("pack");
        }*/

        ShowAds = !HasBought("remove_ads");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        foreach (var product in controller.products.all)
        {
            if (product.hasReceipt)
            {
                PlayerPrefs.SetInt(product.definition.id, 1);
            }
        }
    }

    public bool HasBought(string id)
    {
        if (PlayerPrefs.GetInt(id, 0) == 1)
            return true;

        if (!IsInitialized())
            return false;

        if(controller.products.WithID(id) == null)
            return false;

        return controller.products.WithID(id).hasReceipt;
    }

    /// <summary>
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    ///
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    /// </summary>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        if(initTries < 10)
        {
            StartCoroutine(TryInit());
            initTries++;
        }
    }

    private IEnumerator TryInit()
    {
        yield return new WaitForSeconds(1f);
        UnityPurchasing.Initialize(this, configurationBuilder);
    }

    /// <summary>
    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        OnPurchaseResolved?.Invoke(true);
        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        OnPurchaseResolved?.Invoke(false);
    }
    public void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = controller.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                controller.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return controller != null && extensions != null;
    }

}