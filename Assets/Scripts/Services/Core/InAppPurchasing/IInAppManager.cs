using System;

namespace IdxZero.Services.InAppPurchasing
{
    public interface IInAppManager
    {
        void InitializePurchasing(Action initializedCallback);
        void RestorePurchases(Action successCallback, Action failCallback);
        void BuySubscriptionWithType(Action successCallback, Action failCallback, SubscriptionType subscriptionType);
        SubscriptionPriceDetails GetLocalizedPriceBySubscriptionType(SubscriptionType subscriptionType);
    }
}