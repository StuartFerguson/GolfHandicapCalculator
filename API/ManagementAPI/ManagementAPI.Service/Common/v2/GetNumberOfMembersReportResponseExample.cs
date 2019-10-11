using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common.v2
{
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    public class GetNumberOfMembersReportResponseExample :IExamplesProvider<GetNumberOfMembersReportResponse>
    {
        public GetNumberOfMembersReportResponse GetExamples()
        {
            return new GetNumberOfMembersReportResponse
                   {
                       GolfClubId = Guid.Parse("F8FD8055-D27C-4B2F-BD44-1E66764B9994"),
                       NumberOfMembers = 100
                   };
        }
    }
}
