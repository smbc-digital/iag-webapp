namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "groupsAddEdit")]
    public class GroupsAddEditSteps: UiTestBase
    {
        [Given(@"I click the ""(.*)"" tab")]
        [When(@"I click the ""(.*)"" tab")]
        public void GivenIClickTheTab(string tabName)
        {
            switch (tabName)
            {
                case "About your group or service":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-0").Click();
                    Thread.Sleep(1200);
                    break;
                case "Tell us who your group is suitable for":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-1").Click();
                    Thread.Sleep(1200);
                    break;
                case "Contact details":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-2").Click();
                    Thread.Sleep(1200);
                    break;
                case "Additional information":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-3").Click();
                    Thread.Sleep(1200);
                    break;
            }
        }

        [Then(@"I should see the ""(.*)"" tab enabled")]
        public void ThenIShouldSeeTheTabEnabled(string tabName)
        {
            bool result = false;
            switch (tabName)
            {
                case "About your group or service":
                    result = BrowserSession.FindCss("li[aria-disabled=false] > #multistep-form-sections-wrapper-t-0").Exists();
                    break;
                case "Tell us who your group is suitable for":
                    result = BrowserSession.FindCss("li[aria-disabled=false] > #multistep-form-sections-wrapper-t-1").Exists();
                    break;
                case "Contact details":
                    result = BrowserSession.FindCss("li[aria-disabled=false] > #multistep-form-sections-wrapper-t-2").Exists();
                    break;
                case "Additional information":
                    result = BrowserSession.FindCss("li[aria-disabled=false] > #multistep-form-sections-wrapper-t-3").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then(@"I should see the ""(.*)"" tab disabled")]
        public void ThenIShouldSeeTheTabDisabled(string tabName)
        {
            bool result = false;
            switch (tabName)
            {
                case "Tell us who your group is suitable for":
                    result = BrowserSession.FindCss("li[aria-disabled=true] > #multistep-form-sections-wrapper-t-1").Exists();
                    break;
                case "Contact details":
                    result = BrowserSession.FindCss("li[aria-disabled=true] > #multistep-form-sections-wrapper-t-2").Exists();
                    break;
                case "Additional information":
                    result = BrowserSession.FindCss("li[aria-disabled=true] > #multistep-form-sections-wrapper-t-3").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then(@"I should see the ""(.*)"" page")]
        public void ThenIShouldSeeThePage(string pageNumber)
        {
            bool result = false;
            switch (pageNumber)
            {
                case "First":
                    result = BrowserSession.FindCss("#multistep-form-sections-wrapper-p-0[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-1[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-2[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-3[aria-hidden='false']").Exists();
                    break;
                case "Second":
                    result = BrowserSession.FindCss("#multistep-form-sections-wrapper-p-1[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-0[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-2[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-3[aria-hidden='false']").Exists();
                    break;
                case "Third":
                    result = BrowserSession.FindCss("#multistep-form-sections-wrapper-p-2[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-1[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-0[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-3[aria-hidden='false']").Exists();
                    break;
                case "Fourth":
                    result = BrowserSession.FindCss("#multistep-form-sections-wrapper-p-3[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-0[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-1[aria-hidden='false']").Exists()
                             && !BrowserSession.FindCss("#multistep-form-sections-wrapper-p-2[aria-hidden='false']").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then(@"I should see the ""(.*)"" link button")]
        public void ThenIShouldSeeButton(string buttonName)
        {
            bool result = false;
            switch (buttonName)
            {
                case "next step":
                    result = BrowserSession.FindCss("a[href='#next']").Exists();
                    break;
                case "back":
                    result = BrowserSession.FindCss("a[href='#previous']").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then("I click the next step button")]
        [When("I click the next step button")]
        [Given("I click the next step button")]
        public void ThenIClickTheNextStepButton()
        {
            BrowserSession.FindCss("a[href='#next']").Click();
            Thread.Sleep(1200);
        }

        [Then(@"I should see ""(.*)"" group category drop down list")]
        public void ThenIShouldSeeGroupCategoryDropDownList(int dropDownCount)
        {
            Assert.True(BrowserSession.FindAllCss(".CategoriesList-select").Count() == dropDownCount);
        }

        [Then("I should see the add another category button")]
        public void ThenIShouldSeeAddAnotherCategoryButton()
        {
            Assert.True(BrowserSession.FindCss(".CategoriesList-add").Exists());
        }
        
        [When("I click the add another category button")]
        public void WhenIClickTheAddAnotherCategoryButton()
        {
            BrowserSession.FindCss(".CategoriesList-add").Click();
        }

        [Then("I should see the remove category button")]
        public void ThenIShouldSeeRemoveCategoryButton()
        {
            Assert.True(BrowserSession.FindCss(".CategoriesList-remove-link").Exists());
        }

        [When("I click the remove category button")]
        public void WhenIClickTheRemoveCategoryButton()
        {
            BrowserSession.FindCss(".CategoriesList-remove-link").Click();
        }

        [Then(@"I should see checkboxes for ""(.*)""")]
        public void ThenIShouldSeeCheckboxes(string checkboxNames)
        {
            bool result = false;
            switch (checkboxNames)
            {
                case "Suitability":
                    result = BrowserSession.FindAllCss("input[id^='Suitabilities_']").Any();
                    break;
                case "Age ranges":
                    result = BrowserSession.FindAllCss("input[id^='AgeRanges_']").Any();
                    break;
            }
            Assert.True(result);
        }

        [Then("I should see both select all option")]
        public void ThenIShouldSeeSelectAllOption()
        {
            Assert.True(BrowserSession.FindAllCss(".select-all-checkboxes").Count() == 2);
        }

        [When("I click the select all option")]
        public void WhenIClickTheSelectAllOption()
        {
            BrowserSession.FindAllCss(".select-all-checkboxes").FirstOrDefault().Click();
        }

        [Then("I should see a deselect all option")]
        public void ThenIShouldSeeDeselectAllOption()
        {
            Assert.True(BrowserSession.HasContent("Deselect all"));
        }

        [Then("I should see the provide additional information toggle")]
        public void ThenIShouldSeeProvideAdditionalInfoToggle()
        {
            Assert.True(BrowserSession.FindCss(".switch").Exists());
        }

        [When(@"I check the ""(.*)"" checkbox")]
        public void WhenICheckTheCheckbox(string checkboxName)
        {
            switch (checkboxName)
            {
                case "Volunteering":
                    BrowserSession.Check("Volunteering");
                    break;
                case "Donations":
                    BrowserSession.Check("Donations");
                    break;
                case "Additional information":
                    BrowserSession.FindCss(".lever").Click();
                    break;
            }
        }
    }
}
