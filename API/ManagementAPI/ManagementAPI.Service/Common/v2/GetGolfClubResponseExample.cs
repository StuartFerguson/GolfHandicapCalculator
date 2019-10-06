namespace ManagementAPI.Service.Common.v2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using DataTransferObjects.Responses.v2;
    using Swashbuckle.AspNetCore.Filters;

    [ExcludeFromCodeCoverage]
    public class GetGolfClubResponseExample : IExamplesProvider<GetGolfClubResponse>
    {
        #region Methods

        public GetGolfClubResponse GetExamples()
        {
            return new GetGolfClubResponse
                   {
                       Name = "Test Golf Club",
                       EmailAddress = "testemail@golfclub.com",
                       TelephoneNumber = "1234567890",
                       AddressLine1 = "Address Line 1",
                       AddressLine2 = string.Empty,
                       Id = Guid.Parse("F303BB78-40FF-495B-A21B-8AF136934CEB"),
                       PostalCode = "TE57 1NG",
                       Region = "TestRegion",
                       Town = "TestTown",
                       Website = "www.golfclub.com",
                       MeasuredCourses = new List<MeasuredCourseListResponse>
                                         {
                                             new MeasuredCourseListResponse
                                             {
                                                 MeasuredCourseId = Guid.Parse("D4F0BC0C-8E56-4EEE-8F5C-D74A56431957"),
                                                 Name = "Test Course Name",
                                                 TeeColour = "White",
                                                 StandardScratchScore = 70
                                             }
                                         },
                       Users = new List<GolfClubUserResponse>
                               {
                                   new GolfClubUserResponse
                                   {
                                       FamilyName = "FamilyName",
                                       GivenName = "GivenName",
                                       MiddleName = String.Empty,
                                       UserId = Guid.Parse("36B1F497-510C-4542-B052-BFC64CA3FAD3"),
                                       PhoneNumber = "1234567890",
                                       UserName = "testuser@testclub.co.uk",
                                       Email = "testuser@testclub.co.uk",
                                       UserType = "Golf Club Administrator"
                                   }
                               },
                       GolfClubMembershipDetailsResponseList = new List<GolfClubMembershipDetailsResponse>
                                                               {
                                                                   new GolfClubMembershipDetailsResponse
                                                                   {
                                                                       PlayerId = Guid.Parse("8D8C291F-6601-463D-84DA-8C56AFAD47A4"),
                                                                       Name = "Test Player",
                                                                       MembershipStatus = MembershipStatus.Accepted,
                                                                       PlayerGender = "M",
                                                                       PlayerDateOfBirth = "13/12/1980",
                                                                       MembershipNumber = "000001",
                                                                       PlayerFullName = "Test Player"

                                                                   }
                                                               }
                   };
        }

        #endregion
    }
}