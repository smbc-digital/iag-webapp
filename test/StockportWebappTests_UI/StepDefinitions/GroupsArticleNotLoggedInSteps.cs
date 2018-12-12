using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "groupsArticleNotLoggedIn")]
    public sealed class GroupsArticleNotLoggedInSteps : UiTestBase
    {
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeTheSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "Add to favourites":
                    result = BrowserSession.FindCss(".add-favourite").Exists();
                    break;
                case "Report this page as inappropriate":
                    result = BrowserSession.FindId("report-page").Exists();
                    break;
                case "About us":
                    result = BrowserSession.FindId("about-us").Exists();
                    break;
                case "What we do":
                    result = BrowserSession.FindId("what-we-do").Exists();
                    break;
                case "Contact us":
                    result = BrowserSession.FindId("contact-us").Exists();
                    break;
                case "directions":
                    result = BrowserSession.FindCss(".directions-list").Exists();
                    break;
                case "Share this":
                    result = BrowserSession.FindId("share").Exists();
                    break;
                case "Print this page":
                    result = BrowserSession.FindId("print").Exists();
                    break;
                case "Download as PDF":
                    result = BrowserSession.FindId("download-pdf").Exists();
                    break;
                case "Disclaimer":
                    result = BrowserSession.FindCss(".content-disclaimers").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then("I should see a map")]
        public void ThenIShouldSeeAMap()
        {
            Assert.True(BrowserSession.FindId("map").Exists());
        }
    }
}
