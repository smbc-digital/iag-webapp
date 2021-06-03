namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "groupsHomepage")]
    public class GroupsHomepageSteps : UiTestBase
    {
        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "Add your group or service":
                    result = BrowserSession.FindCss(".add-group-mobile").Exists();
                    break;
                case "Search everything":
                    result = BrowserSession.FindId("search-everything").Exists();
                    break;
                case "Find help and support":
                    result = BrowserSession.FindId("find-help-and-support").Exists();
                    break;
                case "Whats near me":
                    result = BrowserSession.FindId("currentLocationgroup").Exists();
                    break;
                case "Find where to volunteer":
                    result = BrowserSession.FindId("find-where-to-volunteer").Exists();
                    break;
                case "Find events and activities in Stockport":
                    result = BrowserSession.FindCss(".event-banner").Exists();
                    break;
                case "additional categories":
                    result = BrowserSession.FindCss(".generic-list-see-more-container").Exists();
                    break;
            }

            Assert.True(result);
        }

        [Then(@"I should see additional categories")]
        public void ThenIShouldSeeAdditionalCategories()
        {
            Assert.False(BrowserSession.FindAllCss(".generic-list-see-more-container:not([style*='display:none'])").Any());
        }
    }
}
