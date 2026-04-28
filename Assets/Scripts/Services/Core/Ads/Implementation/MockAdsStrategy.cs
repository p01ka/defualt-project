using System;

namespace IdxZero.Services.Ads
{
    public class MockAdsStrategy : IAdsStrategy
    {
        public void InitStrategy(Action adsStarteCallback)
        {
            adsStarteCallback?.Invoke();
        }

        public void SetAdsUnitStartedCallback(Action<AdsUnitType> onAdsUnitStartedCallback,
                                              Action<AdsUnitType> onAdsUnitFinishedCallback,
                                              Action bannerStartedCallback,
                                              Action bannerCompletedCallback)
        {
        }

        public void SetImpressionCallback(Action<AdPaidData> adPaidCallback)
        {
        }

        public void ShowAdsBanner()
        {
            UnityEngine.Debug.Log("=====SHOW BANNER======");
        }

        public void HideAdsBanner()
        {
            UnityEngine.Debug.Log("=====HIDE BANNER======");
        }

        public bool IsRewardedVideoEnable()
        {
            return true;
        }

        public bool ShowRewardedVideo(Action successCallback, Action errorCallback)
        {
            successCallback?.Invoke();
            return true;
        }

        public void TryToLoadRewardedVideo(Action rewardedVideoLoaded)
        {
        }

        public void LoadInterstitial()
        {
        }

        public bool TryToShowInterstitial(Action interShowedCallback)
        {
            interShowedCallback?.Invoke();
            return true;
        }
    }
}