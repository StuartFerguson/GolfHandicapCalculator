using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class GetNumberOfMembersByAgeCategoryReportResponseExample : IExamplesProvider<GetNumberOfMembersByAgeCategoryReportResponse>
    {
        public GetNumberOfMembersByAgeCategoryReportResponse GetExamples()
        {
            return new GetNumberOfMembersByAgeCategoryReportResponse
                   {
                       GolfClubId = Guid.Parse("2FD302B4-4B41-4F60-ADB4-8664520544FD"),
                       MembersByAgeCategoryResponse = new List<MembersByAgeCategoryResponse>
                                                      {
                                                          new MembersByAgeCategoryResponse
                                                          {
                                                              AgeCategory = "Youth",
                                                              NumberOfMembers = 100
                                                          }
                                                      }
                   };
        }
    }
}
