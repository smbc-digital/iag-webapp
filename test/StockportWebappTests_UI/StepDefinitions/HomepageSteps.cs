using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class HomepageSteps : UiTestBase
    {
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "Popular services":
                    result = BrowserSession.FindAllCss(".task-block-container .task-block").Any();
                    break;
                case "latest news":
                    result = BrowserSession.FindCss(".news").Exists();
                    break;
                case "whats on in stockport":
                    result = BrowserSession.FindCss(".event").Exists();
                    break;
                case "stockport local":
                    result = BrowserSession.FindCss(".group").Exists();
                    break;
                case "find services A-Z":
                    result = BrowserSession.FindCss(".atoz").Exists();
                    break;
                case "additional topics":
                    result = BrowserSession.FindCss(".generic-list-see-more-container").Exists();
                    break;
            }

            Assert.True(result);
        }

        [Then("I should see the footer")]
        public void ThenIShouldSeeFooter()
        {
            Assert.True(BrowserSession.FindCss(".l-container-footer").Exists());
        }

        [Then("I should see the cookies banner")]
        public void ThenIShouldSeeCookies()
        {
            Assert.True(BrowserSession.FindCss(".cc_banner.cc_container.cc_container--open").Exists());
        }
    }
}
