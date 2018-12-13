@atoz
Feature: AToZ
	In order to navigate Stockport.gov.uk/uitest-testarticle
	As a website user
	I want to see a list of all articles beginning with a certain letter

Scenario: User navigates to B section of A To Z
	Given I navigate to "/atoz/b"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see a link to an article
	And I should see the footer

	Scenario: User navigates to Z section of A To Z, which has no articles
	Given I navigate to "/atoz/z"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see No Results Found
	And I should see the footer