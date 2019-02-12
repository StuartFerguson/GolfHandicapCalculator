using System;
using System.Collections.Generic;
using System.Text;
using ManagementAPI.Player;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Services.DataTransferObjects;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace ManagementAPI.Service.Tests.Player
{
    public class PlayerTestData
    {
        public static Guid AggregateId = Guid.Parse("9A13D29F-484D-4ABD-BF85-813A0A7F075E");
        public static String FirstName = "First";
        public static String MiddleName = "Middle";
        public static String LastName = "Last";
        public static String Gender = "M";
        public static DateTime DateOfBirth = new DateTime(1980, 12, 13);
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
        
        public static Guid GolfClubId = Guid.Parse("6AF76DC0-3913-4A6D-BF60-2E0A1DE06333");
        
        public static Guid MembershipId = Guid.Parse("B9D67FC1-E16B-43B7-AA96-F689F3C7314E");
        
        public static String MembershipNumber = "000001";

        public static String RejectionReason = "Rejected";
        
        public static DateTime MembershipAcceptedDateTime = new DateTime(2019,2,12);

        public static DateTime MembershipRejectedDateTime = new DateTime(2019,1,12);


        public static PlayerAggregate GetEmptyPlayerAggregate()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregate()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                DateOfBirth, ExactHandicapCat1, EmailAddress);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregateWithSecurityUserCreated()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                DateOfBirth, ExactHandicapCat1, EmailAddress);

            playerAggregate.CreateSecurityUser(SecurityUserId);

            return playerAggregate;
        }

        public static PlayerAggregate GetRegisteredPlayerAggregateWithMembershipAdded()
        {
            PlayerAggregate playerAggregate = PlayerAggregate.Create(AggregateId);

            playerAggregate.Register(FirstName, MiddleName, LastName, Gender,
                DateOfBirth, ExactHandicapCat1, EmailAddress);

            playerAggregate.CreateSecurityUser(SecurityUserId);

            playerAggregate.AddAcceptedMembership(GolfClubId, MembershipId, MembershipNumber,MembershipAcceptedDateTime);

            return playerAggregate;
        }
        
        public static RegisterPlayerRequest RegisterPlayerRequest = new RegisterPlayerRequest
        {
            DateOfBirth = DateOfBirth,
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

        public static AddAcceptedMembershipToPlayerCommand GetAddAcceptedMembershipToPlayerCommand()
        {
            return AddAcceptedMembershipToPlayerCommand.Create(AggregateId, GolfClubId, MembershipId, MembershipNumber,
                MembershipAcceptedDateTime);
        }

        public static AddRejectedMembershipToPlayerCommand GetAddRejectedMembershipToPlayerCommand()
        {
            return AddRejectedMembershipToPlayerCommand.Create(AggregateId, GolfClubId, MembershipId, RejectionReason,
                MembershipRejectedDateTime);
        }
    }
}
