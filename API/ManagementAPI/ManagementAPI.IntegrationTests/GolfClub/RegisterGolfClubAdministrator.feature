@golfclub
Feature: Register Golf Club Administrator

Background: 
	Given The Golf Handicapping System Is Running

Scenario: Register Golf Club Administrator
	When I register my details as a golf club administrator
	Then my registration should be successful
