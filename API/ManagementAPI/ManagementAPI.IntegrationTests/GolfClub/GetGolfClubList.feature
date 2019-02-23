@golfclub
Feature: Get Golf Club List
	In order to request membership of a golf club
	As a player
	I want to be be able to get a list of golf clubs

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a player has been registered

@EndToEnd
Scenario: Get Golf Club List	
	Given I am logged in as a player
	When I request the list of golf clubs
	Then a list of golf clubs will be returned

