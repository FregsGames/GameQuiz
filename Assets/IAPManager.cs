using System;
using System.Collections;
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

    private void Start()
    {
        configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        configurationBuilder.AddProduct("test_product", ProductType.NonConsumable);
        configurationBuilder.AddProduct("remove_ads", ProductType.NonConsumable);
        configurationBuilder.AddProduct("test_2", ProductType.NonConsumable);
        configurationBuilder.AddProduct("test_3", ProductType.NonConsumable);
        configurationBuilder.AddProduct("test_4", ProductType.NonConsumable);
        configurationBuilder.AddProduct("pack_test_0", ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, configurationBuilder);

        ShowAds = !HasBought("remove_ads");
    }

    /// <summary>
    /// Called when Unity IAP is ready to make purchases.
    /// </summary>
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