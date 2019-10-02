@base @golfclubadministrator
Feature: Register Golf Club Administrator

Scenario: Register Golf Club Administrator
	When I register the following details for a golf club administrator
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	Then the golf club administrator registration should be successful
