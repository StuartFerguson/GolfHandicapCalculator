namespace ManagementAPI.Service.Tests.Player
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ManagementAPI.Player;
    using ManagementAPI.Player.DomainEvents;
    using Shared.EventSourcing;
    using Shared.Exceptions;
    using Shouldly;
    using Xunit;

    public class PlayerAggregateTests
    {
        #region Methods
        
        [Fact]
        public void PlayerAggregate_CanBeCreated_IsCreated()
        {
            PlayerAggregate player = PlayerAggregate.Create(PlayerTestData.AggregateId);

            player.ShouldNotBeNull();
            player.AggregateId.ShouldBe(PlayerTestData.AggregateId);
        }

        [Fact]
        public void PlayerAggregate_CreateSecurityUser_InvalidData_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            Should.Throw<ArgumentNullException>(() => { playerAggregate.CreateSecurityUser(Guid.Empty); });
        }

        [Fact]
        public void PlayerAggregate_CreateSecurityUser_PlayerNotRegistered_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Should.Throw<InvalidOperationException>(() => { playerAggregate.CreateSecurityUser(PlayerTestData.SecurityUserId); });
        }

        [Fact]
        public void PlayerAggregate_CreateSecurityUser_SecurityUserAlreadyAdded_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregateWithSecurityUserCreated();

            Should.Throw<InvalidOperationException>(() => { playerAggregate.CreateSecurityUser(PlayerTestData.SecurityUserId); });
        }

        [Fact]
        public void PlayerAggregate_CreateSecurityUser_SecurityUserCreated()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            playerAggregate.CreateSecurityUser(PlayerTestData.SecurityUserId);

            playerAggregate.SecurityUserId.ShouldBe(PlayerTestData.SecurityUserId);
            playerAggregate.HasSecurityUserBeenCreated.ShouldBeTrue();
        }
        
        [Fact]
        public void PlayerAggregate_InvalidData_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    PlayerAggregate player = PlayerAggregate.Create(Guid.Empty);
                                                });
        }

        [Theory]
        [InlineData("", "lastname", "M", false, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData(null, "lastname", "M", false, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "", "M", false, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", null, "M", false, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", "", false, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", null, false, 11.0, "testemail@email.com", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", "A", false, 11.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "AA", false, 11.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "M", true, 55.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "M", false, 55.0, "testemail@email.com", typeof(ArgumentOutOfRangeException))]
        [InlineData("firstname", "lastname", "M", false, 11.0, "", typeof(ArgumentNullException))]
        [InlineData("firstname", "lastname", "M", false, 11.0, null, typeof(ArgumentNullException))]
        public void PlayerAggregate_Register_InvalidData(String firstName,
                                                         String lastName,
                                                         String gender,
                                                         Boolean dateOfBirthInFuture,
                                                         Decimal exactHandicap,
                                                         String emailAddress,
                                                         Type exceptionType)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            DateTime dateOfBirth = dateOfBirthInFuture ? DateTime.Now.AddYears(5) : PlayerTestData.DateOfBirth;

            Should.Throw(() => { playerAggregate.Register(firstName, PlayerTestData.MiddleName, lastName, gender, dateOfBirth, exactHandicap, emailAddress); },
                         exceptionType);
        }

        [Fact]
        public void PlayerAggregate_Register_PlayerAlreadyRegistered_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        playerAggregate.Register(PlayerTestData.FirstName,
                                                                                 PlayerTestData.MiddleName,
                                                                                 PlayerTestData.LastName,
                                                                                 PlayerTestData.Gender,
                                                                                 PlayerTestData.DateOfBirth,
                                                                                 PlayerTestData.ExactHandicapCat1,
                                                                                 PlayerTestData.EmailAddress);
                                                    });
        }

        [Theory]
        [InlineData(1,0.0)]
        [InlineData(2,5.5)]
        [InlineData(3, 12.5)]
        [InlineData(4, 21.5)]
        [InlineData(5,28.5)]
        public void PlayerAggregate_Register_PlayerRegistered_HandicapEdgeCase_LowEnd(Int32 category,Decimal lowendValue)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Decimal exactHandicap = 0;
            Int32 handicapCategory = 0;

            switch (category)
            {
                case 1:
                    exactHandicap = lowendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat1;
                    break;
                case 2:
                    exactHandicap = lowendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat2;
                    break;
                case 3:
                    exactHandicap = lowendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat3;
                    break;
                case 4:
                    exactHandicap = lowendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat4;
                    break;
                case 5:
                    exactHandicap = lowendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat5;
                    break;
            }

            playerAggregate.Register(PlayerTestData.FirstName,
                                     PlayerTestData.MiddleName,
                                     PlayerTestData.LastName,
                                     PlayerTestData.Gender,
                                     PlayerTestData.DateOfBirth,
                                     exactHandicap,
                                     PlayerTestData.EmailAddress);

            playerAggregate.HandicapCategory.ShouldBe(handicapCategory);
        }

        [Theory]
        [InlineData(1, 5.4)]
        [InlineData(2, 12.4)]
        [InlineData(3, 21.4)]
        [InlineData(4, 28.4)]
        [InlineData(5, 35.9)]
        public void PlayerAggregate_Register_PlayerRegistered_HandicapEdgeCase_HighEnd(Int32 category, Decimal highendValue)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Decimal exactHandicap = 0;
            Int32 handicapCategory = 0;

            switch (category)
            {
                case 1:
                    exactHandicap = highendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat1;
                    break;
                case 2:
                    exactHandicap = highendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat2;
                    break;
                case 3:
                    exactHandicap = highendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat3;
                    break;
                case 4:
                    exactHandicap = highendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat4;
                    break;
                case 5:
                    exactHandicap = highendValue;
                    handicapCategory = PlayerTestData.HandicapCategoryCat5;
                    break;
            }

            playerAggregate.Register(PlayerTestData.FirstName,
                                     PlayerTestData.MiddleName,
                                     PlayerTestData.LastName,
                                     PlayerTestData.Gender,
                                     PlayerTestData.DateOfBirth,
                                     exactHandicap,
                                     PlayerTestData.EmailAddress);

            playerAggregate.HandicapCategory.ShouldBe(handicapCategory);
        }

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

            switch(category)
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

            playerAggregate.Register(PlayerTestData.FirstName,
                                     PlayerTestData.MiddleName,
                                     PlayerTestData.LastName,
                                     PlayerTestData.Gender,
                                     PlayerTestData.DateOfBirth,
                                     exactHandicap,
                                     PlayerTestData.EmailAddress);

            playerAggregate.FirstName.ShouldBe(PlayerTestData.FirstName);
            playerAggregate.MiddleName.ShouldBe(PlayerTestData.MiddleName);
            playerAggregate.LastName.ShouldBe(PlayerTestData.LastName);
            playerAggregate.Gender.ShouldBe(PlayerTestData.Gender);
            playerAggregate.DateOfBirth.ShouldBe(PlayerTestData.DateOfBirth);
            playerAggregate.ExactHandicap.ShouldBe(exactHandicap);
            playerAggregate.EmailAddress.ShouldBe(PlayerTestData.EmailAddress);
            playerAggregate.PlayingHandicap.ShouldBe(playingHandicap);
            playerAggregate.HandicapCategory.ShouldBe(handicapCategory);
            playerAggregate.FullName.ShouldBe(PlayerTestData.FullName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void PlayerAggregate_Register_NoMiddleName_PlayerRegistered(String middleName)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();
            
            Decimal exactHandicap = PlayerTestData.ExactHandicapCat1;
                    Int32 playingHandicap = PlayerTestData.PlayingHandicapCat1;
                    Int32 handicapCategory = PlayerTestData.HandicapCategoryCat1;
            

            playerAggregate.Register(PlayerTestData.FirstName,
                                     middleName,
                                     PlayerTestData.LastName,
                                     PlayerTestData.Gender,
                                     PlayerTestData.DateOfBirth,
                                     exactHandicap,
                                     PlayerTestData.EmailAddress);

            playerAggregate.FirstName.ShouldBe(PlayerTestData.FirstName);
            playerAggregate.MiddleName.ShouldBe(middleName);
            playerAggregate.LastName.ShouldBe(PlayerTestData.LastName);
            playerAggregate.Gender.ShouldBe(PlayerTestData.Gender);
            playerAggregate.DateOfBirth.ShouldBe(PlayerTestData.DateOfBirth);
            playerAggregate.ExactHandicap.ShouldBe(exactHandicap);
            playerAggregate.EmailAddress.ShouldBe(PlayerTestData.EmailAddress);
            playerAggregate.PlayingHandicap.ShouldBe(playingHandicap);
            playerAggregate.HandicapCategory.ShouldBe(handicapCategory);
            playerAggregate.FullName.ShouldBe(PlayerTestData.FullNameEmptyMiddleName);
        }

        [Fact]
        public void PlayerAggregate_AdjustHandicap_HandicapAdjusted()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();
            
            playerAggregate.AdjustHandicap(PlayerTestData.HandicapAdjustmentDecrease, PlayerTestData.TournamentId, PlayerTestData.GolfClubId, PlayerTestData.MeasuredCourseId,
                                           PlayerTestData.ScoreDate);

            playerAggregate.ExactHandicap.ShouldBe(PlayerTestData.NewExactHandicap);
            playerAggregate.PlayingHandicap.ShouldBe(PlayerTestData.NewPlayingHandicap);
        }

        [Fact]
        public void PlayerAggregate_AdjustHandicap_HandicapAdjustmentIsZero_HandicapNotAdjusted()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();
            
            playerAggregate.AdjustHandicap(PlayerTestData.HandicapAdjustmentNoChange, PlayerTestData.TournamentId, PlayerTestData.GolfClubId, PlayerTestData.MeasuredCourseId,
                                           PlayerTestData.ScoreDate);

            playerAggregate.ExactHandicap.ShouldBe(PlayerTestData.ExactHandicap);
            playerAggregate.PlayingHandicap.ShouldBe(PlayerTestData.PlayingHandicap);
        }

        [Fact]
        public void PlayerAggregate_AdjustHandicap_HandicapIncrease_HandicapAdjusted()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();
            
            playerAggregate.AdjustHandicap(PlayerTestData.HandicapAdjustmentIncrease, PlayerTestData.TournamentId, PlayerTestData.GolfClubId, PlayerTestData.MeasuredCourseId,
                                           PlayerTestData.ScoreDate);

            playerAggregate.ExactHandicap.ShouldBe(PlayerTestData.NewExactHandicapIncreased);
            playerAggregate.PlayingHandicap.ShouldBe(PlayerTestData.NewPlayingHandicapIncreased);
        }

        [Fact]
        public void PlayerAggregate_AdjustHandicap_DuplicateAdjustment_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregateWithHandicapAdjustment();
            
            Should.Throw<InvalidOperationException>(() => playerAggregate.AdjustHandicap(PlayerTestData.HandicapAdjustment,
                                                                                         PlayerTestData.TournamentId,
                                                                                         PlayerTestData.GolfClubId,
                                                                                         PlayerTestData.MeasuredCourseId,
                                                                                         PlayerTestData.ScoreDate));
        }

        [Fact]
        public void PlayerAggregate_AdjustHandicap_PlayerNotRegistered_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Should.Throw<InvalidOperationException>(() => playerAggregate.AdjustHandicap(PlayerTestData.HandicapAdjustment,
                                                                                         PlayerTestData.TournamentId,
                                                                                         PlayerTestData.GolfClubId,
                                                                                         PlayerTestData.MeasuredCourseId,
                                                                                         PlayerTestData.ScoreDate));
        }

        [Theory]
        [InlineData(false,true,true,true,true, typeof(ArgumentNullException))]
        [InlineData(true, false, true, true, true, typeof(ArgumentNullException))]
        [InlineData(true, true, false, true, true, typeof(ArgumentNullException))]
        [InlineData(true, true, true, false, true, typeof(ArgumentNullException))]
        [InlineData(true, true, true, true, false, typeof(ArgumentNullException))]
        public void PlayerAggregate_AdjustHandicap_InvalidData_ErrorThrown(Boolean validAdjustment, Boolean validTournamentId, Boolean validGolfClubId,
                                                                           Boolean validMeasuredCourseId, Boolean validScoreDate, Type exceptionType)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            HandicapAdjustmentDataTransferObject handicapAdjustment = validAdjustment
                ? PlayerTestData.HandicapAdjustment
                : null;

            Guid tournamentId = validTournamentId ? PlayerTestData.TournamentId : Guid.Empty;
            Guid golfClubId = validGolfClubId ? PlayerTestData.GolfClubId : Guid.Empty;
            Guid measuredCourseId = validMeasuredCourseId ? PlayerTestData.MeasuredCourseId : Guid.Empty;
            DateTime scoreDate = validScoreDate ? PlayerTestData.ScoreDate: DateTime.MinValue;

            Should.Throw(() =>
                                                {
                                                    playerAggregate.AdjustHandicap(handicapAdjustment,
                                                                                   tournamentId,
                                                                                   golfClubId,
                                                                                   measuredCourseId,
                                                                                   scoreDate);
                                                }, exceptionType);
        }

        #endregion
    }
}