﻿namespace StockportWebapp.ContentFactory;

public interface ISectionFactory
{
    ProcessedSection Build(Section section, string articleTitle);
}

public class SectionFactory : ISectionFactory
{
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly ITagParserContainer _tagParserContainer;
    private readonly IDynamicTagParser<Models.Profile> _profileTagParser;
    private readonly IDynamicTagParser<Document> _documentTagParser;
    private readonly IDynamicTagParser<Alert> _alertsInlineTagParser;
    private readonly IDynamicTagParser<PrivacyNotice> _privacyNoticeTagParser;
    private readonly IRepository _repository;


    public SectionFactory(ITagParserContainer tagParserContainer, IDynamicTagParser<StockportWebapp.Models.Profile> profileTagParser, MarkdownWrapper markdownWrapper,
        IDynamicTagParser<Document> documentTagParser, IDynamicTagParser<Alert> alertsInlineTagParser, IDynamicTagParser<PrivacyNotice> privacyNoticeTagParser, IRepository repository)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
        _profileTagParser = profileTagParser;
        _documentTagParser = documentTagParser;
        _alertsInlineTagParser = alertsInlineTagParser;
        _privacyNoticeTagParser = privacyNoticeTagParser;
        _repository = repository;
    }

    public ProcessedSection Build(Section section, string articleTitle = null)
    {
        var parsedBody = _markdownWrapper.ConvertToHtml(section.Body);
        parsedBody = _profileTagParser.Parse(parsedBody, section.Profiles);
        parsedBody = _documentTagParser.Parse(parsedBody, section.Documents);
        parsedBody = _alertsInlineTagParser.Parse(parsedBody, section.AlertsInline);

        if (section.Body.Contains("PrivacyNotice:"))
        {
            section.PrivacyNotices = GetPrivacyNotices().Result;
            parsedBody = _privacyNoticeTagParser.Parse(parsedBody, section.PrivacyNotices);
        }

        parsedBody = _tagParserContainer.ParseAll(parsedBody, articleTitle);

        return new ProcessedSection(
            section.Title,
            section.Slug,
            section.MetaDescription,
            parsedBody,
            section.Profiles,
            section.Documents,
            section.AlertsInline
        );
    }

    private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
    {
        var response = await _repository.Get<List<PrivacyNotice>>();
        return response.Content as List<PrivacyNotice>;
    }
}
