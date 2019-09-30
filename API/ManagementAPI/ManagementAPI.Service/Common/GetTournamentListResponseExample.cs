namespace ManagementAPI.Service.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GetTournamentListResponseExample : IExamplesProvider<GetTournamentListResponse>
    {
        public GetTournamentListResponse GetExamples()
        {
            return new GetTournamentListResponse
                   {
                       Tournaments = new List<GetTournamentResponse>
                                     {
                                         new GetTournamentResponse
                                         {
                                             HasResultBeenProduced = false,
                                             MeasuredCourseId = Guid.Parse("ED6AFEA6-CE03-46D4-9304-9CE3064606EB"),
                                             MeasuredCourseSSS = 70,
                                             TournamentFormat = TournamentFormat.StrokePlay,
                                             TournamentDate = new DateTime(2019, 7, 25),
                                             PlayersSignedUpCount = 5,
                                             TournamentId = Guid.Parse("030FAA48-AB63-4486-9A47-2C5B5E723A6A"),
                                             PlayerCategory = PlayerCategory.Gents,
                                             PlayersScoresRecordedCount = 4,
                                             MeasuredCourseName = "Test Course",
                                             MeasuredCourseTeeColour = "White",
                                             TournamentName = "Test Tournament"

                                         }
                                     }
                   };
        }
    }
}