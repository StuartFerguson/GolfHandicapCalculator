@golfclub
Feature: Get Golf Club List
	In order to request membership of a golf club
	As a player
	I want to be be able to get a list of golf clubs

Background: 
	Given The Golf Handicapping System Is Running
	And a golf club has already been created
	And I am logged in as a player

Scenario: Get Golf Club List	
	When I request the list of golf clubs
	Then a list of golf clubs will be returned

