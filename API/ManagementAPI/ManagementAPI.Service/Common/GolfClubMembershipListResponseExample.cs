namespace ManagementAPI.Service.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GolfClubMembershipListResponseExample : IExamplesProvider
    {
        #region Methods

        public Object GetExamples()
        {
            return new List<GetGolfClubMembershipDetailsResponse>
                   {
                       new GetGolfClubMembershipDetailsResponse
                       {
                           PlayerId = Guid.Parse("66B7C12E-A5A7-4DEB-8DB0-5632092AB5C6"),
                           MembershipNumber = "000001",
                           GolfClubId = Guid.Parse("4A82201B-6BF8-4F54-87CC-2C93C7FEEDCA"),
                           PlayerGender = "M",
                           PlayerDateOfBirth = "01/01/2000",
                           PlayerFullName = "Test Player",
                           MembershipStatus = MembershipStatus.Accepted,
                           Name = "Test Club"
                       }
                   };
        }

        #endregion
    }
}