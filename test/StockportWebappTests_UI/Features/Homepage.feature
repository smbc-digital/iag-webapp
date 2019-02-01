@homepage
Feature: Homepage
	In order to navigate Stockport.gov.uk
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk
	Given I navigate to "/"
	Then I should see the header
	And I should see the "Popular services" section
	And I should see the "View more services" button
	And I should see the "latest news" section
	And I should see the "whats on in stockport" section
	And I should not see the additional categories section
	And I should see the "stockport local" section
	And I should see the "Subscribe" button
	And I should see the footer

Scenario: User clicks view more services
	Given I navigate to "/"
	When I click the "View more services" button
	Then I should see the "additional topics" section