using System.Collections.Generic;
using UnityEngine;

namespace IdxZero.Services.Localization
{
    public static class LocalizationConsts
    {
        public static readonly string[] AVAILABLE_LANGUAGES =
        {
            "en-US",
            "ru-RU",
            // "ko-KR",
            // "es-ES",
            // "it-IT",
            // "fr-FR",
            // "pt-BR",
            // "pl-PL",
            // "ja-JP"
        };

        public static readonly Dictionary<SystemLanguage, string> UNITY_SYSTEM_LANGUAGE_WITH_CULTURE_CODE =
            new Dictionary<SystemLanguage, string>
            {
                { SystemLanguage.English, "en-US" },
                { SystemLanguage.Russian, "ru-RU" },
                // { SystemLanguage.Korean, "ko-KR" },
                // {SystemLanguage.Spanish, "es-ES"},
                // {SystemLanguage.Italian, "it-IT"},
                // {SystemLanguage.French, "fr-FR"},
                // {SystemLanguage.Portuguese, "pt-BR"},
                // {SystemLanguage.Polish, "pl-PL"},
                // { SystemLanguage.Japanese, "ja-JP" }
            };
    }


    public static class LocalizationKeys
    {
        public static class Notifications
        {
            public class Content
            {
                public string Title;
                public string Description;
            }

            public static readonly List<Content> DailyRepeated = new()
            {
                new Content
                {
                    Title = "Notification.DailyRepeated.Title1",
                    Description = "Notification.DailyRepeated.Description1"
                },
                new Content
                {
                    Title = "Notification.DailyRepeated.Title2",
                    Description = "Notification.DailyRepeated.Description2"
                },
                new Content
                {
                    Title = "Notification.DailyRepeated.Title3",
                    Description = "Notification.DailyRepeated.Description3"
                },
                new Content
                {
                    Title = "Notification.DailyRepeated.Title4",
                    Description = "Notification.DailyRepeated.Description4"
                },
                new Content
                {
                    Title = "Notification.DailyRepeated.Title5",
                    Description = "Notification.DailyRepeated.Description5"
                },
                new Content
                {
                    Title = "Notification.DailyRepeated.Title6",
                    Description = "Notification.DailyRepeated.Description6"
                },
                new Content
                {
                    Title = "Notification.DailyRepeated.Title7",
                    Description = "Notification.DailyRepeated.Description7"
                }
            };
        }
    }
}