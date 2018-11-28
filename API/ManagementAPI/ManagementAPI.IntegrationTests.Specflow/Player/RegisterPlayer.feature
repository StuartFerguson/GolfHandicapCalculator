@player
Feature: Register a Player
	In order to use the golf club handicapping system
	As a player
	I want to be register my details on the system

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Register Player
	Given I have my details to register
	When I register my details on the system
	Then my details are registered
	And my player id will be returned in the response