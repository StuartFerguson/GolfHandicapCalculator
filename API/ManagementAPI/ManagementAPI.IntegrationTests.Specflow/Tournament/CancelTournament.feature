@tournamentmanagement
Feature: Cancel Tournament
	In order to run a golf club handicapping system
	As a match secretary
	I need to be able to cancel tournaments

Background: 
	Given The Golf Handicapping System Is Running
	And My Club configuration has been already created
	And I am logged in as a club administrator
	And the club has a measured course

Scenario: Cancel Tournament
	Given I have created a tournament
	When I request to cancel the tournament
	Then the tournament is cancelled