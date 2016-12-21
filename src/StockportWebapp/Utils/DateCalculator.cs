using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using StockportWebapp.Utils;

namespace StockportWebapp.Utils
{
    public class DateCalculator
    {
        private readonly ITimeProvider _timeProvider;
        private readonly DateTime _today;
        public Dictionary<string,EventFilter> EventFilters;

        public DateCalculator(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
            _today = _timeProvider.Today();
            EventFilters = new Dictionary<string,EventFilter>();
        }

        public Dictionary<string,EventFilter> GetEventFilters()
        {
            var todayEventFilter = new EventFilter(Today(), Today(), "Today");
            EventFilters.Add("today", todayEventFilter);

            var tomorrowEventFilter = new EventFilter(Tomorrow(), Tomorrow(), "Tomorrow");
            EventFilters.Add("tomorrow", tomorrowEventFilter);

            var thisWeekEventFilter = new EventFilter(Today(), NearestSunday(), "This week");
            EventFilters.Add("thisweek", thisWeekEventFilter);

            var thisWeekendEventFilter = new EventFilter(NearestSaturday(), NearestSunday(), "This weekend");
            EventFilters.Add("thisweekend", thisWeekendEventFilter);

            var nextWeekEventFilter = new EventFilter(NearestMonday(), NextSunday(), "Next week");
            EventFilters.Add("nextweek", nextWeekEventFilter);

            var thismonthEventFilter = new EventFilter(Today(), LastDayOfMonth(), "This month");
            EventFilters.Add("thismonth", thismonthEventFilter);

            return EventFilters;
        }
        

        public string Today()
        {
            return _today.ToString("yyyy-MM-dd");
        }

        public string Tomorrow()
        {
            return _today.AddDays(1).ToString("yyyy-MM-dd");
        }

        public string NearestSaturday()
        {
            return _today.AddDays(6 - (int)_today.DayOfWeek).ToString("yyyy-MM-dd");            
        }

        public string NearestSunday()
        {
            return _today.AddDays(7 - (_today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }

        public string NearestMonday()
        {
            var x = _today.DayOfWeek == DayOfWeek.Monday ? 0 : (int)_today.DayOfWeek;
            return _today.AddDays(8 - (_today.DayOfWeek == DayOfWeek.Monday ? 1 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }

        public string NextSunday()
        {
            return _today.AddDays(14 - (_today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }

        public string LastDayOfMonth()
        {
            var daysInMonth = DateTime.DaysInMonth(_today.Year, _today.Month);
            return _today.AddDays(daysInMonth - _today.Day).ToString("yyyy-MM-dd");
        }
    }
}
