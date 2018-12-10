using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "events")]
    class EventsSteps : UiTestBase
    {
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "events header":
                    result = BrowserSession.FindCss("h1").Exists();
                    break;
                case "whats on form":
                    result = BrowserSession.FindCss("form#events-filter-bar-container").Exists();
                    break;
                case "event listings":
                    result = BrowserSession.FindCss("#event-listing-container").Exists() && BrowserSession.FindAllCss(".primary-topics .featured-topic-list .featured-topic").Any();
                    break;
                case "generic event listings":
                    result = BrowserSession.FindCss(".generic-list-see-more-container").Exists()
                             && BrowserSession.FindAllCss(".primary-topics .featured-topic-list .featured-topic").Any()
                             && BrowserSession.FindAllCss(".generic-list-see-more-container:not[style*='display:none']").Any();
                    break;
            }
            Assert.True(result);
        }
    }
}
