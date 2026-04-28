using Cysharp.Threading.Tasks;
#if USE_INAPP_PURCHASING
using Unity.Services.Core;
using Unity.Services.Core.Environments;
#endif
using UnityEngine;
using IdxZero.Application.Model.UserStatus;
using IdxZero.Utils;

namespace IdxZero.Services.InAppPurchasing
{
    public class InAppInitializer
    {
        private readonly IInAppManager _inAppManager;
        private readonly ISubscriptionValidator _subscriptionValidator;
        private readonly IUserStatusSetter _userStatus;
        private readonly SubscriptionExpireTimeKeeper _subscriptionExpireTimeKeeper;

        public InAppInitializer(IInAppManager inAppManager, ISubscriptionValidator subscriptionValidator, IUserStatusSetter userStatus, SubscriptionExpireTimeKeeper subscriptionExpireTimeKeeper)
        {
            _inAppManager = inAppManager;
            _subscriptionValidator = subscriptionValidator;
            _userStatus = userStatus;
            _subscriptionExpireTimeKeeper = subscriptionExpireTimeKeeper;
        }

        public UniTask StartInApps()
        {
#if USE_INAPP_PURCHASING
            var options = new InitializationOptions()
                   .SetEnvironmentName("production");
            return UnityServices.InitializeAsync(options).AsUniTask().ContinueWith(InitializePurchasingAsync);
#endif
            return UniTask.DelayFrame(1);
        }

        private UniTask InitializePurchasingAsync()
        {
            var tcs = new UniTaskCompletionSource<bool>();
            UnityEngine.Debug.Log("+++----INITIALIZE PURCHASE+++----");
            _inAppManager.InitializePurchasing(async () =>
            {
                UnityEngine.Debug.Log("+++----CHECK IN APPS +++----");
                UnityEngine.Debug.Log("+++----CHECK SUB +++----");
                bool tempValue = await _subscriptionValidator.IsAnySubscriptionActive().AsUniTask();

                if (!_userStatus.IsNoAdsUser)
                    if (tempValue)
                        _userStatus.IsNoAdsUser = tempValue;
                    else
                        _userStatus.IsNoAdsUser = GetUserStatusBySubscriptionExpireDate();

                var res = true;
                tcs.TrySetResult(res);
            });
            return tcs.Task;
        }

        private bool GetUserStatusBySubscriptionExpireDate()
        {
            long currentTimestamp = TimeUtils.GetCurrentDeviceTimeStamp();
            Debug.Log("CURRENT TIMESTAMP " + currentTimestamp);
            long expireTimestamp = GetSubscriptionExpireTimestamp();
            Debug.Log("EXPIRED TIMESTAMP " + expireTimestamp);
            return currentTimestamp < expireTimestamp;
        }

        private long GetSubscriptionExpireTimestamp()
        {
            return _subscriptionExpireTimeKeeper.GetSubscriptionExpireTimestamp();
        }
    }
}
