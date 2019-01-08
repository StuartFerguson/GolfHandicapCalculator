@golfclub
Feature: Create Golf Club
	In order to run a golf club handicapping system
	As a club administrator
	I want to be able to create my golf club

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Create Golf Club
	Given I have the details of the new club
	When I call Create Golf Club
	Then the golf club configuration will be created
	And I will get the new Golf Club Id in the response