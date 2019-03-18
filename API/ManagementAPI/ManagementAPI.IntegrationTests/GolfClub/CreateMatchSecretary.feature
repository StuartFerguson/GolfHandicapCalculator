@golfclub
Feature: Create Match Secretary
	In order to manage tournaments
	As a club administrator
	I must be able to create a match secretary user

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created

Scenario: Create Match Secretary
	Given I have the details of the match secretary
	When I create the Match Secretary
	Then the divsion is addded successfully with an Http Status Code 204
