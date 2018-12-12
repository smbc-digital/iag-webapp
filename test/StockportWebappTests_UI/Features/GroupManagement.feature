@groupManagement
Feature: GroupManagement
	In order to manage a group
	As a group admin
	I want to be able to see all relevant options and pages in the groups management portal

Scenario: User navigates to groups management portal
	Given I have signed in as UiTest
	And I navigate to "/groups/manage"
	Then I should see the UITest group
	And I should see the "/groups/add-a-group" link
	And I should see the header
	And I should see the footer

Scenario: User navigates to manage uitest group
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing"
	Then I should see the "View your group page" table row
	And I should see the "Change your group information" table row
	And I should see the "Manage your upcoming events" table row
	And I should see the "Add or remove users" table row
	And I should see the "Delete your group" button
	And I should see the "Archive your group" button
	And I should see the header
	And I should see the footer
	
#Scenario: User navigates to change your group information first step
#	Given I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
#	#Then I should see the header
#	#And I should see the footer
#	
#Scenario: User navigates to change your group information second step
#	Given I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
#	And I click the "Tell us who your group is suitable for" tab
#	#Then I should see the header
#	#And I should see the footer
#
#Scenario: User navigates to change your group information third step
#	Given I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
#	And I click the "Contact details" tab
#	#Then I should see the header
#	#And I should see the footer
#	
#Scenario: User navigates to change your group information second step
#	Given I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
#	And I click the "Additional information" tab
#	#Then I should see the header
#	#And I should see the footer
#	
#Scenario: User navigates to manage your upcoming events
#	Given I navigate to "/groups/manage/uitest-a-group-for-ui-testing/events"
#	#Then I should see the header
#	#And I should see the footer
#	
#Scenario: User navigates to add or remove users
#	Given I navigate to "/groups/manage/uitest-a-group-for-ui-testing/users"
#	#Then I should see the header
#	#And I should see the footer