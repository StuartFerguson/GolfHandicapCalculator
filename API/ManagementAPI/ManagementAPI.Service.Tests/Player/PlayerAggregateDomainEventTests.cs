using ManagementAPI.Player.DomainEvents;
using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Player
{
    public class PlayerAggregateDomainEventTests
    {
        [Fact]
        public void PlayerRegisteredEvent_CanBeCreated_IsCreated()
        {
            PlayerRegisteredEvent playerRegisteredEvent = PlayerRegisteredEvent.Create(PlayerTestData.AggregateId, PlayerTestData.FirstName,
                PlayerTestData.MiddleName, PlayerTestData.LastName, PlayerTestData.Gender, PlayerTestData.DateOfBirth, 
                PlayerTestData.ExactHandicap, PlayerTestData.EmailAddress);

            playerRegisteredEvent.ShouldNotBeNull();
            playerRegisteredEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            playerRegisteredEvent.EventId.ShouldNotBe(Guid.Empty);
            playerRegisteredEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            playerRegisteredEvent.FirstName.ShouldBe(PlayerTestData.FirstName);
            playerRegisteredEvent.MiddleName.ShouldBe(PlayerTestData.MiddleName);
            playerRegisteredEvent.LastName.ShouldBe(PlayerTestData.LastName);
            playerRegisteredEvent.Gender.ShouldBe(PlayerTestData.Gender);
            playerRegisteredEvent.DateOfBirth.ShouldBe(PlayerTestData.DateOfBirth);
            playerRegisteredEvent.ExactHandicap.ShouldBe(PlayerTestData.ExactHandicap);
            playerRegisteredEvent.EmailAddress.ShouldBe(PlayerTestData.EmailAddress);
        }

        [Fact]
        public void SecurityUserCreatedEvent_CanBeCreated_IsCreated()
        {
            SecurityUserCreatedEvent securityUserCreatedEvent =
                SecurityUserCreatedEvent.Create(PlayerTestData.AggregateId, PlayerTestData.SecurityUserId);

            securityUserCreatedEvent.ShouldNotBeNull();
            securityUserCreatedEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            securityUserCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            securityUserCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            securityUserCreatedEvent.SecurityUserId.ShouldBe(PlayerTestData.SecurityUserId);
        }

        [Fact]
        public void AcceptedMembershipAddedEvent_CanBeCreated_IsCreated()
        {
            AcceptedMembershipAddedEvent acceptedMembershipAddedEvent = AcceptedMembershipAddedEvent.Create(PlayerTestData.AggregateId, PlayerTestData.GolfClubId, PlayerTestData.MembershipId, PlayerTestData.MembershipNumber,
                PlayerTestData.MembershipAcceptedDateTime);

            acceptedMembershipAddedEvent.ShouldNotBeNull();
            acceptedMembershipAddedEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            acceptedMembershipAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            acceptedMembershipAddedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            acceptedMembershipAddedEvent.GolfClubId.ShouldBe(PlayerTestData.GolfClubId);
            acceptedMembershipAddedEvent.MembershipId.ShouldBe(PlayerTestData.MembershipId);
            acceptedMembershipAddedEvent.MembershipNumber.ShouldBe(PlayerTestData.MembershipNumber);
            acceptedMembershipAddedEvent.AcceptedDateTime.ShouldBe(PlayerTestData.MembershipAcceptedDateTime);
        }

        [Fact]
        public void RejectedMembershipAddedEvent_CanBeCreated_IsCreated()
        {
            RejectedMembershipAddedEvent acceptedMembershipAddedEvent = RejectedMembershipAddedEvent.Create(PlayerTestData.AggregateId, PlayerTestData.GolfClubId, PlayerTestData.MembershipId, PlayerTestData.RejectionReason,
                PlayerTestData.MembershipRejectedDateTime);

            acceptedMembershipAddedEvent.ShouldNotBeNull();
            acceptedMembershipAddedEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            acceptedMembershipAddedEvent.EventId.ShouldNotBe(Guid.Empty);
            acceptedMembershipAddedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            acceptedMembershipAddedEvent.GolfClubId.ShouldBe(PlayerTestData.GolfClubId);
            acceptedMembershipAddedEvent.MembershipId.ShouldBe(PlayerTestData.MembershipId);
            acceptedMembershipAddedEvent.RejectionReason.ShouldBe(PlayerTestData.RejectionReason);
            acceptedMembershipAddedEvent.RejectedDateTime.ShouldBe(PlayerTestData.MembershipRejectedDateTime);
        }
    }
}
