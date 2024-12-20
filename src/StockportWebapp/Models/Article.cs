namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Article
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Body { get; set; }
    public string Teaser { get; set; }
    public string MetaDescription { get; set; }
    public IEnumerable<Section> Sections { get; set; }
    public string Icon { get; set; }
    public string BackgroundImage { get; set; }
    public string Image { get; set; }
    public string AltText { get; set; }
    public IEnumerable<Crumb> Breadcrumbs { get; set; }
    public IEnumerable<Alert> Alerts { get; set; }
    public IEnumerable<Profile> Profiles { get; set; }
    public Topic ParentTopic { get; set; }
    public IEnumerable<Document> Documents { get; set; }
    public IEnumerable<Alert> AlertsInline { get; set; }
    public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool HideLastUpdated { get; set; }
    public List<GroupBranding> ArticleBranding { get; init; }
    public string LogoAreaTitle { get; }
    public IEnumerable<SubItem> RelatedContent { get; set; }
    public string Author { get; }
    public string Photographer { get; }
    public DateTime PublishedOn { get; set; }
    public IEnumerable<InlineQuote> InlineQuotes { get; set; }
    public string AssociatedTagCategory;
    public List<Event> Events;

    public Article(string title,
                string slug,
                string body,
                string teaser,
                string metaDescription,
                IEnumerable<Section> sections,
                string icon,
                string backgroundImage,
                string image,
                string altText,
                IEnumerable<Crumb> breadcrumbs,
                IEnumerable<Profile> profiles,
                IEnumerable<Document> documents,
                IEnumerable<Alert> alertsInline,
                DateTime updatedAt,
                bool hideLastUpdated,
                List<GroupBranding> articleBranding,
                string logoAreaTitle,
                IEnumerable<SubItem> relatedContent,
                string author,
                string photographer,
                DateTime publishedOn,
                IEnumerable<InlineQuote> inlineQuotes,
                string associatedTagCategory,
                List<Event> events)
    {
        Title = title;
        Slug = slug;
        Body = body;
        Teaser = teaser;
        MetaDescription = metaDescription;
        Sections = sections;
        Icon = icon;
        BackgroundImage = backgroundImage;
        Image = image;
        AltText = altText;
        Breadcrumbs = breadcrumbs;
        Profiles = profiles;
        Documents = documents;
        AlertsInline = alertsInline;
        UpdatedAt = updatedAt;
        HideLastUpdated = hideLastUpdated;
        ArticleBranding = articleBranding;
        LogoAreaTitle = logoAreaTitle;
        RelatedContent = relatedContent;
        Author = author;
        Photographer = photographer;
        PublishedOn = publishedOn;
        InlineQuotes = inlineQuotes;
        AssociatedTagCategory = associatedTagCategory;
        Events = events;
    }
}