@tournamentmanagement
Feature: Produce Tournament Result
	In order to run a golf club handicapping system
	As a match secretary
	I need to be able to produce tournament results

Background: 
	Given The Golf Handicapping System Is Running
	And My Club configuration has been already created
	And the club has a measured course	

Scenario: Produce Tournament Result
	Given I have created a tournament
	And some scores have been recorded
	And the tournament is completed
	When I request to produce a tournament result
	Then the results are produced
