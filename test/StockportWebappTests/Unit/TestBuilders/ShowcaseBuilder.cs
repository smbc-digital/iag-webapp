namespace StockportWebappTests_Unit.Unit.TestBuilders;

public class ShowcaseBuilder
{
    private string _title = "title";
    private string _slug = "showcase_slug";
    private string _teaser = "teaser";
    private string _metaDescription = "metaDescription";
    private string _subheading = "subheading";
    private string _eventSubheading = "event subheading";
    private string _eventCategory = "event category";
    private readonly string _eventCategoryOrtag = "event category or tag";
    private readonly string _newsSubheading = "news subheading";
    private readonly string _newsCategory = "news category";
    private readonly string _newsCategoryOrTag = string.Empty;
    private string _bodySubheading = "body subheading";
    private string _body = "body";
    private string _heroImageUrl = "image-url.jpg";
    private string EmailAlertsTopicId { get; set; } = "alertId";
    private string EmailAlertsText { get; set; } = "alertText";
    private readonly string _showcaseIcon = "fa-icon";
    private readonly CallToActionBanner _callToActionBanner = new();
    private IEnumerable<Crumb> _breadcrumbs = new List<Crumb>() { new("link", "title", "type") };
    private IEnumerable<SocialMediaLink> _socialMediaLinks = new List<SocialMediaLink>() { new("title", "slug", "url", "icon", "accountName", "screenreader") };
    private IEnumerable<Event> _events = new List<Event>();
    private readonly IEnumerable<Alert> alerts = new List<Alert> {new("title", "subHeading", "body", Severity.Information, new DateTime(0001, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                                                             new DateTime(9999, 9, 9, 0, 0, 0, DateTimeKind.Utc), string.Empty, false, string.Empty)};
    public string _profileHeading { get; set; }
    public string _profileLink { get; set; }
    public string _triviaSubheading { get; set; }
    public string _socialMediaLinksSubheading { get; set; } = "";
    public string _eventsReadMoreText { get; set; } = "";
    public List<Trivia> _triviaSection { get; set; }
    public string _featuredItemsSubheading { get; set; } = "";
    public IEnumerable<SubItem> _featuredItems = new List<SubItem>()
    {
        new("slug", "title", "teaser", "icon", "type", "contentType", "image.jpg", string.Empty, string.Empty, new List<SubItem>() {new("slug", "title", "teaser", "icon", "type", "contentType", "image.jpg", string.Empty, string.Empty, new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Blue, string.Empty, string.Empty) }, string.Empty, string.Empty, EColourScheme.Blue, string.Empty, string.Empty)
    };

    private readonly IEnumerable<SubItem> _secondaryItems = new List<SubItem>()
    {
        new("slug", "title", "teaser", "icon", "type", "contentType", "image.jpg", string.Empty, string.Empty, new List<SubItem>() {new("slug", "title", "teaser", "icon", "type", "contentType", "image.jpg", string.Empty, string.Empty, new List<SubItem>(), string.Empty, string.Empty, EColourScheme.Blue, string.Empty, string.Empty) }, string.Empty, string.Empty,EColourScheme.Blue, string.Empty, string.Empty)
    };

    public Showcase Build() => new()
    {
        Title = _title,
        Slug = _slug,
        Teaser = _teaser,
        MetaDescription = _metaDescription,
        Subheading = _subheading,
        EventCategory = _eventCategory,
        EventsCategoryOrTag = _eventCategoryOrtag,
        EventSubheading = _eventSubheading,
        NewsSubheading = _newsSubheading,
        NewsCategoryTag = _newsCategory,
        NewsCategoryOrTag = _newsCategoryOrTag,
        BodySubheading = _bodySubheading,
        Body = _body,
        HeroImageUrl = _heroImageUrl,
        Breadcrumbs = _breadcrumbs,
        SecondaryItems = _secondaryItems,
        SocialMediaLinksSubheading = _socialMediaLinksSubheading,
        SocialMediaLinks = _socialMediaLinks,
        Events = _events,
        EmailAlertsTopicId = EmailAlertsTopicId,
        EmailAlertsText = EmailAlertsText,
        Alerts = alerts,
        FeaturedItemsSubheading = _featuredItemsSubheading,
        FeaturedItems = _featuredItems,
        Profile = new Profile(),
        ProfileHeading = _profileHeading,
        ProfileLink = _profileLink,
        Profiles = new List<Profile>(),
        FieldOrder = new FieldOrder(),
        Icon = _showcaseIcon,
        TriviaSubheading = _triviaSubheading,
        TriviaSection = _triviaSection,
        CallToActionBanner = _callToActionBanner,
        EventsReadMoreText = _eventsReadMoreText,
        Video = new Video(),
        NewsArticle = null,
        PrimaryItems = null
    };

    public ShowcaseBuilder Title(string title)
    {
        _title = title;
        return this;
    }

    public ShowcaseBuilder Slug(string slug)
    {
        _slug = slug;
        return this;
    }

    public ShowcaseBuilder Teaser(string teaser)
    {
        _teaser = teaser;
        return this;
    }
    public ShowcaseBuilder MetaDescription(string metaDescription)
    {
        _metaDescription = metaDescription;
        return this;
    }

    public ShowcaseBuilder Subheading(string subheading)
    {
        _subheading = subheading;
        return this;
    }

    public ShowcaseBuilder EventSubheading(string subheading)
    {
        _eventSubheading = subheading;
        return this;
    }

    public ShowcaseBuilder EventCategory(string category)
    {
        _eventCategory = category;
        return this;
    }

    public ShowcaseBuilder BodySubheading(string subheading)
    {
        _bodySubheading = subheading;
        return this;
    }

    public ShowcaseBuilder Body(string body)
    {
        _body = body;
        return this;
    }

    public ShowcaseBuilder HeroImageUrl(string heroImageUrl)
    {
        _heroImageUrl = heroImageUrl;
        return this;
    }

    public ShowcaseBuilder Breadcrumbs(IEnumerable<Crumb> breadcrumbs)
    {
        _breadcrumbs = breadcrumbs;
        return this;
    }

    public ShowcaseBuilder FeaturedItems(IEnumerable<SubItem> featuredItems)
    {
        _featuredItems = featuredItems;
        return this;
    }

    public ShowcaseBuilder SocialMediaLinks(IEnumerable<SocialMediaLink> socialMediaLinks)
    {
        _socialMediaLinks = socialMediaLinks;
        return this;
    }

    public ShowcaseBuilder Events(IEnumerable<Event> events)
    {
        _events = events;
        return this;
    }
}