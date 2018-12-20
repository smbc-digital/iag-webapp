@groupManagement
Feature: GroupManagement
	In order to manage a group
	As a group admin
	I want to be able to see all relevant options and pages in the groups management portal

Scenario: User navigates to groups management portal
	Given I have signed in as UiTest
	And I navigate to "/groups/manage"
	Then I should see the UITest group
	And I should see the "Add your group or service" link styled as button
	And I should see the header
	And I should see the footer

Scenario: User navigates to manage uitest group
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing"
	Then I should see the "View your group page" table row
	And I should see the "Change your group information" table row
	And I should see the "Manage your upcoming events" table row
	And I should see the "Add or remove users" table row
	And I should see the "Delete your group" link styled as button
	And I should see the "Archive your group" link styled as button
	And I should see the header
	And I should see the footer

Scenario: User navigates to manage uitest events
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/events"
	Then I should see the "Add your event" link styled as button
	And I should see the "UITEST: Hats Amazing" event
	And I should see the "UITest Event" event
	And I should see the header
	And I should see the footer
	
Scenario: User navigates to manage uitest users
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/users"
	Then I should see the "Add new user" link styled as button
	And I should see the "Edit user" link styled as button
	And I should see the header
	And I should see the footer

Scenario: User navigates to add user to uitest
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/newuser"
	Then I should see the "GroupAdministratorItem.Name" input
	And I should see the "GroupAdministratorItem.Email" input
	And I should see the "GroupAdministratorItem.Permission" input
	And I should see the "Add new user" button
	And I should see the header
	And I should see the footer

Scenario: User enters nothing into add new user
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/newuser"
	When I click the "Add new user" button
	Then I should see the validation message section at the top
	And I should see a validation message for "GroupAdministratorItem.Name" input
	And I should see a validation message for "GroupAdministratorItem.Email" input
	And I should see a validation message for "GroupAdministratorItem.Permission" input

Scenario: User navigates to edit user for uitest
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/edituser?email=scn.uitest@gmail.com"
	Then I should see the "GroupAdministratorItem.Name" input
	And I should see the "GroupAdministratorItem.Permission" input
	And I should see the remove user link
	And I should see the "Save changes" button
	And I should see the "Cancel" link styled as button
	And I should see the header
	And I should see the footer