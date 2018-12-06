using System;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding]
    public class HomepageSteps : UiTestBase
    {
        [Then("I should see the 5th task block with title UITEST: Article with Section for Contact Us form")]
        public void ThenIShouldSee5thTaskBlock()
        {
            var result = BrowserSession.FindCss("#task-block-5:last-child h3");
            Assert.Equal("UITEST: Article with Section for Contact Us form", result.Text);
        }
    }
}
