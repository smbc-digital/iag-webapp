using System;
using System.Collections.Generic;

namespace StockportWebapp.Utils
{
    public interface IDateCalculator
    {
        Dictionary<string, EventFilter> EventFilters { get; set; }
        string ReturnDisplayNameForFilter(string key);
        EventFilter ReturnFilterForKey(string key);
        string Today();
        string Tomorrow();
        string NearestFriday();
        string NearestSunday();
        string NearestMonday();
        string NextSunday();       
    }

    public class DateCalculator : IDateCalculator
    {
        private readonly DateTime _today;
        public Dictionary<string, EventFilter> EventFilters { get; set; }

        public DateCalculator(ITimeProvider timeProvider)
        {
            _today = timeProvider.Today();
            EventFilters = GetEventFilters();
        }

        private Dictionary<string,EventFilter> GetEventFilters()
        {
            if (EventFilters != null) return EventFilters;

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

            return EventFilters;
        }

        public string ReturnDisplayNameForFilter(string key)
        {
            return !string.IsNullOrEmpty(key) && EventFilters.ContainsKey(key) 
                ? EventFilters[key].DateRange 
                : string.Empty;
        }

        public EventFilter ReturnFilterForKey(string key)
        {
            return !string.IsNullOrEmpty(key) && EventFilters.ContainsKey(key)
                ? EventFilters[key]
                : new EventFilter(string.Empty, string.Empty, string.Empty);
        }

        public string Today()
        {
            return _today.ToString("yyyy-MM-dd");
        }

        public string Tomorrow()
        {
            return _today.AddDays(1).ToString("yyyy-MM-dd");
        }

        public string NearestFriday()
        {
            return _today.AddDays(5 - (_today.DayOfWeek == DayOfWeek.Saturday || _today.DayOfWeek == DayOfWeek.Sunday ? 5 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }

        public string NearestSunday()
        {
            return _today.AddDays(7 - (_today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }

        public string NearestMonday()
        {
            return _today.AddDays(8 - (_today.DayOfWeek == DayOfWeek.Monday ? 1 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }

        public string NextSunday()
        {
            return _today.AddDays(14 - (_today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");
        }
    }
}
