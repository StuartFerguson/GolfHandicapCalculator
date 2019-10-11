using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class GetMembersHandicapListReportResponseExample :IExamplesProvider<GetMembersHandicapListReportResponse>
    {
        public GetMembersHandicapListReportResponse GetExamples()
        {
            return new GetMembersHandicapListReportResponse
                   {
                       GolfClubId = Guid.Parse("43456107-5D5E-4035-B29A-F5227C03F408"),
                       MembersHandicapListReportResponse = new List<MembersHandicapListReportResponse>
                                                           {
                                                               new MembersHandicapListReportResponse
                                                               {
                                                                   ExactHandicap = 0.0m,
                                                                   GolfClubId = Guid.Parse("43456107-5D5E-4035-B29A-F5227C03F408"),
                                                                   PlayerId = Guid.Parse("617933B2-9DD4-4262-8492-C6B6855D86B6"),
                                                                   PlayingHandicap = 0,
                                                                   PlayerName = "Test Player 1",
                                                                   HandicapCategory = 1
                                                               },
                                                               new MembersHandicapListReportResponse
                                                               {
                                                                   ExactHandicap = 6.0m,
                                                                   GolfClubId = Guid.Parse("43456107-5D5E-4035-B29A-F5227C03F408"),
                                                                   PlayerId = Guid.Parse("617933B2-9DD4-4262-8492-C6B6855D86B6"),
                                                                   PlayingHandicap = 6,
                                                                   PlayerName = "Test Player 2",
                                                                   HandicapCategory = 2
                                                               },
                                                               new MembersHandicapListReportResponse
                                                               {
                                                                   ExactHandicap = 14.0m,
                                                                   GolfClubId = Guid.Parse("43456107-5D5E-4035-B29A-F5227C03F408"),
                                                                   PlayerId = Guid.Parse("617933B2-9DD4-4262-8492-C6B6855D86B6"),
                                                                   PlayingHandicap = 14,
                                                                   PlayerName = "Test Player 3",
                                                                   HandicapCategory = 3
                                                               },
                                                               new MembersHandicapListReportResponse
                                                               {
                                                                   ExactHandicap = 21.0m,
                                                                   GolfClubId = Guid.Parse("43456107-5D5E-4035-B29A-F5227C03F408"),
                                                                   PlayerId = Guid.Parse("617933B2-9DD4-4262-8492-C6B6855D86B6"),
                                                                   PlayingHandicap = 21,
                                                                   PlayerName = "Test Player 4",
                                                                   HandicapCategory = 4
                                                               }
                                                           }
                   };
        }
    }
}
