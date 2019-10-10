using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;
    using ClubMembershipResponse = DataTransferObjects.Responses.v2.ClubMembershipResponse;
    using MembershipStatus = DataTransferObjects.Responses.v2.MembershipStatus;
    using TournamentFormat = DataTransferObjects.Responses.v2.TournamentFormat;

    public class GetPlayerResponseExample : IExamplesProvider<GetPlayerResponse>
    {
        #region Methods

        public GetPlayerResponse GetExamples()
        {
            return new GetPlayerResponse
            {
                       DateOfBirth = DateTime.Now.Date,
                       EmailAddress = "testplayer@playeremail.com",
                       ExactHandicap = 5.9m,
                       FirstName = "Test",
                       LastName = "Player",
                       MiddleName = string.Empty,
                       Gender = "M",
                       FullName = "Test Player",
                       HasBeenRegistered = true,
                       HandicapCategory = 2,
                       PlayingHandicap = 6,
                       SignedUpTournaments = new List<SignedUpTournamentResponse>
                                             {
                                                 new SignedUpTournamentResponse
                                                 {
                                                     GolfClubId = Guid.Parse("973A3527-BE2B-4CC1-9CCB-CB8120FEFF3F"),
                                                     MeasuredCourseId = Guid.Parse("7DDB1A8E-D792-48DD-82D5-40721AB965D7"),
                                                     TournamentFormat = TournamentFormat.StrokePlay,
                                                     PlayerId = Guid.Parse("9A0BC5AB-E761-481B-ACC8-AFD6C4EB13F0"),
                                                     TournamentDate = DateTime.Now.Date,
                                                     MeasuredCourseName = "Test Course",
                                                     GolfClubName = "Test Club",
                                                     TournamentId = Guid.Parse("C46236A3-08E5-4714-8B9D-13B2EB283FB7"),
                                                     MeasuredCourseTeeColour = "White",
                                                     TournamentName = "Test Tournament",
                                                     ScoreEntered = false
                                                 }
                                             },
                       ClubMemberships = new List<ClubMembershipResponse>
                                         {
                                             new ClubMembershipResponse
                                             {
                                                 GolfClubId = Guid.Parse("973A3527-BE2B-4CC1-9CCB-CB8120FEFF3F"),
                                                 GolfClubName = "Test Club",
                                                 Status = MembershipStatus.Accepted,
                                                 MembershipId = Guid.Parse("3F45880C-2796-49B7-A8BE-54DDA8EDB516"),
                                                 MembershipNumber = "000001",
                                                 AcceptedDateTime = DateTime.Now.AddDays(30),
                                                 RejectionReason = null,
                                                 RejectedDateTime = null
                                             }
                                         }
                   };
        }

        #endregion
    }
}
