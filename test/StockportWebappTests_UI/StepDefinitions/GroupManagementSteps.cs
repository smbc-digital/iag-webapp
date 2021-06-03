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

        [Then(@"I should see the ""(.*)"" link styled as button")]
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
                case "Add your event":
                    result = BrowserSession.FindCss("a[href='/groups/manage/uitest-a-group-for-ui-testing/events/add-your-event']").Exists();
                    break;
                case "Add new user":
                    result = BrowserSession.FindCss("a[href='/groups/manage/uitest-a-group-for-ui-testing/newuser']").Exists();
                    break;
                case "Edit user":
                    result = BrowserSession.FindCss("a[href='/groups/manage/uitest-a-group-for-ui-testing/edituser?email=scn.uitest@gmail.com']").Exists();
                    break;
                case "Add your group or service":
                    result = BrowserSession.FindCss("a[href='/groups/add-a-group']").Exists();
                    break;
                case "Cancel":
                    result = BrowserSession.FindCss("a[class*='button-default'][href='/groups/manage/uitest-a-group-for-ui-testing/users']").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then("I should see the remove user link")]
        public void ThenIShouldSeeTheRemoveUserLink()
        {
            Assert.True(BrowserSession.FindId("remove-user").Exists());
        }

        [Then(@"I should see the ""(.*)"" event")]
        public void ThenIShouldSeeTheEvent(string eventName)
        {
            bool result = false;
            switch (eventName)
            {
                case "UITEST: Hats Amazing":
                    result = BrowserSession.FindId("event-item-hats-amazing").Exists();
                    break;
                case "UITest Event":
                    result = BrowserSession.FindId("event-item-uitest-event").Exists();
                    break;
            }
            Assert.True(result);
        }

        [Then("I should see the validation message section at the top")]
        public void ThenIShouldSeeTheValidationMessageSection()
        {
            Assert.True(BrowserSession.FindCss(".alert.alert-error.contact-validation-error").Exists());
        }
    }
}
