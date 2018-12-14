@groupsAddEdit
Feature: GroupsAddEdit
	In order to create or update a group
	As a user
	I want to go to add or edit my group and have the relevant pages and information displayed
	
#Logged in as UITest editing existing group

Scenario: User navigates to tab one and should see correct inputs and buttons
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	Then I should see the header
	And I should see the footer
	And I should see the "About your group or service" tab enabled
	And I should see the "Tell us who your group is suitable for" tab disabled
	And I should see the "Contact details" tab disabled
	And I should see the "Additional information" tab disabled
	And I should see the "Name" input
	And I should see the "Address" input
	And I should see the "Description" input
	And I should see "1" group category drop down list
	And I should see the add another category button
	And I should see the "next step" link button

Scenario: User tries to navigate to tab two from tab one using tabs bar
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	Then I should see the "First" page
	When I click the "Tell us who your group is suitable for" tab
	Then I should see the "First" page
	
Scenario: User navigates to tab two and should see correct inputs and buttons
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	When I click the next step button
	Then I should see the header
	And I should see the footer
	And I should see the "About your group or service" tab enabled
	And I should see the "Tell us who your group is suitable for" tab enabled
	And I should see the "Contact details" tab disabled
	And I should see the "Additional information" tab disabled
	And I should see checkboxes for "Suitability"
	And I should see checkboxes for "Age ranges"
	And I should see both select all option
	And I should see the "next step" link button
	And I should see the "back" link button
	When I click the select all option
	Then I should see a deselect all option

Scenario: User tries to navigate to tab one from tab two using tabs bar
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	And I click the next step button
	Then I should see the "Second" page
	When I click the "About your group or service" tab
	Then I should see the "First" page

Scenario: User tries to navigate to tab three from tab two using tabs bar
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	And I click the next step button
	Then I should see the "Second" page
	When I click the "Contact details" tab
	Then I should see the "Second" page
	
Scenario: User navigates to tab three and should see correct inputs and buttons
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	When I click the next step button
	And I click the next step button
	Then I should see the header
	And I should see the footer
	And I should see the "About your group or service" tab enabled
	And I should see the "Tell us who your group is suitable for" tab enabled
	And I should see the "Contact details" tab enabled
	And I should see the "Additional information" tab disabled
	And I should see the "Email" input
	And I should see the "PhoneNumber" input
	And I should see the "Website" input
	And I should see the "Facebook" input
	And I should see the "Twitter" input
	And I should see the "next step" link button
	And I should see the "back" link button

Scenario: User tries to navigate to tab two from tab three using tabs bar
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	And I click the next step button
	And I click the next step button
	Then I should see the "Third" page
	When I click the "Tell us who your group is suitable for" tab
	Then I should see the "Second" page

Scenario: User tries to navigate to tab four from tab three using tabs bar
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	And I click the next step button
	And I click the next step button
	Then I should see the "Third" page
	When I click the "Additional information" tab
	Then I should see the "Third" page
	
Scenario: User navigates to tab four and should see correct inputs and buttons
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	When I click the next step button
	And I click the next step button
	And I click the next step button
	Then I should see the "Image" input
	And I should see the "Volunteering" input
	And I should see the "VolunteeringText" input
	And I should see the "Donations" input
	And I should see the "DonationsText" input
	And I should see the provide additional information toggle
	And I should see the "Edit your group" button
	And I should see the "back" link button
	When I check the "Additional information" checkbox
	Then I should see the "AdditionalInformation" input

Scenario: User tries to navigate to tab three from tab four using tabs bar
	Given I have signed in as UiTest
	And I navigate to "/groups/manage/uitest-a-group-for-ui-testing/update"
	And I click the next step button
	And I click the next step button
	And I click the next step button
	Then I should see the "Fourth" page
	When I click the "Contact details" tab
	Then I should see the "Third" page

#Creating new group not logged in

Scenario: User fills in all fields
	Given I navigate to "/groups/add-a-group"
	When I enter "Test" in "Name"
	And I enter "Test" in "Address"
	And I enter "Test" in "Description"
	And I select "Culture" in "Categories"
	And I click the next step button
	Then I should see the "Second" page
	When I click the next step button
	Then I should see the "Third" page
	When I enter "test@test.com" in "Email"
	And I click the next step button
	Then I should see the "Fourth" page
	
Scenario: User clicks to add another category on tab one
	Given I navigate to "/groups/add-a-group"
	When I select "Culture" in "Categories"
	And I click the add another category button
	Then I should see "2" group category drop down list
	And I should see the remove category button
	When I click the remove category button
	Then I should see "1" group category drop down list

Scenario: User enters nothing in tab one fields
	Given I navigate to "/groups/add-a-group"
	When I click the next step button
	Then I should see a validation message for "Name" input
	And I should see a validation message for "Address" input
	And I should see a validation message for "Description" input
	
Scenario: User enters nothing in tab three fields
	Given I navigate to "/groups/add-a-group"
	When I enter "Test" in "Name"
	And I enter "Test" in "Address"
	And I enter "Test" in "Description"
	And I select "Culture" in "Categories"
	And I click the next step button
	And I click the next step button
	And I click the next step button
	Then I should see a validation message for "Email" input

Scenario: User navigates to tab three and enters invalid data into email, facebook and twitter inputs
	Given I navigate to "/groups/add-a-group"
	When I enter "Test" in "Name"
	And I enter "Test" in "Address"
	And I enter "Test" in "Description"
	And I select "Culture" in "Categories"
	And I click the next step button
	And I click the next step button
	And I enter "invalid" in "Email"
	And I enter "invalid" in "Facebook"
	And I enter "invalid" in "Twitter"
	And I click the next step button
	Then I should see a validation message for "Email" input
	Then I should see a validation message for "Facebook" input
	And I should see a validation message for "Twitter" input
	
Scenario: User navigates to tab four and checks volunteering, donations and additional information
	Given I navigate to "/groups/add-a-group"
	When I enter "Test" in "Name"
	And I enter "Test" in "Address"
	And I enter "Test" in "Description"
	And I select "Culture" in "Categories"
	And I click the next step button
	And I click the next step button
	And I enter "test@test.com" in "Email"
	And I click the next step button
	And I check the "Volunteering" checkbox
	And I check the "Donations" checkbox
	Then I should see the "VolunteeringText" input
	And I should see the "DonationsText" input
	When I enter "invalid" in "DonationsUrl" 
	And I click the "Add your group or service" button
	Then I should see a validation message for "DonationsUrl" input