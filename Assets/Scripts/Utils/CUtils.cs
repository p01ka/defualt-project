using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

// ReSharper disable PossibleLossOfFraction

namespace IdxZero.Utils
{
    public class CUtils
    {
        public static string BuildStringFromCollection(ICollection values, char split = '|')
        {
            StringBuilder paintingStatuses = new StringBuilder();
            int i = 0;
            foreach (var value in values)
            {
                paintingStatuses.Append(value);
                if (i != values.Count - 1)
                {
                    paintingStatuses.Append(split);
                }
                i++;
            }
            return paintingStatuses.ToString();
        }

        public static List<T> BuildListFromString<T>(string values, char split = '|')
        {
            List<T> list = new List<T>();
            if (string.IsNullOrEmpty(values))
                return list;

            string[] arr = values.Split(split);
            foreach (string value in arr)
            {
                if (string.IsNullOrEmpty(value)) continue;
                T val = (T)Convert.ChangeType(value, typeof(T));
                list.Add(val);
            }
            return list;
        }

        public static void SaveToDisk(string name, Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, name + ".png");
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            File.WriteAllBytes(filepath, bytes);
        }

        public static void SaveToDiskByPath(string filepath, Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(filepath, bytes);
        }

        public static Texture2D LoadFromDisk(string name)
        {
            string filepath = Path.Combine(UnityEngine.Application.persistentDataPath, name + ".png");
            if (File.Exists(filepath))
            {
                var bytes = File.ReadAllBytes(filepath);
                var tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                tex.LoadImage(bytes);
                return tex;
            }
            return null;
        }

        public static Texture2D LoadFromDisk(string directoryPath, string name)
        {
            string filepath = Path.Combine(directoryPath, name + ".png");
            return LoadFromDiskByPath(filepath);
        }

        public static Texture2D LoadFromDiskByPath(string path)
        {
            if (File.Exists(path))
            {
                var bytes = File.ReadAllBytes(path);
                var tex = new Texture2D(0, 0, TextureFormat.RGBA32, false);
                tex.LoadImage(bytes);
                return tex;
            }
            return null;
        }

        public static Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
        {
            source.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight) { hideFlags = HideFlags.HideAndDontSave };
            nTex.ReadPixels(new Rect(0, 0, newWidth, newWidth), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            return nTex;
        }

        public static double GetCurrentTimestamp()
        {
            TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return span.TotalSeconds;
        }

        public static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear
                    (incX * ((float)px % targetWidth),
                     incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }

        public static int GetRandomExceptList(int fromNr, int exclusiveToNr, List<int> exceptNr)
        {
            int randomNr = UnityEngine.Random.Range(fromNr, exclusiveToNr);

            while (exceptNr.Contains(randomNr))
            {
                randomNr = UnityEngine.Random.Range(fromNr, exclusiveToNr);
            }
            return randomNr;
        }

        public static CultureInfo GetCurrentCultureInfo(Dictionary<SystemLanguage, string> cultureCodesDict = null)
        {
            SystemLanguage currentLanguage = UnityEngine.Application.systemLanguage;
            try
            {
                string currentCultureCode = "en-US";
                if (cultureCodesDict != null && cultureCodesDict.TryGetValue(currentLanguage, out var cultureCode))
                {
                    currentCultureCode = cultureCode;
                }
                return CultureInfo.GetCultureInfo(currentCultureCode);
            }
            catch
            {
                CultureInfo correspondingCultureInfo = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(x => x.EnglishName.Equals(currentLanguage.ToString()));
                return CultureInfo.CreateSpecificCulture(correspondingCultureInfo.TwoLetterISOLanguageName);
            }
        }

        private static float _cachedAdsPanelAnchorHeight;
        public static float GetAdsPanelAnchorMax(float adsPanelAnchorHeight = default)
        {
            Utils.DeviceType deviceType = DeviceUtils.IsTablet ? Utils.DeviceType.TABLET : Utils.DeviceType.PHONE;
            float bannerHeight = GetBannerHeight(deviceType);
            float anchorMax = default;
#if UNITY_ANDROID
            anchorMax = ((bannerHeight / (float)Display.main.systemHeight));
#elif UNITY_IOS
            if(adsPanelAnchorHeight == default)
            {
                adsPanelAnchorHeight=_cachedAdsPanelAnchorHeight;
            }else
            {
                _cachedAdsPanelAnchorHeight=adsPanelAnchorHeight;
            }
            //BANNER HEIGHT TO ANCHOR POSITION CALCULATIONS BY DEVICE TYPE
            anchorMax = (bannerHeight * (DeviceUtils.IsTablet ? 1.31f : 1.1f) / (float)Screen.height) / (adsPanelAnchorHeight);
#endif
            return anchorMax;
        }


        private static float GetBannerHeight(DeviceType deviceType)
        {
            float baseBannerSize = deviceType == DeviceType.TABLET ? 90f : 50f;
            float screenDpi = default;
#if UNITY_EDITOR
            screenDpi = deviceType == DeviceType.TABLET ? 324f : 462.63f; //iPad mini Retina and iPhoneX
#else
            screenDpi = Screen.dpi;
#endif
            return Mathf.RoundToInt((float)baseBannerSize * screenDpi / 160);
        }
    }

    public enum DeviceType
    {
        PHONE,
        TABLET
    }

    public struct DevicePanelSizes
    {
        public Vector2 AdsSelectedTextSize;
        public Vector2 AdsItemScrollSize;

        public Vector2 NoAdsSelectedTextSize;
        public Vector2 NoAdsItemScrollSize;
    }

    public static class AdsPanelSizes
    {
        private static Dictionary<DeviceType, DevicePanelSizes> PanelSizesByDeviceType = new Dictionary<DeviceType, DevicePanelSizes>()
        {
            {DeviceType.PHONE, new DevicePanelSizes(){
                AdsSelectedTextSize=new Vector2(0.21f,0.26f),
                AdsItemScrollSize=new Vector2(0.1f,0.2f),

                NoAdsSelectedTextSize=new Vector2(0.14f,0.19f),
                NoAdsItemScrollSize=new Vector2(0.03f,0.13f)
            }},

            {DeviceType.TABLET, new DevicePanelSizes(){
                AdsSelectedTextSize=new Vector2(0.26f,0.31f),
                AdsItemScrollSize=new Vector2(0.14f,0.24f),

                NoAdsSelectedTextSize=new Vector2(0.15f,0.2f),
                NoAdsItemScrollSize=new Vector2(0.03f,0.13f)
            }}
        };

        public static DevicePanelSizes GetAdsPanelSizes()
        {
            DeviceType deviceType = DeviceUtils.IsTablet ? DeviceType.TABLET : DeviceType.PHONE;
            DevicePanelSizes devicePanelSizes = PanelSizesByDeviceType[deviceType];
            return devicePanelSizes;
        }

    }
}