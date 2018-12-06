Feature: Homepage
	In order to navigate Stockport.gov.uk
	As a website user
	I want to be able to see all available UI elements

Scenario: Go to homepage
	Given I navigate to "/"
	Then I should see the "myaccount.stockport.gov.uk/" link
	Then I should see the 5th task block with title UITEST: Article with Section for Contact Us form
	When I click the "View more services" button
	Then I should see the ".generic-list-see-more-container" element
	Then I should see the "/topic/do-it-online" link
	Then I should see the ".news" element with child link "/news"
	Then I should see the ".event" element with child link "/events"
	Then I should see the ".group" element with child link "/groups"
	Then I should see the "#test-subscribe" button
	Then I should see the ".atoz" element
	Then I should see the ".l-container-footer" element
	Then I should see the ".cc_banner.cc_container.cc_container--open" element