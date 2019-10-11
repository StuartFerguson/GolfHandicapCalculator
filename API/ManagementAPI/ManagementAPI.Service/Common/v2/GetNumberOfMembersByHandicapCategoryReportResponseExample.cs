using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class GetNumberOfMembersByHandicapCategoryReportResponseExample : IExamplesProvider<GetNumberOfMembersByHandicapCategoryReportResponse>
    {
        public GetNumberOfMembersByHandicapCategoryReportResponse GetExamples()
        {
            return new GetNumberOfMembersByHandicapCategoryReportResponse
                   {
                       GolfClubId = Guid.Parse("C366062E-4287-45B8-9DB4-8414632B3A54"),
                       MembersByHandicapCategoryResponse = new List<MembersByHandicapCategoryResponse>
                       {
                           new MembersByHandicapCategoryResponse
                           {
                               NumberOfMembers = 100,
                               HandicapCategory = 1
                           }
                       }
                   };
        }
    }
}
