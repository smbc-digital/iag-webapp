using Profile = StockportWebapp.Models.Profile;

namespace StockportWebapp.ContentFactory;

public class ArticleFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly IDynamicTagParser<Profile> _profileTagParser; // check if this is used
    private readonly IDynamicTagParser<Alert> _alertsInlineTagParser; // check if this is used
    private readonly ISectionFactory _sectionFactory;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly IDynamicTagParser<Document> _documentTagParser; // check if this is used
    private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser; // check if this is used
    private readonly IRepository _repository;

    public ArticleFactory(ITagParserContainer tagParserContainer, IDynamicTagParser<Profile> profileTagParser, ISectionFactory sectionFactory, MarkdownWrapper markdownWrapper,
        IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IRepository repository)
    {
        _tagParserContainer = tagParserContainer;
        _sectionFactory = sectionFactory;
        _markdownWrapper = markdownWrapper;
        _profileTagParser = profileTagParser;
        _documentTagParser = documentTagParser;
        _alertsInlineTagParser = alertsInlineTagParser;
        _privacyNoticeTagParser = privacyNoticeTagParser;
        _repository = repository;
    }

    public virtual ProcessedArticle Build(Article article)
    {
        var processedSections = new List<ProcessedSection>();
        foreach (var section in article.Sections)
        {
            processedSections.Add(_sectionFactory.Build(section, article.Title));
        }

        var body = _markdownWrapper.ConvertToHtml(article.Body ?? "");
        if (body.Contains("PrivacyNotice:"))
            article.PrivacyNotices = GetPrivacyNotices().Result;

        body = _tagParserContainer.ParseAll(body, article.Title, true, article.AlertsInline, article.Documents, null, article.PrivacyNotices, article.Profiles);

        return new ProcessedArticle(article.Title, article.Slug, body, article.Teaser, article.MetaDescription,
            processedSections, article.Icon, article.BackgroundImage, article.Image, article.Breadcrumbs, article.Alerts, article.ParentTopic, article.AlertsInline, article.UpdatedAt, article.HideLastUpdated);
    }

    private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
    {
        var response = await _repository.Get<List<PrivacyNotice>>();
        return response.Content as List<PrivacyNotice>;
    }
}