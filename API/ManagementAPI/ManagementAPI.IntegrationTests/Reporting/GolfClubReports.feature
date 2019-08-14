@base @golfclub @reporting
Feature: GolfClubReports

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
	| 2            | testplayer2@players.co.uk | Test      |            | Player2    | 1991-01-01  | M      | 4             |
	| 3            | testplayer3@players.co.uk | Test      |            | Player3    | 1992-01-01  | M      | 10            |
	| 4            | testplayer4@players.co.uk | Test      |            | Player4    | 1993-01-01  | M      | 12            |
	| 5            | testplayer5@players.co.uk | Test      |            | Player5    | 1994-01-01  | M      | 1             |
	| 6            | testplayer6@players.co.uk | Test      |            | Player6    | 1995-01-01  | M      | 28            |
	| 7            | testplayer7@players.co.uk | Test      |            | Player7    | 1996-01-01  | M      | 24            |
	| 8            | testplayer8@players.co.uk | Test      |            | Player8    | 1997-01-01  | M      | 18            |
	| 9            | testplayer9@players.co.uk | Test      |            | Player9    | 1998-01-01  | M      | 18            |
	And the following players are club members of the following golf clubs
	| GolfClubNumber | PlayerNumber |
	| 1              | 1            |
	| 1              | 2            |
	| 1              | 3            |
	| 1              | 4            |
	| 1              | 5            |
	| 1              | 6            |
	| 1              | 7            |
	| 1              | 8            |
	| 1              | 9            |

Scenario: Get Number of Members Report
	When I request a number of members report for club number 1
	Then I am returned the number of members report data successfully
	And the number of members count for club number 1 is 9

Scenario: Get Number of Members By Handicap Category Report
	When I request a number of members by handicap category report for club number 1
	Then I am returned the number of members by handicap category report data successfully
	And the number of members by handicap category count for club number 1 handicap category 1 is 3
	And the number of members by handicap category count for club number 1 handicap category 2 is 2
	And the number of members by handicap category count for club number 1 handicap category 3 is 2
	And the number of members by handicap category count for club number 1 handicap category 4 is 2