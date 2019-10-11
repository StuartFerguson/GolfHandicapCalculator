using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class GetNumberOfMembersByTimePeriodReportResponseExample :IExamplesProvider<GetNumberOfMembersByTimePeriodReportResponse>
    {
        public GetNumberOfMembersByTimePeriodReportResponse GetExamples()
        {
            return new GetNumberOfMembersByTimePeriodReportResponse
                   {
                       GolfClubId = Guid.Parse("2349EEF6-5D0C-4CB4-A896-DB7B10F2E573"),
                       TimePeriod = TimePeriod.Day,
                       MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>
                                                     {
                                                         new MembersByTimePeriodResponse
                                                         {
                                                             NumberOfMembers = 100,
                                                             Period = "2019-10-11"
                                                         },
                                                         new MembersByTimePeriodResponse
                                                         {
                                                             NumberOfMembers = 78,
                                                             Period = "2019-10-10"
                                                         },
                                                         new MembersByTimePeriodResponse
                                                         {
                                                             NumberOfMembers = 55,
                                                             Period = "2019-10-09"
                                                         }
                                                     }
                   };
        }
    }
}
