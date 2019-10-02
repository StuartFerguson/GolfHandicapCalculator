@base @golfclub @player @golfclubadministrator
Feature: Get Player

Background:
	Given I register the following details for a player
	| PlayerNumber | EmailAddress              | GivenName | MiddleName | FamilyName | DateOfBirth | Gender | ExactHandicap |
	| 1            | testplayer1@players.co.uk | Test      |            | Player1    | 1990-01-01  | M      | 2             |
	Then the player registration for player number 1 should be successful

Scenario: Get Player
	Given I am logged in as player number 1
	When I request the player details for player number 1
	Then the player details will be returned