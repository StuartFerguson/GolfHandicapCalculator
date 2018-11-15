@clubconfiguration
Feature: Create Club Configuration
	In order to run a golf club handicapping system
	As a match secretary
	I want to be able to create my golf club configuration

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Create Club Configuration
	Given I have the details of the new club
	When I call Create Club Configuration
	Then the club configuration will be created
	And I will get the new Club Configuration Id in the response