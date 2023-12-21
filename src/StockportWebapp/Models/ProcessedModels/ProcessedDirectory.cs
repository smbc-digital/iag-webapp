using StockportWebapp.Extensions;

namespace StockportWebapp.Models.ProcessedModels
{
    public class ProcessedDirectory
    {
        public string Title { get; }
        public string Slug { get; }
        public string ContentfulId { get; }
        public string Teaser { get; }
        public string MetaDescription { get; }
        public string BackgroundImage { get; }
        public string Body { get; }
        public CallToActionBanner CallToAction { get; init; }
        public IEnumerable<Alert> Alerts { get; }
        public IEnumerable<DirectoryEntry> Entries { get; }

        public ProcessedDirectory(string title, string slug, string contentfulId, string teaser,
            string metaDescription, string backgroundImage, string body, CallToActionBanner callToAction,
            IEnumerable<Alert> alerts, IEnumerable<DirectoryEntry> entries)
        {
            Title = title;
            Slug = slug;
            ContentfulId = contentfulId;
            Teaser = teaser;
            MetaDescription = metaDescription;
            BackgroundImage = backgroundImage;
            Body = body;
            CallToAction = callToAction;
            Alerts = alerts;
            Entries = entries;
        }
        public string ToKml() => Entries.GetKmlForList();

    }
}