using System.Collections;
using Newtonsoft.Json;
using UnityEngine;

namespace IdxZero.Utils
{
    public class DeviceUtils
    {
        private static bool? _isTablet;
        private static bool? _isHapticFeedbackAvailable;

        public static bool IsDebugIPadUI;
        public static bool IsDebugInternetNotAvailable;
        public static bool IsDebugMode;
        private const string CountryCodeUrl = "https://pro.ip-api.com/json/?key=Qw6qT2C0iSFSMNE";

        public static bool IsTablet
        {
            get
            {
                //return true;
                if (!_isTablet.HasValue)
                {
                    _isTablet = IsTabletDevice();
                }
                return _isTablet.Value;
            }
        }

        public static bool IsHapticFeedbackAvailable
        {
            get
            {
                if (!_isHapticFeedbackAvailable.HasValue)
                {
#if UNITY_ANDROID
                    // UnityEngine.Debug.Log("HAS VIBRATOR " + MoreMountains.NiceVibrations.MMNVAndroid.AndroidHasVibrator() + " HAS AMPLITUDE CONTROLL " + MoreMountains.NiceVibrations.MMNVAndroid.AndroidHasAmplitudeControl());
                    // _isHapticFeedbackAvailable = MoreMountains.NiceVibrations.MMNVAndroid.AndroidHasVibrator(); //&& MoreMountains.NiceVibrations.MMNVAndroid.AndroidHasAmplitudeControl();
#elif UNITY_IOS
                    // _isHapticFeedbackAvailable = MoreMountains.NiceVibrations.MMVibrationManager.HapticsSupported(); //&& MoreMountains.NiceVibrations.MMVibrationManager.iOSVersion >= 13;
#endif
                    _isHapticFeedbackAvailable = false;
                }
                return _isHapticFeedbackAvailable.Value;
            }
        }

        public static bool IsInternetConnectionAvailable()
        {
            if (IsDebugInternetNotAvailable)
            {
                return false;
            }
            return UnityEngine.Application.internetReachability != NetworkReachability.NotReachable;
        }

        private static bool IsTabletDevice()
        {
            if (IsDebugIPadUI)
            {
                return true;
            }
#if UNITY_IOS
            if (SystemInfo.deviceModel.Contains("iPad")) return true; else return false;
#else
            float ssw = Screen.width > Screen.height ? Screen.width : Screen.height;

            if (ssw < 800) return false;

            if (UnityEngine.Application.platform == RuntimePlatform.Android || UnityEngine.Application.platform == RuntimePlatform.IPhonePlayer)
            {
                float screenWidth = Screen.width / Screen.dpi;
                float screenHeight = Screen.height / Screen.dpi;
                float size = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
                if (size >= 6.5f) return true;
            }
            return false;
#endif
        }
    }

    public class DeviceLocationData
    {
        [JsonProperty("countryCode")] public string CountryCode;
    }


}