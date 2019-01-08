@golfclub
Feature: Get Club Configuration
	In order to run a golf club handicapping system
	As a match secretary
	I want to be able to get my golf club

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Get Created Golf Club
	Given My Golf Club has been created
	And I am logged in as a club administrator
	When I request the details of the golf club
	Then the golf club will be returned
	And the club data returned is the correct club
