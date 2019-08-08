namespace ManagementAPI.Service.Tests.GolfClub
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BusinessLogic.Commands;
    using BusinessLogic.Services.ExternalServices.DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using IdentityModel;
    using ManagementAPI.GolfClub;
    using ManagementAPI.GolfClub.DomainEvents;
    using ManagementAPI.GolfClubMembership.DomainEvents;
    using Tournament;

    public class GolfClubTestData
    {
        #region Fields

        public static Guid MeasuredCourseId = Guid.Parse("B2F334C2-03D3-48DB-9C6F-45FB1133F071");

        public static String MeasuredCourseName = "Test Measured Course";

        public static Int32 StandardScratchScore = 70;

        public static String TeeColour = "White";

        public static AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest = new AddMeasuredCourseToClubRequest
                                                                                      {
                                                                                          MeasuredCourseId = GolfClubTestData.MeasuredCourseId,
                                                                                          Name = GolfClubTestData.MeasuredCourseName,
                                                                                          StandardScratchScore = GolfClubTestData.StandardScratchScore,
                                                                                          TeeColour = GolfClubTestData.TeeColour,
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

        public static String AddressLine1 = "Address Line 1";

        public static String AddressLine2 = "Address Line 2";

        public static Int32 Division = 1;

        public static Int32 EndHandicap = 28;

        public static Int32 StartHandicap = 0;

        public static AddTournamentDivisionToGolfClubRequest AddTournamentDivisionToGolfClubRequest = new AddTournamentDivisionToGolfClubRequest
                                                                                                      {
                                                                                                          Division = GolfClubTestData.Division,
                                                                                                          StartHandicap = GolfClubTestData.StartHandicap,
                                                                                                          EndHandicap = GolfClubTestData.EndHandicap
                                                                                                      };

        public static Guid AggregateId = Guid.Parse("CD64A469-9593-49D6-988D-3842C532D23E");

        public static String EmailAddress = "1@2.com";

        public static String Name = "Test Club Name";

        public static String PostalCode = "TE57 1NG";

        public static String Region = "Test Region";

        public static String TelephoneNumber = "123456789";

        public static String Town = "Test Town";

        public static String Website = "www.website.com";

        public static CreateGolfClubRequest CreateGolfClubRequest = new CreateGolfClubRequest
                                                                    {
                                                                        Name = GolfClubTestData.Name,
                                                                        AddressLine1 = GolfClubTestData.AddressLine1,
                                                                        EmailAddress = GolfClubTestData.EmailAddress,
                                                                        PostalCode = GolfClubTestData.PostalCode,
                                                                        Town = GolfClubTestData.Town,
                                                                        Website = GolfClubTestData.Website,
                                                                        Region = GolfClubTestData.Region,
                                                                        TelephoneNumber = GolfClubTestData.TelephoneNumber,
                                                                        AddressLine2 = GolfClubTestData.AddressLine2
                                                                    };

        public static CreateGolfClubResponse CreateGolfClubResponse = new CreateGolfClubResponse
                                                                      {
                                                                          GolfClubId = GolfClubTestData.AggregateId
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

        public static DateTime MembershipRequestedDateAndTime = new DateTime(2019, 1, 1);

        public static Guid PlayerId = Guid.Parse("357B70E6-8810-40FB-A6AD-9D193D4F6376");

        public static List<GetClubMembershipRequestResponse> GetClubMembershipRequestResponse = new List<GetClubMembershipRequestResponse>
                                                                                                {
                                                                                                    new GetClubMembershipRequestResponse
                                                                                                    {
                                                                                                        ClubId = GolfClubTestData.AggregateId,
                                                                                                        PlayerId = GolfClubTestData.PlayerId,
                                                                                                        MembershipRequestedDateAndTime =
                                                                                                            GolfClubTestData.MembershipRequestedDateAndTime,
                                                                                                        Status = 0 // Pending
                                                                                                    }
                                                                                                };

        public static List<GetGolfClubResponse> GetGolfClubListResponse = new List<GetGolfClubResponse>
                                                                          {
                                                                              new GetGolfClubResponse
                                                                              {
                                                                                  Id = GolfClubTestData.AggregateId,
                                                                                  AddressLine1 = GolfClubTestData.AddressLine1,
                                                                                  Name = GolfClubTestData.Name,
                                                                                  AddressLine2 = GolfClubTestData.AddressLine2,
                                                                                  EmailAddress = GolfClubTestData.EmailAddress,
                                                                                  PostalCode = GolfClubTestData.PostalCode,
                                                                                  Region = GolfClubTestData.Region,
                                                                                  TelephoneNumber = GolfClubTestData.TelephoneNumber,
                                                                                  Town = GolfClubTestData.Town,
                                                                                  Website = GolfClubTestData.Website
                                                                              }
                                                                          };

        public static MeasuredCourseListResponse MeasuredCourseListResponse = new MeasuredCourseListResponse
                                                                              {
                                                                                  StandardScratchScore = GolfClubTestData.StandardScratchScore,
                                                                                  TeeColour = GolfClubTestData.TeeColour,
                                                                                  Name = GolfClubTestData.MeasuredCourseName,
                                                                                  MeasuredCourseId = GolfClubTestData.MeasuredCourseId
                                                                              };

        public static GetMeasuredCourseListResponse GetMeasuredCourseListResponse = new GetMeasuredCourseListResponse
                                                                                    {
                                                                                        GolfClubId = GolfClubTestData.AggregateId,
                                                                                        MeasuredCourses = new List<MeasuredCourseListResponse>
                                                                                                          {
                                                                                                              GolfClubTestData.MeasuredCourseListResponse
                                                                                                          }
                                                                                    };

        public static Guid GolfClubAdministratorSecurityUserId = Guid.Parse("F8EBC624-B103-487A-A68A-6111C22287D6");

        public static Int32 HoleNumber = 1;

        public static Int32 HolePar = 3;

        public static Int32 HoleStrokeIndex = 1;

        public static Guid InvalidMeasuredCourseId = Guid.Parse("F9FAB99E-D315-46FC-A3C6-DDC10FB4FA0E");

        public static Int32 LengthInMeters = 175;

        public static Int32 LengthInYards = 150;

        public static Guid MatchSecretarySecurityUserId = Guid.Parse("028D3AFB-C104-457F-A4EA-5EB0345F56CD");

        public static RegisterClubAdministratorRequest RegisterClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                          {
                                                                                              EmailAddress = "testclubadministrator@test.co.uk",
                                                                                              Password = "123456",
                                                                                              ConfirmPassword = "123456",
                                                                                              TelephoneNumber = "123456789",
                                                                                              GivenName = "Test",
                                                                                              MiddleName = null,
                                                                                              FamilyName = "Club Administrator"
                                                                                          };

        #endregion

        #region Methods

        public static AddMeasuredCourseToClubCommand GetAddMeasuredCourseToClubCommand()
        {
            return AddMeasuredCourseToClubCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.AddMeasuredCourseToClubRequest);
        }

        public static AddTournamentDivisionToGolfClubCommand GetAddTournamentDivisionToGolfClubCommand()
        {
            return AddTournamentDivisionToGolfClubCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.AddTournamentDivisionToGolfClubRequest);
        }

        public static GolfClubAggregate GetCreatedGolfClubAggregate()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            aggregate.CreateGolfClub(GolfClubTestData.Name,
                                     GolfClubTestData.AddressLine1,
                                     GolfClubTestData.AddressLine2,
                                     GolfClubTestData.Town,
                                     GolfClubTestData.Region,
                                     GolfClubTestData.PostalCode,
                                     GolfClubTestData.TelephoneNumber,
                                     GolfClubTestData.Website,
                                     GolfClubTestData.EmailAddress);

            return aggregate;
        }

        public static GolfClubAggregate GetCreatedGolfClubAggregateWithGolfClubAdministratorUser()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            aggregate.CreateGolfClub(GolfClubTestData.Name,
                                     GolfClubTestData.AddressLine1,
                                     GolfClubTestData.AddressLine2,
                                     GolfClubTestData.Town,
                                     GolfClubTestData.Region,
                                     GolfClubTestData.PostalCode,
                                     GolfClubTestData.TelephoneNumber,
                                     GolfClubTestData.Website,
                                     GolfClubTestData.EmailAddress);

            aggregate.CreateGolfClubAdministratorSecurityUser(GolfClubTestData.GolfClubAdministratorSecurityUserId);

            return aggregate;
        }

        public static GolfClubAggregate GetCreatedGolfClubAggregateWithMatchSecretaryUser()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            aggregate.CreateGolfClub(GolfClubTestData.Name,
                                     GolfClubTestData.AddressLine1,
                                     GolfClubTestData.AddressLine2,
                                     GolfClubTestData.Town,
                                     GolfClubTestData.Region,
                                     GolfClubTestData.PostalCode,
                                     GolfClubTestData.TelephoneNumber,
                                     GolfClubTestData.Website,
                                     GolfClubTestData.EmailAddress);

            aggregate.CreateGolfClubAdministratorSecurityUser(GolfClubTestData.GolfClubAdministratorSecurityUserId);

            aggregate.CreateMatchSecretarySecurityUser(GolfClubTestData.MatchSecretarySecurityUserId);

            return aggregate;
        }

        public static CreateGolfClubCommand GetCreateGolfClubCommand()
        {
            return CreateGolfClubCommand.Create(GolfClubTestData.AggregateId,
                                                GolfClubTestData.GolfClubAdministratorSecurityUserId,
                                                GolfClubTestData.CreateGolfClubRequest);
        }

        public static CreateMatchSecretaryCommand GetCreateMatchSecretaryCommand()
        {
            return CreateMatchSecretaryCommand.Create(GolfClubTestData.AggregateId, GolfClubTestData.CreateMatchSecretaryRequest);
        }

        public static GolfClubAggregate GetEmptyGolfClubAggregate()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            return aggregate;
        }

        public static GolfClubAggregate GetGolfClubAggregateWithMeasuredCourse()
        {
            GolfClubAggregate aggregate = GolfClubAggregate.Create(GolfClubTestData.AggregateId);

            aggregate.CreateGolfClub(GolfClubTestData.Name,
                                     GolfClubTestData.AddressLine1,
                                     GolfClubTestData.AddressLine2,
                                     GolfClubTestData.Town,
                                     GolfClubTestData.Region,
                                     GolfClubTestData.PostalCode,
                                     GolfClubTestData.TelephoneNumber,
                                     GolfClubTestData.Website,
                                     GolfClubTestData.EmailAddress);

            aggregate.CreateGolfClubAdministratorSecurityUser(GolfClubTestData.GolfClubAdministratorSecurityUserId);

            MeasuredCourseDataTransferObject measuredCourseDataTransferObject = GolfClubTestData.GetMeasuredCourseToAdd();

            aggregate.AddMeasuredCourse(measuredCourseDataTransferObject);

            return aggregate;
        }

        public static GolfClubCreatedEvent GetGolfClubCreatedEvent()
        {
            GolfClubCreatedEvent domainEvent = GolfClubCreatedEvent.Create(GolfClubTestData.AggregateId,
                                                                           GolfClubTestData.Name,
                                                                           GolfClubTestData.AddressLine1,
                                                                           GolfClubTestData.AddressLine2,
                                                                           GolfClubTestData.Town,
                                                                           GolfClubTestData.Region,
                                                                           GolfClubTestData.PostalCode,
                                                                           GolfClubTestData.TelephoneNumber,
                                                                           GolfClubTestData.Website,
                                                                           GolfClubTestData.EmailAddress);

            return domainEvent;
        }

        public static GolfClubAdministratorSecurityUserCreatedEvent GetGolfClubAdministratorSecurityUserCreatedEvent()
        {
            GolfClubAdministratorSecurityUserCreatedEvent domainEvent =
                GolfClubAdministratorSecurityUserCreatedEvent.Create(GolfClubTestData.AggregateId, GolfClubTestData.GolfClubAdministratorSecurityUserId);

            return domainEvent;
        }

        public static MatchSecretarySecurityUserCreatedEvent GetMatchSecretarySecurityUserCreatedEvent()
        {
            MatchSecretarySecurityUserCreatedEvent domainEvent =
                MatchSecretarySecurityUserCreatedEvent.Create(GolfClubTestData.AggregateId, GolfClubTestData.MatchSecretarySecurityUserId);

            return domainEvent;
        }

        public static MeasuredCourseDataTransferObject GetMeasuredCourseToAdd(Int32 numberHoles = 18)
        {
            MeasuredCourseDataTransferObject result = new MeasuredCourseDataTransferObject();

            result.MeasuredCourseId = GolfClubTestData.MeasuredCourseId;
            result.Name = GolfClubTestData.MeasuredCourseName;
            result.MeasuredCourseId = GolfClubTestData.MeasuredCourseId;
            result.StandardScratchScore = GolfClubTestData.StandardScratchScore;
            result.TeeColour = GolfClubTestData.TeeColour;

            result.Holes = new List<HoleDataTransferObject>();

            if (numberHoles >= 1)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 1,
                                     LengthInYards = 348,
                                     Par = 4,
                                     StrokeIndex = 10
                                 });
            }

            if (numberHoles >= 2)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 2,
                                     LengthInYards = 402,
                                     Par = 4,
                                     StrokeIndex = 4
                                 });
            }

            if (numberHoles >= 3)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 3,
                                     LengthInYards = 207,
                                     Par = 3,
                                     StrokeIndex = 14
                                 });
            }

            if (numberHoles >= 4)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 4,
                                     LengthInYards = 405,
                                     Par = 4,
                                     StrokeIndex = 8
                                 });
            }

            if (numberHoles >= 5)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 5,
                                     LengthInYards = 428,
                                     Par = 4,
                                     StrokeIndex = 2
                                 });
            }

            if (numberHoles >= 6)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 6,
                                     LengthInYards = 477,
                                     Par = 5,
                                     StrokeIndex = 12
                                 });
            }

            if (numberHoles >= 7)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 7,
                                     LengthInYards = 186,
                                     Par = 4,
                                     StrokeIndex = 16
                                 });
            }

            if (numberHoles >= 8)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 8,
                                     LengthInYards = 397,
                                     Par = 4,
                                     StrokeIndex = 6
                                 });
            }

            if (numberHoles >= 9)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 9,
                                     LengthInYards = 130,
                                     Par = 3,
                                     StrokeIndex = 18
                                 });
            }

            if (numberHoles >= 10)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 10,
                                     LengthInYards = 399,
                                     Par = 4,
                                     StrokeIndex = 3
                                 });
            }

            if (numberHoles >= 11)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 11,
                                     LengthInYards = 401,
                                     Par = 4,
                                     StrokeIndex = 13
                                 });
            }

            if (numberHoles >= 12)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 12,
                                     LengthInYards = 421,
                                     Par = 4,
                                     StrokeIndex = 1
                                 });
            }

            if (numberHoles >= 13)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 13,
                                     LengthInYards = 530,
                                     Par = 5,
                                     StrokeIndex = 11
                                 });
            }

            if (numberHoles >= 14)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 14,
                                     LengthInYards = 196,
                                     Par = 3,
                                     StrokeIndex = 5
                                 });
            }

            if (numberHoles >= 15)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 15,
                                     LengthInYards = 355,
                                     Par = 4,
                                     StrokeIndex = 7
                                 });
            }

            if (numberHoles >= 16)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 16,
                                     LengthInYards = 243,
                                     Par = 4,
                                     StrokeIndex = 15
                                 });
            }

            if (numberHoles >= 17)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 17,
                                     LengthInYards = 286,
                                     Par = 4,
                                     StrokeIndex = 17
                                 });
            }

            if (numberHoles >= 18)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 18,
                                     LengthInYards = 399,
                                     Par = 4,
                                     StrokeIndex = 9
                                 });
            }

            if (numberHoles >= 19)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = 19,
                                     LengthInYards = 399,
                                     Par = 4,
                                     StrokeIndex = 9
                                 });
            }

            return result;
        }

        public static MeasuredCourseDataTransferObject GetMeasuredCourseToAddWithMissingHoles(Int32 holeNumber)
        {
            MeasuredCourseDataTransferObject result = new MeasuredCourseDataTransferObject();

            result.Name = GolfClubTestData.MeasuredCourseName;
            result.MeasuredCourseId = GolfClubTestData.MeasuredCourseId;
            result.StandardScratchScore = GolfClubTestData.StandardScratchScore;
            result.TeeColour = GolfClubTestData.TeeColour;

            result.Holes = new List<HoleDataTransferObject>();

            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 1,
                                 LengthInYards = 348,
                                 Par = 4,
                                 StrokeIndex = 10
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 2,
                                 LengthInYards = 402,
                                 Par = 4,
                                 StrokeIndex = 4
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 3,
                                 LengthInYards = 207,
                                 Par = 3,
                                 StrokeIndex = 14
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 4,
                                 LengthInYards = 405,
                                 Par = 4,
                                 StrokeIndex = 8
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 5,
                                 LengthInYards = 428,
                                 Par = 4,
                                 StrokeIndex = 2
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 6,
                                 LengthInYards = 477,
                                 Par = 5,
                                 StrokeIndex = 12
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 7,
                                 LengthInYards = 186,
                                 Par = 4,
                                 StrokeIndex = 16
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 8,
                                 LengthInYards = 397,
                                 Par = 4,
                                 StrokeIndex = 6
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 9,
                                 LengthInYards = 130,
                                 Par = 3,
                                 StrokeIndex = 18
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 10,
                                 LengthInYards = 399,
                                 Par = 4,
                                 StrokeIndex = 3
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 11,
                                 LengthInYards = 401,
                                 Par = 4,
                                 StrokeIndex = 13
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 12,
                                 LengthInYards = 421,
                                 Par = 4,
                                 StrokeIndex = 1
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 13,
                                 LengthInYards = 530,
                                 Par = 5,
                                 StrokeIndex = 11
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 14,
                                 LengthInYards = 196,
                                 Par = 3,
                                 StrokeIndex = 5
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 15,
                                 LengthInYards = 355,
                                 Par = 4,
                                 StrokeIndex = 7
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 16,
                                 LengthInYards = 243,
                                 Par = 4,
                                 StrokeIndex = 15
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 17,
                                 LengthInYards = 286,
                                 Par = 4,
                                 StrokeIndex = 17
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 18,
                                 LengthInYards = 399,
                                 Par = 4,
                                 StrokeIndex = 9
                             });

            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 19,
                                 LengthInYards = 100,
                                 Par = 3,
                                 StrokeIndex = result.Holes.Where(h => h.HoleNumber == holeNumber).Single().StrokeIndex
                             });

            result.Holes.Remove(result.Holes.Where(h => h.HoleNumber == holeNumber).Single());

            return result;
        }

        public static MeasuredCourseDataTransferObject GetMeasuredCourseToAddWithMissingStrokeIndex(Int32 strokeIndex)
        {
            MeasuredCourseDataTransferObject result = new MeasuredCourseDataTransferObject();

            result.Name = GolfClubTestData.MeasuredCourseName;
            result.MeasuredCourseId = GolfClubTestData.MeasuredCourseId;
            result.StandardScratchScore = GolfClubTestData.StandardScratchScore;
            result.TeeColour = GolfClubTestData.TeeColour;

            result.Holes = new List<HoleDataTransferObject>();

            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 1,
                                 LengthInYards = 348,
                                 Par = 4,
                                 StrokeIndex = 10
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 2,
                                 LengthInYards = 402,
                                 Par = 4,
                                 StrokeIndex = 4
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 3,
                                 LengthInYards = 207,
                                 Par = 3,
                                 StrokeIndex = 14
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 4,
                                 LengthInYards = 405,
                                 Par = 4,
                                 StrokeIndex = 8
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 5,
                                 LengthInYards = 428,
                                 Par = 4,
                                 StrokeIndex = 2
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 6,
                                 LengthInYards = 477,
                                 Par = 5,
                                 StrokeIndex = 12
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 7,
                                 LengthInYards = 186,
                                 Par = 4,
                                 StrokeIndex = 16
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 8,
                                 LengthInYards = 397,
                                 Par = 4,
                                 StrokeIndex = 6
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 9,
                                 LengthInYards = 130,
                                 Par = 3,
                                 StrokeIndex = 18
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 10,
                                 LengthInYards = 399,
                                 Par = 4,
                                 StrokeIndex = 3
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 11,
                                 LengthInYards = 401,
                                 Par = 4,
                                 StrokeIndex = 13
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 12,
                                 LengthInYards = 421,
                                 Par = 4,
                                 StrokeIndex = 1
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 13,
                                 LengthInYards = 530,
                                 Par = 5,
                                 StrokeIndex = 11
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 14,
                                 LengthInYards = 196,
                                 Par = 3,
                                 StrokeIndex = 5
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 15,
                                 LengthInYards = 355,
                                 Par = 4,
                                 StrokeIndex = 7
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 16,
                                 LengthInYards = 243,
                                 Par = 4,
                                 StrokeIndex = 15
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 17,
                                 LengthInYards = 286,
                                 Par = 4,
                                 StrokeIndex = 17
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = 18,
                                 LengthInYards = 399,
                                 Par = 4,
                                 StrokeIndex = 9
                             });
            result.Holes.Add(new HoleDataTransferObject
                             {
                                 HoleNumber = result.Holes.Where(h => h.StrokeIndex == strokeIndex).Single().HoleNumber,
                                 LengthInYards = 100,
                                 Par = 4,
                                 StrokeIndex = 19
                             });

            result.Holes.Remove(result.Holes.Where(h => h.StrokeIndex == strokeIndex).Single());

            return result;
        }

        public static RegisterUserResponse GetRegisterUserResponse()
        {
            return new RegisterUserResponse
                   {
                       UserId = GolfClubTestData.GolfClubAdministratorSecurityUserId
                   };
        }

        public static RequestClubMembershipCommand GetRequestClubMembershipCommand()
        {
            return RequestClubMembershipCommand.Create(GolfClubTestData.PlayerId, GolfClubTestData.AggregateId);
        }

        public static TournamentDivisionDataTransferObject GetTournamentDivision1()
        {
            return new TournamentDivisionDataTransferObject
                   {
                       Division = 1,
                       StartHandicap = 0,
                       EndHandicap = 5
                   };
        }

        public static TournamentDivisionDataTransferObject GetTournamentDivision2()
        {
            return new TournamentDivisionDataTransferObject
                   {
                       Division = 2,
                       StartHandicap = 6,
                       EndHandicap = 12
                   };
        }

        public static TournamentDivisionDataTransferObject GetTournamentDivision3()
        {
            return new TournamentDivisionDataTransferObject
                   {
                       Division = 3,
                       StartHandicap = 13,
                       EndHandicap = 21
                   };
        }

        public static TournamentDivisionDataTransferObject GetTournamentDivision4()
        {
            return new TournamentDivisionDataTransferObject
                   {
                       Division = 4,
                       StartHandicap = 22,
                       EndHandicap = 28
                   };
        }

        public static GetUserResponse GetClubAdministratorUserResponse(String givenName, String middleName, String familyName)
        {
            return new GetUserResponse
                   {
                       Claims = new Dictionary<String, String>
                                {
                                    {JwtClaimTypes.GivenName, givenName},
                                    {JwtClaimTypes.MiddleName, middleName},
                                    {JwtClaimTypes.FamilyName, familyName}
                                },
                       UserName = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                       Email = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                       PhoneNumber = GolfClubTestData.RegisterClubAdministratorRequest.TelephoneNumber,
                       UserId = Guid.NewGuid(),
                       Roles = new List<String>
                               {
                                   "Club Administrator"
                               }
                   };
        }

        public static GetUserResponse GetMatchSecretaryUserResponse(String givenName, String middleName, String familyName)
        {
            return new GetUserResponse
                   {
                       Claims = new Dictionary<String, String>
                                {
                                    {JwtClaimTypes.GivenName, givenName},
                                    {JwtClaimTypes.MiddleName, middleName},
                                    {JwtClaimTypes.FamilyName, familyName}
                                },
                       UserName = GolfClubTestData.CreateMatchSecretaryRequest.EmailAddress,
                       Email = GolfClubTestData.CreateMatchSecretaryRequest.EmailAddress,
                       PhoneNumber = GolfClubTestData.CreateMatchSecretaryRequest.TelephoneNumber,
                       UserId = Guid.NewGuid(),
                       Roles = new List<String>
                               {
                                   "Match Secretary"
                               }
                   };
        }

        public static Guid MembershipId = Guid.Parse("7C244753-1184-48A7-865C-D424A9E0B6F8");

        public static String PlayerFullName = "Test Player";

        public static DateTime PlayerDateOfBirth = new DateTime(1980, 12, 13);

        public static String PlayerGender = "M";

        public static DateTime MembershipAcceptedDateTime = new DateTime(2019, 7, 23);

        public static String MembershipNumber = "000001";

        public static String MembershipRejectionReason = "Test Reason";

        public static DateTime MembershipRejectionDateTime = new DateTime(2019, 7, 23);

        public static Int32 StatusAccepted= 0;

        public static Int32 StatusRejected = 1;

        public static GetGolfClubUserListResponse GetGolfClubUserListResponse = new GetGolfClubUserListResponse
                                                                                {
                                                                                    Users = new List<GolfClubUserResponse>
                                                                                            {
                                                                                                new GolfClubUserResponse
                                                                                                {
                                                                                                    GivenName = GolfClubTestData
                                                                                                                .RegisterClubAdministratorRequest.GivenName,
                                                                                                    FamilyName = GolfClubTestData
                                                                                                                 .RegisterClubAdministratorRequest.FamilyName,
                                                                                                    MiddleName = GolfClubTestData
                                                                                                                 .RegisterClubAdministratorRequest.MiddleName,
                                                                                                    UserName = GolfClubTestData
                                                                                                               .RegisterClubAdministratorRequest.EmailAddress,
                                                                                                    Email =
                                                                                                        GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                                    UserId = GolfClubTestData.GolfClubAdministratorSecurityUserId,
                                                                                                    GolfClubId = GolfClubTestData.AggregateId,
                                                                                                    PhoneNumber = GolfClubTestData
                                                                                                                  .RegisterClubAdministratorRequest.TelephoneNumber,
                                                                                                    UserType = "Club Administrator"
                                                                                                }
                                                                                            }
                                                                                };

        public static ClubMembershipRequestAcceptedEvent GetClubMembershipRequestAcceptedEvent()
        {
            return ClubMembershipRequestAcceptedEvent.Create(GolfClubTestData.AggregateId,
                                                             GolfClubTestData.MembershipId,
                                                             GolfClubTestData.PlayerId,
                                                             GolfClubTestData.PlayerFullName,
                                                             GolfClubTestData.PlayerDateOfBirth,
                                                             GolfClubTestData.PlayerGender,
                                                             GolfClubTestData.MembershipAcceptedDateTime,
                                                             GolfClubTestData.MembershipNumber);
        }

        public static ClubMembershipRequestRejectedEvent GetClubMembershipRequestRejectedEvent()
        {
            return ClubMembershipRequestRejectedEvent.Create(GolfClubTestData.AggregateId,
                                                             GolfClubTestData.MembershipId,
                                                             GolfClubTestData.PlayerId,
                                                             GolfClubTestData.PlayerFullName,
                                                             GolfClubTestData.PlayerDateOfBirth,
                                                             GolfClubTestData.PlayerGender,
                                                             GolfClubTestData.MembershipRejectionReason,
                                                             GolfClubTestData.MembershipRejectionDateTime);
        }

        #endregion
    }
}