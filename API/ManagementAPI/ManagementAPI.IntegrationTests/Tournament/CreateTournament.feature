@tournament
Feature: Create Tournament
	In order to run a golf club handicapping system
	As a club administrator
	I want to be able to create club tournaments

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a measured course is added to the club

Scenario: Create Tournament
	Given I have the details of the new tournament
	When I call Create Tournament
	Then the tournament will be created