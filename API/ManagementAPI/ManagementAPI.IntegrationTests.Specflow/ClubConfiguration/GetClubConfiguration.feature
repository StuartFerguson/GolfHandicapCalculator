@clubconfiguration
Feature: Get Club Configuration
	In order to run a golf club handicapping system
	As a match secretary
	I want to be able to get my golf club configuration

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Get Created Club Configuration
	Given My Club configuration has been created
	And I am logged in as a club administrator
	When I request the details of the club
	Then the club configuration will be returned
	And the club data returned is the correct club
