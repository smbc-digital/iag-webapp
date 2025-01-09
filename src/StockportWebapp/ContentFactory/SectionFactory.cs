namespace StockportWebapp.ContentFactory;

public interface ISectionFactory
{
    ProcessedSection Build(Section section, string articleTitle);
}

public class SectionFactory(ITagParserContainer tagParserContainer,
                            MarkdownWrapper markdownWrapper,
                            IRepository repository) : ISectionFactory
{
    private readonly MarkdownWrapper _markdownWrapper = markdownWrapper;
    private readonly ITagParserContainer _tagParserContainer = tagParserContainer;
    private readonly IRepository _repository = repository;

    public ProcessedSection Build(Section section, string articleTitle = null)
    {
        string parsedBody = _markdownWrapper.ConvertToHtml(section.Body ?? "");

        if (section.Body.Contains("PrivacyNotice:"))
            section.PrivacyNotices = GetPrivacyNotices().Result;

        parsedBody = _tagParserContainer.ParseAll(parsedBody, articleTitle, true, section.AlertsInline, section.Documents, null, section.PrivacyNotices, section.Profiles, true);

        return new ProcessedSection(
            section.Title,
            section.Slug,
            section.MetaDescription,
            parsedBody,
            section.Profiles,
            section.Documents,
            section.AlertsInline,
            section.SectionBranding,
            section.LogoAreaTitle,
            section.UpdatedAt
        );
    }

    private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
    {
        HttpResponse response = await _repository.Get<List<PrivacyNotice>>();
        
        return response.Content as List<PrivacyNotice>;
    }
}