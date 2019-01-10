@golfclub
Feature: Add Measured Course to Existing Golf Club
	In order to run a golf club handicapping system
	As a club administrator
	I want to be able to add measured courses to my golf club

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created	

Scenario: Add Measured Course to Existing Club
	When I add a measured course to the club
	Then the measured course is added to the club