using System;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using Xunit;

namespace dts_frontend_tests_ui.StepDefinitions
{
    [Binding]
    public class HomepageSteps : UiTestBase
    {
        public HomepageSteps()
        {
            //MockConfiguration.Server.Given(
            //        Request.Create().WithPath("/stockportgov/homepage").UsingGet()
            //    )
            //    .RespondWith(
            //        Response.Create()
            //            .WithStatusCode(200)
            //            .WithBodyAsJson(JObject.Parse("{\r\n  \"PersonReference\": \"9317342\",\r\n  \"Status\": 0,\r\n  \"HasBenefits\": false,\r\n  \"AttemptsRemaining\": 10\r\n}"))
            //    );
        }

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

        [Then("I should see the 5th task block with title UITEST: Article with Section for Contact Us form")]
        public void ThenIShouldSee5thTaskBlock()
        {
            var result = BrowserSession.FindCss("#task-block-5:last-child h3");
            Assert.Equal("UITEST: Article with Section for Contact Us form", result.Text);
        }
    }
}
