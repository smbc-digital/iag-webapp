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
            Assert.True(BrowserSession.FindCss(string.Format("a[href='{0}']", href)).Exists());
        }

        [When(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see the ""(.*)"" element")]
        public void ThenIShouldSeeTheElement(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see the ""(.*)"" element with child link ""(.*)""")]
        public void ThenIShouldSeeTheElementWithChildLink(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I should see the ""(.*)"" button")]
        public void ThenIShouldSeeTheButton(string p0)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
