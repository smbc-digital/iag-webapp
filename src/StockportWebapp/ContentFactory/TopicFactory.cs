namespace StockportWebapp.ContentFactory;

public class TopicFactory
{
    private readonly ITagParserContainer _tagParserContainer;
    private readonly MarkdownWrapper _markdownWrapper;

    public TopicFactory(ITagParserContainer tagParserContainer, MarkdownWrapper markdownWrapper)
    {
        _tagParserContainer = tagParserContainer;
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedTopic Build(Topic topic)
    {
        string summary = _markdownWrapper.ConvertToHtml(topic.Summary ?? string.Empty);
        summary = _tagParserContainer.ParseAll(summary, topic.Name);

        // 02/11 Hotfix to maintain backwards compatibility 
        EventBanner oldEventBanner = null;
        if (topic.EventBanner is not null)
            oldEventBanner = new (topic.EventBanner.Title, topic.EventBanner.Teaser, topic.EventBanner.Icon, topic.EventBanner.Link);

        return new ProcessedTopic(topic.Name,
                                topic.Slug,
                                summary,
                                topic.Teaser,
                                topic.MetaDescription,
                                topic.Icon,
                                topic.BackgroundImage,
                                topic.Image,
                                topic.FeaturedTasks,
                                topic.SubItems,
                                topic.SecondaryItems,
                                topic.Breadcrumbs,
                                topic.Alerts,
                                topic.EmailAlerts,
                                topic.EmailAlertsTopicId,
                                oldEventBanner,
                                topic.EventBanner,
                                topic.DisplayContactUs,
                                topic.CampaignBanner,
                                topic.EventCategory,
                                topic.CallToAction,
                                topic.TopicBranding,
                                topic.LogoAreaTitle)
        {
            TriviaSection = topic.TriviaSection,
            Video = !string.IsNullOrEmpty(topic.Video.VideoEmbedCode) ?
                new Video(topic.Video.Heading, topic.Video.Text, _tagParserContainer.ParseAll(topic.Video.VideoEmbedCode)) : null,
            CallToAction = topic.CallToAction
        };
    }
}