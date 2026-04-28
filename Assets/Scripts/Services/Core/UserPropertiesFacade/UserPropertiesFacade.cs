using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace IdxZero.Services.UserProperties
{
    public class UserPropertiesFacade : IUserPropertiesFacade
    {
        private const string CohortYearKey = "cohort_year";
        private const string CohortMonthKey = "cohort_month";
        private const string CohortWeekKey = "cohort_week";
        private const string CohortDayKey = "cohort_day";

        private const string CohortDateTimeKey = "cohort_date_time_key";

        private readonly List<IUserPropertiesStrategy> _userPropertiesStrategies;

        public UserPropertiesFacade(List<IUserPropertiesStrategy> userPropertiesStrategies)
        {
            _userPropertiesStrategies = userPropertiesStrategies;
        }

        public void SetFirstSessionStartProperties()
        {
            SetCohortProperties();
        }

        private void SetCohortProperties()
        {
            AddCallendars();

            DateTime dateTime = DateTime.Now;

            int year = dateTime.Year;
            string yearString = year.ToString();
            SetUserPropertyToStrategies(CohortYearKey, yearString);

            int month = dateTime.Month;
            string monthString = month.ToString();
            SetUserPropertyToStrategies(CohortMonthKey, monthString);

            int weekOfYear = GetCalendarWeek(dateTime);
            string weekOfYearString = weekOfYear.ToString();
            SetUserPropertyToStrategies(CohortWeekKey, weekOfYearString);

            int dayOfYear = dateTime.DayOfYear;
            string dayOfYearString = dayOfYear.ToString();
            SetUserPropertyToStrategies(CohortDayKey, dayOfYearString);

            PlayerPrefs.SetString(CohortDateTimeKey, dateTime.ToString());
        }

        private int GetCalendarWeek(DateTime dat)
        {
            CultureInfo cult = System.Globalization.CultureInfo.InvariantCulture;

            int wk = cult.Calendar.GetWeekOfYear(dat, cult.DateTimeFormat.CalendarWeekRule, cult.DateTimeFormat.FirstDayOfWeek);
            return wk;
        }

        public void AddCallendars()
        {
            new System.Globalization.GregorianCalendar();
            new System.Globalization.PersianCalendar();
            new System.Globalization.UmAlQuraCalendar();
            new System.Globalization.ThaiBuddhistCalendar();
        }

        private void SetUserPropertyToStrategies(string userPropertyKey, string userPropertyValue)
        {
            foreach (var userPropertiesStrategy in _userPropertiesStrategies)
            {
                userPropertiesStrategy.SetUserProperty(userPropertyKey, userPropertyValue);
            }
        }
    }
}