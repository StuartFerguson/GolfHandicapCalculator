@base @golfclub
Feature: RequestClubMembership

Background: 
	Given the following golf club administrators have been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And the following golf clubs exist
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	And the following players have registered
	| PlayerNumber | EmailAddress              | GivenName | MiddleName | FamilyName | DateOfBirth | Gender | ExactHandicap |
	| 1            | testplayer1@players.co.uk | Test      |            | Player1    | 1990-01-01  | M      | 2             |
	
Scenario: Request Club Membership
	Given I am logged in as player number 1
	When I request membership of club number 1 for player number 1 the request is successful
