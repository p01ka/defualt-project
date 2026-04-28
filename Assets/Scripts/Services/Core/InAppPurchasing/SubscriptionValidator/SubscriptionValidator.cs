using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
#if USE_INAPP_PURCHASING
using UnityEngine.Purchasing;
#endif

namespace IdxZero.Services.InAppPurchasing
{
#pragma warning disable 0618
    public interface ISubscriptionValidator
    {
        Task<bool> IsAnySubscriptionActive();
#if USE_INAPP_PURCHASING
        void SaveSuccessSupscriptionDetail(Product product);
#endif
    }

    public class SubscriptionValidator : ISubscriptionValidator
    {
        private readonly InAppPurchasingDatabase _inAppPurchasingDatabase;
        private readonly SubscriptionExpireTimeKeeper _subscriptionExpireTimeKeeper;
        private string _baseUrl = "https://inapp.idx-zero.com/verify/";

        private Dictionary<string, string> _headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json"},
            { "Accept", "application/json"}
        };

        public SubscriptionValidator(SubscriptionExpireTimeKeeper subscriptionExpireTimeKeeper)
        {
            _inAppPurchasingDatabase = new InAppPurchasingDatabase();
            _subscriptionExpireTimeKeeper = subscriptionExpireTimeKeeper;
        }

        public async Task<bool> IsAnySubscriptionActive()
        {
            string platform = GetPlatformName();
            string packageName = UnityEngine.Application.identifier;

            SubscriptionPlatformData platformData = _inAppPurchasingDatabase.LoadSubscriptionPlatformData();

            if (platformData == null || platformData.SubDetails == null || platformData.SubDetails.Length < 1)
            {
                return false;
            }

            int subscriptionCount = platformData.SubDetails.Length;
            Int64 latestExpirationDateTimestamp = default;
            bool isSubscriptionActive = false;

            for (int i = 0; i < subscriptionCount; i++)
            {
                SubscriptionDetails subDetails = platformData.SubDetails[i];
                SubscriptionValidationRequest request = new SubscriptionValidationRequest();
                request.BundleId = packageName;
                request.ReceiptToken = subDetails.ReceiptToken;
                request.SubscriptionId = subDetails.SubscriptionId;

                JObject postObject = JObject.FromObject(request);

                JObject responseObject = await Post(platform, postObject);
                string responseString = JsonConvert.SerializeObject(responseObject);

                SubscriptionValidationResponse subcriptionValidationResponse = JsonConvert.DeserializeObject<SubscriptionValidationResponse>(responseString);

                if (subcriptionValidationResponse.Success && string.IsNullOrEmpty(subcriptionValidationResponse.Error))
                {
                    if (latestExpirationDateTimestamp < subcriptionValidationResponse.Result.ExpirationDateTimestamp)
                    {
                        latestExpirationDateTimestamp = subcriptionValidationResponse.Result.ExpirationDateTimestamp;
                    }
                    if (!isSubscriptionActive)
                    {
                        isSubscriptionActive = subcriptionValidationResponse.Result.ResultStatus == ResultStatus.ACTIVE;
                    }
                }
            }

            if (latestExpirationDateTimestamp != default)
            {
                double doubleTsInSeconds = latestExpirationDateTimestamp;
                doubleTsInSeconds = TimeSpan.FromMilliseconds(doubleTsInSeconds).TotalSeconds;

                var seconds = (Int64)doubleTsInSeconds;
                UnityEngine.Debug.Log("EXPIRATION TIME STAMP " + doubleTsInSeconds.ToString());
                UnityEngine.Debug.Log("EXPIRATION TIME STAMP PARSED" + seconds.ToString());
                _subscriptionExpireTimeKeeper.SaveExpireTimeInSeconds(seconds.ToString());
            }

            return isSubscriptionActive;
        }

#if USE_INAPP_PURCHASING
        public void SaveSuccessSupscriptionDetail(Product product)
        {
            string receiptToken = default;
#if UNITY_IOS
            receiptToken = GetPurchaseToken(product);
#else
            receiptToken = product.transactionID;
#endif

            string subscriptionId = product.definition.id;
            UnityEngine.Debug.Log("+++++++SAVE RECIEPT TOKEN " + receiptToken + " +++++++++SUB ID " + subscriptionId);

            SubscriptionDetails subDetail = new SubscriptionDetails(receiptToken, subscriptionId);
            SubscriptionPlatformData data = _inAppPurchasingDatabase.LoadSubscriptionPlatformData();

            if (data == null)
            {
                data = new SubscriptionPlatformData();
                data.SubDetails = new SubscriptionDetails[0];
            }

            int availableOrderIdsCount = data.SubDetails.Length;
            if (availableOrderIdsCount == 0)
            {
                data.SubDetails = new SubscriptionDetails[] { subDetail };
            }
            else
            {
                SubscriptionDetails[] updatedOrderIds = new SubscriptionDetails[availableOrderIdsCount + 1];

                for (int i = 0; i < updatedOrderIds.Length; i++)
                {
                    if (i == updatedOrderIds.Length - 1)
                    {
                        updatedOrderIds[i] = subDetail;
                    }
                    else
                    {
                        updatedOrderIds[i] = data.SubDetails[i];
                    }
                }
                data.SubDetails = updatedOrderIds;
            }
            _inAppPurchasingDatabase.SaveSubscriptionPlatformData(data);
        }
#endif  
        private async Task<JObject> Post(string platform, JObject data, bool useTimeout = true)
        {
            UnityWebRequest request = GetFilledRequest(platform);
            request.method = UnityWebRequest.kHttpVerbPOST;

            string json = JsonConvert.SerializeObject(data);
            UnityEngine.Debug.Log("SUB CHECK REQUEST " + json);
            byte[] rawBody = Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(rawBody);
            if (useTimeout)
            {
                request.timeout = 10;
            }

            await SendRequest(request);

            if (request.isHttpError || request.isNetworkError)
                UnityEngine.Debug.LogError(request.error);

            string resultRaw = request.downloadHandler.text;
            request.Dispose();

            UnityEngine.Debug.Log("SUB CHECK RESPONSE " + resultRaw);
            if (string.IsNullOrEmpty(resultRaw))
                return new JObject();

            JObject result = JObject.Parse(resultRaw);
            return result;

        }

        private UnityWebRequest GetFilledRequest(string platform)
        {
            UnityWebRequest request = new UnityWebRequest();

            // #if UNITY_EDITOR
            UnityEngine.Debug.Log("+++++++++++++FILLED REQUEST++++++++++++++++");
            // #endif

            request.url = $"{_baseUrl}{platform}";

            foreach (KeyValuePair<string, string> header in _headers)
            {
                request.SetRequestHeader(header.Key, header.Value);

                // #if UNITY_EDITOR
                UnityEngine.Debug.Log("HEADER KEY " + header.Key + " HEADER VALUE " + header.Value);
                // #endif

            }

            // #if UNITY_EDITOR
            UnityEngine.Debug.Log("REQUEST URL " + request.url);
            UnityEngine.Debug.Log("+++++++++++++FILLED REQUEST FINISHED++++++++++++++++");
            // #endif

            request.downloadHandler = new DownloadHandlerBuffer();
            return request;
        }

        private async Task SendRequest(UnityWebRequest request)
        {
            request.SendWebRequest();

            for (int i = 0; i < 110; i++)
            {
                if (request.isDone)
                    return;

                await Task.Delay(100);
            }

            request.Abort();
        }

#if USE_INAPP_PURCHASING
        private static string GetPurchaseToken(Product product)
        {
            string purchaseToken = default;
            var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(product.receipt);

            var store = (string)receipt_wrapper["Store"];
            var payload = (string)receipt_wrapper["Payload"];

            if (payload != null)
            {
                switch (store)
                {
                    case GooglePlay.Name:
                        var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                        if (!payload_wrapper.ContainsKey("json"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                        }
                        var originalJson = (string)payload_wrapper["json"];
                        var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(originalJson);
                        if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("purchaseToken"))
                        {
                            Debug.Log("The product receipt does not contain enough information, the 'purchaseToken' field is missing");
                        }
                        purchaseToken = (string)original_json_payload_wrapper["purchaseToken"];
                        break;

                    case AppleAppStore.Name:
                        purchaseToken = payload;
                        break;
                }
            }
            return purchaseToken;
        }
#endif

        private string GetPlatformName()
        {
#if UNITY_IOS && !UNITY_EDITOR
            return "ios";
#elif UNITY_ANDROID && !UNITY_EDITOR
            return "android";
#endif
            return "android";
        }
    }

    [Serializable]
    public class SubscriptionValidationRequest
    {
        [JsonProperty("bundleId")] public string BundleId;
        [JsonProperty("receiptToken")] public string ReceiptToken;
        [JsonProperty("subscriptionId")] public string SubscriptionId;
    }

    [Serializable]
    public class SubscriptionValidationResponse
    {
        [JsonProperty("success")] public bool Success;
        [JsonProperty("error")] public string Error;
        [JsonProperty("result")] public SubscriptionValidationResult Result;
    }

    [Serializable]
    public class SubscriptionValidationResult
    {
        [JsonProperty("trial")] public string Trial;
        [JsonProperty("expirationDateMs")] public Int64 ExpirationDateTimestamp;
        [JsonProperty("status")] public ResultStatus ResultStatus;
    }

    [Serializable]
    public class SubscriptionPlatformData
    {
        public SubscriptionDetails[] SubDetails;
    }

    [Serializable]
    public class SubscriptionDetails
    {
        [JsonProperty("receiptToken")] public string ReceiptToken;
        [JsonProperty("subscriptionId")] public string SubscriptionId;

        public SubscriptionDetails(string receiptToken, string subscriptionId)
        {
            ReceiptToken = receiptToken;
            SubscriptionId = subscriptionId;
        }
    }

    public enum ResultStatus
    {
        ACTIVE,
        CANCELLED_BY_USER,
        IN_BILLING_RETRY_PERIOD,
        CANCELLED_BY_BILLING_ERROR,
        CANCELLED_OTHER
    }
}