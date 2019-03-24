namespace ManagementAPI.Service.Tests.Tournament
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ManagementAPI.Tournament;
    using ManagementAPI.Tournament.DataTransferObjects;
    using Shared.Exceptions;
    using Shouldly;
    using Xunit;

    public class TournamentAggregateTests
    {
        #region Methods

        [Fact]
        public void TournamentAggregate_CalculateCSS_CSSAlreadyCalculated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(1, 2, 7, 20, 5, 5);

            aggregate.CalculateCSS();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Theory]
        [InlineData(1, 2, 7, 20, 5, 5, 1, 71)]
        [InlineData(9, 32, 34, 5, 0, 23, 0, 70)]
        [InlineData(10, 24, 25, 11, 0, 19, 0, 70)]
        [InlineData(5, 27, 5, 11, 0, 5, 3, 73)]
        [InlineData(1, 29, 18, 20, 5, 7, 3, 73)]
        [InlineData(21, 37, 8, 10, 9, 11, 2, 72)]
        public void TournamentAggregate_CalculateCSS_CSSCalculated(Int32 category1Scores,
                                                                   Int32 category2Scores,
                                                                   Int32 category3Scores,
                                                                   Int32 category4Scores,
                                                                   Int32 category5Scores,
                                                                   Int32 bufferorbetter,
                                                                   Int32 expectedAdjustment,
                                                                   Int32 expectedCSS)
        {
            TournamentAggregate aggregate =
                TournamentTestData.GetCompletedTournamentAggregate(category1Scores, category2Scores, category3Scores, category4Scores, category5Scores, bufferorbetter);

            aggregate.CalculateCSS();

            aggregate.Adjustment.ShouldBe(expectedAdjustment);
            aggregate.CSS.ShouldBe(expectedCSS);
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_TournamentCancelled_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCancelledTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_TournamentNotCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Fact]
        public void TournamentAggregate_CalculateCSS_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CalculateCSS(); });
        }

        [Fact]
        public void TournamentAggregate_CanBeCreated_EmptyAggregateId_ErrorThrown()
        {
            Should.Throw<ArgumentNullException>(() =>
                                                {
                                                    TournamentAggregate aggregate = TournamentAggregate.Create(Guid.Empty);
                                                });
        }

        [Fact]
        public void TournamentAggregate_CanBeCreated_IsCreated()
        {
            TournamentAggregate aggregate = TournamentAggregate.Create(TournamentTestData.AggregateId);

            aggregate.ShouldNotBeNull();
            aggregate.AggregateId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Theory]
        [InlineData(false, "reason")]
        [InlineData(true, "")]
        [InlineData(true, null)]
        public void TournamentAggregate_CancelTournament_InvalidData_ErrorThrown(Boolean validCancellationDate,
                                                                                 String cancellationReason)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            DateTime cancellationDateTime = validCancellationDate ? TournamentTestData.CancelledDateTime : DateTime.MinValue;

            Should.Throw<ArgumentNullException>(() => { aggregate.CancelTournament(cancellationDateTime, cancellationReason); });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentAlreadyCancelled_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCancelledTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason); });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentAlreadyCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(1, 2, 7, 20, 5, 5);

            Should.Throw<InvalidOperationException>(() => { aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason); });
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentComplete()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason);

            aggregate.HasBeenCancelled.ShouldBeTrue();
            aggregate.CancelledDateTime.ShouldBe(TournamentTestData.CancelledDateTime);
            aggregate.CancelledReason.ShouldBe(TournamentTestData.CancellationReason);
        }

        [Fact]
        public void TournamentAggregate_CancelTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CancelTournament(TournamentTestData.CancelledDateTime, TournamentTestData.CancellationReason); });
        }

        [Theory]
        [InlineData(false)]
        public void TournamentAggregate_CompleteTournament_InvalidData_ErrorThrown(Boolean validCompleteDate)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            DateTime completeDateTime = validCompleteDate ? TournamentTestData.CompletedDateTime : DateTime.MinValue;

            Should.Throw<ArgumentNullException>(() => { aggregate.CompleteTournament(completeDateTime); });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentAlreadyCancelled_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCancelledTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CompleteTournament(TournamentTestData.CompletedDateTime); });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentAlreadyCompleted_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCompletedTournamentAggregate(1, 2, 7, 20, 5, 5);

            Should.Throw<InvalidOperationException>(() => { aggregate.CompleteTournament(TournamentTestData.CompletedDateTime); });
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentComplete()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            aggregate.CompleteTournament(TournamentTestData.CompletedDateTime);

            aggregate.HasBeenCompleted.ShouldBeTrue();
            aggregate.CompletedDateTime.ShouldBe(TournamentTestData.CompletedDateTime);
            aggregate.GetScores().All(s=> s.IsPublished).ShouldBeTrue();
        }

        [Fact]
        public void TournamentAggregate_CompleteTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.CompleteTournament(TournamentTestData.CompletedDateTime); });
        }

        [Theory]
        [InlineData(false, true, true, 70, "tournament name", PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true, false, true, 70, "tournament name", PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true, true, false, 70, "tournament name", PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true, true, true, 0, "tournament name", PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentOutOfRangeException))]
        [InlineData(true, true, true, -70, "tournament name", PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentOutOfRangeException))]
        [InlineData(true, true, true, 70, null, PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true, true, true, 70, "", PlayerCategory.Gents, TournamentFormat.Strokeplay, typeof(ArgumentNullException))]
        [InlineData(true, true, true, 70, "tournament name", (PlayerCategory)99, TournamentFormat.Strokeplay, typeof(ArgumentOutOfRangeException))]
        [InlineData(true, true, true, 70, "tournament name", PlayerCategory.Gents, (TournamentFormat)99, typeof(ArgumentOutOfRangeException))]
        public void TournamentAggregate_CreateTournament_InvalidData_ErrorThrown(Boolean validTournamentDate,
                                                                                 Boolean validGolfClubId,
                                                                                 Boolean validMeasuredCourseId,
                                                                                 Int32 measuredCourseSSS,
                                                                                 String name,
                                                                                 PlayerCategory memberCategory,
                                                                                 TournamentFormat tournamentFormat,
                                                                                 Type exceptionType)
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            DateTime tournamentDate = validTournamentDate ? TournamentTestData.TournamentDate : DateTime.MinValue;
            Guid golfClubId = validGolfClubId ? TournamentTestData.GolfClubId : Guid.Empty;
            Guid measuredCourseId = validMeasuredCourseId ? TournamentTestData.MeasuredCourseId : Guid.Empty;

            Should.Throw(() => { aggregate.CreateTournament(tournamentDate, golfClubId, measuredCourseId, measuredCourseSSS, name, memberCategory, tournamentFormat); },
                         exceptionType);
        }

        [Fact]
        public void TournamentAggregate_CreateTournament_TournamentAlreadyCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.CreateTournament(TournamentTestData.TournamentDate,
                                                                                   TournamentTestData.GolfClubId,
                                                                                   TournamentTestData.MeasuredCourseId,
                                                                                   TournamentTestData.MeasuredCourseSSS,
                                                                                   TournamentTestData.Name,
                                                                                   TournamentTestData.PlayerCategoryEnum,
                                                                                   TournamentTestData.TournamentFormatEnum);
                                                    });
        }

        [Fact]
        public void TournamentAggregate_CreateTournament_TournamentCreated()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            aggregate.CreateTournament(TournamentTestData.TournamentDate,
                                       TournamentTestData.GolfClubId,
                                       TournamentTestData.MeasuredCourseId,
                                       TournamentTestData.MeasuredCourseSSS,
                                       TournamentTestData.Name,
                                       TournamentTestData.PlayerCategoryEnum,
                                       TournamentTestData.TournamentFormatEnum);

            aggregate.TournamentDate.ShouldBe(TournamentTestData.TournamentDate);
            aggregate.GolfClubId.ShouldBe(TournamentTestData.GolfClubId);
            aggregate.MeasuredCourseId.ShouldBe(TournamentTestData.MeasuredCourseId);
            aggregate.MeasuredCourseSSS.ShouldBe(TournamentTestData.MeasuredCourseSSS);
            aggregate.Name.ShouldBe(TournamentTestData.Name);
            aggregate.PlayerCategory.ShouldBe(TournamentTestData.PlayerCategoryEnum);
            aggregate.Format.ShouldBe(TournamentTestData.TournamentFormatEnum);
            aggregate.HasBeenCreated.ShouldBeTrue();
        }

        [Fact]
        public void TournamentAggregate_GetScores_ScoresReturned()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            List<PlayerScoreRecordDataTransferObject> scores = aggregate.GetScores();

            scores.ShouldNotBeNull();
            scores.Count.ShouldBe(1);
        }
        
        [Theory]
        [InlineData(false, 6, true, typeof(ArgumentNullException))]
        [InlineData(true, 40, true, typeof(InvalidDataException))]
        [InlineData(true, 6, false, typeof(ArgumentNullException))]
        public void TournamentAggregate_RecordPlayerScore_InvalidData_ErrorThrown(Boolean validPlayerId,
                                                                                  Int32 playingHandicap,
                                                                                  Boolean validHoleScores,
                                                                                  Type exceptionType)
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Guid playerId = validPlayerId ? TournamentTestData.PlayerId : Guid.Empty;
            Dictionary<Int32, Int32> holeScores = validHoleScores ? TournamentTestData.HoleScores : null;

            Should.Throw(() => { aggregate.RecordPlayerScore(playerId, playingHandicap, holeScores); }, exceptionType);
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_InvalidData_ExtraHoleScores_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.Throw<InvalidDataException>(() =>
                                               {
                                                   aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                               TournamentTestData.PlayingHandicap,
                                                                               TournamentTestData.HoleScoresExtraScores);
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
        public void TournamentAggregate_RecordPlayerScore_InvalidData_MissingHoleScores_ErrorThrown(Int32 holeNumber)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.Throw<InvalidDataException>(() =>
                                               {
                                                   aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                               TournamentTestData.PlayingHandicap,
                                                                               TournamentTestData.HoleScoresMissingHole(holeNumber));
                                               });
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_InvalidData_MissingHoleScores_MissingHole_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.Throw<InvalidDataException>(() =>
                                               {
                                                   aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                               TournamentTestData.PlayingHandicap,
                                                                               TournamentTestData.HoleScoresMissingHoles);
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
        public void TournamentAggregate_RecordPlayerScore_InvalidData_NegativeHoleScores_ErrorThrown(Int32 holeNumber)
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.Throw<InvalidDataException>(() =>
                                               {
                                                   aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                               TournamentTestData.PlayingHandicap,
                                                                               TournamentTestData.HoleScoresNegativeScore(holeNumber));
                                               });
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_InvalidData_NotAllHoleScores_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.Throw<InvalidDataException>(() =>
                                               {
                                                   aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                               TournamentTestData.PlayingHandicap,
                                                                               TournamentTestData.HoleScoresNotAllPresent);
                                               });
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_PlayerScoreAlreadyRecorded_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                                    TournamentTestData.PlayingHandicap,
                                                                                    TournamentTestData.HoleScores);
                                                    });
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_PlayerScoreRecorded()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.NotThrow(() => { aggregate.RecordPlayerScore(TournamentTestData.PlayerId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScores); });
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_PlayerNotSignedUp_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetCreatedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { aggregate.RecordPlayerScore(TournamentTestData.PlayerId, TournamentTestData.PlayingHandicap, TournamentTestData.HoleScores); });
        }

        [Fact]
        public void TournamentAggregate_RecordPlayerScore_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate aggregate = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() =>
                                                    {
                                                        aggregate.RecordPlayerScore(TournamentTestData.PlayerId,
                                                                                    TournamentTestData.PlayingHandicap,
                                                                                    TournamentTestData.HoleScores);
                                                    });
        }

        [Fact]
        public void TournamentAggregate_SignUpForTournament_PlayerAlreadySignedUp_ErrorThrown()
        {
            TournamentAggregate tournament = TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp();

            Should.Throw<InvalidOperationException>(() => { tournament.SignUpForTournament(TournamentTestData.PlayerId); });
        }

        [Fact]
        public void TournamentAggregate_SignUpForTournament_PlayerSignedUp()
        {
            TournamentAggregate tournament = TournamentTestData.GetCreatedTournamentAggregate();

            tournament.SignUpForTournament(TournamentTestData.PlayerId);
        }

        [Fact]
        public void TournamentAggregate_SignUpForTournament_TournamentCancelled_ErrorThrown()
        {
            TournamentAggregate tournament = TournamentTestData.GetCancelledTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { tournament.SignUpForTournament(TournamentTestData.PlayerId); });
        }

        [Fact]
        public void TournamentAggregate_SignUpForTournament_TournamentCompleted_ErrorThrown()
        {
            TournamentAggregate tournament = TournamentTestData.GetCompletedTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { tournament.SignUpForTournament(TournamentTestData.PlayerId); });
        }

        [Fact]
        public void TournamentAggregate_SignUpForTournament_TournamentNotCreated_ErrorThrown()
        {
            TournamentAggregate tournament = TournamentTestData.GetEmptyTournamentAggregate();

            Should.Throw<InvalidOperationException>(() => { tournament.SignUpForTournament(TournamentTestData.PlayerId); });
        }

        #endregion

        [Fact]
        public void TournamentAggregate_ProduceResult_ResultIsProduced()
        {
            TournamentAggregate tournamentAggregate = TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(20, 15, 23, 16, 0, 5);

            tournamentAggregate.ProduceResult();

            tournamentAggregate.HasResultBeenProduced.ShouldBeTrue();

            List<PlayerScoreRecordDataTransferObject> scores = tournamentAggregate.GetScores();
            scores.Any(s => s.Last9HolesScore == 0).ShouldBeFalse();
            scores.Any(s => s.Last6HolesScore == 0).ShouldBeFalse();
            scores.Any(s => s.Last3HolesScore == 0).ShouldBeFalse();
            scores.Any(s => s.TournamentDivision == 0).ShouldBeFalse();
            scores.Any(s => s.Position == 0).ShouldBeFalse();
        }

        [Fact]
        public void TournamentAggregate_ProduceResult_CSSNotCalculated_ErrorThrown()
        {
            TournamentAggregate tournamentAggregate = TournamentTestData.GetCompletedTournamentAggregate(20, 15, 23, 16, 0, 5);

            Should.Throw<InvalidOperationException>( () => tournamentAggregate.ProduceResult());
        }

        [Fact]
        public void TournamentAggregate_ProduceResult_UnsupportedFormat_ErrorThrown()
        {
            TournamentAggregate tournamentAggregate = TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(20, 15, 23, 16, 0, 5, TournamentFormat.Stableford);

            Should.Throw<NotSupportedException>(() => tournamentAggregate.ProduceResult());
        }
    }
}