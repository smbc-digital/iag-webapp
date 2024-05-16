using System.Diagnostics.CodeAnalysis;

namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class EventBanner
{
    public string Title { get; }
    public string Teaser { get; }
    public string Icon { get; }
    public string Link { get; }

    public EventBanner(string title, string teaser, string icon, string link)
    {
        Title = title;
        Teaser = teaser;
        Icon = icon;
        Link = link;
    }

    [ExcludeFromCodeCoverage]
    public class NullEventBanner : EventBanner
    {
        public NullEventBanner() : base(string.Empty, string.Empty, string.Empty, string.Empty) { }
    }
}
