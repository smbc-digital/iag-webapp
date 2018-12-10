using System.Linq;
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

        [Then("I should see the header")]
        public void ThenIShouldSeeTheHeaderSection()
        {
            ThenIShouldSeeTheLink("myaccount.stockport.gov.uk");
            Assert.True(BrowserSession.FindAllCss("[class*='search-button']").Any());
            Assert.True(BrowserSession.FindAllCss("a[href*='stockport.gov.uk'] img").Any());
        }

        [Then("I should see the breadcrumbs")]
        public void ThenIShouldSeeBreadcrumbs()
        {
            Assert.True(BrowserSession.FindCss(".breadcrumb-container").Exists());
        }

        [Then("I should see the footer")]
        public void ThenIShouldSeeTheFooterSection()
        {
            Assert.True(BrowserSession.FindCss(".atoz").Exists());
            Assert.True(BrowserSession.FindCss(".l-container-footer").Exists());
            Assert.True(BrowserSession.FindCss(".cc_banner.cc_container.cc_container--open").Exists());
        }
    
        [Then("I should see the pagination section")]
        public void ThenIShouldSeeThePagination()
        {
            Assert.True(BrowserSession.FindAllCss(".pagination-section").Any());
        }

        [Then(@"I should see the ""(.*)"" link")]
        public void ThenIShouldSeeTheLink(string href)
        {
            Assert.True(BrowserSession.FindAllCss(string.Format("a[href*='{0}']", href)).Any());
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
