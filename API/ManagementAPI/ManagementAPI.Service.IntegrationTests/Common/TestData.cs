namespace ManagementAPI.Service.IntegrationTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    public static class TestData
    {
        #region Fields

        public static String AddressLine1 = "Address Line 1";

        public static String AddressLine2 = "Address Line 2";

        public static String ConfirmPassword = "123456";

        public static String EmailAddress = "testclubadministrator@golfclub.co.uk";

        public static String FamilyName = "User";

        public static String GivenName = "Test";

        public static Guid GolfClubAdministratorUserId = Guid.Parse("0A05AD53-F7B1-4A18-A697-816DD32714B7");

        public static String GolfClubEmailAddress = "testclub@golfclub.co.uk";

        public static Guid GolfClubId = Guid.Parse("7698E8B3-1A94-46CC-B27E-6BCFCB8A1CF6");

        public static String GolfClubName = "Test Golf Club";

        public static String GolfClubTelephoneNumber = "1234567890";

        public static String GolfClubWebsite = "www.golfclub.co.uk";

        public static Guid MeasuredCourseId = Guid.Parse("B2F334C2-03D3-48DB-9C6F-45FB1133F071");

        public static String MeasuredCourseName = "Test Measured Course";

        public static String MemberName = "MemberName";

        public static String MembershipNumber = "000001";

        public static MembershipStatus MembershipStatus = MembershipStatus.Accepted;

        public static String MiddleName = string.Empty;

        public static String Password = "123456";

        public static String PlayerDateOfBirthString = "13/12/1980";

        public static String PlayerFullName = "MemberName";

        public static String PlayerGender = "M";

        public static Guid PlayerId = Guid.Parse("6E079AE9-156E-424D-A52B-1941B3D4AE9F");

        public static String PostalCode = "TE57 1NG";

        public static String Region = "Test Region";

        public static Int32 StandardScratchScore = 70;

        public static String TeeColour = "White";

        public static String TelephoneNumber = "1234567890";

        public static String Town = "Test Town";

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

        public static List<GetGolfClubResponse> GetGolfClubListResponse = new List<GetGolfClubResponse>
                                                                          {
                                                                              TestData.GetGolfClubResponse
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
                                                                                                          PlayerDateOfBirth = TestData.PlayerDateOfBirthString,
                                                                                                          MembershipNumber = TestData.MembershipNumber,
                                                                                                          PlayerFullName = TestData.PlayerFullName
                                                                                                      }
                                                                                                  };

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

        public static GetGolfClubUserListResponse GetGolfClubUserListResponse = new GetGolfClubUserListResponse
                                                                                {
                                                                                    Users = new List<GolfClubUserResponse>
                                                                                            {
                                                                                                new GolfClubUserResponse
                                                                                                {
                                                                                                    GivenName = TestData.RegisterClubAdministratorRequest.GivenName,
                                                                                                    FamilyName = TestData.RegisterClubAdministratorRequest.FamilyName,
                                                                                                    MiddleName = TestData.RegisterClubAdministratorRequest.MiddleName,
                                                                                                    UserName = TestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                                    Email = TestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                                    UserId = TestData.GolfClubAdministratorUserId,
                                                                                                    GolfClubId = TestData.GolfClubId,
                                                                                                    PhoneNumber = TestData
                                                                                                                  .RegisterClubAdministratorRequest.TelephoneNumber,
                                                                                                    UserType = "Club Administrator"
                                                                                                }
                                                                                            }
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

        public static DateTime MembershipAcceptedDateTime = new DateTime(2019, 10, 1);

        public static Guid MembershipId = Guid.Parse("0F9BA2A1-789A-4030-8095-228A825F0A9F");

        public static DateTime? MembershipRejectedDateTime = null;

        public static String MembershipRejectionReason = null;

        public static Int32 PlayerHandicapCategory = 2;

        public static Boolean PlayerHasBeenRegistered = true;

        public static Boolean ScoreEntered = true;

        public static DateTime TournamentDate = new DateTime(2019, 10, 12);

        public static TournamentFormat TournamentFormatStrokeplay = TournamentFormat.StrokePlay;

        public static Guid TournamentId = Guid.Parse("8A66BBEF-64BD-47BE-AADB-D8ACEAF90DA3");

        public static String TournamentName = "Test Tournament";

        public static AddTournamentDivisionToGolfClubRequest AddTournamentDivisionToGolfClubRequest = new AddTournamentDivisionToGolfClubRequest
                                                                                                      {
                                                                                                          Division = TestData.GetTournamentDivision1().Division,
                                                                                                          EndHandicap = TestData.GetTournamentDivision1().EndHandicap,
                                                                                                          StartHandicap = TestData.GetTournamentDivision1().StartHandicap
                                                                                                      };

        public static DateTime PlayerDateOfBirth = new DateTime(1980, 12, 31);

        public static String PlayerEmailAddress = "test@email.com";

        public static Decimal PlayerExactHandicap = 11.4m;

        public static String PlayerFirstName = "First";

        public static String PlayerLastName = "Last";

        public static String PlayerMiddleName = "Middle";

        public static Int32 PlayerPlayingHandicap = 11;

        public static RegisterPlayerRequest RegisterPlayerRequest = new RegisterPlayerRequest
                                                                    {
                                                                        DateOfBirth = TestData.PlayerDateOfBirth,
                                                                        EmailAddress = TestData.PlayerEmailAddress,
                                                                        GivenName = TestData.PlayerFirstName,
                                                                        MiddleName = TestData.PlayerMiddleName,
                                                                        FamilyName = TestData.PlayerLastName,
                                                                        Gender = TestData.PlayerGender,
                                                                        ExactHandicap = TestData.PlayerExactHandicap
                                                                    };

        public static PlayerCategory PlayerCategoryGents = PlayerCategory.Gents;

        public static Boolean TournamentHasResultBeenProduced = false;

        public static Boolean TournamentHasBeenCompleted = true;
        public static Int32 TournamentPlayersScoresRecordedCount=50;
        public static Int32 TournamentPlayersSignedUpCount = 50;

        public static Boolean TournamentHasBeenCancelled = false;

        public static GetTournamentListResponse GetTournamentListResponse = new GetTournamentListResponse
                                                                            {
                                                                                Tournaments = new List<GetTournamentResponse>
                                                                                              {
                                                                                                  new GetTournamentResponse
                                                                                                  {
                                                                                                      TournamentFormat = TestData.TournamentFormatStrokeplay,
                                                                                                      MeasuredCourseId = TestData.MeasuredCourseId,
                                                                                                      TournamentDate = TestData.TournamentDate,
                                                                                                      MeasuredCourseName = TestData.MeasuredCourseName,
                                                                                                      TournamentId = TestData.TournamentId,
                                                                                                      MeasuredCourseTeeColour = TestData.TeeColour,
                                                                                                      TournamentName = TestData.TournamentName,
                                                                                                      PlayerCategory = TestData.PlayerCategoryGents,
                                                                                                      HasResultBeenProduced = TestData.TournamentHasResultBeenProduced,
                                                                                                      HasBeenCompleted = TestData.TournamentHasBeenCompleted,
                                                                                                      PlayersScoresRecordedCount =
                                                                                                          TestData.TournamentPlayersScoresRecordedCount,
                                                                                                      PlayersSignedUpCount = TestData.TournamentPlayersSignedUpCount,
                                                                                                      HasBeenCancelled = TestData.TournamentHasBeenCancelled,
                                                                                                      MeasuredCourseSSS = TestData.StandardScratchScore
                                                                                                  }
                                                                                              }
                                                                            };

        public static TournamentPatchRequest TournamentPatchRequestCompleteTournament = new TournamentPatchRequest
                                                                                        {
                                                                                            Status = TournamentStatusUpdate.Complete
                                                                                        };

        public static String TournamentCancellationReason = "Cancelled";

        public static TournamentPatchRequest TournamentPatchRequestCancelTournament = new TournamentPatchRequest
                                                                                        {
                                                                                            Status = TournamentStatusUpdate.Cancel,
                                                                                            CancellationReason = TestData.TournamentCancellationReason
        };

        public static TournamentPatchRequest TournamentPatchRequestProduceTournamentResult = new TournamentPatchRequest
                                                                                        {
                                                                                            Status = TournamentStatusUpdate.ProduceResult
                                                                                        };

        public static Int32 TournamentMemberCategory = 1;

        public static CreateTournamentRequest CreateTournamentRequest = new CreateTournamentRequest
                                                                        {
                                                                            Format = (Int32)TestData.TournamentFormatStrokeplay,
                                                                            MeasuredCourseId = TestData.MeasuredCourseId,
                                                                            TournamentDate = TestData.TournamentDate,
                                                                            Name = TestData.TournamentName,
                                                                            MemberCategory = TournamentMemberCategory
                                                                        };

        public static CancelTournamentRequest CancelTournamentRequest = new CancelTournamentRequest
                                                                        {
                                                                            CancellationReason = TestData.TournamentCancellationReason
                                                                        };

        public static Dictionary<Int32, Int32> HoleScores = new Dictionary<Int32, Int32>()
                                                            {
                                                                {1, 4}, {2, 4}, {3, 3}, {4, 4}, {5, 4}, {6, 5}, {7, 3}, {8, 4}, {9, 3},
                                                                {10, 4}, {11, 4}, {12, 4}, {13, 5}, {14, 3}, {15, 4}, {16, 4}, {17, 4}, {18, 4}
                                                            };

        public static RecordPlayerTournamentScoreRequest RecordPlayerTournamentScoreRequest = new RecordPlayerTournamentScoreRequest
                                                                                              {
                                                                                                  HoleScores = TestData.HoleScores
                                                                                              };

        private static Int32 CSS = 70;

        private static Int32 NetScore = 71;

        private static Int32 GrossScore = 82;

        #endregion

        public static GetMembersHandicapListReportResponse GetMembersHandicapListReportResponse()
        {
            return new GetMembersHandicapListReportResponse
                   {
                       GolfClubId = TestData.GolfClubId,
                       MembersHandicapListReportResponse = new List<MembersHandicapListReportResponse>
                                                           {
                                                               new MembersHandicapListReportResponse
                                                               {
                                                                   GolfClubId = TestData.GolfClubId,
                                                                   PlayerId = TestData.PlayerId,
                                                                   PlayingHandicap = TestData.PlayerPlayingHandicap,
                                                                   HandicapCategory = TestData.PlayerHandicapCategory,
                                                                   ExactHandicap = TestData.PlayerExactHandicap,
                                                                   PlayerName = TestData.PlayerFullName,
                                                               }
                                                           }
                   };
        }

        #region Methods

        public static GetPlayerDetailsResponse GetPlayerDetailsResponse()
        {
            return new GetPlayerDetailsResponse
                   {
                       EmailAddress = TestData.PlayerEmailAddress,
                       PlayingHandicap = TestData.PlayerPlayingHandicap,
                       MiddleName = TestData.PlayerMiddleName,
                       ExactHandicap = TestData.PlayerExactHandicap,
                       FullName = TestData.PlayerFullName,
                       HandicapCategory = TestData.PlayerHandicapCategory,
                       Gender = TestData.PlayerGender,
                       DateOfBirth = TestData.PlayerDateOfBirth,
                       FirstName = TestData.PlayerFirstName,
                       LastName = TestData.PlayerLastName,
                       HasBeenRegistered = TestData.PlayerHasBeenRegistered
                   };
        }

        public static List<ClubMembershipResponse> GetPlayersClubMembershipsResponse()
        {
            return new List<ClubMembershipResponse>
                   {
                       new ClubMembershipResponse
                       {
                           GolfClubId = TestData.GolfClubId,
                           GolfClubName = TestData.GolfClubName,
                           MembershipId = TestData.MembershipId,
                           Status = TestData.MembershipStatus,
                           MembershipNumber = TestData.MembershipNumber,
                           AcceptedDateTime = TestData.MembershipAcceptedDateTime,
                           RejectionReason = TestData.MembershipRejectionReason,
                           RejectedDateTime = TestData.MembershipRejectedDateTime
                       }
                   };
        }

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

        public static PlayerSignedUpTournamentsResponse PlayerSignedUpTournamentsResponse()
        {
            return new PlayerSignedUpTournamentsResponse
                   {
                       PlayerSignedUpTournaments = new List<PlayerSignedUpTournament>
                                                   {
                                                       new PlayerSignedUpTournament
                                                       {
                                                           GolfClubId = TestData.GolfClubId,
                                                           GolfClubName = TestData.GolfClubName,
                                                           MeasuredCourseId = TestData.MeasuredCourseId,
                                                           TournamentFormat = TestData.TournamentFormatStrokeplay,
                                                           PlayerId = TestData.PlayerId,
                                                           TournamentDate = TestData.TournamentDate,
                                                           MeasuredCourseName = TestData.MeasuredCourseName,
                                                           TournamentId = TestData.TournamentId,
                                                           MeasuredCourseTeeColour = TestData.TeeColour,
                                                           TournamentName = TestData.TournamentName,
                                                           ScoreEntered = TestData.ScoreEntered
                                                       }
                                                   }
                   };
        }

        #endregion

        public static GetNumberOfMembersByAgeCategoryReportResponse GetNumberOfMembersByAgeCategoryReportResponse()
        {
            return new GetNumberOfMembersByAgeCategoryReportResponse
                   {
                       GolfClubId = TestData.GolfClubId,
                       MembersByAgeCategoryResponse = new List<MembersByAgeCategoryResponse>
                                                      {
                                                          new MembersByAgeCategoryResponse
                                                          {
                                                              NumberOfMembers = 100,
                                                              AgeCategory = "Gents"
                                                          }
                                                      }
                   };
        }

        public static GetNumberOfMembersByHandicapCategoryReportResponse GetNumberOfMembersByHandicapCategoryReportResponse()
        {
            return new GetNumberOfMembersByHandicapCategoryReportResponse
                   {
                       GolfClubId = TestData.GolfClubId,
                       MembersByHandicapCategoryResponse = new List<MembersByHandicapCategoryResponse>
                                                           {
                                                               new MembersByHandicapCategoryResponse
                                                               {
                                                                   NumberOfMembers = 10,
                                                                   HandicapCategory = 1
                                                               },
                                                               new MembersByHandicapCategoryResponse
                                                               {
                                                                   NumberOfMembers = 20,
                                                                   HandicapCategory = 2
                                                               }
                                                           }
                   };
        }

        public static GetNumberOfMembersByTimePeriodReportResponse GetNumberOfMembersByTimePeriodReportDayResponse()
        {
            return new GetNumberOfMembersByTimePeriodReportResponse
                   {
                GolfClubId = TestData.GolfClubId,
                TimePeriod = TimePeriod.Day,
                MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>
                                              {
                                                  new MembersByTimePeriodResponse
                                                  {
                                                      NumberOfMembers = 100,
                                                      Period = "2019-10-11"
                                                  }
                                              }
                   };
        }

        public static GetNumberOfMembersByTimePeriodReportResponse GetNumberOfMembersByTimePeriodReportMonthResponse()
        {
            return new GetNumberOfMembersByTimePeriodReportResponse
                   {
                       GolfClubId = TestData.GolfClubId,
                       TimePeriod = TimePeriod.Month,
                       MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>
                                                     {
                                                         new MembersByTimePeriodResponse
                                                         {
                                                             NumberOfMembers = 100,
                                                             Period = "2019-10"
                                                         }
                                                     }
                   };
        }

        public static GetNumberOfMembersByTimePeriodReportResponse GetNumberOfMembersByTimePeriodReportYearResponse()
        {
            return new GetNumberOfMembersByTimePeriodReportResponse
                   {
                       GolfClubId = TestData.GolfClubId,
                       TimePeriod = TimePeriod.Year,
                       MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>
                                                     {
                                                         new MembersByTimePeriodResponse
                                                         {
                                                             NumberOfMembers = 100,
                                                             Period = "2019"
                                                         }
                                                     }
                   };
        }

        public static GetNumberOfMembersReportResponse GetNumberOfMembersReportResponse()
        {
            return new GetNumberOfMembersReportResponse
                   {
                       GolfClubId = TestData.GolfClubId,
                       NumberOfMembers = 100
                   };
        }

        public static GetPlayerScoresResponse GetPlayerScoresResponse()
        {
            return new GetPlayerScoresResponse
            {
                Scores = new List<PlayerScoreResponse>
                         {
                             new PlayerScoreResponse
                             {
                                 MeasuredCourseId = TestData.MeasuredCourseId,
                                 TournamentFormat = TestData.TournamentFormatStrokeplay,
                                 TournamentDate = TestData.TournamentDate,
                                 GolfClubId = TestData.GolfClubId,
                                 TournamentName = TestData.TournamentName,
                                 MeasuredCourseName = TestData.MeasuredCourseName,
                                 TournamentId = TestData.TournamentId,
                                 MeasuredCourseTeeColour = TestData.TeeColour,
                                 PlayerId = TestData.PlayerId,
                                 GolfClubName = TestData.GolfClubName,
                                 CSS = TestData.CSS,
                                 NetScore = TestData.NetScore,
                                 GrossScore = TestData.GrossScore,
                                 PlayingHandicap = TestData.PlayerPlayingHandicap
                             }
                         }
            };
        }
    }
}