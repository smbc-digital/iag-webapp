using System;
using System.Linq;
using TechTalk.SpecFlow;
using Xunit;

namespace StockportWebappTests_UI.StepDefinitions
{
    [Binding, Scope(Tag = "groupManagement")]
    public class GroupManagementSteps: UiTestBase
    {
        [Then(@"I should see the UITest group")]
        public void ThenIShouldSeeTheUiTestGroup()
        {
            Assert.True(BrowserSession.FindId("group-item-uitest-a-group-for-ui-testing").Exists()); 
        }

        [Given(@"I click the ""(.*)"" tab")]
        public void GivenIClickTheTab(string tabName)
        {
            switch (tabName)
            {
                case "Tell us who your group is suitable for":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-1").Click();
                    break;
                case "Contact details":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-2").Click();
                    break;
                case "Additional information":
                    BrowserSession.FindId("multistep-form-sections-wrapper-t-3").Click();
                    break;
            }
        }
        
        [Then(@"I should see the ""(.*)"" table row")]
        public void ThenIShouldSeeSection(string rowName)
        {
            bool result = false;
            switch (rowName)
            {
                case "View your group page":
                    result = BrowserSession.FindId("view-group").Exists();
                    break;
                case "Change your group information":
                    result = BrowserSession.FindId("edit-group").Exists();
                    break;
                case "Manage your upcoming events":
                    result = BrowserSession.FindId("view-group-events").Exists();
                    break;
                case "Add or remove users":
                    result = BrowserSession.FindId("add-or-remove-users").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then(@"I should see the ""(.*)"" button")]
        public void ThenIShouldSeeButton(string buttonName)
        {
            bool result = false;
            switch (buttonName)
            {
                case "Delete your group":
                    result = BrowserSession.FindId("delete-group").Exists();
                    break;
                case "Archive your group":
                    result = BrowserSession.FindId("archive-group").Exists();
                    break;
            }
            Assert.True(result);
        }
    }
}
