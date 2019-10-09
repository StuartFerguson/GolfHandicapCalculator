namespace ManagementAPI.Service.Common.v2
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class RequestClubMembershipResponseExample : IExamplesProvider<RequestClubMembershipResponse>
    {
        public RequestClubMembershipResponse GetExamples()
        {
            return new RequestClubMembershipResponse
                   {
                       GolfClubId = Guid.Parse("BC40C9C9-E031-4586-8626-904FB0A1F56D"),
                       PlayerId = Guid.Parse("EAFEC24D-DC5F-4728-AEC7-0A13619EFD03"),
                       MembershipId = Guid.Empty
                   };
        }
    }
}