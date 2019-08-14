@base @golfclub
Feature: GetClubMembersList

Background: 
	Given the following golf club administrators have been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And the following golf clubs exist
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	And the following players have registered
	| PlayerNumber | EmailAddress              | GivenName | MiddleName | FamilyName | DateOfBirth | Gender | ExactHandicap |
	| 1            | testplayer1@players.co.uk | Test      |            | Player1    | 1990-01-01  | M      | 2.0             |
	| 2            | testplayer2@players.co.uk | Test      |            | Player2    | 1991-01-01  | M      | 4.6             |
	| 3            | testplayer3@players.co.uk | Test      |            | Player3    | 1992-01-01  | M      | 10.4            |
	| 4            | testplayer4@players.co.uk | Test      |            | Player4    | 1993-01-01  | M      | 12.8            |
	| 5            | testplayer5@players.co.uk | Test      |            | Player5    | 1994-01-01  | M      | 1.5             |
	| 6            | testplayer6@players.co.uk | Test      |            | Player6    | 1995-01-01  | M      | 20.1            |
	| 7            | testplayer7@players.co.uk | Test      |            | Player7    | 1996-01-01  | M      | 24.9            |
	And the following players are club members of the following golf clubs
	| GolfClubNumber | PlayerNumber |
	| 1              | 1            |
	| 1              | 2            |
	| 1              | 3            |
	| 1              | 4            |
	| 1              | 5            |
	| 1              | 6            |
	| 1              | 7            |

Scenario: Get List of Golf Club Members
	When I request a list of members for golf club number 1
	And the a list of 7 members is returned