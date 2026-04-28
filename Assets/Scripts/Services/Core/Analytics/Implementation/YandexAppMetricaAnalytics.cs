//!!! EXAMPLE !!!
// using System.Collections.Generic;

// using System.Collections.Generic;

// namespace IdxZero.Services.Analytics
// {
// public class YandexAppMetricaAnalytics : IAnalyticsStrategy
// {
//         private readonly AppMetrica _appMetricaGeneral;
//         private const string ApiKey = "2a5a1c0c-64d5-4e7e-8ce5-66bb38df4ef9";

//         public YandexAppMetricaAnalytics(AppMetrica appMetricaGeneral)
//         {
//             _appMetricaGeneral = appMetricaGeneral;
//         }

//         public void InitStrategy()
//         {
//             _appMetricaGeneral.InitAppMetrica(ApiKey);
//             _appMetricaGeneral.ResumeSession();
//         }

//         public void LogEventWithName(string eventName)
//         {
//             AppMetrica.Instance.ReportEvent(eventName);
//             // UnityEngine.Debug.Log("TRY TO REPORT IN APP METRICA " + eventName);
//         }

//         public void LogEventWithDetails(string eventName,
//                                         Dictionary<string, object> details)
//         {
//             AppMetrica.Instance.ReportEvent(eventName, details);
//         }
// }
// }