namespace ManagementAPI.Tournament
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using DataTransferObjects;
    using DomainEvents;
    using Newtonsoft.Json;
    using Shared.EventSourcing;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.Aggregate" />
    public class TournamentAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The player score records
        /// </summary>
        private List<PlayerScoreRecord> PlayerScoreRecords;

        /// <summary>
        /// The signed up players
        /// </summary>
        private List<Guid> SignedUpPlayers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentAggregate()
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private TournamentAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the adjustment.
        /// </summary>
        /// <value>
        /// The adjustment.
        /// </value>
        public Int32 Adjustment { get; private set; }

        /// <summary>
        /// Gets the cancelled date time.
        /// </summary>
        /// <value>
        /// The cancelled date time.
        /// </value>
        public DateTime CancelledDateTime { get; private set; }

        /// <summary>
        /// Gets the cancelled reason.
        /// </summary>
        /// <value>
        /// The cancelled reason.
        /// </value>
        public String CancelledReason { get; private set; }

        /// <summary>
        /// Gets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        public DateTime CompletedDateTime { get; private set; }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public Int32 CSS { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [CSS has been calculated].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CSS has been calculated]; otherwise, <c>false</c>.
        /// </value>
        public Boolean CSSHasBeenCalculated { get; private set; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public TournamentFormat Format { get; private set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCancelled { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCompleted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; private set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; private set; }

        /// <summary>
        /// Gets the measured course SSS.
        /// </summary>
        /// <value>
        /// The measured course SSS.
        /// </value>
        public Int32 MeasuredCourseSSS { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the player category.
        /// </summary>
        /// <value>
        /// The player category.
        /// </value>
        public PlayerCategory PlayerCategory { get; private set; }

        /// <summary>
        /// Gets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        public DateTime TournamentDate { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Produces the result.
        /// </summary>
        public void ProduceResult()
        {
            this.CheckTournamentCSSHasBeenCalculated();

            // TODO: Use the divisions configured for the tournament
            // TODO: Need the measured course and its holes for this (in stableford)

            Int32 division1Start = -10;
            Int32 division1End = 5;

            Int32 division2Start = 6;
            Int32 division2End = 12;

            Int32 division3Start = 13;
            Int32 division3End = 21;

            Int32 division4Start = 22;
            Int32 division4End = 28;

            // calculate the count back elements of each score
            foreach (PlayerScoreRecord playerScoreRecord in this.PlayerScoreRecords)
            {
                CountbackScores countBackScores = this.CalculateCountBackScores(playerScoreRecord, this.Format);
                playerScoreRecord.SetCountBackScores(countBackScores.Last9Holes, countBackScores.Last6Holes,
                                                     countBackScores.Last3Holes);
            }

            IEnumerable<PlayerScoreRecord> division1Scores = this.PlayerScoreRecords.Where(p => p.PlayingHandicap >= division1Start && p.PlayingHandicap <= division1End);
            IEnumerable<PlayerScoreRecord> division2Scores = this.PlayerScoreRecords.Where(p => p.PlayingHandicap >= division2Start && p.PlayingHandicap <= division2End);
            IEnumerable<PlayerScoreRecord> division3Scores = this.PlayerScoreRecords.Where(p => p.PlayingHandicap >= division3Start && p.PlayingHandicap <= division3End);
            IEnumerable<PlayerScoreRecord> division4Scores = this.PlayerScoreRecords.Where(p => p.PlayingHandicap >= division4Start && p.PlayingHandicap <= division4End);

            IOrderedEnumerable<PlayerScoreRecord> division1Result = division1Scores.OrderBy(s => s.NetScore).ThenBy(s => s.Last9HolesScore).ThenBy(s => s.Last6HolesScore).ThenBy(s => s.Last3HolesScore);
            IOrderedEnumerable<PlayerScoreRecord> division2Result = division2Scores.OrderBy(s => s.NetScore).ThenBy(s => s.Last9HolesScore).ThenBy(s => s.Last6HolesScore).ThenBy(s => s.Last3HolesScore);
            IOrderedEnumerable<PlayerScoreRecord> division3Result = division3Scores.OrderBy(s => s.NetScore).ThenBy(s => s.Last9HolesScore).ThenBy(s => s.Last6HolesScore).ThenBy(s => s.Last3HolesScore);
            IOrderedEnumerable<PlayerScoreRecord> division4Result = division4Scores.OrderBy(s => s.NetScore).ThenBy(s => s.Last9HolesScore).ThenBy(s => s.Last6HolesScore).ThenBy(s => s.Last3HolesScore);

            // Start the position counter at 1 for division 1
            Int32 position = 1;
            foreach (PlayerScoreRecord playerScoreRecord in division1Result)
            {
                TournamentResultForPlayerScoreProducedEvent tournamentResultForPlayerScoreProducedEvent = TournamentResultForPlayerScoreProducedEvent.Create(this.AggregateId, 
                                                                                                                                                             playerScoreRecord.PlayerId,
                                                                                                                                                             1, position, playerScoreRecord.GrossScore,
                                                                                                                                                             playerScoreRecord.PlayingHandicap,
                                                                                                                                                             playerScoreRecord.NetScore,
                                                                                                                                                             playerScoreRecord.Last9HolesScore,
                                                                                                                                                             playerScoreRecord.Last6HolesScore,
                                                                                                                                                             playerScoreRecord.Last3HolesScore);

                this.ApplyAndPend(tournamentResultForPlayerScoreProducedEvent);
                position++;
            }

            // Reset the position
            position = 1;
            foreach (PlayerScoreRecord playerScoreRecord in division2Result)
            {
                TournamentResultForPlayerScoreProducedEvent tournamentResultForPlayerScoreProducedEvent = TournamentResultForPlayerScoreProducedEvent.Create(this.AggregateId,
                                                                                                                                                             playerScoreRecord.PlayerId,
                                                                                                                                                             2, position, playerScoreRecord.GrossScore,
                                                                                                                                                             playerScoreRecord.PlayingHandicap,
                                                                                                                                                             playerScoreRecord.NetScore,
                                                                                                                                                             playerScoreRecord.Last9HolesScore,
                                                                                                                                                             playerScoreRecord.Last6HolesScore,
                                                                                                                                                             playerScoreRecord.Last3HolesScore);

                this.ApplyAndPend(tournamentResultForPlayerScoreProducedEvent);
                position++;
            }

            // Reset the position
            position = 1;
            foreach (PlayerScoreRecord playerScoreRecord in division3Result)
            {
                TournamentResultForPlayerScoreProducedEvent tournamentResultForPlayerScoreProducedEvent = TournamentResultForPlayerScoreProducedEvent.Create(this.AggregateId,
                                                                                                                                                             playerScoreRecord.PlayerId,
                                                                                                                                                             3, position, playerScoreRecord.GrossScore,
                                                                                                                                                             playerScoreRecord.PlayingHandicap,
                                                                                                                                                             playerScoreRecord.NetScore,
                                                                                                                                                             playerScoreRecord.Last9HolesScore,
                                                                                                                                                             playerScoreRecord.Last6HolesScore,
                                                                                                                                                             playerScoreRecord.Last3HolesScore);

                this.ApplyAndPend(tournamentResultForPlayerScoreProducedEvent);
                position++;
            }

            // Reset the position
            position = 1; 
            foreach (PlayerScoreRecord playerScoreRecord in division4Result)
            {
                TournamentResultForPlayerScoreProducedEvent tournamentResultForPlayerScoreProducedEvent = TournamentResultForPlayerScoreProducedEvent.Create(this.AggregateId,
                                                                                                                                                             playerScoreRecord.PlayerId,
                                                                                                                                                             4, position, playerScoreRecord.GrossScore,
                                                                                                                                                             playerScoreRecord.PlayingHandicap,
                                                                                                                                                             playerScoreRecord.NetScore,
                                                                                                                                                             playerScoreRecord.Last9HolesScore,
                                                                                                                                                             playerScoreRecord.Last6HolesScore,
                                                                                                                                                             playerScoreRecord.Last3HolesScore);

                this.ApplyAndPend(tournamentResultForPlayerScoreProducedEvent);
                position++;
            }

            TournamentResultProducedEvent tournamentResultProducedEvent = TournamentResultProducedEvent.Create(this.AggregateId, DateTime.Now);
            this.ApplyAndPend(tournamentResultProducedEvent);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentResultForPlayerScoreProducedEvent domainEvent)
        {
            PlayerScoreRecord playerScoreRecord = this.PlayerScoreRecords.Single(p => p.PlayerId == domainEvent.PlayerId);
            playerScoreRecord.SetCountBackScores(domainEvent.Last9Holes, domainEvent.Last6Holes, domainEvent.Last3Holes);
            playerScoreRecord.SetResultDetails(domainEvent.DivisionPosition, domainEvent.Division);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentResultProducedEvent domainEvent)
        {
            this.HasResultBeenProduced = true;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has result been produced.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has result been produced; otherwise, <c>false</c>.
        /// </value>
        [JsonProperty]
        public Boolean HasResultBeenProduced { get; private set; }

        /// <summary>
        /// Calculates the CSS.
        /// </summary>
        public void CalculateCSS()
        {
            this.CheckTournamentHasBeenCreated();

            this.CheckTournamentHasBeenCompleted();

            this.CheckTournamentNotAlreadyCancelled();

            this.CheckTournamentCSSNotAlreadyCalculated();

            // Filter out category 5 players scores
            List<PlayerScoreRecord> filteredScores = this.PlayerScoreRecords.Where(m => m.PlayingHandicap <= 28).ToList();

            // Get the total score count 
            Int32 totalScoreCount = filteredScores.Count();

            // Get a per category score count (including NRs and DQ's)
            Int32 category1ScoreCount = filteredScores.Count(f => f.PlayingHandicap <= 5);
            Int32 category2ScoreCount = filteredScores.Count(f => f.PlayingHandicap >= 6 && f.PlayingHandicap <= 12);
            Int32 category3And4ScoreCount = filteredScores.Count(f => f.PlayingHandicap >= 13 && f.PlayingHandicap <= 28);

            // Now to get the number of buffers or better (by category excluding NRs)
            IEnumerable<PlayerScoreRecord> cat1Scores = filteredScores.Where(s => s.PlayingHandicap <= 5 && s.NetScore != 0);
            IEnumerable<PlayerScoreRecord> cat2Scores = filteredScores.Where(s => s.PlayingHandicap >= 6 && s.PlayingHandicap <= 12 && s.NetScore != 0);
            IEnumerable<PlayerScoreRecord> cat3Scores = filteredScores.Where(s => s.PlayingHandicap >= 13 && s.PlayingHandicap <= 20 && s.NetScore != 0);
            IEnumerable<PlayerScoreRecord> cat4Scores = filteredScores.Where(s => s.PlayingHandicap >= 21 && s.PlayingHandicap <= 28 && s.NetScore != 0);

            Int32 cat1BufferorbetterCount = cat1Scores.Count(s => s.NetScore - this.MeasuredCourseSSS <= 1);
            Int32 cat2BufferorbetterCount = cat2Scores.Count(s => s.NetScore - this.MeasuredCourseSSS <= 2);
            Int32 cat3BufferorbetterCount = cat3Scores.Count(s => s.NetScore - this.MeasuredCourseSSS <= 3);
            Int32 cat4BufferorbetterCount = cat4Scores.Count(s => s.NetScore - this.MeasuredCourseSSS <= 4);

            // Get count of total buffer or better
            Int32 totalBufferOrBetter = cat1BufferorbetterCount + cat2BufferorbetterCount + cat3BufferorbetterCount + cat4BufferorbetterCount;

            // Get percentage of category 1 scores (to nearest 10%)
            Int32 cat1ScorePercentage = ((category1ScoreCount * 100) / totalScoreCount).RoundOff();

            // Get percentage of category 2 scores (to nearest 10%)
            Int32 cat2ScorePercentage = ((category2ScoreCount * 100) / totalScoreCount).RoundOff();

            // Remainder is category 3 & 4
            Int32 cat3ScorePercentage = 100 - (cat1ScorePercentage + cat2ScorePercentage);

            // Calculate percentage of scores at buffer or better
            Decimal scoresAtBufferOrBetterPercentage = Math.Round(Convert.ToDecimal((totalBufferOrBetter * 100) / totalScoreCount));

            // Get the required table entry
            CSSScoreTableEntry cssScoreTableEntry = this.GetCSSScoreTableEntry(cat1ScorePercentage, cat2ScorePercentage, cat3ScorePercentage);

            Int32 adjustment = 0;
            // Get the adjustment based on net scores buffer or better
            foreach (AdjustmentRange adjustmentRange in cssScoreTableEntry.AdjustmentRanges)
            {
                if (scoresAtBufferOrBetterPercentage >= adjustmentRange.RangeStart && scoresAtBufferOrBetterPercentage <= adjustmentRange.RangeEnd)
                {
                    adjustment = adjustmentRange.Adjustment;
                    break;
                }
            }

            // Record the CSS Calculated Event
            TournamentCSSCalculatedEvent tournamentCssCalculatedEvent =
                TournamentCSSCalculatedEvent.Create(this.AggregateId, adjustment, this.MeasuredCourseSSS + adjustment);

            this.ApplyAndPend(tournamentCssCalculatedEvent);
        }

        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="cancelledDateTime">The cancelled date time.</param>
        /// <param name="cancellationReason">The cancellation reason.</param>
        public void CancelTournament(DateTime cancelledDateTime,
                                     String cancellationReason)
        {
            Guard.ThrowIfInvalidDate(cancelledDateTime, typeof(ArgumentNullException), "A completed date time must be provided to cancel a tournament");
            Guard.ThrowIfNullOrEmpty(cancellationReason, typeof(ArgumentNullException), "A cancellation reason time must be provided to cancel a tournament");

            this.CheckTournamentHasBeenCreated();

            this.CheckTournamentNotAlreadyCompleted();

            this.CheckTournamentNotAlreadyCancelled();

            TournamentCancelledEvent tournamentCancelledEvent = TournamentCancelledEvent.Create(this.AggregateId, cancelledDateTime, cancellationReason);
            this.ApplyAndPend(tournamentCancelledEvent);
        }

        /// <summary>
        /// Completes the tournament.
        /// </summary>
        /// <param name="completedDateTime">The completed date time.</param>
        public void CompleteTournament(DateTime completedDateTime)
        {
            Guard.ThrowIfInvalidDate(completedDateTime, typeof(ArgumentNullException), "A completed date time must be provided to complete a tournament");

            this.CheckTournamentHasBeenCreated();

            this.CheckTournamentNotAlreadyCompleted();

            this.CheckTournamentNotAlreadyCancelled();

            foreach (PlayerScoreRecord playerScoreRecord in this.PlayerScoreRecords)
            {
                // Create a new event to 'publish' the players score
                PlayerScorePublishedEvent playerScorePublishedEvent = PlayerScorePublishedEvent.Create(this.AggregateId,
                                                                                                       playerScoreRecord.PlayerId,
                                                                                                       playerScoreRecord.PlayingHandicap,
                                                                                                       playerScoreRecord.HoleScores,
                                                                                                       this.GolfClubId,
                                                                                                       this.MeasuredCourseId);

                this.ApplyAndPend(playerScorePublishedEvent);
            }

            TournamentCompletedEvent tournamentCompletedEvent = TournamentCompletedEvent.Create(this.AggregateId, completedDateTime);
            this.ApplyAndPend(tournamentCompletedEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static TournamentAggregate Create(Guid aggregateId)
        {
            return new TournamentAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="tournamentDate">The tournament date.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="measuredCourseSSS">The measured course SSS.</param>
        /// <param name="name">The name.</param>
        /// <param name="memberCategory">The member category.</param>
        /// <param name="tournamentFormat">The tournament format.</param>
        public void CreateTournament(DateTime tournamentDate,
                                     Guid golfClubId,
                                     Guid measuredCourseId,
                                     Int32 measuredCourseSSS,
                                     String name,
                                     PlayerCategory memberCategory,
                                     TournamentFormat tournamentFormat)
        {
            Guard.ThrowIfInvalidDate(tournamentDate, typeof(ArgumentNullException), " A tournament requires a valid date to be created");
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), " A tournament requires a valid Golf Club Id to be created");
            Guard.ThrowIfInvalidGuid(measuredCourseId, typeof(ArgumentNullException), " A tournament requires a valid Measured Course Id to be created");
            Guard.ThrowIfZero(measuredCourseSSS, typeof(ArgumentOutOfRangeException), "Measured Course SS must not be zero");
            Guard.ThrowIfNegative(measuredCourseSSS, typeof(ArgumentOutOfRangeException), "Measured Course SS must not be negative");
            Guard.ThrowIfNullOrEmpty(name, typeof(ArgumentNullException), " A tournament requires a valid Name to be created");
            Guard.ThrowIfInvalidEnum(typeof(PlayerCategory),
                                     memberCategory,
                                     typeof(ArgumentOutOfRangeException),
                                     " A tournament requires a valid member category to be created");
            Guard.ThrowIfInvalidEnum(typeof(TournamentFormat),
                                     tournamentFormat,
                                     typeof(ArgumentOutOfRangeException),
                                     " A tournament requires a valid tournament format to be created");

            this.CheckTournamentNotAlreadyCreated();

            this.CheckTournamentNotAlreadyCompleted();

            this.CheckTournamentNotAlreadyCancelled();

            TournamentCreatedEvent tournamentCreatedEvent = TournamentCreatedEvent.Create(this.AggregateId,
                                                                                          tournamentDate,
                                                                                          golfClubId,
                                                                                          measuredCourseId,
                                                                                          measuredCourseSSS,
                                                                                          name,
                                                                                          (Int32)memberCategory,
                                                                                          (Int32)tournamentFormat);

            this.ApplyAndPend(tournamentCreatedEvent);
        }

        /// <summary>
        /// Gets the scores.
        /// </summary>
        /// <returns></returns>
        public List<PlayerScoreRecordDataTransferObject> GetScores()
        {
            List<PlayerScoreRecordDataTransferObject> result = new List<PlayerScoreRecordDataTransferObject>();

            foreach (PlayerScoreRecord playerScoreRecord in this.PlayerScoreRecords)
            {
                result.Add(new PlayerScoreRecordDataTransferObject
                           {
                               PlayingHandicap = playerScoreRecord.PlayingHandicap,
                               HoleScores = playerScoreRecord.HoleScores,
                               NetScore = playerScoreRecord.NetScore,
                               PlayerId = playerScoreRecord.PlayerId,
                               GrossScore = playerScoreRecord.GrossScore,
                               HandicapCategory = playerScoreRecord.HandicapCategory,
                               IsPublished = playerScoreRecord.IsPublished,
                               Last9HolesScore = playerScoreRecord.Last9HolesScore,
                               TournamentDivision = playerScoreRecord.TournamentDivision,
                               Last6HolesScore = playerScoreRecord.Last6HolesScore,
                               Last3HolesScore = playerScoreRecord.Last3HolesScore,
                               Position = playerScoreRecord.Position
                });
            }

            return result;
        }

        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="playerId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <exception cref="InvalidDataException"></exception>
        public void RecordPlayerScore(Guid playerId,
                                      Int32 playingHandicap,
                                      Dictionary<Int32, Int32> holeScores)
        {
            Guard.ThrowIfInvalidGuid(playerId, typeof(ArgumentNullException), "Player Id must be provided to record a score");
            Guard.ThrowIfNull(holeScores, typeof(ArgumentNullException), "Hole Scores must be provided to record a score");

            // Check the players playing handicap is valid
            if (playingHandicap > 36)
            {
                throw new InvalidDataException($"{playingHandicap} is greater than the Maximum Playing handicap of {TournamentAggregate.MaximumPlayingHandicap}");
            }

            // Check tournament has been created
            this.CheckTournamentHasBeenCreated();

            // Tournament is not completed
            this.CheckTournamentNotAlreadyCompleted();

            // Tournament is not cancelled
            this.CheckTournamentNotAlreadyCancelled();

            // Player has signed up
            this.CheckPlayerHasSignedUp(playerId);

            // Player must not have already entered a score
            this.CheckForDuplicatePlayerScoreRecord(playerId);

            // Must have 18 hole scores
            this.ValidateHoleScores(holeScores);

            // Crete the event to record the score
            PlayerScoreRecordedEvent playerScoreRecordedEvent = PlayerScoreRecordedEvent.Create(this.AggregateId, playerId, playingHandicap, holeScores);

            // Apply and Pend
            this.ApplyAndPend(playerScoreRecordedEvent);
        }

        /// <summary>
        /// Signs up for tournament.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        public void SignUpForTournament(Guid playerId)
        {
            // Check tournament has been created
            this.CheckTournamentHasBeenCreated();

            // Tournament is not completed
            this.CheckTournamentNotAlreadyCompleted();

            // Tournament is not cancelled
            this.CheckTournamentNotAlreadyCancelled();

            // Check player not already signed up
            this.CheckPlayerNotSignedUp(playerId);

            // Create the domain event
            PlayerSignedUpEvent playerSignedUpEvent = PlayerSignedUpEvent.Create(this.AggregateId, playerId);

            // Apply and Pend
            this.ApplyAndPend(playerSignedUpEvent);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Checks for duplicate player score record.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <exception cref="InvalidOperationException">Player Id {playerId} has already recorded a score for this competition</exception>
        private void CheckForDuplicatePlayerScoreRecord(Guid playerId)
        {
            if (this.PlayerScoreRecords.Any(m => m.PlayerId == playerId))
            {
                throw new InvalidOperationException($"Player Id {playerId} has already recorded a score for this competition");
            }
        }
        
        /// <summary>
        /// Checks the player has signed up.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <exception cref="InvalidOperationException">Player Id {playerId}</exception>
        private void CheckPlayerHasSignedUp(Guid playerId)
        {
            if (this.SignedUpPlayers.All(x => x != playerId))
            {
                throw new InvalidOperationException($"Player Id {playerId} has not signed up for this tournament");
            }
        }

        /// <summary>
        /// Checks the player not signed up.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <exception cref="InvalidOperationException">Player Id {playerId}</exception>
        private void CheckPlayerNotSignedUp(Guid playerId)
        {
            if (this.SignedUpPlayers.Any(x => x == playerId))
            {
                throw new InvalidOperationException($"Player Id {playerId} is already signed up for this tournament");
            }
        }

        /// <summary>
        /// Checks the tournament CSS has been calculated.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has not had a CSS Calculated</exception>
        private void CheckTournamentCSSHasBeenCalculated()
        {
            if (!this.CSSHasBeenCalculated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has not had a CSS Calculated");
            }
        }

        /// <summary>
        /// Checks the tournament CSS not already calculated.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has the CSS Calculated</exception>
        private void CheckTournamentCSSNotAlreadyCalculated()
        {
            if (this.CSSHasBeenCalculated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has the CSS Calculated");
            }
        }

        /// <summary>
        /// Checks the tournament has been completed.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has not already been completed</exception>
        private void CheckTournamentHasBeenCompleted()
        {
            if (!this.HasBeenCompleted)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has not already been completed");
            }
        }

        /// <summary>
        /// Checks the tournament has been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has not been created</exception>
        private void CheckTournamentHasBeenCreated()
        {
            if (!this.HasBeenCreated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has not been created");
            }
        }

        /// <summary>
        /// Checks the tournament not already cancelled.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has already been cancelled</exception>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a tournament that has already been cancelled</exception>
        private void CheckTournamentNotAlreadyCancelled()
        {
            if (this.HasBeenCancelled)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has already been cancelled");
            }
        }

        /// <summary>
        /// Checks the tournament not already completed.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has already been completed</exception>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a tournament that has already been completed</exception>
        private void CheckTournamentNotAlreadyCompleted()
        {
            if (this.HasBeenCompleted)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has already been completed");
            }
        }

        /// <summary>
        /// Checks the tournament not already created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has already been created</exception>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a tournament that has already been created</exception>
        private void CheckTournamentNotAlreadyCreated()
        {
            if (this.HasBeenCreated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has already been created");
            }
        }

        /// <summary>
        /// Gets the CSS score table.
        /// </summary>
        /// <param name="category1Percentage">The category1 percentage.</param>
        /// <param name="category2Percentage">The category2 percentage.</param>
        /// <param name="category3And4Percentage">The category3 and4 percentage.</param>
        /// <returns></returns>
        private CSSScoreTableEntry GetCSSScoreTableEntry(Int32 category1Percentage,
                                                         Int32 category2Percentage,
                                                         Int32 category3And4Percentage)
        {
            List<CSSScoreTableEntry> cssScoreTableEntries = new List<CSSScoreTableEntry>();

            #region Category 1 Zero

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 100,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 9,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 15,
                                                                    RangeStart = 10
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 22,
                                                                    RangeStart = 16
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 45,
                                                                    RangeStart = 23
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 46
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 90,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 9,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 15,
                                                                    RangeStart = 10
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 22,
                                                                    RangeStart = 16
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 45,
                                                                    RangeStart = 23
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 46
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 80,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 9,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 15,
                                                                    RangeStart = 10
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 22,
                                                                    RangeStart = 16
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 45,
                                                                    RangeStart = 23
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 46
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 70,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 60,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 50,
                                         Category3And4Percentage = 50,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 60,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 70,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 80,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 90,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 0,
                                         Category2Percentage = 100,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 Ten

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 90,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 9,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 15,
                                                                    RangeStart = 10
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 22,
                                                                    RangeStart = 16
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 45,
                                                                    RangeStart = 23
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 46
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 80,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 70,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 60,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 50,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 50,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 60,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 70,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 80,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 10,
                                         Category2Percentage = 90,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 20

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 80,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 70,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 60,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 50,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 50,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 60,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 70,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 20,
                                         Category2Percentage = 80,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 30

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 70,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 10,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 16,
                                                                    RangeStart = 11
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 23,
                                                                    RangeStart = 17
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 47,
                                                                    RangeStart = 24
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 48
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 60,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 50,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 50,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 60,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 30,
                                         Category2Percentage = 70,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 40

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 60,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 50,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 17,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 25,
                                                                    RangeStart = 18
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 49,
                                                                    RangeStart = 26
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 50
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 50,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 40,
                                         Category2Percentage = 60,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 50

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 50,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 50,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 50,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 50,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 50,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 51,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 50,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 50,
                                         Category2Percentage = 50,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 60

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 60,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 40,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 60,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 6,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 11,
                                                                    RangeStart = 7
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 18,
                                                                    RangeStart = 12
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 26,
                                                                    RangeStart = 19
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 27
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 52
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 60,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 60,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 60,
                                         Category2Percentage = 40,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 70

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 70,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 30,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 70,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 70,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 70,
                                         Category2Percentage = 30,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 29,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 30
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 58
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 80

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 80,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 80,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 28,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 55,
                                                                    RangeStart = 29
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 56
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 80,
                                         Category2Percentage = 20,
                                         Category3And4Percentage = 20,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 29,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 30
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 58
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 90

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 90,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 10,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 29,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 30
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 58
                                                                }
                                                            }
                                     });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 90,
                                         Category2Percentage = 10,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 29,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 30
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 58
                                                                }
                                                            }
                                     });

            #endregion

            #region Category 1 100

            cssScoreTableEntries.Add(new CSSScoreTableEntry
                                     {
                                         Category1Percentage = 100,
                                         Category2Percentage = 0,
                                         Category3And4Percentage = 0,
                                         AdjustmentRanges = new List<AdjustmentRange>
                                                            {
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 4,
                                                                    RangeEnd = 7,
                                                                    RangeStart = 0
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 3,
                                                                    RangeEnd = 12,
                                                                    RangeStart = 8
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 2,
                                                                    RangeEnd = 19,
                                                                    RangeStart = 13
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 1,
                                                                    RangeEnd = 29,
                                                                    RangeStart = 20
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = 0,
                                                                    RangeEnd = 57,
                                                                    RangeStart = 30
                                                                },
                                                                new AdjustmentRange
                                                                {
                                                                    Adjustment = -1,
                                                                    RangeEnd = 100,
                                                                    RangeStart = 58
                                                                }
                                                            }
                                     });

            #endregion

            // Get the required table entry
            CSSScoreTableEntry cssScoreTableEntry = cssScoreTableEntries.Single(e => e.Category1Percentage == category1Percentage &&
                                                                                     e.Category2Percentage == category2Percentage &&
                                                                                     e.Category3And4Percentage == category3And4Percentage);

            return cssScoreTableEntry;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(PlayerSignedUpEvent domainEvent)
        {
            this.SignedUpPlayers.Add(domainEvent.PlayerId);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentCreatedEvent domainEvent)
        {
            this.Name = domainEvent.Name;
            this.GolfClubId = domainEvent.GolfClubId;
            this.Format = (TournamentFormat)domainEvent.Format;
            this.HasBeenCreated = true;
            this.MeasuredCourseId = domainEvent.MeasuredCourseId;
            this.MeasuredCourseSSS = domainEvent.MeasuredCourseSSS;
            this.PlayerCategory = (PlayerCategory)domainEvent.PlayerCategory;
            this.TournamentDate = domainEvent.TournamentDate;
            this.PlayerScoreRecords = new List<PlayerScoreRecord>();
            this.SignedUpPlayers = new List<Guid>();
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(PlayerScoreRecordedEvent domainEvent)
        {
            PlayerScoreRecord playerScoreRecord = PlayerScoreRecord.Create(domainEvent.PlayerId, domainEvent.PlayingHandicap, domainEvent.HoleScores);

            this.PlayerScoreRecords.Add(playerScoreRecord);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(PlayerScorePublishedEvent domainEvent)
        {
            // Find the recorded score
            PlayerScoreRecord playerScoreRecord = this.PlayerScoreRecords.Single(p => p.PlayerId == domainEvent.PlayerId);
            playerScoreRecord.Publish();
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentCompletedEvent domainEvent)
        {
            this.HasBeenCompleted = true;
            this.CompletedDateTime = domainEvent.CompletedDate;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentCancelledEvent domainEvent)
        {
            this.HasBeenCancelled = true;
            this.CancelledDateTime = domainEvent.CancelledDate;
            this.CancelledReason = domainEvent.CancellationReason;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentCSSCalculatedEvent domainEvent)
        {
            this.CSS = domainEvent.CSS;
            this.Adjustment = domainEvent.Adjustment;
            this.CSSHasBeenCalculated = true;
        }

        /// <summary>
        /// Validates the hole scores.
        /// </summary>
        /// <param name="holeScores">The hole scores.</param>
        /// <exception cref="InvalidDataException">
        /// A score to record must have 18 individual scores
        /// or
        /// Hole numbers {string.Join(",", missingHoleNumbers)}
        /// or
        /// Hole numbers {string.Join(",", missingHoleNumbers)}
        /// </exception>
        /// <exception cref="NotImplementedException"></exception>
        private void ValidateHoleScores(Dictionary<Int32, Int32> holeScores)
        {
            // Check we have 18 scores
            if (holeScores.Count != 18)
            {
                throw new InvalidDataException("A score to record must have 18 individual scores");
            }

            List<Int32> missingHoleNumbers = Enumerable
                                             .Range(TournamentAggregate.MinimumHoleNumber,
                                                    TournamentAggregate.MaximumHoleNumber - TournamentAggregate.MinimumHoleNumber + 1).Except(holeScores.Keys).ToList();

            if (missingHoleNumbers.Count > 0)
            {
                // there are missing scores
                throw new InvalidDataException($"Hole numbers {string.Join(",", missingHoleNumbers)} are missing a score");
            }

            Boolean holesWithNegativeScore = holeScores.Any(h => h.Value < 0);

            if (holesWithNegativeScore)
            {
                // there are negative scores
                throw new InvalidDataException($"Hole numbers {string.Join(",", missingHoleNumbers)} have a negative score");
            }
        }

        /// <summary>
        /// Calculates the count back scores.
        /// </summary>
        /// <param name="playerScoreRecord">The player score record.</param>
        /// <param name="tournamentFormat">The tournament format.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Tournament Format {tournamentFormat}</exception>
        private CountbackScores CalculateCountBackScores(PlayerScoreRecord playerScoreRecord, TournamentFormat tournamentFormat)
        {
            CountbackScores countBackScores = new CountbackScores();
            if (tournamentFormat == TournamentFormat.Strokeplay)
            {
                countBackScores.Last9Holes = this.CalculateStrokePlayCountBackScores(playerScoreRecord, 9);
                countBackScores.Last6Holes = this.CalculateStrokePlayCountBackScores(playerScoreRecord, 6);
                countBackScores.Last3Holes = this.CalculateStrokePlayCountBackScores(playerScoreRecord, 3);
            }
            else
            {
                throw new NotSupportedException($"Tournament Format {tournamentFormat} is not yet supported");
            }

            return countBackScores;
        }

        /// <summary>
        /// Calculates the stroke play count back scores.
        /// </summary>
        /// <param name="playerScoreRecord">The player score record.</param>
        /// <param name="lastHoleCount">The last hole count.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private Decimal CalculateStrokePlayCountBackScores(PlayerScoreRecord playerScoreRecord, Int32 lastHoleCount)
        {
            Int32 lastHoleCountScores = playerScoreRecord.HoleScores.OrderBy(h => h.Key).TakeLast(lastHoleCount).Sum(h => h.Value);

            Decimal lastHoleCountHandicap = Decimal.Round((Decimal)playerScoreRecord.PlayingHandicap / (Decimal)(18 / lastHoleCount), 2, MidpointRounding.AwayFromZero);
            
            return lastHoleCountScores - lastHoleCountHandicap;
        }

        #endregion

        #region Others

        /// <summary>
        /// The maximum hole number
        /// </summary>
        private const Int32 MaximumHoleNumber = 18;

        /// <summary>
        /// The maximum playing handicap
        /// </summary>
        private const Int32 MaximumPlayingHandicap = 36;

        /// <summary>
        /// The minimum hole number
        /// </summary>
        private const Int32 MinimumHoleNumber = 1;

        #endregion
    }
}