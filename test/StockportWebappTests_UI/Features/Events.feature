@events
Feature: Events
	In order to navigate Stockport.gov.uk/events
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk/events
	Given I navigate to "/events"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see the "events header" section
	And I should see the "what's on form" section
	And I should see the "event listings" section
	And I should see the "upcoming events" section
	And I should see the "find something to do" section
	And I should not see the "generic event listings" section
	And I should see the footer

Scenario: User clicks View more categories drop down button
	Given I navigate to "/events"
	When I click the "View more categories" button
	Then I should see the "generic event listings" section

Scenario: Assert that find something to do in stockport section has start date input
	Given I navigate to "/events"
	Then I should see a start date picker
	And I should see a find what's on button

Scenario: Assert that find something to do in stockport section has end date input
	Given I navigate to "/events"
	Then I should see an end date picker