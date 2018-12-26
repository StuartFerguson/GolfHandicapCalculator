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
                PlayerTestData.MiddleName, PlayerTestData.LastName, PlayerTestData.Gender, PlayerTestData.Age, 
                PlayerTestData.ExactHandicap, PlayerTestData.EmailAddress);

            playerRegisteredEvent.ShouldNotBeNull();
            playerRegisteredEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            playerRegisteredEvent.EventId.ShouldNotBe(Guid.Empty);
            playerRegisteredEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            playerRegisteredEvent.FirstName.ShouldBe(PlayerTestData.FirstName);
            playerRegisteredEvent.MiddleName.ShouldBe(PlayerTestData.MiddleName);
            playerRegisteredEvent.LastName.ShouldBe(PlayerTestData.LastName);
            playerRegisteredEvent.Gender.ShouldBe(PlayerTestData.Gender);
            playerRegisteredEvent.Age.ShouldBe(PlayerTestData.Age);
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
        public void ClubMembershipRequestedEvent_CanBeCreated_IsCreated()
        {
            ClubMembershipRequestedEvent clubMembershipRequestedEvent = ClubMembershipRequestedEvent.Create(
                PlayerTestData.AggregateId, PlayerTestData.ClubId,
                PlayerTestData.MembershipRequestedDateAndTime);

            clubMembershipRequestedEvent.ShouldNotBeNull();
            clubMembershipRequestedEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            clubMembershipRequestedEvent.EventId.ShouldNotBe(Guid.Empty);
            clubMembershipRequestedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            clubMembershipRequestedEvent.ClubId.ShouldBe(PlayerTestData.ClubId);
            clubMembershipRequestedEvent.MembershipRequestedDateAndTime.ShouldBe(PlayerTestData.MembershipRequestedDateAndTime);
        }
    }
}
