@golfclub
Feature: Get Pending Club Membership Requests

Background: 
	Given The Golf Handicapping System Is Running
	And a golf club has already been created
	And a player has already registed
	And a player has requested membership of the club
	And I am logged in as a club administrator

Scenario: Get Pending Membership Request List	
	When I request the list of pending membership requests
	Then a list of pending membership requests will be returned