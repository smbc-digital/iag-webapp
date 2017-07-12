using System;
using System.Text.RegularExpressions;

namespace StockportWebapp.Utils
{
    public class ViewHelpers
    {
        private readonly ITimeProvider _timeProvider;

        public ViewHelpers(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public string FormatEventDate(DateTime eventDate, string startTime)
        {
            if (startTime.IndexOf(':') < 0)
            {
                return "Invalid Date";
            }

            var date = "";
            if (eventDate == _timeProvider.Now().Date)
            {
                date = "Today";
            }
            else if (eventDate == _timeProvider.Now().AddDays(1).Date)
            {
                date = "Tomorrow";
            }
            else
            {
                date = eventDate.ToString("dddd dd MMMM");
            }

            var time = startTime.Split(':');
            var hour = 0;
            int.TryParse(time[0], out hour);
            if (hour >= 12)
            {
                date = $"{date} at {hour - 12}:{time[1]}pm";
            }
            else
            {
                date = $"{date} at {hour}:{time[1]}am";
            }

            return date;
        }

        public string StripUnwantedHtml(string html)
        {
            return Regex.Replace(html, @"&lt;(.|\n)*?&gt;", string.Empty);
        }
    }
}
