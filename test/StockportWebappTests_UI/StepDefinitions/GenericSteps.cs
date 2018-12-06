using System;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class GenericSteps: UiTestBase
    {
        [Given(@"I navigate to ""(.*)""")]
        public void GivenINavigateTo(string url)
        {
            BrowserSession.Visit(url);
        }

        [Then(@"I should see the ""(.*)"" link")]
        public void ThenIShouldSeeTheLink(string href)
        {
            Assert.True(BrowserSession.FindCss(string.Format("a[href*='{0}']", href)).Exists());
        }

        [When(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string name)
        {
            BrowserSession.ClickButton(name);
        }
        
        [Then(@"I should see the ""(.*)"" button")]
        public void ThenIShouldSeeTheButton(string name)
        {
            Assert.True(BrowserSession.FindButton(name).Exists());
        }
    }
}
