@player
Feature: Request a Club Membership
	In order to use the golf club handicapping system
	As a player
	I want to be request membership of a club

Background: 
	Given The Golf Handicapping System Is Running
	And I am registered as a player
	And I am logged in as a player

Scenario: Request Club Membership
	Given The club I want to register for is already created
	When I request club membership
	Then my request is successful