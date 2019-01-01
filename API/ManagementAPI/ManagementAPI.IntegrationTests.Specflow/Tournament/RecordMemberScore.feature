@tournamentmanagement
Feature: Record Member Score For Tournament
	In order to run a golf club handicapping system
	As a match secretary
	I want members to be able to record their tournament scores

Background: 
	Given The Golf Handicapping System Is Running
	And My Club configuration has been already created
	And I am logged in as a club administrator
	And the club has a measured course

Scenario: Record Member Score For Tournament
	Given I have created a tournament
	And I am logged in as a player
	When a member records their score
	Then the score is recorded against the tournament

