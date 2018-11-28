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

    }
}
