namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "groupsFavourites")]
    public sealed class GroupsFavouritesSteps : UiTestBase
    {
        [Given(@"I set up two favourite groups")]
        public void GivenISetUpTwoFavouriteGroups()
        {
            BrowserSession.Visit("/groups/results");
            BrowserSession.ClickLink("/favourites/nojs/add?slug=time-for-dance&type=group");
            BrowserSession.ClickLink("/favourites/nojs/add?slug=time-for-dance1&type=group");
            Thread.Sleep(1000);
        }

        [When(@"I click the ""(.*)"" link")]
        public void WhenIClickTheLink(string url)
        {
            BrowserSession.ClickLink(url);
            Thread.Sleep(1000);
        }

        [Then(@"I should see the ""(.*)"" section")]
        public void ThenIShouldSeeTheSection(string sectionName)
        {
            bool result = false;
            switch (sectionName)
            {
                case "Favourite groups":
                    result = BrowserSession.FindId("favourites-list").Exists();
                    break;
                case "Print favourites":
                    result = BrowserSession.FindCss(".print-this").Exists();
                    break;
                case "No results":
                    result = BrowserSession.FindId("no-results").Exists();
                    break;
                case "Go back to Stockport Local":
                    result = BrowserSession.FindCss(".button-default").Exists();
                    break;
            }
            Assert.True(result);
        }
    }
}
