using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "atoz")]
    public class AToZSteps : UiTestBase
    {
        IWebDriver chromeDriver = new ChromeDriver();

        [Then(@"I should see a link to an article")]
        public void ThenIShouldSeeALinkToAnArticle()
        {
            //Assert.True(BrowserSession.FindLink("Benefits & Support").Exists());
            ReadOnlyCollection<IWebElement> linkElements = (ReadOnlyCollection<IWebElement>)chromeDriver.FindElement(By.LinkText("Link Text"));
            bool linkExists = linkElements.Any();
            Assert.False(linkExists);
        }

        [Then(@"I should see No Results Found")]
        public void ThenIShouldSeeNoResultsFound()
        {
            Assert.True(BrowserSession.FindCss(".l-article-container p").HasContent("No results found"));
        }

    }
}
