@base @golfclub @player @tournament @reporting @golfclubadministrator
Feature: PlayerReports

Background: 
	Given the following golf club administrator has been registered
	| GolfClubNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Password | ConfirmPassword | TelephoneNumber |
	| 1              | admin@testgolfclub1.co.uk | Admin     |            | User1      | 123456   | 123456          | 01234567890     |
	And I am logged in as the administrator for golf club 1
	When I create a golf club with the following details
	| GolfClubNumber | GolfClubName     | AddressLine1                  | AddressLine2                | Town      | Region     | PostalCode | TelephoneNumber | EmailAddress              | WebSite             | 
	| 1              | Test Golf Club 1 | Test Golf Club Address Line 1 | Test Golf Club Address Line | TestTown1 | TestRegion | TE57 1NG   | 01234567890     | testclub1@testclub1.co.uk | www.testclub1.co.uk |
	Then the golf club is created successfully
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
	Given When I create a tournament with the following details
	| TournamentNumber | TournamentName    | TournamentMonth | TournamentDay | TournamentFormat | PlayerCategory | MeasuredCourseName | GolfClubNumber |
	| 1                | Test Tournament 1 | April           | 5             | Strokeplay       | Gents          | Test Course        | 1              |
	| 2                | Test Tournament 2 | May             | 5             | Strokeplay       | Gents          | Test Course        | 1              |
	| 3                | Test Tournament 3 | May             | 10            | Strokeplay       | Gents          | Test Course        | 1              |
	| 4                | Test Tournament 4 | June            | 15            | Strokeplay       | Gents          | Test Course        | 1              |
	| 5                | Test Tournament 5 | July            | 12            | Strokeplay       | Gents          | Test Course        | 1              |
	| 6                | Test Tournament 6 | July            | 19            | Strokeplay       | Gents          | Test Course        | 1              |
	| 7                | Test Tournament 7 | August          | 25            | Strokeplay       | Gents          | Test Course        | 1              |
	| 8                | Test Tournament 8 | September       | 1             | Strokeplay       | Gents          | Test Course        | 1              |
	Then tournament number 1 for golf club 1 measured course 'Test Course' will be created
	And tournament number 2 for golf club 1 measured course 'Test Course' will be created
	And tournament number 3 for golf club 1 measured course 'Test Course' will be created
	And tournament number 4 for golf club 1 measured course 'Test Course' will be created
	And tournament number 5 for golf club 1 measured course 'Test Course' will be created
	And tournament number 6 for golf club 1 measured course 'Test Course' will be created
	And tournament number 7 for golf club 1 measured course 'Test Course' will be created
	And tournament number 8 for golf club 1 measured course 'Test Course' will be created

	Given the following players have registered
	| PlayerNumber | EmailAddress              | GivenName | MiddleName | FamilyName | Age | Gender | ExactHandicap |
	| 1            | testplayer1@players.co.uk | Test      |            | Player1    | 25  | M      | 6.2           |
	
	Given I am logged in as player number 1
	
	When I request membership of club number 1 for player number 1 the request is successful
	
	When player number 1 signs up to play in tournament number 1 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 1 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 2 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 2 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 3 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 3 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 4 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 4 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 5 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 5 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 6 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 6 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 7 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 7 for golf club 1 measured course 'Test Course'
	
	When player number 1 signs up to play in tournament number 8 for golf club 1 measured course 'Test Course'
	Then player number 1 is recorded as signed up for tournament number 8 for golf club 1 measured course 'Test Course'
	
	When a player records the following score for tournament number 1 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |
	Then the scores recorded by the players are recorded against tournament number 1 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 1 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 1 for golf club 1 measured course 'Test Course' the results are produced

	When a player records the following score for tournament number 2 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |
	Then the scores recorded by the players are recorded against tournament number 2 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 2 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 2 for golf club 1 measured course 'Test Course' the results are produced
	
	When a player records the following score for tournament number 3 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |
	Then the scores recorded by the players are recorded against tournament number 3 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 3 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 3 for golf club 1 measured course 'Test Course' the results are produced
	
	When a player records the following score for tournament number 4 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |	
	Then the scores recorded by the players are recorded against tournament number 4 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 4 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 4 for golf club 1 measured course 'Test Course' the results are produced
	
	When a player records the following score for tournament number 5 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |
	Then the scores recorded by the players are recorded against tournament number 5 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 5 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 5 for golf club 1 measured course 'Test Course' the results are produced
	
	When a player records the following score for tournament number 6 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |
	Then the scores recorded by the players are recorded against tournament number 6 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 6 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 6 for golf club 1 measured course 'Test Course' the results are produced

	When a player records the following score for tournament number 7 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |
	Then the scores recorded by the players are recorded against tournament number 7 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 7 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 7 for golf club 1 measured course 'Test Course' the results are produced

	When a player records the following score for tournament number 8 for golf club 1 measured course 'Test Course'
	| PlayerNumber | Hole1 | Hole2 | Hole3 | Hole4 | Hole5 | Hole6 | Hole7 | Hole8 | Hole9 | Hole10 | Hole11 | Hole12 | Hole13 | Hole14 | Hole15 | Hole16 | Hole17 | Hole18 |
	| 1            | 4     | 5     | 4     | 4     | 4     | 5     | 3     | 4     | 3     | 4      | 4      | 4      | 5      | 3      | 4      | 4      | 4      | 4      |	
	Then the scores recorded by the players are recorded against tournament number 8 for golf club 1 measured course 'Test Course'
	When I request to complete the tournament number 8 for golf club 1 measured course 'Test Course' the tournament is completed
	When I request to produce a tournament result for tournament number 8 for golf club 1 measured course 'Test Course' the results are produced

Scenario: Player Scores Report
	
	When I request a number of scores report for player number 1 with the last 5 scores
	Then I am returned the number of scores report data successfully
	And the number of scores count for player number 1 is 5
	And the following scores are returned for player 1
	| TournamentName    | TournamentMonth | TournamentDay |
	| Test Tournament 4 | June            | 15            |
	| Test Tournament 5 | July            | 12            |
	| Test Tournament 6 | July            | 19            |
	| Test Tournament 7 | August          | 25            |
	| Test Tournament 8 | September       | 1             |