using System;
using System.Collections.Generic;
using IdxZero.Services.Ads;

namespace IdxZero.Services.Analytics
{
    public class AdsAnalyticsLogger
    {
        private readonly Action<string> _logEvent;
        private readonly Action<string, Dictionary<string, object>> _logEventWithDetails;

        public AdsAnalyticsLogger(Action<string> logEvent,
                                  Action<string, Dictionary<string, object>> logEventWithDetails)
        {
            _logEvent = logEvent;
            _logEventWithDetails = logEventWithDetails;
        }

        public void LogRewardedVideoStartedWithPlacement(string currentPlacement)
        {
            string eventName = "rewarded_started";
            Dictionary<string, object> details = new Dictionary<string, object>()
            {
                {"placement", currentPlacement}
            };
            LogEventWithDetails(eventName, details);
        }

        public void LogRewardedVideoFinishedWithPlacement(string currentPlacement)
        {
            string eventName = "rewarded_complete";
            Dictionary<string, object> details = new Dictionary<string, object>()
            {
                {"placement", currentPlacement}
            };
            LogEventWithDetails(eventName, details);
        }

        public void LogInterstitialStartedWithPlacement(string currentPlacement)
        {
            string eventName = "interstitial_started";
            Dictionary<string, object> details = new Dictionary<string, object>()
            {
                {"placement", currentPlacement}
            };
            LogEventWithDetails(eventName, details);
        }

        public void LogInterstitialFinishedWithPlacement(string currentPlacement)
        {
            string eventName = "interstitial_complete";
            Dictionary<string, object> details = new Dictionary<string, object>()
            {
                {"placement", currentPlacement}
            };
            LogEventWithDetails(eventName, details);
        }

        public void LogBannerStartedEvent()
        {
            string eventName = "banner_started";
            LogEvent(eventName);
        }

        public void LogBannerCompletedEvent()
        {
            string eventName = "banner_completed";
            LogEvent(eventName);
        }

        public void LogAdPaidEvent(AdPaidData adImpressionData)
        {
            // await Cysharp.Threading.Tasks.UniTask.DelayFrame(5);
            // string eventName = "ad_impression";

            // global::Firebase.Analytics.Parameter ad_platform_parameter =
            //     new global::Firebase.Analytics.Parameter("ad_platform", adImpressionData.AdPlatform);
            // global::Firebase.Analytics.Parameter ad_source_parameter =
            //     new global::Firebase.Analytics.Parameter("ad_source", adImpressionData.AdSource);
            // global::Firebase.Analytics.Parameter ad_unit_name_parameter =
            //     new global::Firebase.Analytics.Parameter("ad_unit_name", adImpressionData.AdUnitName);
            // global::Firebase.Analytics.Parameter ad_format_parameter =
            //     new global::Firebase.Analytics.Parameter("ad_format", adImpressionData.AdFormat);
            // global::Firebase.Analytics.Parameter ad_currency_parameter =
            //     new global::Firebase.Analytics.Parameter(global::Firebase.Analytics.FirebaseAnalytics.ParameterCurrency,
            //         adImpressionData.Currency);
            // global::Firebase.Analytics.Parameter ad_value_parameter =
            //     new global::Firebase.Analytics.Parameter(global::Firebase.Analytics.FirebaseAnalytics.ParameterValue,
            //         adImpressionData.Value);

            // global::Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName,
            //     ad_platform_parameter,
            //     ad_source_parameter,
            //     ad_unit_name_parameter,
            //     ad_format_parameter,
            //     ad_currency_parameter,
            //     ad_value_parameter);
        }

        private void LogEvent(string eventName)
        {
            _logEvent?.Invoke(eventName);
        }

        private void LogEventWithDetails(string eventName, Dictionary<string, object> details)
        {
            _logEventWithDetails?.Invoke(eventName, details);
        }
    }
}
