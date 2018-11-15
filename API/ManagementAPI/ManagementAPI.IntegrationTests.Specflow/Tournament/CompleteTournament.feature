@tournamentmanagement
Feature: Complete Tournament
	In order to run a golf club handicapping system
	As a match secretary
	I need to be able to complete tournaments

Background: 
	Given The Golf Handicapping System Is Running
	And My Club configuration has been already created
	And the club has a measured course

Scenario: Complete Tournament
	Given I have created a tournament
	And some scores have been recorded
	When I request to complete the tournament
	Then the tournament is completed
