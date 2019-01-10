@player
Feature: Reject a Club Membership Request

Background: 
	Given The Golf Handicapping System Is Running	
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a player has been registered
	And I am logged in as a player
	And a player has requested membership of the club

Scenario: Reject Club Membership Request
	Given I am logged in as a golf club administrator
	And I reject a club membership request the request is successful