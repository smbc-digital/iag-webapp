@news
Feature: News
	In order to navigate Stockport.gov.uk/news
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk/news
	Given I navigate to "/news"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see the "news header" section
	And I should see the "alert infomation" section
	And I should see the "alert warning" section
	And I should see the "alert error" section
	And I should see the "refine by category" section
	And I should see the "refine by news archive" section
	And I should see the "Email alerts" section	
	And I should see the "news articles" section	
	And I should see the pagination section
	And I should see the footer

Scenario: User clicks Category drop down
	Given I navigate to "/news"
	When I click the "Category" element
	Then I should see the "category list" section

Scenario: User clicks archive drop down
	Given I navigate to "/news"
	When I click the "News archive" element
	Then I should see the "Custom date" section
	Then I should see the "Update" button
	And I should see the "start date" section
	And I should see the "end date" section

Scenario: User clicks custome date 
	Given I navigate to "/news"
	When I click the "Category" element
	Then I should see the "category list" section

Scenario: User clicks close warning alert 
	Given I navigate to "/news"
	When I click the "close warning button" element
	Then I should not see the "alert warning" section