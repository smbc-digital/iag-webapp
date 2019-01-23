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
            Assert.True(BrowserSession.FindAllCss(".alert-container").Any());
        }

        [Then(@"I should see the hero image")]
        public void ThenIShouldSeeTheHeroImage()
        {
            Assert.True(BrowserSession.FindCss(".hero-image").Exists());
        }

        [Then(@"I should see the primary items section")]
        public void ThenIShouldSeeThePrimaryItemsSection()
        {
            Assert.True(BrowserSession.FindAllCss(".hero-image .card-list-container .icon-card").Any());
        }

        [Then(@"I should see the news section")]
        public void IShoulSeeTheNewsSection()
        {
            Assert.True(BrowserSession.FindCss(".news-card-container").Exists());
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
            Assert.True(BrowserSession.FindCss(".showcase-profile").Exists());
        }

        [Then(@"I should see multiple profiles")]
        public void IShouldSeeMultipleProfiles()
        {
            Assert.True(BrowserSession.FindCss(".circle-list-container").Exists());
        }

        [Then(@"I should see showcase banner")]
        public void IShouldSeeShowcaseBanner()
        {
            Assert.True(BrowserSession.FindCss(".showcase-banner").Exists());
        }

        [Then(@"I should see email banner")]
        public void IShouldSeeEmailBanner()
        {
            Assert.True(BrowserSession.FindCss(".email-banner").Exists());
        }

        [Then(@"I should see the trivia section")]
        public void ThenIShouldSeeTheKeyFactsSection()
        {
            Assert.True(BrowserSession.FindCss(".information-list").Exists());
        }

        [Then(@"I should see the body section")]
        public void ThenIShouldSeeTheBodySection()
        {
            Assert.True(BrowserSession.FindAllCss("article").Any());
        }
    }
}
