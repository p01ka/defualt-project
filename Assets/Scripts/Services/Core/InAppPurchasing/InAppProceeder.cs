using System;
using IdxZero.Application.Model.UserStatus;
using IdxZero.Application.UI.ApplicationScreen;
using IdxZero.Services.Analytics;
using Zenject;

namespace IdxZero.Services.InAppPurchasing
{
    public class InAppProceeder
    {
        private readonly IInAppManager _inAppManager;
        private readonly IAnalyticsFacade _analyticsFacade;
        private readonly IUserStatusSetter _userStatusSetter;
        private readonly ApplicationScreenAdapter _applicationScreenAdapter;

        public InAppProceeder(IInAppManager inAppManager,
                              IAnalyticsFacade analyticsFacade,
                              SignalBus signals,
                              IUserStatusSetter userStatusSetter,
                              ApplicationScreenAdapter applicationScreenAdapter)
        {
            _inAppManager = inAppManager;
            _analyticsFacade = analyticsFacade;
            _userStatusSetter = userStatusSetter;
            _applicationScreenAdapter = applicationScreenAdapter;
        }

        public SubscriptionPriceDetails GetLocalizedPriceBySubscriptionType(SubscriptionType type)
        {
            SubscriptionPriceDetails spd = default;
            try
            {
                spd = _inAppManager.GetLocalizedPriceBySubscriptionType(type);
            }
            catch
            {
                spd.IsDefaultPrice = true;
            }
            return spd;
        }

        public void BuySubscriptionWithType(Action premiumGot,
                                            Action premiumFailed,
                                            SubscriptionType type)
        {
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveLoadingWheelScreen(true);

            _analyticsFacade.LogEvent("user_view_product");
            _inAppManager.BuySubscriptionWithType(() =>
            {
                PremiumGot();
                premiumGot?.Invoke();
            }, PremiumFailed,
            type);
        }

        public void TryToRestorePurchase(Action premiumGot)
        {
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveLoadingWheelScreen(true);
            _inAppManager.RestorePurchases(() =>
            {
                PremiumGot();
                premiumGot?.Invoke();
            }, PremiumFailed);
        }

        private void PremiumGot()
        {
            UnityEngine.Debug.Log("PremiumGot");
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveLoadingWheelScreen(false);
            _analyticsFacade.LogEvent("user_got_subscription");
            _userStatusSetter.IsNoAdsUser = true;
        }

        private void PremiumFailed()
        {
            UnityEngine.Debug.Log("PremiumFailed");
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveLoadingWheelScreen(false);
        }
    }

    public enum SubscriptionType
    {
        WEEKLY,
        MONTHLY
    }
}
