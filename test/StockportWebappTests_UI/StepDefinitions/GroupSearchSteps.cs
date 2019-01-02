using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "groupsSearch")]
    public sealed class GroupsSearchSteps : UiTestBase
    {
        [When(@"I click the ""(.*)"" section")]
        public void WhenIClickTheSection(string sectionName)
        {
            switch (sectionName)
            {
                case "Edit search":
                    BrowserSession.FindId("open-edit-search").Click();
                    break;
                case "Enter a location":
                    BrowserSession.FindId("postcode").Click();
                    break;
                case "Nearest":
                    BrowserSession.FindCss(".group-sort-by-order").Click();
                    break;
            }
        }
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "Edit search":
                    result = BrowserSession.FindId("open-edit-search").Exists();
                    break;
                case "Nearest":
                    result = BrowserSession.FindCss(".group-sort-by-order").Exists();
                    break;
                case "Filter":
                    result = BrowserSession.FindCss(".group-sort-by-filter").Exists();
                    break;
                case "Content disclaimer":
                    result = BrowserSession.FindCss(".content-disclaimers").Exists();
                    break;
                case "Add your group or service":
                    result = BrowserSession.FindCss(".add-group-mobile").Exists();
                    break;
                case "Choose a category":
                    result = BrowserSession.FindId("selectCategory").Exists();
                    break;
                case "Enter a location":
                    result = BrowserSession.FindId("postcode").Exists();
                    break;
                case "Postcode input":
                    result = BrowserSession.FindId("location-autocomplete").Exists();
                    break;
                case "Find my current location":
                    result = BrowserSession.FindId("currentLocation").Exists();
                    break;
                case "Search all locations":
                    result = BrowserSession.FindId("allLocations").Exists();
                    break;
                case "options":
                    result = BrowserSession.FindAllCss("option").Any();
                    break;
                case "filter list":
                    result = BrowserSession.FindAllCss("input").Any();
                    break;
                case "clear all filters":
                    result = BrowserSession.FindCss(".clear-all-filters").Exists();
                    break;
            }
            Assert.True(result);
        }
    }
}
