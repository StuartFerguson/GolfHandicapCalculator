@player
Feature: Request a Club Membership
	In order to use the golf club handicapping system
	As a player
	I want to be request membership of a club

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a player has been registered

Scenario: Request Club Membership
	Given I am logged in as a player
	When I request club membership the request is successful