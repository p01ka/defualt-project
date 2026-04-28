using System.Collections.Generic;

namespace IdxZero.Services.Analytics
{
    public interface IAnalyticsStrategy
    {
        void InitStrategy();
        void LogEventWithName(string eventName);
        void LogEventWithDetails(string eventName, Dictionary<string, object> details);
    }
}