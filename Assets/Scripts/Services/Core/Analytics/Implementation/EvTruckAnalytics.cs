// !!! EXAMPLE !!!
// using System.Collections.Generic;
// using MagnusSdk.EvTruck;

// namespace IdxZero.Services.Analytics
// {
//     public class EvTruckAnalytics : IAnalyticsStrategy
//     {
//         private bool _isEvTruckInitialized = false;
//         public void InitStrategy()
//         {
//             TryToInitializeEvTruck();
//         }

//         public void LogEventWithName(string eventName)
//         {
//             TryToInitializeEvTruck();
//             EvTruck.TrackEvent(eventName);
//             UnityEngine.Debug.Log("EV " + eventName);
//         }

//         public void LogEventWithDetails(string eventName, Dictionary<string, object> details)
//         {
//             TryToInitializeEvTruck();
//             EvTruck.TrackEvent(eventName, details);
//         }

//         public void SetUserProperty(string property, string value)
//         {
//             TryToInitializeEvTruck();
//             EvTruck.SetUserProperty(property, value);
//         }

//         private void TryToInitializeEvTruck()
//         {
//             if (MagnusSdk.Core.Magnus.IsInitialized && !_isEvTruckInitialized)
//             {
//                 EvTruck.Initialize();
//                 _isEvTruckInitialized = true;
//             }
//         }
//     }
// }