using System;
using System.Collections.Generic;
using StockportWebapp.Models;

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
        DateTime GetEventEndDate(Event detail);
        int GetEventOccurences(EventFrequency freq, DateTime startDate, DateTime endDate);
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

        public DateTime GetEventEndDate(Event detail)
        {
            var result = detail.EventDate;

            switch (detail.EventFrequency)
            {
                case EventFrequency.Daily:
                    result = detail.EventDate.AddDays(detail.Occurences);
                    break;
                case EventFrequency.Weekly:
                    result = detail.EventDate.AddDays(detail.Occurences * 7);
                    break;
                case EventFrequency.Fortnightly:
                    result = detail.EventDate.AddDays(detail.Occurences * 14);
                    break;
                case EventFrequency.Monthly:
                case EventFrequency.MonthlyDate:
                case EventFrequency.MonthlyDay:
                    result = detail.EventDate.AddMonths(detail.Occurences);
                    break;
                case EventFrequency.Yearly:
                    result = detail.EventDate.AddYears(detail.Occurences);
                    break;
            }

            return result;
        }

        public int GetEventOccurences(EventFrequency freq, DateTime startDate, DateTime endDate)
        {
            double diff = 0;
            switch (freq)
            {
                case EventFrequency.None:
                    diff = 0;
                    break;
                case EventFrequency.Daily:
                    diff = endDate.Subtract(startDate).Days;
                    break;
                case EventFrequency.Weekly:
                    diff = endDate.Subtract(startDate).Days / 7;
                    break;
                case EventFrequency.Fortnightly:
                    diff = endDate.Subtract(startDate).Days / 14;
                    break;
                case EventFrequency.Monthly:
                case EventFrequency.MonthlyDate:
                case EventFrequency.MonthlyDay:
                    var temp = startDate;
                    do
                    {
                        temp = temp.AddMonths(1);
                        diff++;
                    } while (temp <= endDate && diff < 1000);

                    return (int)diff;
                case EventFrequency.Yearly:
                    var tempYears = startDate;
                    do
                    {
                        tempYears = tempYears.AddYears(1);
                        diff++;
                    } while (tempYears <= endDate && diff < 1000);

                    return (int)diff;
            }

            return (int)Math.Floor(diff) + 1; // Add 1 for the initial occurence;
        }
    }
}
