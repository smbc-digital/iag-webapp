using StockportWebapp.Models;
using StockportWebapp.ProcessedModels;
using StockportWebapp.Utils;

namespace StockportWebapp.ContentFactory
{
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

            return new ProcessedHomepage(homepage.PopularSearchTerms, homepage.FeaturedTasksHeading, homepage.FeaturedTasksSummary, homepage.FeaturedTasks, homepage.FeaturedTopics, homepage.Alerts, homepage.CarouselContents, homepage.BackgroundImage, homepage.LastNews, freeText, homepage.FeaturedGroup);
        }   
    }
}
