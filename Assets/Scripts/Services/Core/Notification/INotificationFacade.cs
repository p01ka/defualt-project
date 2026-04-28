using System;

namespace IdxZero.Services.Notification
{
    public interface INotificationFacade
    {
        void TrySetDailyRepeatedNotifications();
    }
}