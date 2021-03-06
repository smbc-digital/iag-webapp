﻿@contactUsArea
Feature: ContactUsArea
	In order to navigate Stockport.gov.uk/contactusarea
	As a website user
	I want to be able to see all available UI elements

Scenario: User navigates to stockport.gov.uk/contact
	Given I navigate to "/contact"
	Then I should see the header
	And I should see the footer
	And I should see the "inset text" section
	And I should see the "top three items" section
	And I should see the "categories container" section
	And I should see the "contact us categories" section
	And I should see the "category header" section
	And I should see the "category body" section
