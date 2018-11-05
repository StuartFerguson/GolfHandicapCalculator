using ManagementAPI.TournamentAggregate;
using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class TournamentAggregateTests
    {
        #region Create Tests
        
        [Fact]
        public void TournamentAggregate_CanBeCreated_IsCreated()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Fact]
        public void TournamentAggregate_CanBeCreated_EmptyAggregateId_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                TournamentAggregate.TournamentAggregate aggregate = TournamentAggregate.TournamentAggregate.Create(Guid.Empty);
            });
        }

        #endregion

        #region Create Tournament Tests

        [Fact]
        public void TournamentAggregate_CreateTournament_TournamentCreated()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.ClubConfigurationId, TournamentTestData.MeasuredCourseId, TournamentTestData.Name,
                TournamentTestData.MemberCategoryEnum, TournamentTestData.TournamentFormatEnum);

            aggregate.TournamentDate.ShouldBe(TournamentTestData.TournamentDate);
            aggregate.ClubConfigurationId.ShouldBe(TournamentTestData.ClubConfigurationId);
            aggregate.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            aggregate.Name.ShouldBe(TournamentTestData.Name);
            aggregate.MemberCategory.ShouldBe(TournamentTestData.MemberCategoryEnum);
            aggregate.Format.ShouldBe(TournamentTestData.TournamentFormatEnum);
            aggregate.HasBeenCreated.ShouldBeTrue();
        }

        [Theory]
        [InlineData(false,true, true, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,false, true, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, false, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, null,MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, "",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, "tournament name",(MemberCategory)99, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, "tournament name",MemberCategory.Gents, (TournamentFormat)99, typeof(ArgumentNullException))]
        public void TournamentAggregate_CreateTournament_InvalidData_ErrorThrown(Boolean validTournamentDate, Boolean validClubConfigurationId, Boolean validMeasuredCourseId,
            String name, MemberCategory memberCategory, TournamentFormat tournamentFormat, Type exceptionType)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            DateTime tournamentDate = validTournamentDate ? TournamentTestData.TournamentDate : DateTime.MinValue;
            Guid clubConfigurationId = validClubConfigurationId ? TournamentTestData.ClubConfigurationId : Guid.Empty;
            Guid measuredCourseId = validMeasuredCourseId ? TournamentTestData.MeasuredCourseId : Guid.Empty;

            Should.Throw(() =>
            {                
                aggregate.CreateTournament(tournamentDate, clubConfigurationId, measuredCourseId, name, memberCategory, tournamentFormat);

            },exceptionType);
        }

        [Fact]
        public void TournamentAggregate_CreateTournament_TournamentAlreadyCreated_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.ClubConfigurationId,
                    TournamentTestData.MeasuredCourseId, TournamentTestData.Name,
                    TournamentTestData.MemberCategoryEnum, TournamentTestData.TournamentFormatEnum);
            });
        }

        #endregion
    }
}
