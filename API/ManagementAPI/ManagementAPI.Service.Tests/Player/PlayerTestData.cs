using System;
using System.Collections.Generic;
using System.Text;
using ManagementAPI.Player;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Services.DataTransferObjects;

namespace ManagementAPI.Service.Tests.Player
{
    public class PlayerTestData
    {
        public static Guid AggregateId = Guid.Parse("9A13D29F-484D-4ABD-BF85-813A0A7F075E");
        public static String FirstName = "First";
        public static String MiddleName = "Middle";
        public static String LastName = "Last";
        public static String Gender = "M";
        public static Int32 Age = 30;
        public static Decimal ExactHandicap = 11.4m;

        public static Decimal ExactHandicapCat1 = 0.5m;
        public static Int32 PlayingHandicapCat1 = 1;
        public static Int32 HandicapCategoryCat1 = 1;

        public static Decimal ExactHandicapCat2= 5.5m;
        public static Int32 PlayingHandicapCat2 = 6;
        public static Int32 HandicapCategoryCat2 = 2;

        public static Decimal ExactHandicapCat3 = 12.5m;
        public static Int32 PlayingHandicapCat3 = 13;
        public static Int32 HandicapCategoryCat3 = 3;

        public static Decimal ExactHandicapCat4 = 21.5m;
        public static Int32 PlayingHandicapCat4 = 22;
        public static Int32 HandicapCategoryCat4 = 4;

        public static Decimal ExactHandicapCat5 = 28.5m;
        public static Int32 PlayingHandicapCat5 = 29;
        public static Int32 HandicapCategoryCat5 = 5;

        public static String EmailAddress = "test@email.com";

        public static Guid SecurityUserId = Guid.Parse("A78FF418-1CEB-47AC-9A8E-78CA6933E183");

        public static PlayerAggregate GetEmptyPlayerAggregate()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregate()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                Age, ExactHandicapCat1, EmailAddress);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregateWithSecurityUserCreated()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                Age, ExactHandicapCat1, EmailAddress);

            playerAggregate.CreateSecurityUser(SecurityUserId);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregateWithPendingMembershipRequest()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                Age, ExactHandicapCat1, EmailAddress);

            playerAggregate.CreateSecurityUser(SecurityUserId);

            playerAggregate.RequestClubMembership(ClubId, MembershipRequestedDateAndTime);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregateWithApprovedMembershipRequest()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                Age, ExactHandicapCat1, EmailAddress);

            playerAggregate.CreateSecurityUser(SecurityUserId);

            playerAggregate.RequestClubMembership(ClubId, MembershipRequestedDateAndTime);

            playerAggregate.ApproveClubMembershipRequest(ClubId, MembershipApprovedDateAndTime);

            return playerAggregate;
        }

        public static RegisterPlayerRequest RegisterPlayerRequest = new RegisterPlayerRequest
        {
            Age = Age,
            EmailAddress = EmailAddress,
            FirstName = FirstName,
            MiddleName = MiddleName,
            LastName = LastName,
            Gender = Gender,
            ExactHandicap = ExactHandicap
        };

        public static RegisterPlayerResponse RegisterPlayerResponse = new RegisterPlayerResponse
        {
            PlayerId = AggregateId
        };

        public static RegisterPlayerCommand GetRegisterPlayerCommand()
        {
            return RegisterPlayerCommand.Create(RegisterPlayerRequest);
        }
        
        public static RegisterUserResponse GetRegisterUserResponse()
        {
            return new RegisterUserResponse()
            {
                UserId = SecurityUserId
            };
        }

        public static Guid ClubId = Guid.Parse("3BDDB601-E08F-4B6C-A58A-1493D06E3A6C");

        public static DateTime MembershipRequestedDateAndTime = new DateTime(2018,12,25);

        public static DateTime MembershipApprovedDateAndTime = new DateTime(2018,1,5);

        public static PlayerClubMembershipRequestCommand GetPlayerClubMembershipRequestCommand()
        {
            return PlayerClubMembershipRequestCommand.Create(AggregateId,ClubId);
        }

        public static ApprovePlayerMembershipRequestCommand GetApprovePlayerMembershipRequestCommand()
        {
            return ApprovePlayerMembershipRequestCommand.Create(AggregateId,ClubId);
        }

        public static ClubMembershipRequestedEvent GetClubMembershipRequestedEvent()
        {
            return ClubMembershipRequestedEvent.Create(AggregateId, ClubId, MembershipRequestedDateAndTime);
        }
    }
}
