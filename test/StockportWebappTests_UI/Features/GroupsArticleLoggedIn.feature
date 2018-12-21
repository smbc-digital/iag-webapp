@groupsArticleLoggedIn
Feature: GroupsArticleLoggedIn
       In order to navigate Stockport.gov.uk/groups/zaft
       As the groups administrator
       I want to be able to see all available UI elements

Scenario: User navigates to a group they manage that hasn't changed in more than 3 months
	Given I have signed in as UiTest
	And I navigate to "/groups/wysiwyg"
	Then I should see an "It's up to date" button
	And I should see a "Make a change to this page" button
	And I should see an "Add to favourites" link
	And I should see a "Report this page as inappropriate" link
	And I should see an "About us" section
	And I should see a "summary" section
	And I should see a "contact us" section
	And I should see a map with directions
	And I should see sharing buttons

Scenario: User navigates to a group they manage that has changed yesterday
	Given I have signed in as UiTest
	And I navigate to "/groups/zumba"
	Then I should see a "Manage your groups" button
	And I should see a "Make a change to this page" button
	And I should see an "Add to favourites" link
	And I should see a "Report this page as inappropriate" link
	And I should see an "About us" section
	And I should see a "summary" section
	And I should see a "contact us" section
	And I should see a map with directions
	And I should see sharing buttons

<<<<<<< HEAD
Scenario: Group advisor navigates to a group they have advisor rights for
	Given I have signed in as UiTest
	And I navigate to "/groups/zumba"
	Then I should see an "additional information" section
=======
Scenario: Group advisor navigates to a group they manage
	#Given I have signed in as UiTest
	#And I navigate to "/groups/zumba"
	#Then I should see an "additional information" section
>>>>>>> 660b485afde0ab34a2adbbfbd6da2c5f8720cc82
