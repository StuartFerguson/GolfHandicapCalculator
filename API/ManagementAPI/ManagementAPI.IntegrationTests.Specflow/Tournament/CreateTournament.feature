@tournamentmanagement
Feature: Create Tournament
	In order to run a golf club handicapping system
	As a match secretary
	I want to be able to create club tournaments

Background: 
	Given The Golf Handicapping System Is Running
	And My Club configuration has been already created
	And I am logged in as a club administrator
	And the club has a measured course

Scenario: Create Tournament
	Given I have the details of the new tournament
	When I call Create Tournament
	Then the tournament will be created
	And I will get the new Tournament Id in the response