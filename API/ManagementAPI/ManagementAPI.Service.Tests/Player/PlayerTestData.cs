namespace ManagementAPI.Service.Tests.Player
{
    using System;
    using System.Collections.Generic;
    using Commands;
    using DataTransferObjects;
    using ManagementAPI.Player;
    using Services.DataTransferObjects;

    public class PlayerTestData
    {
        #region Fields

        public static Guid AggregateId = Guid.Parse("9F14D8A4-D8F7-4E32-9600-C3F038E662F6");

        public static Guid GolfClubId = Guid.Parse("6AF76DC0-3913-4A6D-BF60-2E0A1DE06333");

        public static DateTime MembershipAcceptedDateTime = new DateTime(2019, 2, 12);

        public static Guid MembershipId = Guid.Parse("B9D67FC1-E16B-43B7-AA96-F689F3C7314E");

        public static String MembershipNumber = "000001";

        public static List<ClubMembershipResponse> ClubMembershipResponses = new List<ClubMembershipResponse>
                                                                             {
                                                                                 new ClubMembershipResponse
                                                                                 {
                                                                                     AcceptedDateTime = PlayerTestData.MembershipAcceptedDateTime,
                                                                                     MembershipNumber = PlayerTestData.MembershipNumber,
                                                                                     MembershipId = PlayerTestData.MembershipId,
                                                                                     GolfClubId = PlayerTestData.GolfClubId,
                                                                                     RejectionReason = string.Empty,
                                                                                     Status = MembershipStatus.Accepted,
                                                                                     RejectedDateTime = DateTime.MinValue
                                                                                 }
                                                                             };

        public static DateTime DateOfBirth = new DateTime(1980, 12, 13);

        public static String EmailAddress = "test@email.com";

        public static Decimal ExactHandicap = 11.4m;

        public static Decimal ExactHandicapCat1 = 0.5m;

        public static Decimal ExactHandicapCat2 = 5.5m;

        public static Decimal ExactHandicapCat3 = 12.5m;

        public static Decimal ExactHandicapCat4 = 21.5m;

        public static Decimal ExactHandicapCat5 = 28.5m;

        public static String FirstName = "First";

        public static String LastName = "Last";

        public static String MiddleName = "Middle";

        public static String FullName = $"{PlayerTestData.FirstName} {PlayerTestData.MiddleName} {PlayerTestData.LastName}";

        public static String Gender = "M";

        public static Int32 HandicapCategoryCat1 = 1;

        public static Int32 PlayingHandicapCat1 = 1;

        public static GetPlayerDetailsResponse GetPlayerDetailsResponse = new GetPlayerDetailsResponse
                                                                          {
                                                                              FullName = PlayerTestData.FullName,
                                                                              EmailAddress = PlayerTestData.EmailAddress,
                                                                              Gender = PlayerTestData.Gender,
                                                                              HasBeenRegistered = true,
                                                                              DateOfBirth = PlayerTestData.DateOfBirth,
                                                                              LastName = PlayerTestData.LastName,
                                                                              FirstName = PlayerTestData.FirstName,
                                                                              MiddleName = PlayerTestData.MiddleName,
                                                                              ExactHandicap = PlayerTestData.ExactHandicapCat1,
                                                                              HandicapCategory = PlayerTestData.HandicapCategoryCat1,
                                                                              PlayingHandicap = PlayerTestData.PlayingHandicapCat1
                                                                          };

        public static Int32 HandicapCategory = 2;

        public static Int32 HandicapCategoryCat2 = 2;

        public static Int32 HandicapCategoryCat3 = 3;

        public static Int32 HandicapCategoryCat4 = 4;

        public static Int32 HandicapCategoryCat5 = 5;

        public static DateTime MembershipRejectedDateTime = new DateTime(2019, 1, 12);

        public static Int32 PlayingHandicap = 11;

        public static Int32 PlayingHandicapCat2 = 6;

        public static Int32 PlayingHandicapCat3 = 13;

        public static Int32 PlayingHandicapCat4 = 22;

        public static Int32 PlayingHandicapCat5 = 29;

        public static RegisterPlayerRequest RegisterPlayerRequest = new RegisterPlayerRequest
                                                                    {
                                                                        DateOfBirth = PlayerTestData.DateOfBirth,
                                                                        EmailAddress = PlayerTestData.EmailAddress,
                                                                        FirstName = PlayerTestData.FirstName,
                                                                        MiddleName = PlayerTestData.MiddleName,
                                                                        LastName = PlayerTestData.LastName,
                                                                        Gender = PlayerTestData.Gender,
                                                                        ExactHandicap = PlayerTestData.ExactHandicap
                                                                    };

        public static RegisterPlayerResponse RegisterPlayerResponse = new RegisterPlayerResponse
                                                                      {
                                                                          PlayerId = PlayerTestData.AggregateId
                                                                      };

        public static String RejectionReason = "Rejected";

        public static Guid SecurityUserId = Guid.Parse("A78FF418-1CEB-47AC-9A8E-78CA6933E183");

        #endregion

        #region Methods
        
        public static PlayerAggregate GetEmptyPlayerAggregate()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(PlayerTestData.AggregateId);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregate()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(PlayerTestData.AggregateId);

            playerAggregate.Register(PlayerTestData.FirstName,
                                     PlayerTestData.MiddleName,
                                     PlayerTestData.LastName,
                                     PlayerTestData.Gender,
                                     PlayerTestData.DateOfBirth,
                                     PlayerTestData.ExactHandicap,
                                     PlayerTestData.EmailAddress);

            return playerAggregate;
        }
        
        public static PlayerAggregate GetRegisteredPlayerAggregateWithSecurityUserCreated()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(PlayerTestData.AggregateId);

            playerAggregate.Register(PlayerTestData.FirstName,
                                     PlayerTestData.MiddleName,
                                     PlayerTestData.LastName,
                                     PlayerTestData.Gender,
                                     PlayerTestData.DateOfBirth,
                                     PlayerTestData.ExactHandicapCat1,
                                     PlayerTestData.EmailAddress);

            playerAggregate.CreateSecurityUser(PlayerTestData.SecurityUserId);

            return playerAggregate;
        }

        public static RegisterPlayerCommand GetRegisterPlayerCommand()
        {
            return RegisterPlayerCommand.Create(PlayerTestData.RegisterPlayerRequest);
        }

        public static RegisterUserResponse GetRegisterUserResponse()
        {
            return new RegisterUserResponse
                   {
                       UserId = PlayerTestData.SecurityUserId
                   };
        }

        #endregion
    }
}