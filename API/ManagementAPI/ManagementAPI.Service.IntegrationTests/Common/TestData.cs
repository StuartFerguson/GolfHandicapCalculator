namespace ManagementAPI.Service.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    public static class TestData
    {
        private static String ConfirmPassword = "123456";

        private static String EmailAddress = "testclubadministrator@golfclub.co.uk";

        private static String TelephoneNumber = "1234567890";

        private static String FamilyName = "User";

        private static String GivenName = "Test";

        private static String MiddleName = String.Empty;

        private static String Password = "123456";
        
        public static Guid GolfClubAdministratorUserId = Guid.Parse("0A05AD53-F7B1-4A18-A697-816DD32714B7");

        public static Guid GolfClubId = Guid.Parse("7698E8B3-1A94-46CC-B27E-6BCFCB8A1CF6");

        private static String AddressLine1 = "Address Line 1";

        private static String GolfClubEmailAddress = "testclub@golfclub.co.uk";

        private static String AddressLine2 = "Address Line 2";

        private static String GolfClubName = "Test Golf Club";

        private static String GolfClubTelephoneNumber = "1234567890";

        private static String Town = "Test Town";

        private static String GolfClubWebsite ="www.golfclub.co.uk";

        private static String Region = "Test Region";

        private static String PostalCode = "TE57 1NG";

        private static String MemberName = "MemberName";

        private static Guid PlayerId = Guid.Parse("6E079AE9-156E-424D-A52B-1941B3D4AE9F");

        private static MembershipStatus MembershipStatus = MembershipStatus.Accepted;

        private static String PlayerGender = "M";

        private static String PlayerDateOfBirth = "13/12/1980";

        private static String MembershipNumber = "000001";

        private static String PlayerFullName = "MemberName";

        public static Guid MeasuredCourseId = Guid.Parse("B2F334C2-03D3-48DB-9C6F-45FB1133F071");

        public static String MeasuredCourseName = "Test Measured Course";

        public static Int32 StandardScratchScore = 70;

        public static String TeeColour = "White";

        public static CreateGolfClubRequest CreateGolfClubRequest = new CreateGolfClubRequest
                                                                    {
                                                                        AddressLine1 = TestData.AddressLine1,
                                                                        EmailAddress = TestData.GolfClubEmailAddress,
                                                                        AddressLine2 = TestData.AddressLine2,
                                                                        Name = TestData.GolfClubName,
                                                                        TelephoneNumber = TestData.GolfClubTelephoneNumber,
                                                                        Town = TestData.Town,
                                                                        Website = TestData.GolfClubWebsite,
                                                                        Region = TestData.Region,
                                                                        PostalCode = TestData.PostalCode,
                                                                        TournamentDivisions = new List<TournamentDivisionRequest>
                                                                                              {
                                                                                                  TestData.GetTournamentDivision1(),
                                                                                                  TestData.GetTournamentDivision2(),
                                                                                                  TestData.GetTournamentDivision3(),
                                                                                                  TestData.GetTournamentDivision4()
                                                                                              }
                                                                    };

        public static GetGolfClubResponse GetGolfClubResponse = new GetGolfClubResponse
                                                                {
                                                                    Id = TestData.GolfClubId,
                                                                    AddressLine1 = TestData.AddressLine1,
                                                                    Name = TestData.GolfClubName,
                                                                    AddressLine2 = TestData.AddressLine2,
                                                                    EmailAddress = TestData.GolfClubEmailAddress,
                                                                    PostalCode = TestData.PostalCode,
                                                                    Region = TestData.Region,
                                                                    TelephoneNumber = TestData.GolfClubTelephoneNumber,
                                                                    Town = TestData.Town,
                                                                    Website = TestData.GolfClubWebsite
                                                                };

        public static List<GetGolfClubMembershipDetailsResponse> GetGolfClubMembersListResponse = new List<GetGolfClubMembershipDetailsResponse>
                                                                                                  {
                                                                                                      new GetGolfClubMembershipDetailsResponse
                                                                                                      {
                                                                                                          GolfClubId = TestData.GolfClubId,
                                                                                                          Name = TestData.MemberName,
                                                                                                          PlayerId = TestData.PlayerId,
                                                                                                          MembershipStatus = TestData.MembershipStatus,
                                                                                                          PlayerGender = TestData.PlayerGender,
                                                                                                          PlayerDateOfBirth = TestData.PlayerDateOfBirth,
                                                                                                          MembershipNumber = TestData.MembershipNumber,
                                                                                                          PlayerFullName = TestData.PlayerFullName
                                                                                                      }
                                                                                                  };

        public static AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest = new AddMeasuredCourseToClubRequest
                                                                                      {
                                                                                          MeasuredCourseId = TestData.MeasuredCourseId,
                                                                                          Name = TestData.MeasuredCourseName,
                                                                                          TeeColour = TestData.TeeColour,
                                                                                          StandardScratchScore = TestData.StandardScratchScore,
                                                                                          Holes = new List<HoleDataTransferObjectRequest>
                                                                                                  {
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 1,
                                                                                                              LengthInYards = 348,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 10
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 2,
                                                                                                              LengthInYards = 402,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 4
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 3,
                                                                                                              LengthInYards = 207,
                                                                                                              Par = 3,
                                                                                                              StrokeIndex = 14
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 4,
                                                                                                              LengthInYards = 405,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 8
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 5,
                                                                                                              LengthInYards = 428,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 2
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 6,
                                                                                                              LengthInYards = 477,
                                                                                                              Par = 5,
                                                                                                              StrokeIndex = 12
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 7,
                                                                                                              LengthInYards = 186,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 16
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 8,
                                                                                                              LengthInYards = 397,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 6
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 9,
                                                                                                              LengthInYards = 130,
                                                                                                              Par = 3,
                                                                                                              StrokeIndex = 18
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 10,
                                                                                                              LengthInYards = 399,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 3
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 11,
                                                                                                              LengthInYards = 401,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 13
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 12,
                                                                                                              LengthInYards = 421,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 1
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 13,
                                                                                                              LengthInYards = 530,
                                                                                                              Par = 5,
                                                                                                              StrokeIndex = 11
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 14,
                                                                                                              LengthInYards = 196,
                                                                                                              Par = 3,
                                                                                                              StrokeIndex = 5
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 15,
                                                                                                              LengthInYards = 355,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 7
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 16,
                                                                                                              LengthInYards = 243,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 15
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 16,
                                                                                                              LengthInYards = 243,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 15
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 17,
                                                                                                              LengthInYards = 286,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 17
                                                                                                          },
                                                                                                          new HoleDataTransferObjectRequest
                                                                                                          {
                                                                                                              HoleNumber = 18,
                                                                                                              LengthInYards = 399,
                                                                                                              Par = 4,
                                                                                                              StrokeIndex = 9
                                                                                                          }
                                                                                                  }
                                                                                      };    
                                                                                                  
        public static TournamentDivisionRequest GetTournamentDivision1()
        {
            return new TournamentDivisionRequest
            {
                Division = 1,
                StartHandicap = 0,
                EndHandicap = 5
            };
        }

        public static TournamentDivisionRequest GetTournamentDivision2()
        {
            return new TournamentDivisionRequest
            {
                Division = 2,
                StartHandicap = 6,
                EndHandicap = 12
            };
        }

        public static TournamentDivisionRequest GetTournamentDivision3()
        {
            return new TournamentDivisionRequest
            {
                Division = 3,
                StartHandicap = 13,
                EndHandicap = 21
            };
        }

        public static TournamentDivisionRequest GetTournamentDivision4()
        {
            return new TournamentDivisionRequest
            {
                Division = 4,
                StartHandicap = 22,
                EndHandicap = 28
            };
        }

        public static RegisterClubAdministratorRequest RegisterClubAdministratorRequest = new RegisterClubAdministratorRequest
        {
            ConfirmPassword = TestData.ConfirmPassword,
            EmailAddress = TestData.EmailAddress,
            TelephoneNumber = TestData.TelephoneNumber,
            FamilyName = TestData.FamilyName,
            GivenName = TestData.GivenName,
            MiddleName = TestData.MiddleName,
            Password = TestData.Password
        };

        public static GetMeasuredCourseListResponse GetMeasuredCourseListResponse = new GetMeasuredCourseListResponse
                                                                                    {
                                                                                        GolfClubId = TestData.GolfClubId,
                                                                                        MeasuredCourses = new List<MeasuredCourseListResponse>
                                                                                                          {
                                                                                                              new MeasuredCourseListResponse
                                                                                                              {
                                                                                                                  StandardScratchScore = TestData.StandardScratchScore,
                                                                                                                  TeeColour = TestData.TeeColour,
                                                                                                                  Name = TestData.MeasuredCourseName,
                                                                                                                  MeasuredCourseId = TestData.MeasuredCourseId
                                                                                                              }
                                                                                                          }
                                                                                    };

        public static GetGolfClubUserListResponse GetGolfClubUserListResponse = new GetGolfClubUserListResponse
        {
            Users = new List<GolfClubUserResponse>
                                                                                            {
                                                                                                new GolfClubUserResponse
                                                                                                {
                                                                                                    GivenName = TestData
                                                                                                                .RegisterClubAdministratorRequest.GivenName,
                                                                                                    FamilyName = TestData
                                                                                                                 .RegisterClubAdministratorRequest.FamilyName,
                                                                                                    MiddleName = TestData
                                                                                                                 .RegisterClubAdministratorRequest.MiddleName,
                                                                                                    UserName = TestData
                                                                                                               .RegisterClubAdministratorRequest.EmailAddress,
                                                                                                    Email =
                                                                                                        TestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                                    UserId = TestData.GolfClubAdministratorUserId,
                                                                                                    GolfClubId = TestData.GolfClubId,
                                                                                                    PhoneNumber = TestData
                                                                                                                  .RegisterClubAdministratorRequest.TelephoneNumber,
                                                                                                    UserType = "Club Administrator"
                                                                                                }
                                                                                            }
        };

        public static List<GetGolfClubResponse> GetGolfClubListResponse = new List<GetGolfClubResponse>
                                                                          {
                                                                              TestData.GetGolfClubResponse
                                                                          };

        public static CreateMatchSecretaryRequest CreateMatchSecretaryRequest = new CreateMatchSecretaryRequest
                                                                                {
                                                                                    EmailAddress = "testmatchsecretary@test.co.uk",
                                                                                    Password = "123456",
                                                                                    ConfirmPassword = "123456",
                                                                                    TelephoneNumber = "123456789",
                                                                                    GivenName = "Test",
                                                                                    MiddleName = null,
                                                                                    FamilyName = "Match Secretary"
                                                                                };

        public static AddTournamentDivisionToGolfClubRequest AddTournamentDivisionToGolfClubRequest = new AddTournamentDivisionToGolfClubRequest
                                                                                                      {
                                                                                                          Division = TestData.GetTournamentDivision1().Division,
                                                                                                          EndHandicap = TestData.GetTournamentDivision1().EndHandicap,
                                                                                                          StartHandicap = TestData.GetTournamentDivision1().StartHandicap
                                                                                                      };
    }
}