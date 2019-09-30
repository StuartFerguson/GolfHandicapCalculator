namespace ManagementAPI.Service.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class ClubMembershipListResponseExample : IExamplesProvider<List<ClubMembershipResponse>>
    {
        #region Methods

        public List<ClubMembershipResponse> GetExamples()
        {
            return new List<ClubMembershipResponse>
                   {
                       new ClubMembershipResponse
                       {
                           MembershipNumber = "000001",
                           AcceptedDateTime = DateTime.Now.Date,
                           GolfClubId = Guid.Parse("65DB9360-06A0-48D3-AE99-B927B7AA15AA"),
                           MembershipId = Guid.Parse("A9FF899A-84EB-4CD3-B735-CA8FB15F5283"),
                           RejectionReason = string.Empty,
                           RejectedDateTime = DateTime.MinValue,
                           Status = MembershipStatus.Accepted
                       }
                   };
        }

        #endregion
    }
}