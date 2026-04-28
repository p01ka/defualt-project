//!!! EXAMPLE !!!
// using System.Collections.Generic;

// namespace IdxZero.Services.Analytics
// {
//     public class FirebaseAnalyticsStrategy : IAnalyticsStrategy
//     {
//         public void InitStrategy()
//         {
//             global::Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
//         }

//         public void LogEventWithDetails(string eventName, Dictionary<string, object> details)
//         {
//             List<global::Firebase.Analytics.Parameter> eventParameters = new List<global::Firebase.Analytics.Parameter>();
//             foreach (var kvp in details)
//             {
//                 global::Firebase.Analytics.Parameter parameter = new global::Firebase.Analytics.Parameter(kvp.Key, kvp.Value == null ? null : kvp.Value.ToString());
//                 eventParameters.Add(parameter);
//             }
//             global::Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, eventParameters.ToArray());
//             // UnityEngine.Debug.Log("FIREBASE EVENT WITH DETAILS LOGGED " + eventName);
//         }

//         public void LogEventWithName(string eventName)
//         {
//             global::Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
//             // UnityEngine.Debug.Log("FIREBASE EVENT LOGGED " + eventName);
//         }
//     }
// }