@player
Feature: Reject a Club Membership Request

Background: 
	Given The Golf Handicapping System Is Running	
	And I am registered as a player
	And I am logged in as a player
	And The club I want to register for is already created
	And I have requested a club membership

Scenario: Reject Club Membership Request
	Given I am logged in as a club administrator
	And I reject a club membership request
	Then my rejection request is successful
