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

        public string FormatEventDate(DateTime eventDate, string startTime = "")
        {
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

            if (startTime.IndexOf(':') > 0)
            {
                var time = startTime.Split(':');
                var hour = 0;
                int.TryParse(time[0], out hour);
                if (hour == 0)
                {
                    date = $"{date} at 12:{time[1]}am";
                }
                else if (hour == 12)
                {
                    date = $"{date} at 12:{time[1]}pm";
                }
                else if (hour > 12)
                {
                    date = $"{date} at {hour - 12}:{time[1]}pm";
                }
                else
                {
                    date = $"{date} at {hour}:{time[1]}am";
                }
            }

            return date;
        }

        public string StripUnwantedHtml(string html, string allowedTags = "p,a,ol,ul,li,b,strong")
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            // Remove any typed HTML
            html = Regex.Replace(html, @"&lt;(.|\n)*?&gt;", string.Empty);

            // Remove any pasted text
            var replaceReg = @"(?!<\/?(" + allowedTags.Replace(",", "|") + ").*>)<.*?>";

            return Regex.Replace(html, replaceReg, string.Empty);
        }
    }
}
