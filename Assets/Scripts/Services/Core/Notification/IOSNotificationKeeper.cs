#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Linq;
using IdxZero.Services.Localization;
using IdxZero.Services.MonoStandart;
using MEC;
using MyBox;
using Unity.Notifications.iOS;
using UnityEngine;
using Zenject;

namespace IdxZero.Services.Notification
{
    public class IOSNotificationKeeper : INotificationKeeper, IApplicationFocusHandler
    {
        [Inject] private ILocalizationFacade _localizationFacade;

        private const string _notificationIdentifier = "_daily_notification";

        private const string _repeatedNotificationIdentifier = "_repeated_notification";

        private const string DailyRepeatedNotificationIdFormat = "_daily_repeated_notification_iOS_";

        public void InitializeNotification(Action succeedCallback = null, Action errorCallback = null)
        {
            if (GetNotificationStatus() == UserNotificationStatus.UNKNOWN)
            {
                Timing.RunCoroutine(RequestAuthorization(succeedCallback, errorCallback));
            }
            else if (GetNotificationStatus() == UserNotificationStatus.GRANTED)
            {
                succeedCallback?.Invoke();
            }
        }

        public void TryUpdateDailyRepeatedNotifications(DateTime startTime)
        {
            var now = DateTime.Now;
            void ScheduleNotifications(List<LocalizationKeys.Notifications.Content> contents, DateTime start)
            {
                for (var i = 0; i < contents.Count; i++)
                {
                    var localStartTime = start + TimeSpan.FromDays(i);
                    var calendarTrigger = new iOSNotificationCalendarTrigger
                    {
                        Year = localStartTime.Year,
                        Day = localStartTime.Day,
                        Hour = localStartTime.Hour,
                        Minute = localStartTime.Minute,
                        Second = localStartTime.Second,
                        Repeats = false
                    };

                    var identifier = DailyRepeatedNotificationIdFormat + localStartTime.Day;
                    var key = contents[i];
                    var notification = new iOSNotification()
                    {
                        Identifier = identifier,
                        Title = _localizationFacade.GetText(key.Title),
                        Body = _localizationFacade.GetText(key.Description),
                        ShowInForeground = true,
                        ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                        CategoryIdentifier = "category_b",
                        ThreadIdentifier = "thread2",
                        Trigger = calendarTrigger,
                    };

                    iOSNotificationCenter.ScheduleNotification(notification);
                    Debug.Log($"## Schedule Notification {_localizationFacade.GetText(key.Title)} {localStartTime}");
                }
            }

            var scheduledNotifications = iOSNotificationCenter.GetScheduledNotifications()
                .Where(n => n.Identifier.Contains(DailyRepeatedNotificationIdFormat) &&
                            n.Trigger is iOSNotificationCalendarTrigger trigger &&
                            trigger.Day != now.Day).ToList();

            var contentsToSchedule = LocalizationKeys.Notifications.DailyRepeated;
            contentsToSchedule.RemoveAll(pair =>
            {
                return scheduledNotifications.Find(not =>
                {
                    return not.Title == _localizationFacade.GetText(pair.Title) &&
                           not.Body == _localizationFacade.GetText(pair.Description);
                }) != null;
            });
            contentsToSchedule.Shuffle();

            var start = new DateTime(
                now.Year,
                now.Month,
                now.Day,
                startTime.Hour,
                startTime.Minute,
                startTime.Second
            ) + TimeSpan.FromDays(scheduledNotifications.Count + 1);

            ScheduleNotifications(contentsToSchedule, start);
        }

        public UserNotificationStatus GetNotificationStatus()
        {
            iOSNotificationSettings settings = iOSNotificationCenter.GetNotificationSettings();
            AuthorizationStatus currentAuthorizationStatus = settings.AuthorizationStatus;
            switch (currentAuthorizationStatus)
            {
                case AuthorizationStatus.Authorized:
                    return UserNotificationStatus.GRANTED;
                case AuthorizationStatus.Denied:
                    return UserNotificationStatus.NOT_GRANTED;
                case AuthorizationStatus.NotDetermined:
                default:
                    return UserNotificationStatus.UNKNOWN;
            }
        }

        private IEnumerator<float> RequestAuthorization(Action successCallback = null, Action errorCallbak = null)
        {
            Debug.Log("##### RequestAuthorization start");
            using (var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true))
            {
                while (!req.IsFinished)
                {
                    yield return Timing.WaitForOneFrame;
                };

                if (req.Granted)
                {
                    Debug.Log("##### RequestAuthorization successCallback called");
                    successCallback?.Invoke();
                }
                else
                {
                    Debug.Log("##### RequestAuthorization errorCallback called");
                    errorCallbak?.Invoke();
                }
            }
        }

        public void OnGainedFocus()
        {
            iOSNotificationCenter.RemoveDeliveredNotification(_repeatedNotificationIdentifier);
            iOSNotificationCenter.RemoveDeliveredNotification(_notificationIdentifier);
        }

        public void OnLostFocus()
        {
            iOSNotificationCenter.RemoveDeliveredNotification(_repeatedNotificationIdentifier);
            iOSNotificationCenter.RemoveDeliveredNotification(_notificationIdentifier);
        }
    }
}
#endif