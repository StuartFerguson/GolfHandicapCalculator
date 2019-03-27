using System;
using System.Linq;
using ManagementAPI.Tournament.DomainEvents;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Tournament
{
    public class TournamentAggregateDomainEventTests
    {
        [Fact]
        public void TournamentCreatedEvent_CanBeCreated_IsCreated()
        {
            TournamentCreatedEvent tournamentCreatedEvent = TournamentCreatedEvent.Create(TournamentTestData.AggregateId,TournamentTestData.TournamentDate,
                TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, 
                TournamentTestData.MemberCategory, TournamentTestData.TournamentFormat);

            tournamentCreatedEvent.ShouldNotBeNull();
            tournamentCreatedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentCreatedEvent.TournamentDate.ShouldBe(TournamentTestData.TournamentDate);
            tournamentCreatedEvent.GolfClubId.ShouldBe(TournamentTestData.GolfClubId);
            tournamentCreatedEvent.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            tournamentCreatedEvent.MeasuredCourseSSS.ShouldBe(TournamentTestData.MeasuredCourseSSS);
            tournamentCreatedEvent.Name.ShouldBe(TournamentTestData.Name);
            tournamentCreatedEvent.PlayerCategory.ShouldBe(TournamentTestData.MemberCategory);
            tournamentCreatedEvent.Format.ShouldBe(TournamentTestData.TournamentFormat);
            tournamentCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void PlayerScoreRecordedEvent_CanBeCreated_IsCreated()
        {
            PlayerScoreRecordedEvent playerScoreRecordedEvent = PlayerScoreRecordedEvent.Create(TournamentTestData.AggregateId,                
                TournamentTestData.PlayerId,
                TournamentTestData.PlayingHandicap,
                TournamentTestData.HoleScores);

            playerScoreRecordedEvent.ShouldNotBeNull();
            playerScoreRecordedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            playerScoreRecordedEvent.PlayerId.ShouldBe(TournamentTestData.PlayerId);
            playerScoreRecordedEvent.PlayingHandicap.ShouldBe(TournamentTestData.PlayingHandicap);
            playerScoreRecordedEvent.HoleScores.ShouldBe(TournamentTestData.HoleScores);            
            playerScoreRecordedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            playerScoreRecordedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void PlayerScorePublishedEvent_CanBeCreated_IsCreated()
        {
            PlayerScorePublishedEvent playerScorePublishedEvent = PlayerScorePublishedEvent.Create(TournamentTestData.AggregateId,
                                                                                                TournamentTestData.PlayerId,
                                                                                                TournamentTestData.PlayingHandicap,
                                                                                                TournamentTestData.HoleScores,
                                                                                                   TournamentTestData.GolfClubId,
                                                                                                   TournamentTestData.MeasuredCourseId,
                                                                                                   TournamentTestData.GrossScore,
                                                                                                   TournamentTestData.NetScore,
                                                                                                   TournamentTestData.CSS);

            playerScorePublishedEvent.ShouldNotBeNull();
            playerScorePublishedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            playerScorePublishedEvent.PlayerId.ShouldBe(TournamentTestData.PlayerId);
            playerScorePublishedEvent.PlayingHandicap.ShouldBe(TournamentTestData.PlayingHandicap);
            playerScorePublishedEvent.HoleScores.ShouldBe(TournamentTestData.HoleScores);
            playerScorePublishedEvent.GolfClubId.ShouldBe(TournamentTestData.GolfClubId);
            playerScorePublishedEvent.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            playerScorePublishedEvent.GrossScore.ShouldBe(TournamentTestData.GrossScore);
            playerScorePublishedEvent.NetScore.ShouldBe(TournamentTestData.NetScore);
            playerScorePublishedEvent.CSS.ShouldBe(TournamentTestData.CSS);
            playerScorePublishedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            playerScorePublishedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void TournamentCompletedEvent_CanBeCreated_IsCreated()
        {
            TournamentCompletedEvent tournamentCompletedEvent = TournamentCompletedEvent.Create(TournamentTestData.AggregateId, TournamentTestData.CompletedDateTime);

            tournamentCompletedEvent.ShouldNotBeNull();
            tournamentCompletedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentCompletedEvent.CompletedDate.ShouldBe(TournamentTestData.CompletedDateTime);
            tournamentCompletedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentCompletedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void TournamentCancelledEvent_CanBeCreated_IsCreated()
        {
            TournamentCancelledEvent tournamentCancelledEvent = TournamentCancelledEvent.Create(TournamentTestData.AggregateId, TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);

            tournamentCancelledEvent.ShouldNotBeNull();
            tournamentCancelledEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentCancelledEvent.CancelledDate.ShouldBe(TournamentTestData.CancelledDateTime);
            tournamentCancelledEvent.CancellationReason.ShouldBe(TournamentTestData.CancellationReason);
            tournamentCancelledEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentCancelledEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void TournamentCSSCalculatedEvent_CanBeCreated_IsCreated()
        {
            TournamentCSSCalculatedEvent tournamentCssCalculatedEvent =
                TournamentCSSCalculatedEvent.Create(TournamentTestData.AggregateId, TournamentTestData.Adjustment,
                    TournamentTestData.CSS);

            tournamentCssCalculatedEvent.ShouldNotBeNull();
            tournamentCssCalculatedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentCssCalculatedEvent.Adjustment.ShouldBe(TournamentTestData.Adjustment);
            tournamentCssCalculatedEvent.CSS.ShouldBe(TournamentTestData.CSS);
            tournamentCssCalculatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentCssCalculatedEvent.EventId.ShouldNotBe(Guid.Empty);
        }
        
        [Fact]
        public void TournamentResultForPlayerScoreProducedEvent_CanBeCreated_IsCreated()
        {
            TournamentResultForPlayerScoreProducedEvent tournamentResultForPlayerScoreProducedEvent =
                TournamentResultForPlayerScoreProducedEvent.Create(TournamentTestData.AggregateId,
                                                                   TournamentTestData.PlayerId,
                                                                   TournamentTestData.Division,
                                                                   TournamentTestData.DivisionPosition,
                                                                   TournamentTestData.GrossScore,
                                                                   TournamentTestData.PlayingHandicap,
                                                                   TournamentTestData.NetScore,
                                                                   TournamentTestData.Last9HolesScore,
                                                                   TournamentTestData.Last6HolesScore,
                                                                   TournamentTestData.Last3HolesScore);

            tournamentResultForPlayerScoreProducedEvent.ShouldNotBeNull();
            tournamentResultForPlayerScoreProducedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentResultForPlayerScoreProducedEvent.EventId.ShouldNotBe(Guid.Empty);
            tournamentResultForPlayerScoreProducedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentResultForPlayerScoreProducedEvent.PlayerId.ShouldBe(TournamentTestData.PlayerId);
            tournamentResultForPlayerScoreProducedEvent.Division.ShouldBe(TournamentTestData.Division);
            tournamentResultForPlayerScoreProducedEvent.DivisionPosition.ShouldBe(TournamentTestData.DivisionPosition);
            tournamentResultForPlayerScoreProducedEvent.GrossScore.ShouldBe(TournamentTestData.GrossScore);
            tournamentResultForPlayerScoreProducedEvent.PlayingHandicap.ShouldBe(TournamentTestData.PlayingHandicap);
            tournamentResultForPlayerScoreProducedEvent.NetScore.ShouldBe(TournamentTestData.NetScore);
            tournamentResultForPlayerScoreProducedEvent.Last9Holes.ShouldBe(TournamentTestData.Last9HolesScore);
            tournamentResultForPlayerScoreProducedEvent.Last6Holes.ShouldBe(TournamentTestData.Last6HolesScore);
            tournamentResultForPlayerScoreProducedEvent.Last3Holes.ShouldBe(TournamentTestData.Last3HolesScore);
        }

        [Fact]
        public void UnitofWork_CanBeCreated_IsCreated()
        {
            TournamentResultProducedEvent tournamentResultProducedEvent = TournamentResultProducedEvent.Create(TournamentTestData.AggregateId, TournamentTestData.ResultDate);

            tournamentResultProducedEvent.ShouldNotBeNull();
            tournamentResultProducedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentResultProducedEvent.EventId.ShouldNotBe(Guid.Empty);
            tournamentResultProducedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentResultProducedEvent.ResultDate.ShouldBe(TournamentTestData.ResultDate);
        }
    }
}
