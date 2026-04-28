using System;
using UnityEngine;

namespace IdxZero.Services.Notification
{
    public class EditorNotificationKeeper : INotificationKeeper
    {
        public void TryUpdateDailyRepeatedNotifications(DateTime startTime)
        {
            Debug.Log("TRY UPDATE DAILY NOTIFICATIONS START TIME " + startTime);
        }

        public UserNotificationStatus GetNotificationStatus()
        {
            return UserNotificationStatus.GRANTED;
        }

        public void InitializeNotification(Action succeedCallback = null, Action errorCallback = null)
        {
            Debug.Log("EDITOR NOTIFICATIONS INITIALIZED");
            succeedCallback?.Invoke();
        }

        public void SetLocalPushNotification(NotificationContent content, DateTime firetime)
        {
            Debug.Log("EDITOR LOCAL PUSH WILL SEND IN  " + firetime + " WITH CONTENT " + content);
        }

        public void SetLocalRepeatedNotification(NotificationContent content, DateTime startTime, TimeSpan fireInterval)
        {
            Debug.Log("EDITOR LOCAL PUSH WILL SEND IN  " + startTime + "WITH INTERVAL " + fireInterval + " WITH CONTENT " + content);
        }

        public void StopNotification()
        {
            Debug.Log("EDITOR NOTIFICATIONS STOPPED");
        }
    }
}