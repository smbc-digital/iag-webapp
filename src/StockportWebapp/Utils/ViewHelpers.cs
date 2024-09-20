namespace StockportWebapp.Utils;

public class ViewHelpers
{
    private readonly ITimeProvider _timeProvider;

    public ViewHelpers(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public string FormatEventDate(DateTime eventDate, string startTime = "")
    {
        string date;
        
        if (eventDate.Equals(_timeProvider.Now().Date))
            date = "Today";
        else if (eventDate.Equals(_timeProvider.Now().AddDays(1).Date))
            date = "Tomorrow";
        else
            date = eventDate.ToString("dddd dd MMMM");

        if (startTime.IndexOf(':') > 0)
        {
            string[] time = startTime.Split(':');
            int.TryParse(time[0], out int hour);

            date = hour switch
            {
                0 => $"{date} at 12:{time[1]}am",
                12 => $"{date} at 12:{time[1]}pm",
                int t when t > 12 => $"{date} at {hour - 12}:{time[1]}pm",
                _ => $"{date} at {hour}:{time[1]}am"
            };
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
