//!!!!EXAMPLE!!!! IMPORT APPLOVIN MAX AND UNCOMMENT

// using System;
// using IdxZero.Application.Signals;
// using Services.Core.RemoteConfig;
// using UnityEngine;
// using Zenject;

// namespace IdxZero.Services.Ads
// {

//     public class ApplovinMaxAdsStrategy : IAdsStrategy
//     {
//         private readonly SignalBus _signals;
//         private readonly IRemoteConfigDataKeeper _remoteConfigDataKeeper;
//         private readonly UnityMainThreadDispatcher _mtDispatcher;

//         private Action _successRewardedVideoCallback;
//         private Action _errorRewardedVideoCallback;
//         private Action _rewardedVideoLoadedCallback;

//         private Action _bannerStartedCallback;
//         private Action _bannerCompletedCallback;

//         private Action<AdsUnitType> _onAdsUnitStartedCallback;
//         private Action<AdsUnitType> _onAdsUnitFinishedCallback;

//         private const string MaxSdkKey =
//             "hC6ycEUdy7GTDg4a77ZvIZfEGyLC5mgozolZ0DCzKSVT8Z7Ggh9-GqCWmTisIOeFANqBa08wTeTbDatvH1tXr3";

//         private const string RewardedAdUnitIdAndroid = "7f2c6fb1a9bf1737";
//         private const string InterstitialAdUnitIdAndroid = "2bfa01d7e918e996";
//         private const string BannerAdUnitIdAndroid = "dc937a918bb3d106";

//         private const string RewardedAdUnitIdIOS = "b706cf07dd4455b5";
//         private const string InterstitialAdUnitIdIOS = "e818be012acb26ee";
//         private const string BannerAdUnitIdIOS = "bbaf28944d2e0ca4";

//         private string _bannerAdUnit;
//         private bool _isBannerLoaded = false;
//         private bool _isBannerShowing;
//         private bool _isFirstBannerLoading = true;

//         private string _rewardedAdUnitId;
//         private bool _isRewardedAdLoading;
//         private bool _isRewardedAdReady;

// #pragma warning disable 0414

//         private string _interstitialAdUnitId;
//         private bool _isInterstitialLoadedButNotShowed;
//         private bool _isInterstitialLoading;
//         private Action _interShowedCallback;

//         private bool _isMaxSdkInitialized;
//         private bool _isInitializationTimeoutReached;
//         private Action<AdPaidData> _adPaidCallback;

//         public ApplovinMaxAdsStrategy(SignalBus signals,
//                                       IRemoteConfigDataKeeper remoteConfigDataKeeper,
//                                       UnityMainThreadDispatcher mtDispatcher)
//         {
//             _signals = signals;
//             _remoteConfigDataKeeper = remoteConfigDataKeeper;
//             _mtDispatcher = mtDispatcher;
//         }

//         public void InitStrategy(Action adsStartedCallback)
//         {
//             MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
//             {
//                 Debug.Log("MAX SDK Initialized");
//                 _isMaxSdkInitialized = true;

//                 InitializeRewardedAds();

//                 if (_remoteConfigDataKeeper.GetAdsBannerEnableStatus())
//                     InitializeBannerAds();

//                 _signals.Subscribe<ServicesSignals.OnShowAdsDebugger>(ShowAdsDebugger);

//                 adsStartedCallback?.Invoke();
//             };

//             MaxSdk.SetSdkKey(MaxSdkKey);
//             UnityEngine.Debug.Log("START INITIALIZATION ");
//             MaxSdk.InitializeSdk();
//             MaxSdk.SetVerboseLogging(false);
//         }

//         public void SetAdsUnitStartedCallback(Action<AdsUnitType> onAdsUnitStartedCallback,
//                                               Action<AdsUnitType> onAdsUnitFinishedCallback,
//                                               Action bannerStartedCallback,
//                                               Action bannerCompletedCallback)
//         {
//             _onAdsUnitStartedCallback = onAdsUnitStartedCallback;
//             _onAdsUnitFinishedCallback = onAdsUnitFinishedCallback;
//             _bannerStartedCallback = bannerStartedCallback;
//             _bannerCompletedCallback = bannerCompletedCallback;
//         }

//         public void SetImpressionCallback(Action<AdPaidData> adPaidCallback)
//         {
//             _adPaidCallback = adPaidCallback;
//         }

//         private void OnAdSuccessPaid(string adUnitId,
//                                      MaxSdkBase.AdInfo adInfo,
//                                      string adFormat)
//         {
//             AdPaidData adImpressionData = new AdPaidData();
//             adImpressionData.AdPlatform = "AppLovinMAX";
//             adImpressionData.AdSource = adInfo.NetworkName;
//             adImpressionData.AdUnitName = adUnitId;
//             adImpressionData.AdFormat = adFormat;
//             adImpressionData.Currency = "USD";
//             adImpressionData.Value = adInfo.Revenue;

//             _adPaidCallback?.Invoke(adImpressionData);
//         }

//         #region Interstitial

//         private void InitializeInterstitialAds()
//         {
//             _interstitialAdUnitId = default;
// #if UNITY_ANDROID
//             _interstitialAdUnitId = InterstitialAdUnitIdAndroid;
// #elif UNITY_IOS
//             _interstitialAdUnitId = InterstitialAdUnitIdIOS;
// #endif
//             // Attach callbacks
//             MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
//             MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;

//             MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;

//             MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

//             MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += InterPaid;

//             // LoadLocationConfig the first interstitial
//             LoadInterstitial();
//         }

//         private void InterPaid(string arg1, MaxSdkBase.AdInfo arg2)
//         {
//             OnAdSuccessPaid(arg1, arg2, "interstitial");
//         }

//         private void OnInterstitialLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
//         {
//             _isInterstitialLoadedButNotShowed = false;
//             _isInterstitialLoading = false;
//         }

//         private void OnInterstitialLoadedEvent(string obj, MaxSdkBase.AdInfo info)
//         {
//             _isInterstitialLoadedButNotShowed = true;
//             _isInterstitialLoading = false;
//         }

//         private void OnInterstitialHiddenEvent(string obj, MaxSdkBase.AdInfo info)
//         {
//             SetInterToDefaultState();
//         }

//         private void OnInterstitialAdFailedToDisplayEvent(string arg1, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo info)
//         {
//             SetInterToDefaultState();
//         }

//         private void SetInterToDefaultState()
//         {
//             _onAdsUnitFinishedCallback?.Invoke(AdsUnitType.INTERSTITIAL);
//             _isInterstitialLoadedButNotShowed = false;
//             LoadInterstitial();
//             if (_interShowedCallback != null)
//             {
//                 _mtDispatcher.Enqueue(_interShowedCallback);
//                 // _interShowedCallback?.Invoke();
//                 _interShowedCallback = null;
//             }
//         }

//         public void LoadInterstitial()
//         {
//             if (!_isMaxSdkInitialized) return;

//             if (_interstitialAdUnitId != default && !MaxSdk.IsInterstitialReady(_interstitialAdUnitId) &&
//                 !_isInterstitialLoadedButNotShowed &&
//                 !_isInterstitialLoading)
//             {
//                 MaxSdk.LoadInterstitial(_interstitialAdUnitId);
//                 _isInterstitialLoading = true;
//             }
//         }

//         public bool TryToShowInterstitial(Action interShowedCallback)
//         {
//             if (!_isMaxSdkInitialized) return false;
//             _interShowedCallback = interShowedCallback;
//             if (_interstitialAdUnitId != default && MaxSdk.IsInterstitialReady(_interstitialAdUnitId))
//             {
//                 _onAdsUnitStartedCallback?.Invoke(AdsUnitType.INTERSTITIAL);
//                 RunCallbackWithLoadingScreen((() => MaxSdk.ShowInterstitial(_interstitialAdUnitId)));
//                 return true;
//             }
//             else
//                 return false;
//         }

//         #endregion Interstitial

//         #region Rewarded

//         private void InitializeRewardedAds()
//         {
//             _rewardedAdUnitId = default;
// #if UNITY_ANDROID
//             _rewardedAdUnitId = RewardedAdUnitIdAndroid;
// #elif UNITY_IOS
//             _rewardedAdUnitId = RewardedAdUnitIdIOS;
// #endif
//             // Attach callbacks
//             MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
//             MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;

//             MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
//             MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;

//             MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdClosedEvent;

//             MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += RewardPaid;

//             // LoadLocationConfig the first RewardedAd
//             LoadRewardedAd();
//         }

//         private void LoadRewardedAd()
//         {
//             if (!_isRewardedAdLoading)
//             {
//                 if (!_isRewardedAdReady)
//                 {
//                     _isRewardedAdLoading = true;
//                     MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
//                 }
//             }
//         }

//         private void RewardPaid(string arg1, MaxSdkBase.AdInfo arg2)
//         {
//             OnAdSuccessPaid(arg1, arg2, "rewarded");
//         }

//         public bool IsRewardedVideoEnable()
//         {
//             if (!_isMaxSdkInitialized) return false;
//             if (_isRewardedAdReady) return true;
//             _isRewardedAdReady = MaxSdk.IsRewardedAdReady(_rewardedAdUnitId);
//             return _isRewardedAdReady;
//         }

//         public void TryToLoadRewardedVideo(Action rewardedVideoLoaded)
//         {
//             if (!_isMaxSdkInitialized) return;

//             _rewardedVideoLoadedCallback = rewardedVideoLoaded;
//             LoadRewardedAd();
//         }

//         public bool ShowRewardedVideo(Action successCallback, Action errorCallback)
//         {
//             if (!_isMaxSdkInitialized) return false;

//             _successRewardedVideoCallback = successCallback;
//             _errorRewardedVideoCallback = errorCallback;

//             if (MaxSdk.IsRewardedAdReady(_rewardedAdUnitId))
//             {
//                 _onAdsUnitStartedCallback?.Invoke(AdsUnitType.REWARDED_VIDEO);
//                 RunCallbackWithLoadingScreen(() =>
//                 {
//                     _isRewardedAdReady = false;
//                     MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
//                 });
//                 return true;
//             }
//             else if (!_isRewardedAdLoading)
//             {
//                 _isRewardedAdReady = false;
//                 LoadRewardedAd();
//             }

//             return false;
//         }

//         private void OnRewardedAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo info)
//         {
//             _isRewardedAdLoading = false;
//             _isRewardedAdReady = false;
//         }

//         private void OnRewardedAdLoadedEvent(string obj, MaxSdkBase.AdInfo info)
//         {
//             _isRewardedAdLoading = false;
//             _isRewardedAdReady = true;
//             _rewardedVideoLoadedCallback?.Invoke();
//         }

//         private float _lastRewardedVideoTime;
//         private const float RewardedVideoOffset = 0.5f;
//         private void OnRewardedAdReceivedRewardEvent(string arg1,
//                                                      MaxSdkBase.Reward arg2,
//                                                      MaxSdkBase.AdInfo info)
//         {
//             float time = Time.unscaledTime;
//             if (time - _lastRewardedVideoTime > RewardedVideoOffset)
//             {
//                 _successRewardedVideoCallback?.Invoke();
//                 _lastRewardedVideoTime = time;
//             }
//         }

//         private void OnRewardedAdFailedToDisplayEvent(string arg1, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo info)
//         {
//             _errorRewardedVideoCallback?.Invoke();
//             SetRewardedToDefaultState();
//         }

//         private void OnRewardedAdClosedEvent(string obj, MaxSdkBase.AdInfo info)
//         {
//             SetRewardedToDefaultState();
//         }

//         private void SetRewardedToDefaultState()
//         {
//             _onAdsUnitFinishedCallback?.Invoke(AdsUnitType.REWARDED_VIDEO);
//             _isRewardedAdLoading = false;
//             _isRewardedAdReady = false;
//             LoadRewardedAd();
//         }

//         #endregion Rewarded

//         #region Banners

//         private void InitializeBannerAds()
//         {
//             _bannerAdUnit = default;
// #if UNITY_ANDROID
//             _bannerAdUnit = BannerAdUnitIdAndroid;
// #elif UNITY_IOS
//             _bannerAdUnit = BannerAdUnitIdIOS;
// #endif
//             MaxSdk.CreateBanner(_bannerAdUnit, MaxSdkBase.BannerPosition.BottomCenter);
//             MaxSdk.SetBannerBackgroundColor(_bannerAdUnit, Color.white);

//             MaxSdkCallbacks.Banner.OnAdLoadedEvent += BannerLoadedCallback;
//             MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += BannerLoadFailedCallback;
//             MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += BannerPaidCallback;

// #if UNITY_IOS
//             MaxSdk.SetBannerExtraParameter(_bannerAdUnit, "should_stop_auto_refresh_on_ad_expand", "true");
// #endif

// #if UNITY_EDITOR
//             _isBannerLoaded = true;
// #endif
//         }

//         private void BannerLoadedCallback(string arg1, MaxSdkBase.AdInfo arg2)
//         {
//             Debug.Log("BANNER LOADED");
//             _bannerStartedCallback?.Invoke();
//             _isBannerLoaded = true;
//             if (_isFirstBannerLoading)
//             {
//                 if (_isBannerShowing)
//                     ShowAdsBanner();
//                 else
//                     LeanTween.delayedCall(0.1f, HideAdsBanner).setIgnoreTimeScale(true);
//                 _isFirstBannerLoading = false;
//             }
//         }

//         private void BannerLoadFailedCallback(string arg1, MaxSdkBase.ErrorInfo arg2)
//         {
//             Debug.Log("LOAD FAILED " + arg1 + " CODE " + arg2);
//         }

//         private void BannerPaidCallback(string arg1, MaxSdkBase.AdInfo arg2)
//         {
//             _bannerCompletedCallback.Invoke();
//             OnAdSuccessPaid(arg1, arg2, "banner");
//         }

//         public void ShowAdsBanner()
//         {
//             if (!_isMaxSdkInitialized) return;
//             if (_isBannerLoaded)
//             {
//                 MaxSdk.ShowBanner(_bannerAdUnit);
//             }

//             _isBannerShowing = true;
//         }

//         public void HideAdsBanner()
//         {
//             if (!_isMaxSdkInitialized) return;
//             if (_isBannerLoaded)
//             {
//                 MaxSdk.HideBanner(_bannerAdUnit);
//             }

//             _isBannerShowing = false;
//         }

//         #endregion Banners

//         private void ShowAdsDebugger()
//         {
//             UnityEngine.Debug.Log("SHOW ADS DEBUGGER    ");
//             MaxSdk.ShowMediationDebugger();
//         }

//         private void RunCallbackWithLoadingScreen(Action callback)
//         {
//             LeanTween.delayedCall(0.5f, () => { callback?.Invoke(); }).setIgnoreTimeScale(true);
//         }
//     }
// }