using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "article")]
    class ArticleSteps : UiTestBase
    {
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "right side bar":
                    result = BrowserSession.FindCss(".l-right-side-bar").Exists();
                    break;
                case "heading":
                    result = BrowserSession.FindCss("h1").Exists();
                    break;
                case "article navigation":
                    result = BrowserSession.FindCss(".article-navigation-header").Exists();
                    break;
                case "article body":
                    result = BrowserSession.FindAllCss("article").Any();
                    break;
                case "next page":
                    result = BrowserSession.FindCss(".article-pagination").Exists();
                    break;
                case "video":
                    result = BrowserSession.FindAllCss("[id*=buto_]").Any();
                    break;
                case "youtube video":
                    result = BrowserSession.FindCss("iframe[src*='youtube']").Exists();
                    break;
                case "table":
                    result = BrowserSession.FindCss("table").Exists();
                    break;
            }
            Assert.True(result);
        }
    }
}
