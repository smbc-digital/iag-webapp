Feature: Homepage
	In order to navigate Stockport.gov.uk
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk
	Given I navigate to "/"
	Then I should see the "myaccount.stockport.gov.uk/" link
	And I should see the "Popular services" section
	And I should see the "Do it online" button
	And I should see the "latest news" section
	And I should see the "whats on in stockport" section
	And I should see the "stockport local" section
	And I should see the "Subscribe" button
	And I should see the "find services A-Z" section
	And I should see the footer
	And I should see the cookies banner

Scenario: User clicks view more services
	Given I navigate to "/"
	When I click the "View more services" button
	Then I should see the "additional topics" section