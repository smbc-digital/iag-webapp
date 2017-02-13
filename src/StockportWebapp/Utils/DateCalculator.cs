using System;
using System.Collections.Generic;

namespace StockportWebapp.Utils
{
    public class DateCalculator
    {
        private readonly DateTime _today;
        public Dictionary<string,EventFilter> EventFilters;

        public DateCalculator(ITimeProvider timeProvider)
        {
            _today = timeProvider.Today();
            EventFilters = GetEventFilters();
        }

        public Dictionary<string,EventFilter> GetEventFilters()
        {
            EventFilters = new Dictionary<string, EventFilter>();

            var todayEventFilter = new EventFilter(Today(), Today(), "Today");
            EventFilters.Add("today", todayEventFilter);

            var tomorrowEventFilter = new EventFilter(Tomorrow(), Tomorrow(), "Tomorrow");
            EventFilters.Add("tomorrow", tomorrowEventFilter);

            var thisWeekEventFilter = new EventFilter(Today(), NearestSunday(), "This week");
            EventFilters.Add("thisweek", thisWeekEventFilter);

            var thisWeekendEventFilter = new EventFilter(NearestFriday(), NearestSunday(), "This weekend");
            EventFilters.Add("thisweekend", thisWeekendEventFilter);

            var nextWeekEventFilter = new EventFilter(NearestMonday(), NextSunday(), "Next week");
            EventFilters.Add("nextweek", nextWeekEventFilter);

            var thismonthEventFilter = new EventFilter(Today(), LastDayOfMonth(), "This month");
            EventFilters.Add("thismonth", thismonthEventFilter);

            var nextmonthEventFilter = new EventFilter(FirstDayOfNextMonth(), LastDayOfNextMonth(), "Next month");
            EventFilters.Add("nextmonth", nextmonthEventFilter);

            return EventFilters;
        }

        public string ReturnDisplayNameForFilter(string key)
        {
            return EventFilters.ContainsKey(key) ? EventFilters[key].DateRange : string.Empty;
        }

        public string Today()
        {
            return _today.ToString("dd/MM/yyyy");
        }

        public string Tomorrow()
        {
            return _today.AddDays(1).ToString("dd/MM/yyyy");
        }

        public string NearestFriday()
        {
            return _today.AddDays(5 - (_today.DayOfWeek == DayOfWeek.Saturday || _today.DayOfWeek == DayOfWeek.Sunday ? 5 : (int)_today.DayOfWeek)).ToString("dd/MM/yyyy");
        }

        public string NearestSunday()
        {
            return _today.AddDays(7 - (_today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)_today.DayOfWeek)).ToString("dd/MM/yyyy");
        }

        public string NearestMonday()
        {
            var x = _today.DayOfWeek == DayOfWeek.Monday ? 0 : (int)_today.DayOfWeek;
            return _today.AddDays(8 - (_today.DayOfWeek == DayOfWeek.Monday ? 1 : (int)_today.DayOfWeek)).ToString("dd/MM/yyyy");
        }

        public string NextSunday()
        {
            return _today.AddDays(14 - (_today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)_today.DayOfWeek)).ToString("dd/MM/yyyy");
        }

        public string LastDayOfMonth()
        {
            var daysInMonth = DateTime.DaysInMonth(_today.Year, _today.Month);
            return _today.AddDays(daysInMonth - _today.Day).ToString("dd/MM/yyyy");
        }

        public string FirstDayOfNextMonth()
        {
            return new DateTime(_today.AddMonths(1).Year, _today.AddMonths(1).Month, 1).ToString("dd/MM/yyyy");
        }

        public string LastDayOfNextMonth()
        {
            var daysInMonth = DateTime.DaysInMonth(_today.AddMonths(1).Year, _today.AddMonths(1).Month);
            var date = new DateTime(_today.AddMonths(1).Year, _today.AddMonths(1).Month, 1);
            return date.AddDays(daysInMonth - 1).ToString("dd/MM/yyyy");
        }
    }
}
