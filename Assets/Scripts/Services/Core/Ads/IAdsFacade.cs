using System;

namespace IdxZero.Services.Ads
{
    public interface IAdsFacade
    {
        void TryToLoadRewardedVideo();
        bool TryToWatchRewardedVideo(Action successCallback, string placement);
        bool IsRewardedVideoEnable();

        void TryToShowBanner();
        void TryToHideBanner();

        void TryToLoadInterstitial();
        bool TryToShowInterstitial(Action interShowedCallback, string placement);
        void TryDisableBannerTemporary();
        void TryToRestoreTemporaryDisabledBanner();
    }
}
