namespace StockportWebapp.Utils;

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
    int GetEventOccurrences(EventFrequency freq, DateTime startDate, DateTime endDate);
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

    private Dictionary<string, EventFilter> GetEventFilters()
    {
        if (EventFilters is not null)
            return EventFilters;

        EventFilters = new Dictionary<string, EventFilter>();

        EventFilter todayEventFilter = new(Today(), Today(), "Today");
        EventFilters.Add("today", todayEventFilter);

        EventFilter tomorrowEventFilter = new(Tomorrow(), Tomorrow(), "Tomorrow");
        EventFilters.Add("tomorrow", tomorrowEventFilter);

        EventFilter thisWeekEventFilter = new(Today(), NearestSunday(), "This week");
        EventFilters.Add("thisweek", thisWeekEventFilter);

        EventFilter thisWeekendEventFilter = new(NearestFriday(), NearestSunday(), "This weekend");
        EventFilters.Add("thisweekend", thisWeekendEventFilter);

        EventFilter nextWeekEventFilter = new(NearestMonday(), NextSunday(), "Next week");
        EventFilters.Add("nextweek", nextWeekEventFilter);

        return EventFilters;
    }

    public string ReturnDisplayNameForFilter(string key) =>
        !string.IsNullOrEmpty(key) && EventFilters.ContainsKey(key)
            ? EventFilters[key].DateRange
            : string.Empty;

    public EventFilter ReturnFilterForKey(string key) =>
        !string.IsNullOrEmpty(key) && EventFilters.ContainsKey(key)
            ? EventFilters[key]
            : new EventFilter(string.Empty, string.Empty, string.Empty);

    public string Today() =>
        _today.ToString("yyyy-MM-dd");

    public string Tomorrow() =>
        _today.AddDays(1).ToString("yyyy-MM-dd");

    public string NearestFriday() =>
        _today.AddDays(5 - (_today.DayOfWeek.Equals(DayOfWeek.Saturday) || _today.DayOfWeek.Equals(DayOfWeek.Sunday)
            ? 5
            :(int)_today.DayOfWeek)).ToString("yyyy-MM-dd");

    public string NearestSunday() =>
        _today.AddDays(7 - (_today.DayOfWeek.Equals(DayOfWeek.Sunday)
            ? 7
            : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");

    public string NearestMonday() =>
        _today.AddDays(8 - (_today.DayOfWeek.Equals(DayOfWeek.Monday)
            ? 1
            : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");

    public string NextSunday() =>
        _today.AddDays(14 - (_today.DayOfWeek.Equals(DayOfWeek.Sunday)
            ? 7
            : (int)_today.DayOfWeek)).ToString("yyyy-MM-dd");

    public DateTime GetEventEndDate(Event detail)
    {
        DateTime result = detail.EventDate;

        switch (detail.EventFrequency)
        {
            case EventFrequency.Daily:
                result = detail.EventDate.AddDays(detail.Occurrences);
                break;
                
            case EventFrequency.Weekly:
                result = detail.EventDate.AddDays(detail.Occurrences * 7);
                break;

            case EventFrequency.Fortnightly:
                result = detail.EventDate.AddDays(detail.Occurrences * 14);
                break;

            case EventFrequency.Monthly:
            case EventFrequency.MonthlyDate:
            case EventFrequency.MonthlyDay:
                result = detail.EventDate.AddMonths(detail.Occurrences);
                break;

            case EventFrequency.Yearly:
                result = detail.EventDate.AddYears(detail.Occurrences);
                break;
        }

        return result;
    }

    public int GetEventOccurrences(EventFrequency freq, DateTime startDate, DateTime endDate)
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
                DateTime temp = startDate;
                do
                {
                    temp = temp.AddMonths(1);
                    diff++;
                } while (temp <= endDate && diff < 1000);

                return (int)diff;
            
            case EventFrequency.Yearly:
                DateTime tempYears = startDate;
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