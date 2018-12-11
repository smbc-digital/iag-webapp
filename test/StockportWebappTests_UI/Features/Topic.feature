@topic
Feature: Topic
	In order to access information about Stockport Council
	As a resident of Stockport
	I want to be directed to the correct locations within the website

Scenario: User should see primary, secondary and tertiary links
	Given I navigate to "/topic/uitest-hat-works"
	Then I should see Primary links
	And I should see Secondary links
	And I should see Tertiary links

Scenario: User should see an alert relevant to the content they are seeking
	Given I navigate to "/topic/uitest-hat-works"
	Then I should see a topic page alert

Scenario: User should see an advertisment related to the content they are seeking
	Given I navigate to "topic/uitest-hat-works"
	Then I should see an advertisment banner

Scenario: User should see a link to sign up for email alerts
	Given I navigate to "/topic/uitest-hat-works"
	Then I should see an email alerts link

Scenario: User should see an events link
	Given I navigate to "/topic/uitest-hat-works"
	Then I should see an events banner

Scenario: User should be able dismiss an alert
	Given I navigate to "/topic/uitest-hat-works"
	When I click the close alert button
	Then The alert should no longer be visible
