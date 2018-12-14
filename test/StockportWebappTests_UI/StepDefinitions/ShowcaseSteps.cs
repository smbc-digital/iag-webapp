using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class ShowcaseSteps : UiTestBase
    {
        [Then(@"I should see the alerts section")]
        public void ThenIShouldSeeTheAlertsSection()
        {
            Assert.True(BrowserSession.FindAllCss(".global-alert-information").Any());
        }

        [Then(@"I should see the primary items section")]
        public void ThenIShouldSeeThePrimaryItemsSection()
        {
            Assert.True(BrowserSession.FindAllCss(".showcase-primary-item-container").Any());
        }

        [Then(@"I should see the featured items section")]
        public void ThenIShouldSeeTheFeaturedItemsSection()
        {
            Assert.True(BrowserSession.FindCss(".featured-topic-list").Exists());
        }

        [Then(@"I should see the consultations section")]
        public void ThenIShouldSeeTheConsultationsSection()
        {
            Assert.True(BrowserSession.FindLink("View previous consultations").Exists());
        }

        [Then(@"I should see the social media links section")]
        public void ThenIShouldSeeTheSocialMediaLinksSection()
        {
            Assert.True(BrowserSession.FindCss(".showcase-social-media").Exists());
        }

        [Then(@"I should see the events section")]
        public void ThenIShouldSeeTheEventsSection()
        {
            Assert.True(BrowserSession.FindCss(".showcase-event-content").Exists());
        }

        [Then(@"I should see the profile section")]
        public void ThenIShouldSeeTheProfileSection()
        {
            Assert.True(BrowserSession.FindCss(".profile").Exists());
        }

        [Then(@"I should see the key facts section")]
        public void ThenIShouldSeeTheKeyFactsSection()
        {
            Assert.True(BrowserSession.FindCss(".key-facts").Exists());
        }

        [Then(@"I should see the body section")]
        public void ThenIShouldSeeTheBodySection()
        {
            Assert.True(BrowserSession.FindAllCss(".showcase-body").Any());
        }
    }
}
