﻿@base @golfclub @golfclubadministrator
Feature: Add Measured Course to Existing Golf Club
	In order to run a golf club handicapping system
	As a club administrator
	I want to be able to add measured courses to my golf club

Background: 
	Given the following golf club administrator has been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And I am logged in as the administrator for golf club 1
	When I create a golf club with the following details
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	Then the golf club is created successfully

Scenario: Add Measured Course to Existing Club
	When I add a measured course to the club with the following details
	| GolfClubNumber | MeasuredCourseNumber | Name        | StandardScratchScore | TeeColour |
	| 1              | 1                    | Test Course | 70                   | White     |
	And with the following holes
	| GolfClubNumber | MeasuredCourseNumber | HoleNumber | LengthInYards | Par | StrokeIndex |
	| 1              | 1                    | 1          | 348           | 4   | 10          |
	| 1              | 1                    | 2          | 402           | 4   | 4           |
	| 1              | 1                    | 3          | 207           | 3   | 14          |
	| 1              | 1                    | 4          | 405           | 4   | 8           |
	| 1              | 1                    | 5          | 428           | 4   | 2           |
	| 1              | 1                    | 6          | 477           | 5   | 12          |
	| 1              | 1                    | 7          | 186           | 3   | 16          |
	| 1              | 1                    | 8          | 397           | 4   | 6           |
	| 1              | 1                    | 9          | 130           | 3   | 18          |
	| 1              | 1                    | 10         | 399           | 4   | 3           |
	| 1              | 1                    | 11         | 401           | 4   | 13          |
	| 1              | 1                    | 12         | 421           | 4   | 1           |
	| 1              | 1                    | 13         | 530           | 5   | 11          |
	| 1              | 1                    | 14         | 196           | 3   | 5           |
	| 1              | 1                    | 15         | 355           | 4   | 7           |
	| 1              | 1                    | 16         | 243           | 4   | 15          |
	| 1              | 1                    | 17         | 286           | 4   | 17          |
	| 1              | 1                    | 18         | 399           | 4   | 9           |
	Then the measured course is added to the club successfully