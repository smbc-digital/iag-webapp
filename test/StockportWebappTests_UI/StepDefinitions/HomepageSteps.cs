using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "homepage")]
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
                case "additional topics":
                    result = BrowserSession.FindCss(".generic-list-see-more-container").Exists();
                    break;
            }
            Assert.True(result);
        }
    }
}
