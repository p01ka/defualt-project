using System;
using IdxZero.Utils;
using UnityEngine;

namespace IdxZero.Services.InAppPurchasing
{
    public class SubscriptionExpireTimeKeeper
    {
        public static string SubscriptionExpireTime = "SubscriptionExpireTime";

        public void SaveSubscriptionExpireTimeFromProduct(DateTime expireDate)
        {
            Debug.Log("EXPIRE DATE " + expireDate);
            SaveExpireTimeInSeconds(TimeUtils.GetTimestampOfDateTime(expireDate).ToString());
        }

        public void SaveExpireTimeInSeconds(string seconds)
        {
            PlayerPrefs.SetString(SubscriptionExpireTime, seconds);
        }

        public long GetSubscriptionExpireTimestamp()
        {
            Debug.Log("EXPIRE PREFS " + PlayerPrefs.GetString(SubscriptionExpireTime, "0"));
            long expireTimestamp = long.Parse(PlayerPrefs.GetString(SubscriptionExpireTime, "0"));
            return expireTimestamp;
        }
    }
}