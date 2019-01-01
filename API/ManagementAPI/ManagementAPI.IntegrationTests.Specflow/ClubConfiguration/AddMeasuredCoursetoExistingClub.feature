@clubconfiguration
Feature: Add Measured Course to Existing Club
	In order to run a golf club handicapping system
	As a match secretary
	I want to be able to add measured courses to my golf club configuration

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Add Measured Course to Existing Club
	Given My Club configuration has been created
	And I am logged in as a club administrator
	When I add a measured course to the club
	Then the measured course is added to the club
