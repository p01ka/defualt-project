using System;
using System.Collections.Generic;
using IdxZero.Services.InAppPurchasing.Utils;
using IdxZero.Services.Localization;
using Newtonsoft.Json;
using UnityEngine;
#if USE_INAPP_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace IdxZero.Services.InAppPurchasing
{
    public class SubscriptionsPricesKeeper
    {
        private readonly ILocalizationFacade _localizationFacade;
        private List<string> _parsedMetaSubscriptionIds = new List<string>();
        private const string SubscriptionPriceDetailsKey = "subscription_price_details_key";

        public SubscriptionsPricesKeeper(ILocalizationFacade localizationFacade)
        {
            _localizationFacade = localizationFacade;
        }

#if USE_INAPP_PURCHASING
        public SubscriptionPriceDetails GetSubscriptionPriceDetailsFromMeta(ProductMetadata metadata,
                                                                            string subscriptionId)
        {
            SubscriptionPriceDetails subscriptionPriceDetails = default;
            if (metadata != null && !_parsedMetaSubscriptionIds.Contains(subscriptionId))
            {
                Debug.Log("GET FROM META DATA");
                string currencySign = CurrencySymbolMapper.TryGetCurrencySymbol(metadata.isoCurrencyCode);
                int positiveCurrencyFormat = CurrencySymbolMapper.TryToGetPositiveCurrencyFormat(metadata.isoCurrencyCode);

                string price = metadata.localizedPrice.ToString();

                Debug.Log("GET FROM META DATA PRICE " + price);
                Debug.Log("GET FROM META DATA PRICE " + metadata.isoCurrencyCode);
                Debug.Log("GET FROM META DATA PRICE " + metadata.localizedDescription);
                Debug.Log("GET FROM META DATA PRICE " + metadata.localizedTitle);
                Debug.Log("GET FROM META DATA PRICE " + metadata.localizedPriceString);

                // currencySign = "₻";

                if (String.IsNullOrEmpty(currencySign) || !_localizationFacade.IsCurrentFontSupportCharacter(currencySign.ToCharArray()))
                {
                    subscriptionPriceDetails.LocalizedPrice = String.Format("{0} {1}", price, metadata.isoCurrencyCode);
                }
                else
                    subscriptionPriceDetails.LocalizedPrice = FormatPriceWithPositiveCurrencyCode(positiveCurrencyFormat, currencySign, price);

                string serializedSubscriptionPriceDetails = JsonConvert.SerializeObject(subscriptionPriceDetails);
                SaveSubscriptionPriceDetailsWithType(subscriptionId, serializedSubscriptionPriceDetails);
                _parsedMetaSubscriptionIds.Add(subscriptionId);
            }
            else
            {
                Debug.Log("GET FROM PREFS");
                string serializedSubscriptionPriceDetails = LoadSubscriptionPriceDetailsWithType(subscriptionId);

                if (string.IsNullOrEmpty(serializedSubscriptionPriceDetails))
                {
                    Debug.Log("DEFAULT!!");
                    subscriptionPriceDetails = GetDefaultSubscriptionPriceDetails(default);
                }
                else
                {
                    Debug.Log("GET FROM PREFS!!");
                    subscriptionPriceDetails = JsonConvert.DeserializeObject<SubscriptionPriceDetails>(serializedSubscriptionPriceDetails);
                }
            }

            return subscriptionPriceDetails;
        }
#endif

        private void SaveSubscriptionPriceDetailsWithType(string subscriptionId,
                                                          string serializedSubscriptionPriceDetails)
        {
            string subscriptionPriceDetailsKey = SubscriptionPriceDetailsKey + subscriptionId;
            PlayerPrefs.SetString(subscriptionPriceDetailsKey, serializedSubscriptionPriceDetails);
        }

        private string LoadSubscriptionPriceDetailsWithType(string subscriptionId)
        {
            string subscriptionPriceDetailsKey = SubscriptionPriceDetailsKey + subscriptionId;
            return PlayerPrefs.GetString(subscriptionPriceDetailsKey);
        }

        private string FormatPriceWithPositiveCurrencyCode(int positiveCode,
                                                           string currencySign,
                                                           string price)
        {
            price = price.Replace(currencySign, "");
            price = price.Replace(" ", "");

            string formattedString;
            switch (positiveCode)
            {
                case 0:
                    formattedString = String.Format("{0}{1}", currencySign, price);
                    break;
                case 1:
                default:
                    formattedString = String.Format("{0}{1}", price, currencySign);
                    break;
                case 2:
                    formattedString = String.Format("{0} {1}", currencySign, price);
                    break;
                case 3:
                    formattedString = String.Format("{0} {1}", price, currencySign);
                    break;
            }
            return formattedString;
        }

        private SubscriptionPriceDetails GetDefaultSubscriptionPriceDetails(SubscriptionType subscriptionType)
        {
            SubscriptionPriceDetails subscriptionPriceDetails = default;
            subscriptionPriceDetails.IsDefaultPrice = true;
            return subscriptionPriceDetails;
        }
    }

    [Serializable]
    public struct SubscriptionPriceDetails
    {
        [JsonProperty("localizedPrice")]
        public string LocalizedPrice;
        public bool IsDefaultPrice;
    }

    [Serializable]
    public struct IOSIntroductoryProductMetadata
    {
        [JsonProperty("introductoryPrice")]
        public string IntroductoryPrice;

        [JsonProperty("introductoryPriceLocale")]
        public string PriceCurrencyCode;
    }
}
