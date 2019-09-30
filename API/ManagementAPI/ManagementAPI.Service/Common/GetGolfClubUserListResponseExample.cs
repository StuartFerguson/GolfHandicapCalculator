namespace ManagementAPI.Service.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GetGolfClubUserListResponseExample : IExamplesProvider<GetGolfClubUserListResponse>
    {
        public GetGolfClubUserListResponse GetExamples()
        {
            return new GetGolfClubUserListResponse
                   {
                       Users = new List<GolfClubUserResponse>
                               {
                                   new GolfClubUserResponse
                                   {
                                       Email = "testadminuser@golfcub.com",
                                       MiddleName = "",
                                       FamilyName = "Admin",
                                       GivenName = "Test",
                                       UserName = "testadminuser@golfcub.com",
                                       UserId = Guid.Parse("A8B92293-F6A3-4481-83CB-BEC5F9313DBB"),
                                       GolfClubId = Guid.Parse("A4701B76-037A-4CF3-91CC-8BBC550C20F4"),
                                       PhoneNumber = null,
                                       UserType = "Club Administrator"
                                   },
                                   new GolfClubUserResponse
                                   {
                                       Email = "testmatchsecretary@golfcub.com",
                                       MiddleName = "",
                                       FamilyName = "Match Secretary",
                                       GivenName = "Test",
                                       UserName = "testmatchsecretary@golfcub.com",
                                       UserId = Guid.Parse("F615946C-E694-43F6-830D-EA6FABFB2975"),
                                       GolfClubId = Guid.Parse("A4701B76-037A-4CF3-91CC-8BBC550C20F4"),
                                       PhoneNumber = null,
                                       UserType = "Match Secretary"
                                   }
                               }
                   };
        }
    }
}