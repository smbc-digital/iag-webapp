namespace StockportWebapp.ContentFactory;

public class HomepageFactory
{
    private readonly MarkdownWrapper _markdownWrapper;

    public HomepageFactory(MarkdownWrapper markdownWrapper)
    {
        _markdownWrapper = markdownWrapper;
    }

    public virtual ProcessedHomepage Build(Homepage homepage)
    {
        string freeText = _markdownWrapper.ConvertToHtml(homepage.FreeText ?? string.Empty);

        string featuredTasksSummary = _markdownWrapper.ConvertToHtml(homepage.FeaturedTasksSummary);

        string imageOverlayText = _markdownWrapper.ConvertToHtml(homepage.ImageOverlayText ?? string.Empty);

        return new ProcessedHomepage(homepage.Title,
            homepage.PopularSearchTerms,
            homepage.FeaturedTasksHeading,
            featuredTasksSummary,
            homepage.FeaturedTasks,
            homepage.FeaturedTopics,
            homepage.Alerts,
            homepage.CarouselContents,
            homepage.BackgroundImage,
            homepage.ForegroundImage,
            homepage.ForegroundImageLocation,
            homepage.ForegroundImageLink,
            homepage.ForegroundImageAlt,
            homepage.LastNews,
            freeText,
            homepage.FeaturedGroup,
            homepage.EventCategory,
            homepage.MetaDescription,
            homepage.CampaignBanner,
            homepage.CallToAction,
            homepage.CallToActionPrimary,
            homepage.SpotlightOnBanner,
            imageOverlayText);
    }
}