@tournament
Feature: Sign Up For Tournament

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a measured course is added to the club
	And I have created a tournament
	And a player has been registered

Scenario: Sign Up For Tournament
	Given I am logged in as a player	
	And I am requested membership of the golf club
	And my membership has been accepted	
	And I sign up to play the tournament
	Then I am recorded as signed up
