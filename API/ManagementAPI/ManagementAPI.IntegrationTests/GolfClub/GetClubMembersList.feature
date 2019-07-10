@golfclub
Feature: GetClubMembersList

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created
	And the following players have registered
	| PlayerId | EmailAddress                | GivenName | FamilyName | DateOfBirth | Gender | ExactHandicap |
	| 1        | testplayer1@testplayers.com | Test      | Player 1   | 13/12/1980  | M      | 6.0           |
	| 2        | testplayer2@testplayers.com | Test      | Player 2   | 14/12/1980  | M      | 12.0          |
	| 3        | testplayer3@testplayers.com | Test      | Player 3   | 15/12/1980  | M      | 18.0          |
	And The following players have requested membership
	| PlayerId |
	| 1        |
	| 2        |
	| 3        |

Scenario: Get List of Golf Club Members
	Given I am logged in as a golf club administrator	
	When I request a list of members
	Then a list of '3' members should be returned
