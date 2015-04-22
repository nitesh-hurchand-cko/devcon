Feature: Countries
	In order to get list of countries
	As a member of the website
	I want to submit GET requests to the API
	And receive the relevant response

@countries
Scenario: Get list of Countries
	When I send a Get request to the countries API
	Then the result should be list of countries
