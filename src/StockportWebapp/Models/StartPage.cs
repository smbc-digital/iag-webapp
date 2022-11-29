using StockportWebapp.Utils;

namespace StockportWebapp.Models
{
    public class StartPage
    {
        public string Slug { get; }
        public string Title { get; }
        public string Teaser { get; }
        public string Summary { get; }
        public string UpperBody { get; }
        public string FormLinkLabel { get; }
        public string FormLink { get; }
        public string LowerBody { get; }
        public IEnumerable<Crumb> Breadcrumbs { get; }
        public string BackgroundImage { get; }
        public string Icon { get; }
        public List<Alert> Alerts { get; private set; }
        public IEnumerable<Alert> AlertsInline { get; set; }

        public StartPage(string slug, string title, string teaser, string summary, string upperBody, string formLinkLabel,
            string formLink, string lowerBody, IEnumerable<Crumb> breadcrumbs, string backgroundImage,
            string icon, List<Alert> alerts, IEnumerable<Alert> inlineAlerts)
        {
            Slug = slug;
            Title = title;
            Teaser = teaser;
            Summary = summary;
            UpperBody = MarkdownWrapper.ToHtml(upperBody);
            FormLinkLabel = formLinkLabel;
            FormLink = formLink;
            LowerBody = MarkdownWrapper.ToHtml(lowerBody);
            Breadcrumbs = breadcrumbs;
            BackgroundImage = backgroundImage;
            Icon = icon;
            Alerts = alerts;
            AlertsInline = inlineAlerts;
        }
    }
}
