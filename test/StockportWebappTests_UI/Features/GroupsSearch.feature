@groupsSearch
Feature: GroupsSearch
	In order to navigate Stockport.gov.uk/groups/results
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk/groups/results
	Given I navigate to "/groups/results"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see the "Edit search" section
	And I should see the "Nearest" section
	And I should see the "Filter" button
	And I should see the pagination section
	And I should see the "Content disclaimer" section
	And I should see the "Add your group or service" section
	And I should see the footer

Scenario: User clicks edit search
	Given I navigate to "/groups/results"
	When I click the "Edit search" section
	Then I should see the "Choose a category" section
	And I should see the "Enter a location" section
	And I should see the "Search Stockport Local" button

Scenario: User clicks enter a location
	Given I navigate to "/groups/results"
	When I click the "Edit search" section
	And I click the "Enter a location" section
	Then I should see the "Postcode input" section
	And I should see the "Find my current location" section
	And I should see the "Search all locations" section

Scenario: User clicks nearest
	Given I navigate to "/groups/results"
	When I click the "Nearest" section
	Then I should see the "options" section

Scenario: User clicks filter
	Given I navigate to "/groups/results"
	When I click the "Filter" button
	Then I should see the "filter list" section
	And I should see the "clear all filters" section
	And I should see the "Cancel" button
	And I should see the "Apply" button