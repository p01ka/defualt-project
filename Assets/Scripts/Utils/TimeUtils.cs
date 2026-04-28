using System;

namespace IdxZero.Utils
{
    public static class TimeUtils
    {
        private const int CorrectDeviceTimeOffset = 600;

        public static int GetCurrentDeviceTimeStamp(bool useAnticheat = false)
        {
            DateTime currentDateTime = GetCurrentDeviceDateTime(useAnticheat);
            return GetTimestampOfDateTime(currentDateTime);
        }

        public static DateTime GetCurrentDeviceDateTime(bool useAnticheat = false)
        {
            return DateTime.Now;
        }

        private static int _disableItemStartTime;
        private static int _disableItemEndTime;

        public static void SetDisableItemStartTime()
        {
            _disableItemStartTime = GetCurrentDeviceTimeStamp();
        }

        public static void SetDisableItemEndTime()
        {
            _disableItemEndTime = GetCurrentDeviceTimeStamp();
        }

        public static int GetActiveSuscriptionTime()
        {
            int activeTime = _disableItemEndTime - _disableItemStartTime;
            _disableItemStartTime = _disableItemEndTime;
            return activeTime;
        }

        public static int GetTimestampOfDateTime(DateTime currentDateTime)
        {
            return (int)(currentDateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes = 0, int seconds = 0, int milliseconds = 0)
        {
            return new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                hours,
                minutes,
                seconds,
                milliseconds,
                dateTime.Kind);
        }
    }
}