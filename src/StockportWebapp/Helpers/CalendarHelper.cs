using Ical.Net;
using Ical.Net.DataTypes;
using Markdig.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Ical.Net.Serialization;
using Ical.Net.Serialization.iCalendar.Serializers;
using StockportWebapp.Utils;
using Google.Apis.Services;
using Calendar = Ical.Net.Calendar;
using Event = StockportWebapp.Models.Event;

namespace StockportWebapp.Helpers
{
    public class CalendarHelper 
    {
        private readonly ITimeProvider _timeProvider;

        public CalendarHelper(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public string GetIcsText(Event eventItem, string currentUrl)
        {
            var startDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.StartTime);
            var endDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.EndTime);

            var e = new CalendarEvent()
            {
                Name = "VEVENT",
                Summary = eventItem.Title,
                DtStart = new CalDateTime(startDateWithTime),
                DtEnd = new CalDateTime(endDateWithTime),
                Location = eventItem.Location,
                Description = "For details, link here: " + currentUrl
            };

            var calendar = new Calendar();
            calendar.Events.Add(e);

            var serializer = new CalendarSerializer(new SerializationContext());
            return serializer.SerializeToString(calendar);
        }

       public string GetCalendarUrl(Event eventItem, string currentUrl, string calendarType)
        {
            string url = "";
            var startDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.StartTime);
            var endDateWithTime = GetCombinedDateAndTime(eventItem.EventDate, eventItem.EndTime);

            var formattedStartDate = startDateWithTime.ToString("yyyyMMddTHHmmss");
            var formattedEndDate = endDateWithTime.ToString("yyyyMMddTHHmmss");

            if (calendarType == "google")
            {
                url = "https://www.google.com/calendar/render?action=TEMPLATE&text=" + eventItem.Title + "&dates=" +
                          formattedStartDate + "/" + formattedEndDate +
                          "&details=For+details,+link+here: " + currentUrl + " &location=" + eventItem.Location + "&sf=true&output=xml";
            }

            if (calendarType == "yahoo")
            {
                url = "https://calendar.yahoo.com/?v=60&view=d&type=20&title=" + eventItem.Title + "&st=" + formattedStartDate + "&et=" + formattedEndDate + "&desc=For+details,+link+here: " + currentUrl + "&in_loc=" + eventItem.Location;
            }

            return url;
        }

        public DateTime GetCombinedDateAndTime(DateTime eventDate, string time)
        {
            DateTime dateAndTime;
            DateTime.TryParse(time, out dateAndTime);
            var startDateTimeWithTime = eventDate.AddTicks(dateAndTime.TimeOfDay.Ticks);
            return startDateTimeWithTime;
        }
    }
}
