using System.Collections.Generic;
using IdxZero.Services.UserProperties;
using IdxZero.Utils;

namespace IdxZero.Services.Analytics
{
    public class AnalyticsFacade : IAnalyticsFacade
    {
        private readonly List<IAnalyticsStrategy> _analyticsStartegies;
        private readonly IUserPropertiesFacade _userPropertiesFacade;

        public AnalyticsFacade(List<IAnalyticsStrategy> analyticsStartefies,
                               IUserPropertiesFacade userPropertiesFacade)
        {
            _analyticsStartegies = analyticsStartefies;
            _userPropertiesFacade = userPropertiesFacade;
            InitStrategies();
        }

        public AdsAnalyticsLogger AdsAnalyticsLogger { get; private set; }

        private void InitStrategies()
        {
            AdsAnalyticsLogger = new AdsAnalyticsLogger(LogEvent, LogEventWithDetails);
            foreach (var strategy in _analyticsStartegies)
            {
                strategy.InitStrategy();
            }
        }

        public void LogEvent(string eventName)
        {
            foreach (var strategy in _analyticsStartegies)
            {
                strategy.LogEventWithName(eventName);
            }
        }

        private void LogEventWithDetails(string eventName,
                                         Dictionary<string, object> details)
        {
            foreach (var strategy in _analyticsStartegies)
            {
                strategy.LogEventWithDetails(eventName, details);
            }
        }

        public void LogSessionFirst()
        {
            LogEvent("session_first_custom");
        }

        public void LogSessionStart()
        {
            LogEvent("session_start_custom");
        }

        public void LogSessionStartEvents()
        {
            if (UserUtils.IsFirstRun())
            {
                LogSessionFirst();
                _userPropertiesFacade.SetFirstSessionStartProperties();
            }
            else
            {
                LogSessionStart();
            }
        }
    }
}
