@golfclub
Feature: Create Golf Club
	In order to run a golf club handicapping system
	As a club administrator
	I want to be able to create my golf club

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator

Scenario: Create Golf Club
	Given I have the details of the new club
	And I am logged in as a golf club administrator
	When I call Create Golf Club
	Then the golf club configuration will be created successfully
