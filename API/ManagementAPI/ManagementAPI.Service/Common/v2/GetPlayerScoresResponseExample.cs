using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class GetPlayerScoresResponseExample : IExamplesProvider<GetPlayerScoresResponse>
    {
        public GetPlayerScoresResponse GetExamples()
        {
            return new GetPlayerScoresResponse
                   {
                       Scores = new List<PlayerScoreResponse>
                                {
                                    new PlayerScoreResponse
                                    {
                                        CSS = 70,
                                        GolfClubId = Guid.Parse("BCE98CF4-F36F-4F1A-9F7C-BAB94A0D0409"),
                                        TournamentFormat = TournamentFormat.StrokePlay,
                                        MeasuredCourseId = Guid.Parse("A68BD2A8-CF3C-4A1F-9F0B-27B1E0464EC6"),
                                        TournamentDate = DateTime.Today.Date,
                                        TournamentName = "Test Tournament",
                                        MeasuredCourseName = "Test Course",
                                        TournamentId = Guid.Parse("29069580-AFC8-4036-AE50-A1BBE9DAE491"),
                                        PlayerId = Guid.Parse("28217725-0962-43F8-B3F4-83A007C70610"),
                                        MeasuredCourseTeeColour = "White",
                                        GolfClubName = "Test Golf Club",
                                        PlayingHandicap = 6,
                                        NetScore = 71,
                                        GrossScore = 77
                                    }
                                }
                   };
        }
    }
}
