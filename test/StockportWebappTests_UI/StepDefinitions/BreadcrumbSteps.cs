using OpenQA.Selenium.Chrome;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class BreadcrumbSteps : UiTestBase
    {
        private ChromeDriver chromeDriver;

        [Then(@"I shouldn't see any breadcrumbs")]
        public void ThenIShouldnTSeeAnyBreadcrumbs()
        {
            //Assert.False(BrowserSession.FindCss(".breadcrumb-container").Exists());
            bool test = chromeDriver.FindElements(By.ClassName("breadcrumb-container")).Count > 0;
            Assert.False(test);
        }
        
        [Then(@"I should see topic breadcrumbs")]
        public void ThenIShouldSeeTopicBreadcrumbs()
         {
            Assert.True(BrowserSession.FindCss(".breadcrumb-container").Exists());
            Assert.True(BrowserSession.FindCss(".breadcrumb-container .current").HasContent("UITEST: Hat Works"));
        }
        
        [Then(@"I should see article breadcrumbs")]
        public void ThenIShouldSeeArticleBreadcrumbs()
        {
            Assert.True(BrowserSession.FindCss(".breadcrumb-container").Exists());
            Assert.True(BrowserSession.FindCss(".breadcrumb-container .current").HasContent("UITEST: About the Hat Works"));
        }
    }
}
