@golfclub
Feature: Get Pending Club Membership Requests

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And my golf club has been created
	And a player has been registered
	And a player has requested membership of the club

Scenario: Get Pending Membership Request List	
	Given I am logged in as a golf club administrator
	When I request the list of pending membership requests
	Then a list of pending membership requests will be returned