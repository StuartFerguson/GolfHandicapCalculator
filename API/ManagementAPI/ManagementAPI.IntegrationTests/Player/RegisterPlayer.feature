@base @player
Feature: Register a Player
	In order to use the golf club handicapping system
	As a player
	I want to be register my details on the system

Scenario: Register Player
	Given I register the following details for a player
	| PlayerNumber | EmailAddress              | GivenName | MiddleName | FamilyName | DateOfBirth | Gender | ExactHandicap |
	| 1            | testplayer1@players.co.uk | Test      |            | Player1    | 1990-01-01  | M      | 2             |
	Then the player registration for player number 1 should be successful