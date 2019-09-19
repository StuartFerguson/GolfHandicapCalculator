namespace ManagementAPI.Service.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class PlayerSignedUpTournamentsResponseExample : IExamplesProvider
    {
        public Object GetExamples()
        {
            return new PlayerSignedUpTournamentsResponse
                   {
                       PlayerSignedUpTournaments = new List<PlayerSignedUpTournament>
                                                   {
                                                       new PlayerSignedUpTournament
                                                       {
                                                           GolfClubId = Guid.Parse("F38FC4F5-F211-4E7F-86EC-7B755261B56B"),
                                                           TournamentFormat = TournamentFormat.StrokePlay,
                                                           TournamentDate = DateTime.Now.Date,
                                                           MeasuredCourseId = Guid.Parse("1F179357-62FF-4446-B699-872D53A7DDDB"),
                                                           TournamentId = Guid.Parse("A2EC876A-CEA7-4E49-983D-21B779E3E4AF"),
                                                           PlayerId = Guid.Parse("A282130B-DC57-423F-A691-F551775DCB35"),
                                                           MeasuredCourseName = "Test Course",
                                                           GolfClubName = "Test Golf Club",
                                                           MeasuredCourseTeeColour = "White",
                                                           TournamentName = "Test Tournament",
                                                           ScoreEntered = false
                                                       }
                                                   }
                   };
        }
    }
}