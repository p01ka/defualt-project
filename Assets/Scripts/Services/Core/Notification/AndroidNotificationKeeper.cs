#if UNITY_ANDROID

using System;
using System.Collections.Generic;
using IdxZero.Services.Localization;
using IdxZero.Services.MonoStandart;
using MEC;
using Unity.Notifications.Android;
using UnityEngine;
using Zenject;

namespace IdxZero.Services.Notification
{
    public class AndroidNotificationKeeper : INotificationKeeper, IApplicationFocusHandler
    {
        [Inject] private ILocalizationFacade _localizationFacade;

        private const string AndroidNotificationChannelId = "channel_id";
        private const string AndroidNotificationChannelName = "Default Channel";

        private const string AndroidNotificationStartedKey = "android_notification_started_and_inited";

        public void InitializeNotification(Action succeedCallback = null, Action errorCallback = null)
        {
            Timing.RunCoroutine(RequestPermission(() =>
            {
                if (!PlayerPrefs.HasKey(AndroidNotificationStartedKey))
                {
                    AndroidNotificationChannel channel = new AndroidNotificationChannel
                    {
                        Id = AndroidNotificationChannelId,
                        Name = AndroidNotificationChannelName,
                        Importance = Importance.High,
                        Description = "Generic notifications",
                    };
                    AndroidNotificationCenter.RegisterNotificationChannel(channel);
                    PlayerPrefs.SetInt(AndroidNotificationStartedKey, 1);
                }

                succeedCallback?.Invoke();
            }, errorCallback));
        }

        private IEnumerator<float> RequestPermission(Action successCallback = null, Action errorCallbak = null)
        {
            var request = new PermissionRequest();
            while (request.Status == PermissionStatus.RequestPending)
                yield return Timing.WaitForOneFrame;
            if (request.Status == PermissionStatus.Allowed) successCallback?.Invoke();
            else errorCallbak?.Invoke();
        }

        public void TryUpdateDailyRepeatedNotifications(DateTime startTime)
        {
            StopAllNotifications();
            var interval = TimeSpan.FromDays(1);
            for (var i = 0; i < LocalizationKeys.Notifications.DailyRepeated.Count; i++)
            {
                var key = LocalizationKeys.Notifications.DailyRepeated[i];
                var notification = new AndroidNotification();
                SetUpAndroidNotification(
                    ref notification,
                    new NotificationContent
                    {
                        Title = _localizationFacade.GetText(key.Title),
                        Text = _localizationFacade.GetText(key.Description)
                    },
                    startTime + TimeSpan.FromDays(i),
                     TimeSpan.FromDays(LocalizationKeys.Notifications.DailyRepeated.Count));
                AndroidNotificationCenter.SendNotification(notification, AndroidNotificationChannelId);
                Debug.Log($"Scheduled notification: title = {_localizationFacade.GetText(key.Title)}, fireTime = {notification.FireTime}, fireInterval = {notification.RepeatInterval}");
            }
        }

        private void StopAllNotifications() => AndroidNotificationCenter.CancelAllNotifications();

        public void OnGainedFocus()
        {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        }

        public void OnLostFocus()
        {
            AndroidNotificationCenter.CancelAllDisplayedNotifications();
        }

        private void SetUpAndroidNotification(ref AndroidNotification notification,
                                              NotificationContent content,
                                              DateTime firetime,
                                              TimeSpan fireInterval = default)
        {
            notification.Title = content.Title;
            notification.Text = content.Text;
            notification.FireTime = firetime;
            notification.LargeIcon = "icon_0";
            notification.SmallIcon = "icon_1";
            if (fireInterval != default)
                notification.RepeatInterval = fireInterval;
        }
    }
}
#endif