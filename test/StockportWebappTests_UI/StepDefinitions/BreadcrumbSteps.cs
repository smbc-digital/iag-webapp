namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class BreadcrumbSteps : UiTestBase
    {
        [Then(@"I shouldn't see any breadcrumbs")]
        public void ThenIShouldnTSeeAnyBreadcrumbs()
        {
            Assert.False(BrowserSession.FindCss(".breadcrumb-container").Exists());
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
