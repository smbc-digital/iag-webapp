namespace StockportWebapp.Utils;

public class CalendarHelper
{
    public string GetIcsText(Event eventItem, string currentUrl)
    {
        DateTime startDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.StartTime);
        DateTime endDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.EndTime);

        CalendarEvent e = new()
        {
            Name = "VEVENT",
            Summary = eventItem.Title,
            DtStart = new CalDateTime(startDateWithTime),
            DtEnd = new CalDateTime(endDateWithTime),
            Location = eventItem.Location,
            Description = "For details, link here: " + currentUrl
        };

        Calendar calendar = new();
        calendar.Events.Add(e);

        CalendarSerializer serializer = new(new SerializationContext());

        return serializer.SerializeToString(calendar);
    }

    public string GetCalendarUrl(Event eventItem, string currentUrl, string calendarType)
    {
        string url = "";
        DateTime startDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.StartTime);
        DateTime endDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.EndTime);

        string formattedStartDate = startDateWithTime.ToString("yyyyMMddTHHmmss");
        string formattedEndDate = endDateWithTime.ToString("yyyyMMddTHHmmss");

        if (calendarType.Equals("google"))
            url = $"https://www.google.com/calendar/render?action=TEMPLATE&text={eventItem.Title}&dates={formattedStartDate}/{formattedEndDate}&details=For+details,+link+here: {currentUrl} &location={eventItem.Location}&sf=true&output=xml";

        if (calendarType.Equals("yahoo"))
            url = "https://calendar.yahoo.com/?v=60&view=d&type=20&title=" + eventItem.Title + "&st=" + formattedStartDate + "&et=" + formattedEndDate + "&desc=For+details,+link+here: " + currentUrl + "&in_loc=" + eventItem.Location;

        return url;
    }

    public DateTime GetCombinedDateAndTime(DateTime eventDate, string time)
    {
        DateTime.TryParse(time, out DateTime dateAndTime);

        return eventDate.AddTicks(dateAndTime.TimeOfDay.Ticks);
    }
}