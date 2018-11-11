using System;
using System.Linq;
using ManagementAPI.Tournament.DomainEvents;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class TournamentAggregateDomainEventTest
    {
        [Fact]
        public void TournamentCreatedEvent_CanBeCreated_IsCreated()
        {
            TournamentCreatedEvent tournamentCreatedEvent = TournamentCreatedEvent.Create(TournamentTestData.AggregateId,TournamentTestData.TournamentDate,
                TournamentTestData.ClubConfigurationId, TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, 
                TournamentTestData.MemberCategory, TournamentTestData.TournamentFormat);

            tournamentCreatedEvent.ShouldNotBeNull();
            tournamentCreatedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentCreatedEvent.TournamentDate.ShouldBe(TournamentTestData.TournamentDate);
            tournamentCreatedEvent.ClubConfigurationId.ShouldBe(TournamentTestData.ClubConfigurationId);
            tournamentCreatedEvent.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            tournamentCreatedEvent.MeasuredCourseSSS.ShouldBe(TournamentTestData.MeasuredCourseSSS);
            tournamentCreatedEvent.Name.ShouldBe(TournamentTestData.Name);
            tournamentCreatedEvent.MemberCategory.ShouldBe(TournamentTestData.MemberCategory);
            tournamentCreatedEvent.Format.ShouldBe(TournamentTestData.TournamentFormat);
            tournamentCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public void MemberScoreRecordedEvent_CanBeCreated_IsCreated()
        {
            MemberScoreRecordedEvent memberScoreRecordedEvent = MemberScoreRecordedEvent.Create(TournamentTestData.AggregateId,                
                TournamentTestData.MemberId,
                TournamentTestData.PlayingHandicap,
                TournamentTestData.HoleScores);

            memberScoreRecordedEvent.ShouldNotBeNull();
            memberScoreRecordedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            memberScoreRecordedEvent.MemberId.ShouldBe(TournamentTestData.MemberId);
            memberScoreRecordedEvent.PlayingHandicap.ShouldBe(TournamentTestData.PlayingHandicap);
            memberScoreRecordedEvent.HoleScores.ShouldBe(TournamentTestData.HoleScores);            
            memberScoreRecordedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            memberScoreRecordedEvent.EventId.ShouldNotBe(Guid.Empty);
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
    }
}
