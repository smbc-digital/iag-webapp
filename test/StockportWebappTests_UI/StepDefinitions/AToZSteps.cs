using System;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "atoz")]
    public class AToZSteps : UiTestBase
    {
        [Then(@"I should see a link to an article")]
        public void ThenIShouldSeeALinkToAnArticle()
        {
            Assert.True(BrowserSession.FindLink("Benefits & Support").Exists());
        }

        [Then(@"I should see No Results Found")]
        public void ThenIShouldSeeNoResultsFound()
        {
            Assert.True(BrowserSession.FindCss(".l-article-container p").HasContent("No results found"));
        }

    }
}
