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
    }
}
