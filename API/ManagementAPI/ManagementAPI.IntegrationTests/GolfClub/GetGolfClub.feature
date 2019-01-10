@golfclub
Feature: Get Golf Club
	In order to run a golf club handicapping system
	As aclub administrator
	I want to be able to get my golf club

Background: 
	Given The Golf Handicapping System Is Running	
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created

Scenario: Get Created Golf Club
	When I request the details of the golf club
	Then the golf club data will be returned
