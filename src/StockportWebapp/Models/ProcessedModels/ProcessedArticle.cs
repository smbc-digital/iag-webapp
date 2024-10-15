namespace StockportWebapp.Models.ProcessedModels;
public interface IContactUsMessageContainer
{
    void AddContactUsMessage(string message, string slug = "");
}

public class ProcessedArticle : IProcessedContentType, IContactUsMessageContainer
{
    public readonly string Title;
    public string Body { get; private set; }
    public readonly string Teaser;
    public readonly string MetaDescription;
    public readonly IEnumerable<ProcessedSection> Sections;
    public readonly string Icon;
    public readonly string BackgroundImage;
    public readonly string Image;
    public readonly string AltText;
    public readonly IEnumerable<Crumb> Breadcrumbs;
    public readonly IEnumerable<Alert> Alerts;
    public readonly Topic ParentTopic;
    public readonly string NavigationLink;
    public readonly IEnumerable<Alert> AlertsInline;
    public DateTime UpdatedAt;
    public bool HideLastUpdated;
    public List<GroupBranding> ArticleBranding;
    public string LogoAreaTitle;
    public IEnumerable<SubItem> RelatedContent;
    public string Author;
    public string Photographer;
    public DateTime PublishedOn;

    public ProcessedArticle(string title,
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
                            List<GroupBranding> articleBranding,
                            string logoAreaTitle,
                            IEnumerable<SubItem> relatedContent,
                            string author,
                            string photographer,
                            DateTime publishedOn)
    {
        Title = title;
        NavigationLink = TypeRoutes.GetUrlFor("article", slug);
        Body = body;
        Teaser = teaser;
        MetaDescription = metaDescription;
        Sections = sections;
        Icon = icon;
        BackgroundImage = backgroundImage;
        Image = image;
        AltText = altText;
        Breadcrumbs = breadcrumbs;
        Alerts = alerts;
        ParentTopic = topic;
        AlertsInline = alertsInline;
        UpdatedAt = updatedAt;
        HideLastUpdated = hideLastUpdated;
        ArticleBranding = articleBranding;
        LogoAreaTitle = logoAreaTitle;
        RelatedContent = relatedContent;
        Author = author;
        Photographer = photographer;
        PublishedOn = publishedOn;
    }

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
        {
            Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(Body, htmlMessage);
        }
        else if (Sections is not null && Sections.ToList().Count > 0)
        {
            ProcessedSection section = Sections.ToList().First();
            section.Body = ContactUsTagParser.ContactUsMessageTagRegex.Replace(section.Body, htmlMessage);
        }
    }
}