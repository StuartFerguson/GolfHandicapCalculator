@tournament
Feature: Cancel Tournament
	In order to run a golf club handicapping system
	As a club administrator
	I need to be able to cancel tournaments

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And a measured course is added to the club

Scenario: Cancel Tournament
	Given I have created a tournament
	When I request to cancel the tournament the tournament is cancelled