Feature: Homepage
	In order to navigate Stockport.gov.uk
	As a website user
	I want to be able to see all available UI elements

Scenario: Go to homepage
	Given I navigate to "/"
	Then I should see the "https://myaccount.stockport.gov.uk/" link
	Then I should see the 5th task block with title UITEST: Article with Section for Contact Us form
