@golfclub
Feature: GetMeasuredCourseList

Background: 
	Given The Golf Handicapping System Is Running
	And I have registered as a golf club administrator
	And I am logged in as a golf club administrator
	And my golf club has been created	
	When I add a measured course to the club
	Then the measured course is added to the club
	
Scenario: Get Measured Course List
	When I ask for a list of measured courses against the golf club
	Then the list of "1" measured courses should be returned
