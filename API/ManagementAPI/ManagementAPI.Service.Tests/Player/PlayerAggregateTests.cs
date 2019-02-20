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
        public void PlayerAggregate_AddAcceptedMembership_DuplicateMembership_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregateWithMembershipAdded();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        playerAggregate.AddAcceptedMembership(PlayerTestData.GolfClubId,
                                                                                              PlayerTestData.MembershipId,
                                                                                              PlayerTestData.MembershipNumber,
                                                                                              PlayerTestData.MembershipAcceptedDateTime);
                                                    });
        }

        [Theory]
        [InlineData(false, true, "000001")]
        [InlineData(true, false, "000001")]
        [InlineData(true, true, "")]
        [InlineData(true, true, null)]
        public void PlayerAggregate_AddAcceptedMembership_InvalidData_ErrorThrown(Boolean validGolfClubId,
                                                                                  Boolean validMembershipId,
                                                                                  String membershipNumber)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            Guid golfClubId = validGolfClubId ? PlayerTestData.GolfClubId : Guid.Empty;
            Guid membershipId = validMembershipId ? PlayerTestData.MembershipId : Guid.Empty;

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    playerAggregate.AddAcceptedMembership(golfClubId,
                                                                                          membershipId,
                                                                                          membershipNumber,
                                                                                          PlayerTestData.MembershipAcceptedDateTime);
                                                });
        }

        [Fact]
        public void PlayerAggregate_AddAcceptedMembership_MembershipAdded()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            playerAggregate.AddAcceptedMembership(PlayerTestData.GolfClubId,
                                                  PlayerTestData.MembershipId,
                                                  PlayerTestData.MembershipNumber,
                                                  PlayerTestData.MembershipAcceptedDateTime);

            List<DomainEvent> events = playerAggregate.GetPendingEvents();
            events.Count.ShouldBe(2);
            events.Last().ShouldBeOfType<AcceptedMembershipAddedEvent>();
        }

        [Fact]
        public void PlayerAggregate_AddAcceptedMembership_PlayerNotRegistered_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        playerAggregate.AddAcceptedMembership(PlayerTestData.GolfClubId,
                                                                                              PlayerTestData.MembershipId,
                                                                                              PlayerTestData.MembershipNumber,
                                                                                              PlayerTestData.MembershipAcceptedDateTime);
                                                    });
        }

        [Fact]
        public void PlayerAggregate_AddRejectedMembership_DuplicateMembership_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregateWithMembershipAdded();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        playerAggregate.AddRejectedMembership(PlayerTestData.GolfClubId,
                                                                                              PlayerTestData.MembershipId,
                                                                                              PlayerTestData.RejectionReason,
                                                                                              PlayerTestData.MembershipRejectedDateTime);
                                                    });
        }

        [Theory]
        [InlineData(false, true, "Rejected")]
        [InlineData(true, false, "Rejected")]
        [InlineData(true, true, "")]
        [InlineData(true, true, null)]
        public void PlayerAggregate_AddRejectedMembership_InvalidData_ErrorThrown(Boolean validGolfClubId,
                                                                                  Boolean validMembershipId,
                                                                                  String rejectionReason)
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            Guid golfClubId = validGolfClubId ? PlayerTestData.GolfClubId : Guid.Empty;
            Guid membershipId = validMembershipId ? PlayerTestData.MembershipId : Guid.Empty;

            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    playerAggregate.AddRejectedMembership(golfClubId,
                                                                                          membershipId,
                                                                                          rejectionReason,
                                                                                          PlayerTestData.MembershipRejectedDateTime);
                                                });
        }

        [Fact]
        public void PlayerAggregate_AddRejectedMembership_MembershipAdded()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            playerAggregate.AddRejectedMembership(PlayerTestData.GolfClubId,
                                                  PlayerTestData.MembershipId,
                                                  PlayerTestData.RejectionReason,
                                                  PlayerTestData.MembershipRejectedDateTime);

            List<DomainEvent> events = playerAggregate.GetPendingEvents();
            events.Count.ShouldBe(2);
            events.Last().ShouldBeOfType<RejectedMembershipAddedEvent>();
        }

        [Fact]
        public void PlayerAggregate_AddRejectedMembership_PlayerNotRegistered_ErrorThrown()
        {
            PlayerAggregate playerAggregate = PlayerTestData.GetEmptyPlayerAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        playerAggregate.AddRejectedMembership(PlayerTestData.GolfClubId,
                                                                                              PlayerTestData.MembershipId,
                                                                                              PlayerTestData.RejectionReason,
                                                                                              PlayerTestData.MembershipRejectedDateTime);
                                                    });
        }

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
        public void PlayerAggregate_GetMemberships_MembershipListReturned()
        {
            PlayerAggregate aggregate = PlayerTestData.GetRegisteredPlayerAggregateWithMembershipAdded();

            List<ClubMembershipDataTransferObject> membershipList = aggregate.GetClubMemberships();

            membershipList.ShouldNotBeEmpty();
        }

        [Fact]
        public void PlayerAggregate_GetMemberships_NoMemberships_MembershipListReturned()
        {
            PlayerAggregate aggregate = PlayerTestData.GetRegisteredPlayerAggregate();

            Should.Throw<NotFoundException>(() => aggregate.GetClubMemberships());
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
        }

        #endregion
    }
}