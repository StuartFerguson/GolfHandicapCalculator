using System;
using System.Collections.Generic;
using System.Text;
using ManagementAPI.Player;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Player
{
    public class PlayerAggregateTests
    {
        #region Create Tests
        [Fact]
        public void PlayerAggregate_CanBeCreated_IsCreated()
        {
            PlayerAggregate player = PlayerAggregate.Create(PlayerTestData.AggregateId);

            player.ShouldNotBeNull();
            player.AggregateId.ShouldBe(PlayerTestData.AggregateId);
        }

        [Fact]
        public void PlayerAggregate_InvalidData_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                PlayerAggregate player = PlayerAggregate.Create(Guid.Empty);
            });
        }

        #endregion

        #region Register Tests

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void PlayerAggregate_Register_PlayerRegistered(Int32 category)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Decimal exactHandicap = 0;
            Int32 playingHandicap = 0;
            Int32 handicapCategory = 0;

            switch (category)
            {
                case 1:
                    exactHandicap = PlayerTestData.ExactHandicapCat1;
                    playingHandicap = PlayerTestData.PlayingHandicapCat1;
                    handicapCategory = PlayerTestData.HandicapCategoryCat1;
                    break;
                case 2:
                    exactHandicap = PlayerTestData.ExactHandicapCat2;
                    playingHandicap = PlayerTestData.PlayingHandicapCat2;
                    handicapCategory = PlayerTestData.HandicapCategoryCat2;
                    break;
                case 3:
                    exactHandicap = PlayerTestData.ExactHandicapCat3;
                    playingHandicap = PlayerTestData.PlayingHandicapCat3;
                    handicapCategory = PlayerTestData.HandicapCategoryCat3;
                    break;
                case 4:
                    exactHandicap = PlayerTestData.ExactHandicapCat4;
                    playingHandicap = PlayerTestData.PlayingHandicapCat4;
                    handicapCategory = PlayerTestData.HandicapCategoryCat4;
                    break;
                case 5:
                    exactHandicap = PlayerTestData.ExactHandicapCat5;
                    playingHandicap = PlayerTestData.PlayingHandicapCat5;
                    handicapCategory = PlayerTestData.HandicapCategoryCat5;
                    break;
            }

            playerAggregate.Register(PlayerTestData.FirstName, PlayerTestData.MiddleName, PlayerTestData.LastName, PlayerTestData.Gender,
                PlayerTestData.Age, exactHandicap, PlayerTestData.EmailAddress);

            playerAggregate.FirstName.ShouldBe(PlayerTestData.FirstName);
            playerAggregate.MiddleName.ShouldBe(PlayerTestData.MiddleName);
            playerAggregate.LastName.ShouldBe(PlayerTestData.LastName);
            playerAggregate.Gender.ShouldBe(PlayerTestData.Gender);
            playerAggregate.Age.ShouldBe(PlayerTestData.Age);
            playerAggregate.ExactHandicap.ShouldBe(exactHandicap);
            playerAggregate.EmailAddress.ShouldBe(PlayerTestData.EmailAddress);
            playerAggregate.PlayingHandicap.ShouldBe(playingHandicap);
            playerAggregate.HandicapCategory.ShouldBe(handicapCategory);
        }
        
        [Theory]
        [InlineData("", "lastname", "M", 30, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData(null, "lastname", "M", 30, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "", "M", 30, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", null, "M", 30, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", "", 30, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", null, 30, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", "A", 30, 11.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "AA", 30, 11.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "M", -1, 55.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "M", 90, 55.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "M", 30, 11.0, "", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", "M", 30, 11.0, null, typeof(ArgumentNullException))]
        public void PlayerAggregate_Register_InvalidData(String firstName, String lastName, String gender, Int32 age, Decimal exactHandicap, String emailAddress, Type exceptionType)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Should.Throw(() =>
            {
                playerAggregate.Register(firstName, PlayerTestData.MiddleName, lastName, gender, age, exactHandicap, emailAddress);
            }, exceptionType);
        }

        [Fact]
        public void PlayerAggregate_Register_PlayerAlreadyRegistered_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                playerAggregate.Register(PlayerTestData.FirstName, PlayerTestData.MiddleName, PlayerTestData.LastName, PlayerTestData.Gender,
                    PlayerTestData.Age, PlayerTestData.ExactHandicapCat1, PlayerTestData.EmailAddress);
            });
        }

        #endregion
    }
}
