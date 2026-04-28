using System;
using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using IdxZero.Services.Analytics;
using IdxZero.Services.Signals;
using IdxZero.Application.Model.UserStatus;
using IdxZero.Application.UI.ApplicationScreen;

namespace IdxZero.Services.Ads
{
    public class AdsFacade : IAdsFacade, IAdsInitializer, IAdsBannerShowingGetter, IDisposable
    {
        private readonly SignalBus _signals;
        private readonly IUserStatusGetter _statusGetter;
        private readonly IAnalyticsFacade _analyticsFacade;
        private readonly IAdsStrategy _adsStrategy;
        private readonly ApplicationScreenAdapter _applicationScreenAdapter;

        private bool? _isAdsBannerEnable;
        private bool _isBannerShowing;

        private string _currentPlacement;

        public bool IsBannerShowing
        {
            get => _isBannerShowing;
            private set
            {
                _isBannerShowing = value;
                OnBannerShowingChanged?.Invoke();
            }
        }

        public event Action OnBannerShowingChanged;

        public AdsFacade(SignalBus signal,
                         IUserStatusGetter statusGetter,
                         IAnalyticsFacade analyticsFacade,
                         IAdsStrategy adsStrategy,
                         ApplicationScreenAdapter applicationScreenAdapter)
        {
            _signals = signal;
            _analyticsFacade = analyticsFacade;

            _statusGetter = statusGetter;

            _statusGetter.OnUserStatusChanged += ChangeBannerVisibilityByUserStatus;
            _adsStrategy = adsStrategy;
            _applicationScreenAdapter = applicationScreenAdapter;
        }

        public UniTask InitAdsAsync()
        {

            _adsStrategy.SetAdsUnitStartedCallback(OnAdsUnitStartedCallback,
                                                   OnAdsUnitFinishedCallback,
                                                   OnBannerStartedCallback,
                                                   OnBannerCompletedCallback);

            _adsStrategy.SetImpressionCallback(AdSuccessfullyPaid);

            var tcs = new UniTaskCompletionSource<bool>();
            _adsStrategy.InitStrategy(() =>
            {
                var res = true;
                tcs.TrySetResult(res);
            });
            return tcs.Task;
        }

        public bool IsRewardedVideoEnable()
        {
            return _adsStrategy.IsRewardedVideoEnable();
        }

        public void TryToLoadRewardedVideo()
        {
            _adsStrategy.TryToLoadRewardedVideo(null);
        }

        public bool TryToWatchRewardedVideo(Action successCallback, string placement = default)
        {
            _currentPlacement = placement;
            return _adsStrategy.ShowRewardedVideo(successCallback, ErrorRewardedVideoCallback);
        }

        public void TryToLoadInterstitial()
        {
            _adsStrategy.LoadInterstitial();
        }

        public bool TryToShowInterstitial(Action interShowedCallback, string placement)
        {
            _currentPlacement = placement;
            return _adsStrategy.TryToShowInterstitial(interShowedCallback);
        }

        private void OnAdsUnitStartedCallback(AdsUnitType adsUnitType)
        {
            if (adsUnitType == AdsUnitType.REWARDED_VIDEO)
            {
                _analyticsFacade.AdsAnalyticsLogger.LogRewardedVideoStartedWithPlacement(_currentPlacement);
            }
            else if (adsUnitType == AdsUnitType.INTERSTITIAL)
            {
                _analyticsFacade.AdsAnalyticsLogger.LogInterstitialStartedWithPlacement(_currentPlacement);
            }

            _signals.TryFire<ServicesSignals.OnAdsShowStarted>();
        }

        private void OnAdsUnitFinishedCallback(AdsUnitType adsUnitType)
        {
            if (adsUnitType == AdsUnitType.REWARDED_VIDEO)
            {
                _analyticsFacade.AdsAnalyticsLogger.LogRewardedVideoFinishedWithPlacement(_currentPlacement);
            }
            else if (adsUnitType == AdsUnitType.INTERSTITIAL)
            {
                _analyticsFacade.AdsAnalyticsLogger.LogInterstitialFinishedWithPlacement(_currentPlacement);
            }

            _signals.TryFire<ServicesSignals.OnAdsShowClosed>();
        }

        private void OnBannerStartedCallback()
        {
            _analyticsFacade.AdsAnalyticsLogger.LogBannerStartedEvent();
        }

        private void OnBannerCompletedCallback()
        {
            _analyticsFacade.AdsAnalyticsLogger.LogBannerCompletedEvent();
        }

        public void TryToShowBanner()
        {
            if (!_statusGetter.IsNoAdsUser)
            {
                if (!IsBannerShowing)
                {
                    EnableCurrentAdsBanner();
                    IsBannerShowing = true;
                }
            }
        }

        public void TryToHideBanner()
        {
            if (IsBannerShowing)
            {
                DisableCurrentAdsBanner();
                IsBannerShowing = false;
            }
        }

        public void TryDisableBannerTemporary()
        {
            if (IsBannerShowing)
                DisableCurrentAdsBanner();
        }

        public void TryToRestoreTemporaryDisabledBanner()
        {
            if (IsBannerShowing)
                EnableCurrentAdsBanner();
        }

        private void DisableCurrentAdsBanner()
        {
            _adsStrategy.HideAdsBanner();
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveAdsBannerPanel(false);
        }

        private void EnableCurrentAdsBanner()
        {
            _adsStrategy.ShowAdsBanner();
            _applicationScreenAdapter.ApplicationScreenFacade.ActiveAdsBannerPanel(true);
        }

        private void ErrorRewardedVideoCallback()
        {
            Debug.Log("VIDEO NOT WATCHED");
        }

        private void ChangeBannerVisibilityByUserStatus(bool isPremiumUser)
        {
            if (isPremiumUser)
                TryToHideBanner();
        }

        private void AdSuccessfullyPaid(AdPaidData adImpressionData)
        {
            _analyticsFacade.AdsAnalyticsLogger.LogAdPaidEvent(adImpressionData);
        }

        public void Dispose()
        {
            _statusGetter.OnUserStatusChanged -= ChangeBannerVisibilityByUserStatus;
        }
    }

    public interface IAdsBannerShowingGetter
    {
        bool IsBannerShowing { get; }
        event Action OnBannerShowingChanged;
    }

    public enum AdsUnitType
    {
        INTERSTITIAL,
        REWARDED_VIDEO
    }

    public struct AdPaidData
    {
        public string AdPlatform;
        public string AdSource;
        public string AdUnitName;
        public string AdFormat;
        public string Currency;
        public double Value;

        public override string ToString()
        {
            return "[AdInfo adPlatform: " + AdPlatform +
                   ", adSource: " + AdSource +
                   ", adUnitName: " + AdUnitName +
                   ", adFormat: " + AdFormat +
                   ", currency: " + Currency +
                   ", value: " + Value + "]";
        }
    }

    // UNCOMMENT WHEN USE FB ADS
    // #if UNITY_IOS && !UNITY_EDITOR
    //     namespace AudienceNetwork
    //     {
    //         public static class AudienceNetworkiOSAdSettings
    //         {
    //             [System.Runtime.InteropServices.DllImport("__Internal")]
    //             private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

    //             public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
    //             {
    //                 FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
    //             }
    //         }
    //     }
    // #endif
}