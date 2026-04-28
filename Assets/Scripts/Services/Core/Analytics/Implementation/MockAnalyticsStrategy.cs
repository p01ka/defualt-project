using System.Collections.Generic;

namespace IdxZero.Services.Analytics
{
    public class MockAnalyticsStrategy : IAnalyticsStrategy
    {
        public void InitStrategy()
        {
        }

        public void LogEventWithDetails(string eventName, Dictionary<string, object> details)
        {
            UnityEngine.Debug.Log("MOCK ANALYTICS EVENT " + eventName + " PARAMS " + Newtonsoft.Json.JsonConvert.SerializeObject(details));
        }

        public void LogEventWithName(string eventName)
        {
            UnityEngine.Debug.Log("MOCK ANALYTICS EVENT " + eventName);
        }
    }
}