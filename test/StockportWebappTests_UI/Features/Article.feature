@article
Feature: Article
	In order to navigate Stockport.gov.uk/uitest-testarticle
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk/uitest-testarticle
	Given I navigate to "/uitest-testarticle"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see the "right side bar" section
	And I should see the "heading" section
	And I should see the "article navigation" section
	And I should see the "article body" section
	And I should see the "next page" section
	And I should see the "/uitest-testarticle/uitest-section-with-pdf" link
	And I should see the "youtube video" section
	And I should see the "table" section
	And I should see the "profile" section
	And I should see the footer