namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "events")]
    class EventsSteps : UiTestBase
    {
        [Then(@"I should not see the ""(.*)"" section")]
        public void ThenIShouldNotSeeSection(string sectionName)
        {
            Assert.False(BrowserSession.FindAllCss(".generic-list-see-more-container:not([style*='display:none'])").Any());
        }

        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "events header":
                    result = BrowserSession.FindCss("h1").Exists();
                    break;
                case "what's on form":
                    result = BrowserSession.FindCss("form#events-filter-bar-container").Exists();
                    break;
                case "event listings":
                    result = BrowserSession.FindCss("#event-listing-container").Exists() && BrowserSession.FindAllCss(".primary-topics .featured-topic-list .featured-topic").Any();
                    break;
                case "generic event listings":
                    result = BrowserSession.FindAllCss(".generic-list-see-more-container:not([style*='display:none'])").Any();
                    break;
                case "upcoming events":
                    result = BrowserSession.FindAllCss(".home-page-row").Any() && BrowserSession.FindAllCss(".event-container-row").Any();
                    break;
                case "find something to do":
                    result = BrowserSession.FindCss("#events-filter-bar").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then("I should see a find what's on button")]
        public void ThenIShouldSeeAFindWhatsOnButton()
        {
            Assert.True(BrowserSession.FindAllCss(".button-default").Any());
        }

        [Then("I should see a start date picker")]
        public void ThenIShouldSeeAStartDatePicker()
        {
            Assert.True(BrowserSession.FindId("DateFrom").Exists());
        }

        [Then("I should see an end date picker")]
        public void ThenIShouldSeeAnEndDatePicker()
        {
            Assert.True(BrowserSession.FindId("DateTo").Exists());
        }


    }
}
