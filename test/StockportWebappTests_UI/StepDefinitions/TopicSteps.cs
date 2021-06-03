namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "topic")]
    public class TopicSteps : UiTestBase
    {
        [Then("I should see Primary links")]
        public void ThenIShouldSeePrimaryLinks()
        {
            Assert.True(BrowserSession.FindCss(".primary-items").Exists());
        }
        
        [Then("I should see Secondary links")]
        public void ThenIShouldSeeSecondaryLinks()
        {
            Assert.True(BrowserSession.FindCss(".subitems-secondary-two-column").Exists());
        }
        
        [Then("I should see Tertiary links")]
        public void ThenIShouldSeeTertiaryLinks()
        {
            Assert.True(BrowserSession.FindCss(".subitems-tertiary-two-column").Exists());
        }
        
        [Then("I should see a topic page alert")]
        public void ThenIShouldSeeATopicPageAlert()
        {
            Assert.True(BrowserSession.FindCss(".alert-error").Exists());
        }
        
        [Then("I should see an advertisment banner")]
        public void ThenIShouldSeeAnAdvertismentBanner()
        {
            Assert.True(BrowserSession.FindCss(".advertisement-container").Exists());
        }
        
        [Then("I should see an email alerts link")]
        public void ThenIShouldSeeAnEmailAlertsLink()
        {
            Assert.True(BrowserSession.FindLink("Email alerts").Exists());
        }
        
        [Then("I should see an events banner")]
        public void ThenIShouldSeeAnEventsBanner()
        {
            Assert.True(BrowserSession.FindCss(".event-banner").Exists());
        }

        [Then("The alert should no longer be visible")]
        public void ThenTheAlertShouldNoLongerBeVisible()
        {
            Assert.False(BrowserSession.FindCss(".alert").Exists());
        }
    }
}
