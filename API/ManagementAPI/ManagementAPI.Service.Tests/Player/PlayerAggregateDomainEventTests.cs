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
        public void HandicapAdjustedEvent_CanBeCreated_IsCreated()
        {
            HandicapAdjustedEvent handicapAdjustedEvent = HandicapAdjustedEvent.Create(PlayerTestData.AggregateId, PlayerTestData.NumberOfStrokesBelowCss,
                                                                                       PlayerTestData.AdjustmentValuePerStroke, PlayerTestData.TotalAdjustment,
                                                                                       PlayerTestData.TournamentId, PlayerTestData.GolfClubId,
                                                                                       PlayerTestData.MeasuredCourseId, PlayerTestData.ScoreDate);

            handicapAdjustedEvent.ShouldNotBeNull();
            handicapAdjustedEvent.AggregateId.ShouldBe(PlayerTestData.AggregateId);
            handicapAdjustedEvent.EventId.ShouldNotBe(Guid.Empty);
            handicapAdjustedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            handicapAdjustedEvent.NumberOfStrokesBelowCss.ShouldBe(PlayerTestData.NumberOfStrokesBelowCss);
            handicapAdjustedEvent.AdjustmentValuePerStroke.ShouldBe(PlayerTestData.AdjustmentValuePerStroke);
            handicapAdjustedEvent.TotalAdjustment.ShouldBe(PlayerTestData.TotalAdjustment);
            handicapAdjustedEvent.TournamentId.ShouldBe(PlayerTestData.TournamentId);
            handicapAdjustedEvent.GolfClubId.ShouldBe(PlayerTestData.GolfClubId);
            handicapAdjustedEvent.MeasuredCourseId.ShouldBe(PlayerTestData.MeasuredCourseId);
            handicapAdjustedEvent.ScoreDate.ShouldBe(PlayerTestData.ScoreDate);
        }
    }
}
