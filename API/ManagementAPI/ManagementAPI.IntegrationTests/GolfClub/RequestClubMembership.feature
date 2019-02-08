@golfclub
Feature: RequestClubMembership

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And I am registered as a player

Scenario: Request Club Membership
	Given The club I want to register for is already created
	And I am logged in as a player
	When I request club membership my request is accepted
