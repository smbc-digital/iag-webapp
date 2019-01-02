using System;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class GroupsArticleLoggedInSteps : UiTestBase
    {
        [Then(@"I should see an ""(.*)"" button")]
        [Then(@"I should see a ""(.*)"" button")]
        public void ThenIShouldSeeAnButton(string buttonText)
        {
            Assert.True(BrowserSession.FindButton(buttonText).Exists());
        }
        
        [Then(@"I should see an ""(.*)"" link")]
        [Then(@"I should see a ""(.*)"" link")]
        public void ThenIShouldSeeAnLink(string linkText)
        {
            Assert.True(BrowserSession.FindLink(linkText).Exists());
        }
        
        [Then(@"I should see an ""(.*)"" section")]
        [Then(@"I should see a ""(.*)"" section")]
        public void ThenIShouldSeeAnSection(string sectionName)
        {
            switch (sectionName)
            {
                case "About us":
                    Assert.True(BrowserSession.FindId("about-us").Exists());
                    break;
                case "summary":
                    Assert.True(BrowserSession.FindId("what-we-do").Exists());
                    break;
                case "contact us":
                    Assert.True(BrowserSession.FindId("contact-us").Exists());
                    break;
                case "additional information":
                    Assert.True(BrowserSession.FindCss(".additional-information").Exists());
                    break;
            }
        }
        
        [Then(@"I should see a map with directions")]
        public void ThenIShouldSeeAMapWithDirections()
        {
            var mapContainer = BrowserSession.FindCss(".group-map-container");

            Assert.True(mapContainer.FindId("map").Exists());
            Assert.True(mapContainer.FindCss(".directions-list").Exists());
        }
        
        [Then(@"I should see sharing buttons")]
        public void ThenIShouldSeeSharingButtons()
        {
            Assert.True(BrowserSession.FindId("share").Exists());
            Assert.True(BrowserSession.FindId("print").Exists());
            Assert.True(BrowserSession.FindId("download-pdf").Exists());
        }
    }
}
