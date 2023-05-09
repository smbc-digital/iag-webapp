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
        var freeText = _markdownWrapper.ConvertToHtml(homepage.FreeText ?? "");

        var featuredTasksSummary = _markdownWrapper.ConvertToHtml(homepage.FeaturedTasksSummary);

        return new ProcessedHomepage(homepage.PopularSearchTerms, homepage.FeaturedTasksHeading, featuredTasksSummary, homepage.FeaturedTasks, homepage.FeaturedTopics, homepage.Alerts, homepage.CarouselContents, homepage.BackgroundImage, homepage.LastNews, freeText, homepage.FeaturedGroup, homepage.EventCategory, homepage.MetaDescription, homepage.CampaignBanner);
    }
}
