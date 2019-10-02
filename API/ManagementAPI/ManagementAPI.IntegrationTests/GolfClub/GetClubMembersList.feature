@base @golfclub @golfclubadministrator
Feature: GetClubMembersList

Background: 
	Given the following golf club administrators have been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And the following golf clubs exist
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	And the following players have registered
		| PlayerNumber | EmailAddress               | GivenName | MiddleName | FamilyName | Age | Gender | ExactHandicap |
		| 1            | testplayer1@players.co.uk  | Test      |            | Player1    | 16  | M      | 2             |
		| 2            | testplayer2@players.co.uk  | Test      |            | Player2    | 18  | M      | 4             |
		| 3            | testplayer3@players.co.uk  | Test      |            | Player3    | 18  | M      | 10            |
		| 4            | testplayer4@players.co.uk  | Test      |            | Player4    | 19  | M      | 12            |
		| 5            | testplayer5@players.co.uk  | Test      |            | Player5    | 20  | M      | 1             |
		| 6            | testplayer6@players.co.uk  | Test      |            | Player6    | 22  | M      | 28            |
		| 7            | testplayer7@players.co.uk  | Test      |            | Player7    | 24  | M      | 24            |
		| 8            | testplayer8@players.co.uk  | Test      |            | Player8    | 26  | M      | 18            |
		| 9            | testplayer9@players.co.uk  | Test      |            | Player9    | 35  | M      | 18            |
		| 10           | testplayer10@players.co.uk | Test      |            | Player10   | 64  | M      | 6             |
		| 11           | testplayer11@players.co.uk | Test      |            | Player11   | 65  | M      | 9             |
		| 12           | testplayer12@players.co.uk | Test      |            | Player12   | 70  | M      | 0             |
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
	| 1              | 10           |
	| 1              | 11           |
	| 1              | 12           |

Scenario: Get List of Golf Club Members
	When I request a list of members for golf club number 1
	And the a list of 12 members is returned