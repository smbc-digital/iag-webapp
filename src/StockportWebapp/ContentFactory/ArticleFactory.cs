﻿namespace StockportWebapp.ContentFactory;

public class ArticleFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly ISectionFactory _sectionFactory;
    private readonly MarkdownWrapper _markdownWrapper;
    private readonly IRepository _repository;

    public ArticleFactory(ITagParserContainer tagParserContainer, ISectionFactory sectionFactory, MarkdownWrapper markdownWrapper, IRepository repository)
    {
        _tagParserContainer = tagParserContainer;
        _sectionFactory = sectionFactory;
        _markdownWrapper = markdownWrapper;
        _repository = repository;
    }

    public virtual ProcessedArticle Build(Article article)
    {
        List<ProcessedSection> processedSections = article.Sections.Select(section => _sectionFactory.Build(section, article.Title)).ToList();
        string body = _markdownWrapper.ConvertToHtml(article.Body ?? string.Empty);
        if (body.Contains("PrivacyNotice:"))
            article.PrivacyNotices = GetPrivacyNotices().Result;

        body = _tagParserContainer.ParseAll(body ?? string.Empty, article.Title, true, article.AlertsInline, article.Documents, article.InlineQuotes, article.PrivacyNotices, article.Profiles, true);

        return new ProcessedArticle(article.Title,
                                    article.Slug,
                                    body,
                                    article.Teaser,
                                    article.MetaDescription,
                                    processedSections,
                                    article.Icon,
                                    article.BackgroundImage,
                                    article.Image,
                                    article.AltText,
                                    article.Breadcrumbs,
                                    article.Alerts,
                                    article.ParentTopic,
                                    article.AlertsInline,
                                    article.UpdatedAt,
                                    article.HideLastUpdated,
                                    article.ArticleBranding,
                                    article.LogoAreaTitle,
                                    article.RelatedContent,
                                    article.Author,
                                    article.Photographer,
                                    article.PublishedOn,
                                    article.InlineQuotes,
                                    article.Events);
    }

    private async Task<IEnumerable<PrivacyNotice>> GetPrivacyNotices()
    {
        HttpResponse response = await _repository.Get<List<PrivacyNotice>>();
        return response.Content as List<PrivacyNotice>;
    }
}