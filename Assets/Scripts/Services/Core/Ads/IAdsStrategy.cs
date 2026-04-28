using System;
using IdxZero.Services.Ads;

public interface IAdsStrategy
{
    void InitStrategy(Action adsStarteCallbac);

    void SetAdsUnitStartedCallback(Action<AdsUnitType> onAdsUnitStartedCallback,
                                   Action<AdsUnitType> onAdsUnitFinishedCallback,
                                   Action bannerStartedCallback,
                                   Action bannerCompletedCallback);
    void SetImpressionCallback(Action<AdPaidData> adPaidCallback);

    bool ShowRewardedVideo(Action successCallback, Action errorCallback);

    void ShowAdsBanner();
    void HideAdsBanner();

    void LoadInterstitial();
    bool TryToShowInterstitial(Action interShowedCallback);
    bool IsRewardedVideoEnable();
    void TryToLoadRewardedVideo(Action rewardedVideoLoaded);
}