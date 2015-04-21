Feature: Login
	In order to login successfully
	As a member of the site
	So that member can log in to the site and use its features

@login
Scenario: Invalid Login
	Given I have a username Yusraa
	And I have a password Kirtee!
	When I send a request to log in
	Then I should should not be authenticated successfully

@login
Scenario: Valid Login
	Given I have a username Yusraa
	And I have a password Kirtee123!
	When I send a request to log in
	Then I should be authenticated successfully
