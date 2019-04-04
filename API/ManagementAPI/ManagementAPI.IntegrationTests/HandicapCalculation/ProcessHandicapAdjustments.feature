Feature: ProcessHandicapAdjustments

	In order to run a golf club handicapping system
	As a club administrator
	I need to be able to process handicap adjustments

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a measured course is added to the club
	And a player has been registered with an exact handicap of 6.2
	Given I have created a tournament
	And I am logged in as a player
	And I am requested membership of the golf club
	And my membership has been accepted
	And I have signed in to play the tournament
	And my score of 4 shots below the CSS has been recorded
	When I request to complete the tournament the tournament is completed

Scenario: Start Calculation Process with a Single Score
	When I start the handicap calculation process
	Then the process completes successfully
	And my new playing handicap is adjusted to 5
	And my new exact handicap is adjusted to 5.0
	