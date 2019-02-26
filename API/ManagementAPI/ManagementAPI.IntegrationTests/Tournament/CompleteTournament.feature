@tournament
Feature: Complete Tournament
	In order to run a golf club handicapping system
	As a club administrator
	I need to be able to complete tournaments

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a measured course is added to the club
	And a player has been registered		

Scenario: Complete Tournament
	Given I have created a tournament
	And I am logged in as a player
	And I am requested membership of the golf club
	And my membership has been accepted
	And I have signed in to play the tournament
	And some scores have been recorded
	When I request to complete the tournament the tournament is completed