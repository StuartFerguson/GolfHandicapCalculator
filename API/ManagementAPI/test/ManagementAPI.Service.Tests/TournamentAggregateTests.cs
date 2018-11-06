using ManagementAPI.TournamentAggregate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ManagementAPI.Service.Commands;
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

        #region Record Member Scores Tests

        [Fact]
        public void TournamentAggregate_RecordMemberScore_MemberScoreRecorded()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.NotThrow(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScores);
            });
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void TournamentAggregate_RecordMemberScore_InvalidData_ErrorThrown(Boolean validMemberId, Boolean validHoleScores)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Guid memberId = validMemberId ? TournamentTestData.MemberId : Guid.Empty;
            Dictionary<Int32, Int32> holeScores = validHoleScores ? TournamentTestData.HoleScores : null;
            
            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.RecordMemberScore(memberId, holeScores);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_MemberScoreAlreadyRecorded_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScores);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScores);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();
            
            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScores);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_InvalidData_NotAllHoleScores_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScoresNotAllPresent);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_InvalidData_MissinHoleScores_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScoresMissingHoles);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_InvalidData_ExtraHoleScores_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScoresExtraScores);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        public void TournamentAggregate_RecordMemberScore_InvalidData_MissingHoleScores_ErrorThrown(Int32 holeNumber)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScoresMissingHole(holeNumber));
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(11)]
        [InlineData(12)]
        [InlineData(13)]
        [InlineData(14)]
        [InlineData(15)]
        [InlineData(16)]
        [InlineData(17)]
        [InlineData(18)]
        public void TournamentAggregate_RecordMemberScore_InvalidData_NegativeHoleScores_ErrorThrown(Int32 holeNumber)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.HoleScoresNegativeScore(holeNumber));
            });
        }
        

        #endregion

        #region Complete Tournament Tests

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentComplete()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);

            aggregate.HasBeenCompleted.ShouldBeTrue();
            aggregate.CompletedDateTime.ShouldBe(TournamentTestData.CompletedDateTime);
        }

        [Theory]
        [InlineData(false)]
        public void TournamentAggregate_CompleteTournament_InvalidData_ErrorThrown(Boolean validCompleteDate)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            DateTime completeDateTime = validCompleteDate ? TournamentTestData.CompletedDateTime : DateTime.MinValue;

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CompleteTournament(completeDateTime);
            });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);
            });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentAlreadyCompleted_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);
            });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentAlreadyCancelled_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCancelledTournament();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);
            });
        }

        #endregion

        #region Cancel Tournament Tests

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentComplete()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);

            aggregate.HasBeenCancelled.ShouldBeTrue();
            aggregate.CancelledDateTime.ShouldBe(TournamentTestData.CancelledDateTime);
            aggregate.CancelledReason.ShouldBe(TournamentTestData.CancellationReason);
        }

        [Theory]
        [InlineData(false,"reason")]
        [InlineData(true,"")]
        [InlineData(true,null)]
        public void TournamentAggregate_CancelTournament_InvalidData_ErrorThrown(Boolean validCancellationDate, String cancellationReason)
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            DateTime cancellationDateTime = validCancellationDate ? TournamentTestData.CancelledDateTime : DateTime.MinValue;

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CancelTournament(cancellationDateTime, cancellationReason);
            });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
            });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentAlreadyCompleted_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
            });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentAlreadyCancelled_ErrorThrown()
        {
            TournamentAggregate.TournamentAggregate aggregate = TournamentTestData.GetCancelledTournament();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
            });
        }

        #endregion
    }
}
