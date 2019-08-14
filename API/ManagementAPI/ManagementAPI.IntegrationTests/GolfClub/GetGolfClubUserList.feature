@base @golfclub
Feature: GetGolfClubUserList

Background: 
Given the following golf club administrator has been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And I am logged in as the administrator for golf club 1
	When I create a golf club with the following details
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	Then the golf club is created successfully
	When I register the following details for a match secretary
	| GolfClubNumber | EmailAddress                       | GivenName | MiddleName | FamilyName | Password | ConfirmPassword |TelephoneNumber |
	| 1              | matchsecretary@testgolfclub1.co.uk | Match     |            | Secretary1 | 123456   | 123456          |01234567890     |
	Then the match secretary registration should be successful

@EndToEnd
Scenario: Get User List
	Given I am logged in as a golf club administrator for golf club 1
	When I request a list of users for the for golf club 1
	Then 2 users details are returned for golf club 1
	And the golf club administrators details are returned for golf club 1
	And the match secretarys details are returned for golf club 1
