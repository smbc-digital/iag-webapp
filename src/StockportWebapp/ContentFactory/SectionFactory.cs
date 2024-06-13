namespace StockportWebapp.ContentFactory;

public interface ISectionFactory
{
    ProcessedSection Build(Section section, string articleTitle);
}

public class SectionFactory : ISectionFactory
{
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly ITagParserContainer _tagParserContainer;
    private readonly IRepository _repository;


    public SectionFactory(ITagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper, IRepository repository)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
        _repository = repository;
    }

    public ProcessedSection Build(Section section, string articleTitle = null)
    {
        string parsedBody = _markdownWrapper.ConvertToHtml(section.Body ?? "");

        if (section.Body.Contains("PrivacyNotice:"))
            section.PrivacyNotices = GetPrivacyNotices().Result;

        parsedBody = _tagParserContainer.ParseAll(parsedBody, articleTitle, true, section.AlertsInline, section.Documents, null, section.PrivacyNotices, section.Profiles);

        return new ProcessedSection(
            section.Title,
            section.Slug,
            section.MetaDescription,
            parsedBody,
            section.Profiles,
            section.Documents,
            section.AlertsInline,
            section.SectionBranding,
            section.LogoAreaTitle
        );
    }

    private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
    {
        HttpResponse response = await _repository.Get<List<PrivacyNotice>>();
        return response.Content as List<PrivacyNotice>;
    }
}