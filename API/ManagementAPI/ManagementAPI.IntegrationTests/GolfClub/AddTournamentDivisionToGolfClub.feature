@golfclub
Feature: AddTournamentDivisionToGolfClub

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created

Scenario: Add Tournament Divisions
	When I add tournament division 1 with a start handicap of 0 and and end handicap of 5
	Then the divsion is addded successfully with an Http Status Code 204
	When I add tournament division 2 with a start handicap of 6 and and end handicap of 12
	Then the divsion is addded successfully with an Http Status Code 204
	When I add tournament division 3 with a start handicap of 13 and and end handicap of 21
	Then the divsion is addded successfully with an Http Status Code 204
	When I add tournament division 4 with a start handicap of 22 and and end handicap of 28
	Then the divsion is addded successfully with an Http Status Code 204
