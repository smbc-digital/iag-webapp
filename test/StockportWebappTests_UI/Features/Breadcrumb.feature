Feature: Breadcrumb
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

Scenario: User navigates to the homepage and shouldn't see ay breadcrumbs
	Given I navigate to "/"
	Then I shouldn't see any breadcrumbs

Scenario: User navigates to a topic page
	Given I navigate to "/topic/uitest-hat-works"
	Then I should see topic breadcrumbs

Scenario: User navigates to an article page
	Given I navigate to "/uitest-about-the-hat-works"
	Then I should see article breadcrumbs