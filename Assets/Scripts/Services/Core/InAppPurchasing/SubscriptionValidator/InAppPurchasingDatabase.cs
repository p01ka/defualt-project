using Newtonsoft.Json;
using UnityEngine;

namespace IdxZero.Services.InAppPurchasing
{
    public class InAppPurchasingDatabase
    {
        private const string SubscriptionPlatformDataKey = "subscriptionPlatformKey";
        public SubscriptionPlatformData LoadSubscriptionPlatformData()
        {
            string subscriptionDataKey = SubscriptionPlatformDataKey;
            string jsonData = PlayerPrefs.GetString(subscriptionDataKey);
            Debug.Log("LOADED_SUB_PLATFORM_DATA " + jsonData);
            SubscriptionPlatformData suscriptionPlatformData = JsonConvert.DeserializeObject<SubscriptionPlatformData>(jsonData);
            return suscriptionPlatformData;
        }

        public void SaveSubscriptionPlatformData(SubscriptionPlatformData data)
        {
            string subscriptionDataKey = SubscriptionPlatformDataKey;
            string bundleDetailsJson = JsonConvert.SerializeObject(data);
            Debug.Log("SUB_PLATFORM_DATA " + bundleDetailsJson);
            PlayerPrefs.SetString(subscriptionDataKey, bundleDetailsJson);
        }
    }
}