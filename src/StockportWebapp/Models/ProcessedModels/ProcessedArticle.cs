namespace StockportWebapp.Models.ProcessedModels;

public interface IContactUsMessageContainer
{
    void AddContactUsMessage(string message, string slug = "");
}

public class ProcessedArticle(string title,
                            string slug,
                            string body,
                            string teaser,
                            string metaDescription,
                            IEnumerable<ProcessedSection> sections,
                            string icon,
                            string backgroundImage,
                            string image,
                            string altText,
                            IEnumerable<Crumb> breadcrumbs,
                            IEnumerable<Alert> alerts,
                            Topic topic,
                            IEnumerable<Alert> alertsInline,
                            DateTime updatedAt,
                            bool hideLastUpdated,
                            List<TrustedLogo> trustedLogos,
                            string logoAreaTitle,
                            IEnumerable<SubItem> relatedContent,
                            string author,
                            string photographer,
                            DateTime publishedOn,
                            IEnumerable<InlineQuote> inlineQuotes,
                            List<Event> events,
                            string contentfulId) : IProcessedContentType, IContactUsMessageContainer
{
    public readonly string Title = title;
    public string Body { get; private set; } = body;
    public readonly string Teaser = teaser;
    public readonly string MetaDescription = metaDescription;
    public readonly IEnumerable<ProcessedSection> Sections = sections;
    public readonly string Icon = icon;
    public readonly string BackgroundImage = backgroundImage;
    public readonly string Image = image;
    public readonly string AltText = altText;
    public readonly IEnumerable<Crumb> Breadcrumbs = breadcrumbs;
    public readonly IEnumerable<Alert> Alerts = alerts;
    public readonly Topic ParentTopic = topic;
    public readonly string NavigationLink = TypeRoutes.GetUrlFor("article", slug);
    public readonly IEnumerable<Alert> AlertsInline = alertsInline;
    public DateTime UpdatedAt = updatedAt;
    public bool HideLastUpdated = hideLastUpdated;
    public List<TrustedLogo> TrustedLogos = trustedLogos;
    public string LogoAreaTitle = logoAreaTitle;
    public IEnumerable<SubItem> RelatedContent = relatedContent;
    public string Author = author;
    public string Photographer = photographer;
    public DateTime PublishedOn = publishedOn;
    public readonly IEnumerable<InlineQuote> InlineQuotes = inlineQuotes;
    public List<Event> Events = events;
    public string ContentfulId = contentfulId;

    public void AddContactUsMessage(string message, string slug = "")
    {
        if (string.IsNullOrEmpty(slug))
            AddMessageToArticleBodyOrFirstSection(message);
        else
            AddMessageToArticleSectionWithMatchingSlug(slug, message);
    }

    private void AddMessageToArticleSectionWithMatchingSlug(string slug, string htmlMessage)
    {
        ProcessedSection section = Sections?.ToList().Find(_ => _.Slug.Equals(slug));

        if (section is not null)
            section.Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(section.Body, htmlMessage);
    }

    private void AddMessageToArticleBodyOrFirstSection(string htmlMessage)
    {
        MatchCollection matches = ContactUsTagParser.ContactUsMessageTagRegex.Matches(Body);
        
        if (matches.Count > 0)
            Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(Body, htmlMessage);
        else if (Sections is not null && Sections.ToList().Count > 0)
        {
            ProcessedSection section = Sections.ToList().First();
            section.Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(section.Body, htmlMessage);
        }
    }
}