@groupsFavourites
Feature: GroupsFavourites
	In order to manage my favourite groups
	As a website user
	I want to be able to see all available UI elements

Scenario: User favourites two groups
	Given I set up two favourite groups
	Then I should see the "/groups/favourites" link

Scenario: User with two favourite groups navigates to /groups/time-for-dance
	Given I set up two favourite groups
	When I navigate to "/groups/time-for-dance"
	Then I should see the "/groups/favourites" link

Scenario: User with two favourite groups navigates to /groups
	Given I set up two favourite groups
	When I navigate to "/groups"
	Then I should see the "/groups/favourites" link

Scenario: User with two favourite groups navigates to /groups/favourites
	Given I set up two favourite groups
	When I click the "/groups/favourites" link
	Then I should see the header
	And I should see the breadcrumbs
	And I should see the "Favourite groups" section
	And I should see the "Print favourites" section
	And I should see the "/groups/exportpdf/favourites" link
	And I should see the "/groups/favourites/clearall" link
	And I should see the footer

Scenario: User removes two favourites one by one
	Given I set up two favourite groups
	When I click the "/groups/favourites" link
	And I click the "/favourites/nojs/remove?slug=time-for-dance&type=group" link
	And I click the "/favourites/nojs/remove?slug=time-for-dance1&type=group" link
	Then I should see the "No results" section
	And I should see the "Go back to Stockport Local" section

Scenario: User removes all favourites using clear all favourites
	Given I set up two favourite groups
	When I click the "/groups/favourites" link
	And I click the "/groups/favourites/clearall" link
	Then I should see the "/groups/favourites" link
	And I should see the "Clear favourites" button

Scenario: User confirms they want to clear all favourites
	Given I set up two favourite groups
	When I click the "/groups/favourites" link
	And I click the "/groups/favourites/clearall" link
	And I click the "Clear favourites" button
	Then I should see the "No results" section
	And I should see the "Go back to Stockport Local" section