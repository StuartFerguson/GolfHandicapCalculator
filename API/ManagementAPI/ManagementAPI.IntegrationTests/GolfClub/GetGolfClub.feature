@base @golfclub @golfclubadministrator
Feature: Get Golf Club
	In order to run a golf club handicapping system
	As a club administrator
	I want to be able to get my golf club

Background: 
	Given the following golf club administrator has been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And I am logged in as the administrator for golf club 1
	When I create a golf club with the following details
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	Then the golf club is created successfully

Scenario: Get Golf Club
	When I request the details of the golf club 1
	Then the golf club data will be returned