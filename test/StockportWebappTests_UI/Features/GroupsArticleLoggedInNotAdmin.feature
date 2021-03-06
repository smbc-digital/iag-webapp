﻿@groupsArticleNotLoggedIn
Feature: GroupsArticleLoggedInNotAdmin
       In order to navigate Stockport.gov.uk/groups/zaft
       As a logged in user
       I want to be able to see all available UI elements

Scenario: Logged in user navigates to a group they don't manage
	Given I have signed in as UiTest
	And I navigate to "/groups/zaft"
	Then I should see the header
	And I should see the breadcrumbs
	And I should see the "Add to favourites" section
	And I should see the "Report this page as inappropriate" section
	And I should see the "/groups/zaft/change-group-info?groupname=ZAFT" link
	And I should see the "/groups/manage" link
	And I should not see an "It's up to date" button
	And I should not see a "Make a change to this page" button
	And I should see the "About us" section
	And I should see the "What we do" section
	And I should see the "Contact us" section
	And I should see a map
	And I should see the "directions" section
	And I should see the "Share this" section
	And I should see the "Print this page" section
	And I should see the "Download as PDF" section
	And I should see the "Disclaimer" section
	And I should see the footer
