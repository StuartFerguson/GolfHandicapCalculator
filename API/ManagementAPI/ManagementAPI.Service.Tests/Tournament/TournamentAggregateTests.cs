using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManagementAPI.Tournament;
using Shared.EventStore;
using Shared.Exceptions;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Tournament
{
    public class TournamentAggregateTests
    {
        #region Create Tests
        
        [Fact]
        public void TournamentAggregate_CanBeCreated_IsCreated()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Fact]
        public void TournamentAggregate_CanBeCreated_EmptyAggregateId_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
            {
                TournamentAggregate aggregate = TournamentAggregate.Create(Guid.Empty);
            });
        }

        #endregion

        #region Create Tournament Tests

        [Fact]
        public void TournamentAggregate_CreateTournament_TournamentCreated()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId, TournamentTestData.MeasuredCourseId, 
                TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name, TournamentTestData.MemberCategoryEnum, TournamentTestData.TournamentFormatEnum);

            aggregate.TournamentDate.ShouldBe(TournamentTestData.TournamentDate);
            aggregate.GolfClubId.ShouldBe(TournamentTestData.GolfClubId);
            aggregate.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            aggregate.MeasuredCourseSSS.ShouldBe(TournamentTestData.MeasuredCourseSSS);
            aggregate.Name.ShouldBe(TournamentTestData.Name);
            aggregate.MemberCategory.ShouldBe(TournamentTestData.MemberCategoryEnum);
            aggregate.Format.ShouldBe(TournamentTestData.TournamentFormatEnum);
            aggregate.HasBeenCreated.ShouldBeTrue();
        }

        [Theory]
        [InlineData(false,true, true, 70, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,false, true, 70, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, false, 70, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, 0, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentOutOfRangeException))]
        [InlineData(true,true, true, -70, "tournament name",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentOutOfRangeException))]
        [InlineData(true,true, true, 70, null,MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, 70, "",MemberCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true,true, true, 70, "tournament name",(MemberCategory)99, TournamentFormat.Strokeplay, typeof(ArgumentOutOfRangeException))]
        [InlineData(true,true, true, 70, "tournament name",MemberCategory.Gents, (TournamentFormat)99, typeof(ArgumentOutOfRangeException))]
        public void TournamentAggregate_CreateTournament_InvalidData_ErrorThrown(Boolean validTournamentDate, Boolean validGolfClubId, Boolean validMeasuredCourseId,
            Int32 measuredCourseSSS, String name, MemberCategory memberCategory, TournamentFormat tournamentFormat, Type exceptionType)
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            DateTime tournamentDate = validTournamentDate ? TournamentTestData.TournamentDate : DateTime.MinValue;
            Guid golfClubId = validGolfClubId ? TournamentTestData.GolfClubId : Guid.Empty;
            Guid measuredCourseId = validMeasuredCourseId ? TournamentTestData.MeasuredCourseId : Guid.Empty;

            Should.Throw(() =>
            {                
                aggregate.CreateTournament(tournamentDate, golfClubId, measuredCourseId, measuredCourseSSS, name, memberCategory, tournamentFormat);

            },exceptionType);
        }

        [Fact]
        public void TournamentAggregate_CreateTournament_TournamentAlreadyCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CreateTournament(TournamentTestData.TournamentDate, TournamentTestData.GolfClubId,
                    TournamentTestData.MeasuredCourseId, TournamentTestData.MeasuredCourseSSS, TournamentTestData.Name,
                    TournamentTestData.MemberCategoryEnum, TournamentTestData.TournamentFormatEnum);
            });
        }

        #endregion

        #region Record Member Scores Tests

        [Fact]
        public void TournamentAggregate_RecordMemberScore_MemberScoreRecorded()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.NotThrow(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScores);
            });
        }

        [Theory]
        [InlineData(false, 6, true, typeof(ArgumentNullException))]
        [InlineData(true, 40, true, typeof(InvalidDataException))]
        [InlineData(true, 6, false, typeof(ArgumentNullException))]
        public void TournamentAggregate_RecordMemberScore_InvalidData_ErrorThrown(Boolean validMemberId, Int32 playingHandicap, Boolean validHoleScores, Type exceptionType)
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Guid memberId = validMemberId ? TournamentTestData.MemberId : Guid.Empty;
            Dictionary<Int32, Int32> holeScores = validHoleScores ? TournamentTestData.HoleScores : null;
            
            Should.Throw(() =>
            {
                aggregate.RecordMemberScore(memberId, playingHandicap, holeScores);
            }, exceptionType);
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_MemberScoreAlreadyRecorded_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap,TournamentTestData.HoleScores);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScores);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();
            
            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap,TournamentTestData.HoleScores);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_InvalidData_NotAllHoleScores_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap,TournamentTestData.HoleScoresNotAllPresent);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_InvalidData_MissinHoleScores_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap,TournamentTestData.HoleScoresMissingHoles);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordMemberScore_InvalidData_ExtraHoleScores_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap,TournamentTestData.HoleScoresExtraScores);
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
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScoresMissingHole(holeNumber));
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
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidDataException>(() =>
            {
                aggregate.RecordMemberScore(TournamentTestData.MemberId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScoresNegativeScore(holeNumber));
            });
        }
        

        #endregion

        #region Complete Tournament Tests

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentComplete()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);

            aggregate.HasBeenCompleted.ShouldBeTrue();
            aggregate.CompletedDateTime.ShouldBe(TournamentTestData.CompletedDateTime);
        }

        [Theory]
        [InlineData(false)]
        public void TournamentAggregate_CompleteTournament_InvalidData_ErrorThrown(Boolean validCompleteDate)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            DateTime completeDateTime = validCompleteDate ? TournamentTestData.CompletedDateTime : DateTime.MinValue;

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CompleteTournament(completeDateTime);
            });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);
            });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentAlreadyCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(1,2,7,20,5,5);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);
            });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentAlreadyCancelled_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCancelledTournamentAggregate();

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
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

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
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            DateTime cancellationDateTime = validCancellationDate ? TournamentTestData.CancelledDateTime : DateTime.MinValue;

            Should.Throw<ArgumentNullException>(() =>
            {
                aggregate.CancelTournament(cancellationDateTime, cancellationReason);
            });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
            });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentAlreadyCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(1,2,7,20,5,5);

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
            });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentAlreadyCancelled_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCancelledTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);
            });
        }

        #endregion

        #region Calculate CSS Tests

        [Theory]
        [InlineData(1,2,7,20,5,5,1,71)]
        [InlineData(9,32,34,5,0,23,0,70)]
        [InlineData(10,24,25,11,0,19,0,70)]
        [InlineData(5,27,5,11,0,5,3,73)]
        [InlineData(1,29,18,20,5,7,3,73)]
        [InlineData(21,37,8,10,9,11,2,72)]
        public void TournamentAggregate_CalculateCSS_CSSCalculated(Int32 category1Scores, Int32 category2Scores, Int32 category3Scores,
            Int32 category4Scores, Int32 category5Scores, Int32 bufferorbetter, Int32 expectedAdjustment, Int32 expectedCSS)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(category1Scores, category2Scores, category3Scores,
                category4Scores,category5Scores, bufferorbetter);

            aggregate.CalculateCSS();

            aggregate.Adjustment.ShouldBe(expectedAdjustment);
            aggregate.CSS.ShouldBe(expectedCSS);
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_TournamentNotCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_TournamentCancelled_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCancelledTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_CSSAlreadyCalculated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(1,2,7,20,5,5);

            aggregate.CalculateCSS();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        #endregion

        #region Get Scores Tests

        [Fact]
        public void TournamentAggregate_GetScores_ScoresReturned()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            var scores = aggregate.GetScores();

            scores.ShouldNotBeNull();
            scores.Count.ShouldBe(1);
        }

        #endregion

        #region Record Handicap Adjustment Tests

        [Fact]
        public void TournamentAggregate_RecordHandicapAdjustment_HandicapAdjustmentRecorded()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate();

            var memberScore = aggregate.GetScores().First();
            Should.NotThrow(() =>
            {
                aggregate.RecordHandicapAdjustment(memberScore.MemberId, TournamentTestData.Adjustments);
            });
        }

        [Theory]
        [InlineData(false, true, typeof(ArgumentNullException))]
        [InlineData(true, false, typeof(InvalidDataException))]
        public void TournamentAggregate_InvalidData_ErrorThrown(Boolean validMemberId, Boolean validAdjustments, Type exceptionType)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            Guid memberId = validMemberId ? TournamentTestData.MemberId : Guid.Empty;
            List<Decimal> adjustments = validAdjustments ? TournamentTestData.Adjustments : new List<Decimal>();
            
            Should.Throw(() =>
            {
                aggregate.RecordHandicapAdjustment(memberId, adjustments);
            }, exceptionType);
        }

        [Fact]
        public void TournamentAggregate_RecordHandicapAdjustment_MemberNotFound_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate();

            Should.Throw<NotFoundException>(() =>
            {
                aggregate.RecordHandicapAdjustment(TournamentTestData.MemberId, TournamentTestData.Adjustments);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordHandicapAdjustment_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordHandicapAdjustment(TournamentTestData.MemberId, TournamentTestData.Adjustments);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordHandicapAdjustment_TournamentNotCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordHandicapAdjustment(TournamentTestData.MemberId, TournamentTestData.Adjustments);
            });
        }

        [Fact]
        public void TournamentAggregate_RecordHandicapAdjustment_CSSNotCalculated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
            {
                aggregate.RecordHandicapAdjustment(TournamentTestData.MemberId, TournamentTestData.Adjustments);
            });
        }

        #endregion
    }
}
