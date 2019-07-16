@golfclub
Feature: GetGolfClubUserList

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	Given I have the details of the match secretary
	When I create the Match Secretary
	Then the divsion is addded successfully with an Http Status Code 204

@EndToEnd
Scenario: Get User List
	Given I am logged in as a golf club administrator
	When I request a list of users for the club
	Then 2 users details are returned
	And the golf club administrators details are returned
	And the metch secretarys details are returned 
