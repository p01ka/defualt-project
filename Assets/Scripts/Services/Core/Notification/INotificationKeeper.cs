using System;
using UnityEngine;

namespace IdxZero.Services.Notification
{
    public interface INotificationKeeper
    {
        void InitializeNotification(Action succeedCallback = null, Action errorCallback = null);
        void TryUpdateDailyRepeatedNotifications(DateTime fireTime);
    }

    [Serializable]
    public struct NotificationContent
    {
        public string Title;
        [TextArea]
        public string Text;

        public override string ToString()
        {
            return "Title: " + Title + "TEXT:" + Text;
        }
    }
}