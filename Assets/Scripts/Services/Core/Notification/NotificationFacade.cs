using System;
using IdxZero.Services.Localization;
using IdxZero.Utils;
using Zenject;

namespace IdxZero.Services.Notification
{
    public class NotificationFacade : INotificationFacade
    {
        private readonly INotificationKeeper _notificationKeeper;
        private readonly ILocalizationFacade _localizationFacade;
        private readonly SignalBus _signalBus;

        private readonly TimeSpan _midDaySpan;

        public NotificationFacade(INotificationKeeper notificationKeeper,
                                  SignalBus signalBus,
                                  ILocalizationFacade localizationFacade = null)
        {
            _notificationKeeper = notificationKeeper;
            _localizationFacade = localizationFacade;
            _signalBus = signalBus;

            _midDaySpan = new TimeSpan(20, 14, 0);
        }

        public void TrySetDailyRepeatedNotifications()
        {
            InitNotifications(SetDailyRepeatedNotifications);
        }

        private void SetDailyRepeatedNotifications()
        {
            DateTime startTime = GetStartTimeWithSpan(_midDaySpan);
            _notificationKeeper.TryUpdateDailyRepeatedNotifications(startTime);
        }

        private void InitNotifications(Action succeedCallback = null, Action errorCallback = null)
        {

            void SucceedInitCallback()
            {
                succeedCallback?.Invoke();
            }

            void ErrorInitCallback()
            {
                errorCallback?.Invoke();
            }

            _notificationKeeper.InitializeNotification(SucceedInitCallback, ErrorInitCallback);
        }

        private DateTime GetStartTimeWithSpan(TimeSpan span)
        {
            DateTime currentTime = DateTime.Now;
            int currentTimestamp = TimeUtils.GetTimestampOfDateTime(currentTime);

            DateTime startTime = currentTime.ChangeTime(span.Hours, span.Minutes, span.Seconds);
            int startTimestamp = TimeUtils.GetTimestampOfDateTime(startTime);

            if (startTimestamp < currentTimestamp)
            {
                startTime = startTime.AddDays(1);
            }

            return startTime;
        }
    }
}