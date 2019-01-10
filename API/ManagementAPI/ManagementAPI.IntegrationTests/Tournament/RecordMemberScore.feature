@tournament
Feature: Record Member Score For Tournament
	In order to run a golf club handicapping system
	As a club administrator
	I want members to be able to record their tournament scores

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a measured course is added to the club
	And I have created a tournament
	And a player has been registered

Scenario: Record Member Score For Tournament
	Given I am logged in as a player
	When a player records their score
	Then the score is recorded against the tournament

