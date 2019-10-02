@base @golfclub @golfclubadministrator
Feature: AddTournamentDivisionToGolfClub

Background: 
	Given the following golf club administrator has been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And I am logged in as the administrator for golf club 1
	When I create a golf club with the following details
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	Then the golf club is created successfully

Scenario: Add Tournament Divisions
	When I add tournament division 1 with a start handicap of 0 and and end handicap of 5
	Then the divsion is addded successfully to golf club 1 
	When I add tournament division 2 with a start handicap of 6 and and end handicap of 12
	Then the divsion is addded successfully to golf club 1 
	When I add tournament division 3 with a start handicap of 13 and and end handicap of 21
	Then the divsion is addded successfully to golf club 1 
	When I add tournament division 4 with a start handicap of 22 and and end handicap of 28
	Then the divsion is addded successfully to golf club 1 