@base @golfclub
Feature: Get Golf Club List
	In order to request membership of a golf club
	As a player
	I want to be be able to get a list of golf clubs

Background: 
Given the following golf club administrators have been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	| 2              | admin@testgolfclub2.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	| 3              | admin@testgolfclub3.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	| 4              | admin@testgolfclub4.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And the following golf clubs exist
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	| 2              | Test Golf Club 2 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub2.co.uk | www.testclub2.co.uk |
	| 3              | Test Golf Club 3 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub3.co.uk | www.testclub3.co.uk |
	| 4              | Test Golf Club 4 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub4.co.uk | www.testclub4.co.uk |
	And the following players have registered
	| PlayerNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Age | Gender | ExactHandicap |
	| 1            | testplayer1@players.co.uk | Test      |            | Player1    | 25  | M      | 2             |

@EndToEnd
Scenario: Get Golf Club List
	Given I am logged in as player number 1
	When I request the list of golf clubs
	Then a list of golf clubs will be returned
	And the list will contain 4 golf clubs