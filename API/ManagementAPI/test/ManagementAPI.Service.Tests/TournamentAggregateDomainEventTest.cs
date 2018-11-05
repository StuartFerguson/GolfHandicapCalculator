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
                TournamentTestData.ClubConfigurationId, TournamentTestData.MeasuredCourseId,TournamentTestData.Name, TournamentTestData.MemberCategory,
                TournamentTestData.TournamentFormat);

            tournamentCreatedEvent.ShouldNotBeNull();
            tournamentCreatedEvent.AggregateId.ShouldBe(TournamentTestData.AggregateId);
            tournamentCreatedEvent.TournamentDate.ShouldBe(TournamentTestData.TournamentDate);
            tournamentCreatedEvent.ClubConfigurationId.ShouldBe(TournamentTestData.ClubConfigurationId);
            tournamentCreatedEvent.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            tournamentCreatedEvent.Name.ShouldBe(TournamentTestData.Name);
            tournamentCreatedEvent.MemberCategory.ShouldBe(TournamentTestData.MemberCategory);
            tournamentCreatedEvent.Format.ShouldBe(TournamentTestData.TournamentFormat);
            tournamentCreatedEvent.EventCreatedDateTime.ShouldNotBe(DateTime.MinValue);
            tournamentCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
            tournamentCreatedEvent.EventId.ShouldNotBe(Guid.Empty);
        }
    }
}
