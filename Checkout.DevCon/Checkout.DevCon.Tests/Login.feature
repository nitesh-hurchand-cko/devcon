Feature: Account
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@login
Scenario: Invalid Login
	Given I have a username Test
	And I have a password Test
	When I send a request to log in
	Then I should should not be authenticated successfully

@login
Scenario: Valid Login
	Given I have a username admin
	And I have a password admin
	When I send a request to log in
	Then I should be authenticated successfully
