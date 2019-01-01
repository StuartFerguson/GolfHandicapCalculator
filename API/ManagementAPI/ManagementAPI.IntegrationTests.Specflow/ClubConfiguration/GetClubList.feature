@clubconfiguration
Feature: Get Club List
	In order to request membership of a club
	As a player
	I want to be be able to get a list of clubs

Background: 
	Given The Golf Handicapping System Is Running
	And a club has already been created
	And I am logged in as a player

Scenario: Get Club List	
	When I request the list of clubs
	Then a list of clubs will be returned

