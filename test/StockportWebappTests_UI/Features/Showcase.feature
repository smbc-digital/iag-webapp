Feature: Showcase
	In order to see information I want
	As a resident of Stockport
	I want the showcase page to render correctly

Scenario: User loads showcase should see alerts section
	Given I navigate to "/showcase/test-showcase"
	Then I should see the alerts section

Scenario: User loads showcase should see primary items
	Given I navigate to "/showcase/test-showcase"
	Then I should see the primary items section

Scenario: User loads showcase should see features items
	Given I navigate to "/showcase/test-showcase"
	Then I should see the featured items section

Scenario: User loads showcase should see consultations
	Given I navigate to "/showcase/test-showcase"
	Then I should see the consultations section

Scenario: User loads showcase should see social media links
	Given I navigate to "/showcase/test-showcase"
	Then I should see the social media links section

Scenario: User loads showcase should see the profile section
	Given I navigate to "/showcase/test-showcase"
	Then I should see the profile section

Scenario: User loads showcase should see the events section
	Given I navigate to "/showcase/test-showcase"
	Then I should see the events section

Scenario: User loads showcase should see the key facts section
	Given I navigate to "/showcase/test-showcase"
	Then I should see the key facts section

Scenario: User loads showcase should see the body section
	Given I navigate to "/showcase/test-showcase"
	Then I should see the body section