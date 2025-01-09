namespace StockportWebapp.Models;

[ExcludeFromCodeCoverage]
public class Article(string title,
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
    public string Title { get; set; } = title;
    public string Slug { get; set; } = slug;
    public string Body { get; set; } = body;
    public string Teaser { get; set; } = teaser;
    public string MetaDescription { get; set; } = metaDescription;
    public IEnumerable<Section> Sections { get; set; } = sections;
    public string Icon { get; set; } = icon;
    public string BackgroundImage { get; set; } = backgroundImage;
    public string Image { get; set; } = image;
    public string AltText { get; set; } = altText;
    public IEnumerable<Crumb> Breadcrumbs { get; set; } = breadcrumbs;
    public IEnumerable<Alert> Alerts { get; set; }
    public IEnumerable<Profile> Profiles { get; set; } = profiles;
    public Topic ParentTopic { get; set; }
    public IEnumerable<Document> Documents { get; set; } = documents;
    public IEnumerable<Alert> AlertsInline { get; set; } = alertsInline;
    public IEnumerable<PrivacyNotice> PrivacyNotices { get; set; }
    public DateTime UpdatedAt { get; set; } = updatedAt;
    public bool HideLastUpdated { get; set; } = hideLastUpdated;
    public List<GroupBranding> ArticleBranding { get; init; } = articleBranding;
    public string LogoAreaTitle { get; } = logoAreaTitle;
    public IEnumerable<SubItem> RelatedContent { get; set; } = relatedContent;
    public string Author { get; } = author;
    public string Photographer { get; } = photographer;
    public DateTime PublishedOn { get; set; } = publishedOn;
    public IEnumerable<InlineQuote> InlineQuotes { get; set; } = inlineQuotes;
    public string AssociatedTagCategory = associatedTagCategory;
    public List<Event> Events = events;
}