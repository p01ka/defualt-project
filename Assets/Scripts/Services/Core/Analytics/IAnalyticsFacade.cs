namespace IdxZero.Services.Analytics
{
    public interface IAnalyticsFacade
    {
        AdsAnalyticsLogger AdsAnalyticsLogger { get; }
        void LogEvent(string eventName);
        void LogSessionFirst();
        void LogSessionStart();
        void LogSessionStartEvents();
    }
}