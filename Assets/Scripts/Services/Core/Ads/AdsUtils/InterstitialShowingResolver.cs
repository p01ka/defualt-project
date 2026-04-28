using System;
using IdxZero.Application.Model.UserStatus;
using IdxZero.Services.ActivityChecker;
using IdxZero.Services.RemoteConfig;
using IdxZero.Services.Signals;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace IdxZero.Services.Ads
{
    public interface IInterstitialShowingResolver
    {
        void SetResolver();
        void ResolveInterstitialShowing(Action onInterstitialShowedCallback, string placement);
    }

    [Serializable]
    public class InterstitialShowingDetails
    {
        [JsonProperty("interstitial_enable")] public bool IsInterstitialEnable;
        [JsonProperty("first_showing_timeout")] public int FirstShowingTimeout;
        [JsonProperty("after_showing_timeout")] public int AfterShowingTimeout;
        [JsonProperty("long_user_inactivity_trigger_enable")] public bool IsLongUserInactivityTriggerEnable;
        [JsonProperty("long_user_inactivity_timespan")] public int LongUserInactivityTimeSpan;
    }

    public class InterstitialShowingResolver : IInterstitialShowingResolver
    {
        private readonly SignalBus _signals;
        private readonly IAdsFacade _adsFacade;
        private readonly IRemoteConfigDataKeeper _remoteConfigDataKeeper;
        private readonly IUserStatusGetter _userStatus;
        private readonly IUserActivityChecker _userActivityChecker;

        private float _lastTimeoutCountingTime;

        private InterstitialShowingDetails _interstitialShowingDetails;

        public InterstitialShowingResolver(SignalBus signals,
                                           IRemoteConfigDataKeeper remoteConfigKeeeper,
                                           IAdsFacade adsFacade,
                                           IUserStatusGetter userStatus,
                                           IUserActivityChecker userActivityChecker)
        {
            _signals = signals;
            _adsFacade = adsFacade;
            _remoteConfigDataKeeper = remoteConfigKeeeper;
            _userStatus = userStatus;
            _userActivityChecker = userActivityChecker;
            _userActivityChecker.OnTimeout += TryToShowTimeoutInterstitial;

            _signals.Subscribe<ServicesSignals.OnAdsShowClosed>(StartChecker);
            _signals.Subscribe<ServicesSignals.OnAdsShowStarted>(StopChecker);
        }

        public void SetResolver()
        {
            string interstitialShowingDetailsJson = _remoteConfigDataKeeper.GetInterstitialShowingDetailsJson();
            _interstitialShowingDetails = JsonConvert.DeserializeObject<InterstitialShowingDetails>(interstitialShowingDetailsJson);
            int firstAndAfterDifference = -_interstitialShowingDetails.AfterShowingTimeout + _interstitialShowingDetails.FirstShowingTimeout;
            _lastTimeoutCountingTime = firstAndAfterDifference;
            if (_interstitialShowingDetails.IsLongUserInactivityTriggerEnable)
            {
                _userActivityChecker.InitializeActivityChecker(_interstitialShowingDetails.LongUserInactivityTimeSpan);
                _userActivityChecker.ActiveChecker(true);
                _userActivityChecker.OnTimeout += TryToShowTimeoutInterstitial;
            }
        }

        public void ResolveInterstitialShowing(Action onInterstitialShowedCallback,
                                               string placement)
        {
            UnityEngine.Debug.Log("RESOLVE INTER " + placement);
            if (!IsInterstitialEnabled())
            {
                onInterstitialShowedCallback?.Invoke();
                return;
            }

            float currentTime = Time.time;
            float difference = currentTime - _lastTimeoutCountingTime;
            if (IsInterstitialEnabledByTimeout(difference))
            {
                if (!_adsFacade.TryToShowInterstitial(
                    () =>
                        {
                            UnityEngine.Debug.Log("SHOWED");
                            onInterstitialShowedCallback?.Invoke();
                        }, placement))
                {
                    _adsFacade.TryToLoadInterstitial();
                    onInterstitialShowedCallback?.Invoke();
                }
            }
            else
            {
                _adsFacade.TryToLoadInterstitial();
                onInterstitialShowedCallback?.Invoke();
            }
        }

        private void TryToShowTimeoutInterstitial()
        {
            _lastTimeoutCountingTime = -1000f;
            ResolveInterstitialShowing(null, "long_user_inactivity");
        }

        private bool IsInterstitialEnabledByTimeout(float difference)
        {
            return difference > _interstitialShowingDetails.AfterShowingTimeout;
        }

        private void StopChecker()
        {
            _userActivityChecker.ActiveChecker(false);
        }

        private void StartChecker()
        {
            _lastTimeoutCountingTime = Time.time;
            _userActivityChecker.ActiveChecker(true);
        }

        public bool IsInterstitialEnabled()
        {
            if (_interstitialShowingDetails == null)
                SetResolver();

            return _interstitialShowingDetails.IsInterstitialEnable && !_userStatus.IsNoAdsUser;
        }
    }
}