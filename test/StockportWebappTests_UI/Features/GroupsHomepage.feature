@groupsHomepage
Feature: GroupsHomepage
	In order to navigate Stockport.gov.uk/groups
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk/groups
	Given I navigate to "/groups"
	Then I should see the header
	And I should see the "Add your group or service" section
	And I should see the "Search everything" section
	And I should see the "Find help and support" section
	And I should see the "Whats near me" section
	And I should see the "Find where to volunteer" section
	And I should see the "View more categories" button
	And I should see the "Find events and activities in Stockport" section
	And I should see additional categories
	And I should see the footer

Scenario: User clicks view more categories
	Given I navigate to "/groups"
	When I click the "View more categories" button
	Then I should see the "additional categories" section