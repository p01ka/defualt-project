#if USE_INAPP_PURCHASING
using System;
using System.Collections.Generic;
using IdxZero.Services.Localization;
using IdxZero.Utils;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace IdxZero.Services.InAppPurchasing
{
    public class InAppManager : MonoBehaviour, IInAppManager, IStoreListener
    {
        private static IStoreController _storeController;
        private static IExtensionProvider _storeExtensionProvider;

        private Action _onInitializedCallback;
        private Action _successSubscriptionCallback;
        private Action _failedPurchaseCallback;
        private const string WeeklySubscriptionAndroid = "mock_premium";
        private const string WeeklySubscriptionIOS = "mock_premium";
        private string _currentWeeklySubscription;
        private ISubscriptionValidator _subscriptionValidator;
        private SubscriptionsPricesKeeper _subscriptionsPricesKeeper;
        private SubscriptionExpireTimeKeeper _subscriptionExpireTimeKeeper;
        private bool? _isManagerInitializedStatus;
        private bool _isIOSRestoringProccess = false;
        private bool _isInitializationActive = false;

        [Inject]
        private void Construct(ISubscriptionValidator subscriptionValidator,
                               ILocalizationFacade localizationFacade,
                               SubscriptionExpireTimeKeeper subscriptionExpireTimeKeeper)
        {
            _subscriptionValidator = subscriptionValidator;
            _subscriptionExpireTimeKeeper = subscriptionExpireTimeKeeper;
            _subscriptionsPricesKeeper = new SubscriptionsPricesKeeper(localizationFacade);
        }

        #region INITIALIZATION
        public void InitializePurchasing(Action successInitializationCallback = null)
        {
            if (_isInitializationActive) return;

            _onInitializedCallback = successInitializationCallback;

            if (IsInitialized())
            {
                return;
            }

            var module = StandardPurchasingModule.Instance();
            module.useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            var builder = ConfigurationBuilder.Instance(module);

            string remoteWeeklySub = default;

            _currentWeeklySubscription = string.IsNullOrEmpty(remoteWeeklySub) ? WeeklySubscriptionAndroid : remoteWeeklySub;

#if UNITY_IOS
            _currentWeeklySubscription = WeeklySubscriptionIOS;
#endif

            AddSubscriptionProductToBuilder(builder, _currentWeeklySubscription);

            UnityPurchasing.Initialize(this, builder);
            _isInitializationActive = true;

            // IN APPS INITIALIZATION TIME OUT
            LeanTween.delayedCall(Consts.WEB_REQUEST_TIME_OUT, () =>
            {
                UnityEngine.Debug.Log("IN APPS AFTER DELAY");
                if (!IsInitialized())
                {
                    UnityEngine.Debug.Log("CALLBACK");
                    successInitializationCallback?.Invoke();
                    _onInitializedCallback = null;
                }
            }).setIgnoreTimeScale(true);
        }

        private static void AddSubscriptionProductToBuilder(ConfigurationBuilder builder, string productId)
        {
            builder.AddProduct(productId, ProductType.Subscription, new IDs()
            {
                {productId,AppleAppStore.Name},
                {productId,GooglePlay.Name},
            });
        }

        public void OnInitialized(IStoreController controller,
                                  IExtensionProvider extensions)
        {
            Debug.Log("OnInitialized: Completed!");
            _storeController = controller;
            _storeExtensionProvider = extensions;
            _isManagerInitializedStatus = true;
            _onInitializedCallback?.Invoke();
            _isInitializationActive = false;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            _onInitializedCallback?.Invoke();
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error + " msg: " + message);
            _isInitializationActive = false;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            _isInitializationActive = false;
        }

        private bool IsInitialized()
        {
            return _storeController != null && _storeExtensionProvider != null;
        }

        private IEnumerator<float> TryInitializeWithCallback(Action callback)
        {
            InitializePurchasing();
            while (!_isManagerInitializedStatus.HasValue)
            {
                yield return MEC.Timing.WaitForOneFrame;
            }
            if (_isManagerInitializedStatus.Value)
                callback?.Invoke();
        }

        #endregion INITIALIZATION

        #region  RESTORE
        enum RestoreState
        {
            None,
            Requesting,
            Successful
        }
        RestoreState restoreState = RestoreState.None;

        private Action _restoreSuccessCallback;
        private Action _restoreFailedCallback;

        public void RestorePurchases(Action successCallback, Action failCallback)
        {
            _successSubscriptionCallback = null;
            _failedPurchaseCallback = null;

            _restoreSuccessCallback = successCallback;
            _restoreFailedCallback = failCallback;

            if (IsInitialized())
            {
                TryToRestore();
            }
            else
            {
                StartCoroutine(TryInitializeWithCallback(() => TryToRestore()));
            }
        }

        private void TryToRestore()
        {
            if (UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer || UnityEngine.Application.platform == RuntimePlatform.OSXPlayer)
            {
                restoreState = RestoreState.Requesting;
                Debug.Log("RestorePurchases started ...");

                var apple = _storeExtensionProvider.GetExtension<IAppleExtensions>();
                apple.RestoreTransactions((result, message) =>
                {
                    _isIOSRestoringProccess = true;
                    Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");

                    if (result)
                    {
                        // If at this point the flag is still marked as Requesting then nothing was restored
                        if (restoreState == RestoreState.Successful)
                        {
                            // Purchases restored
                            _restoreSuccessCallback?.Invoke();
                        }
                        else
                        {
                            // Nothing to restore
                            _restoreFailedCallback?.Invoke();
                        }
                    }
                    else
                    {
                        // Restoration failed
                        _restoreFailedCallback?.Invoke();
                    }
                    // Reset the flag
                    restoreState = RestoreState.None;
                });
            }
            else
            {
                _restoreFailedCallback?.Invoke();
                Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + UnityEngine.Application.platform);
            }
        }
        #endregion RESTORE

        #region PURCHASING
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            UnityEngine.Debug.Log("+++----PROCESS PURCHASE +++----");
            Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
            return ProcessPurchaseSubscription(args);
        }


        public void OnPurchaseFailed(Product product,
                                     PurchaseFailureReason failureReason)
        {
            Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
            _failedPurchaseCallback?.Invoke();
        }

        #endregion PURCHASING

        #region SUBSCRIPTION

        public void BuySubscriptionWithType(Action succesCallback,
                                            Action failCallback,
                                            SubscriptionType subscriptionType)
        {
            restoreState = RestoreState.None;
            _successSubscriptionCallback = succesCallback;
            _failedPurchaseCallback = failCallback;
            string subscriptionId = GetSubscriptionIdByType(subscriptionType);
            TryToBuySubscription(subscriptionId);
        }

        public SubscriptionPriceDetails GetLocalizedPriceBySubscriptionType(SubscriptionType subscriptionType)
        {
            if (!IsInitialized())
            {
                InitializePurchasing();
            }
            string subscriptionId = GetSubscriptionIdByType(subscriptionType);
            ProductMetadata metadata = null;
            if (_storeController != null && _storeController.products != null && _storeController.products.all.Length > 0)
                metadata = _storeController.products.WithID(subscriptionId).metadata;
            return _subscriptionsPricesKeeper.GetSubscriptionPriceDetailsFromMeta(metadata,
                                                                                  subscriptionId);
        }

        private PurchaseProcessingResult ProcessPurchaseSubscription(PurchaseEventArgs args)
        {
            if (restoreState == RestoreState.Requesting)
                return PurchaseProcessingResult.Complete;

            Dictionary<string, string> dict = new Dictionary<string, string>();
            string androidIntroJson = default;

#if UNITY_IOS
            dict = _storeExtensionProvider.GetExtension<IAppleExtensions>().GetIntroductoryPriceDictionary();
#elif UNITY_ANDROID
            if (args.purchasedProduct.metadata.GetGoogleProductMetadata() != null)
                androidIntroJson = args.purchasedProduct.metadata.GetGoogleProductMetadata().originalJson;
#endif
            if (args.purchasedProduct.receipt != null)
            {
                if (args.purchasedProduct.definition.type == ProductType.Subscription)
                {
#if UNITY_IOS
                    string intro_json = (dict == null || !dict.ContainsKey(args.purchasedProduct.definition.storeSpecificId)) ? null : dict[args.purchasedProduct.definition.storeSpecificId];
#elif UNITY_ANDROID
                    string intro_json = androidIntroJson;
#endif
                    SubscriptionManager p = new SubscriptionManager(args.purchasedProduct, intro_json);
                    try
                    {
                        SubscriptionInfo info = p.getSubscriptionInfo();
                        DateTime expireDate = info.getExpireDate();
                        _subscriptionExpireTimeKeeper.SaveSubscriptionExpireTimeFromProduct(expireDate);
                    }
                    catch
                    {
                        UnityEngine.Debug.Log("EDITOR PURCHASE");
                    }
                }
                else
                {
                    Debug.Log("the product is not a subscription product");
                }
            }
            else
            {
                Debug.Log("the product should have a valid receipt");
            }

            _successSubscriptionCallback?.Invoke();
            _successSubscriptionCallback = null;

            HandleTransaction(args.purchasedProduct);

            return PurchaseProcessingResult.Complete;
        }

        private void TryToBuySubscription(string productId)
        {
            if (IsInitialized())
            {
                BuySubscription(productId);
            }
            else
            {
                StartCoroutine(TryInitializeWithCallback(() => BuySubscription(productId)));
            }
        }

        private void BuySubscription(string productId)
        {
            try
            {
                Product product = _storeController.products.WithID(productId);
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log($"Purchasing product asynchronously: '{product.definition.id}'");// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                    _storeController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            catch (Exception e)
            {
                _failedPurchaseCallback?.Invoke();
                Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }

        private void HandleTransaction(Product product)
        {
            if (product.hasReceipt)
            {
                UnityEngine.Debug.Log("+++----SAVE HAS RECEIPT +++---- ");
                UnityEngine.Debug.Log("+++----AVAILABLE TO PURCHASE +++----  " + (product.availableToPurchase));
                UnityEngine.Debug.Log("+++----RECEIPT +++----  " + (product.receipt));
                UnityEngine.Debug.Log("+++----METADATA +++----  " + product.metadata);
                _subscriptionValidator.SaveSuccessSupscriptionDetail(product);
            }
        }
        #endregion

        private string GetSubscriptionIdByType(SubscriptionType subscriptionType)
        {
            switch (subscriptionType)
            {
                case SubscriptionType.WEEKLY:
                case SubscriptionType.MONTHLY:
                default:
                    return _currentWeeklySubscription;
            }
        }
    }
}
#endif