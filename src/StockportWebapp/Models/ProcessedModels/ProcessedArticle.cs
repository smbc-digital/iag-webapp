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
    private string v1;
    private string v2;
    private string defaultBody;
    private string v3;
    private string v4;
    private List<ProcessedSection> processedSections;
    private string v5;
    private string v6;
    private string v7;
    private string v8;
    private List<Crumb> crumbs;
    private List<Alert> alerts1;
    private Topic topic;
    private List<Alert> alerts2;
    private DateTime dateTime;
    private bool v9;
    private object value1;
    private string v10;
    private object value2;

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

    public ProcessedArticle(string v1, string v2, string defaultBody, string v3, string v4, List<ProcessedSection> processedSections, string v5, string v6, string v7, string v8, List<Crumb> crumbs, List<Alert> alerts1, Topic topic, List<Alert> alerts2, DateTime dateTime, bool v9, object value1, string v10, object value2)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.defaultBody = defaultBody;
        this.v3 = v3;
        this.v4 = v4;
        this.processedSections = processedSections;
        this.v5 = v5;
        this.v6 = v6;
        this.v7 = v7;
        this.v8 = v8;
        this.crumbs = crumbs;
        this.alerts1 = alerts1;
        this.topic = topic;
        this.alerts2 = alerts2;
        this.dateTime = dateTime;
        this.v9 = v9;
        this.value1 = value1;
        this.v10 = v10;
        this.value2 = value2;
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