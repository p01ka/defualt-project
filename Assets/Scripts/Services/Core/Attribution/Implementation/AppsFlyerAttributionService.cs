// !!! EXAMPLE !!! IMPORT APPSFLYER SDK AND UNCOMMENT
// using AppsFlyerSDK;
// using UnityEngine;

// namespace IdxZero.Services.Attribution
// {
//     public class AppsFlyerAttributionService : IAttributionService
//     {
//         private const string DevKey = "u6nifXYACGiTnNxKHGvdkG";
//         private const string ItunesApplicationId = "1627042762";

//         private const string ViewProductEventName = "view_product";
//         private const string SubscribeGotEventName = "subscription_successful";

//         private MonoBehaviour _appsFlyerObject;

//         public void StartAttributionService()
//         {
//             string itunesApplicationId = null;
// #if UNITY_IOS
//                         itunesApplicationId = ItunesApplicationId;
// #endif
//             AppsFlyer.setIsDebug(false);
//             AppsFlyer.initSDK(DevKey, itunesApplicationId);
//             AppsFlyer.startSDK();
//             UnityEngine.Debug.Log("APPS FLYER INIT");
//         }

//         public void SendViewProductEvent()
//         {
//             AppsFlyer.sendEvent(ViewProductEventName, null);
//         }

//         public void SendSubscriptionSuccessfullEvent()
//         {
//             AppsFlyer.sendEvent(SubscribeGotEventName, null);
//         }
//     }
// }
