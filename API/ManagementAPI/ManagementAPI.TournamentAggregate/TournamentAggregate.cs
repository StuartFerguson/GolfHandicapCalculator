using Shared.EventStore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ManagementAPI.Tournament.DataTransferObjects;
using ManagementAPI.Tournament.DomainEvents;
using Newtonsoft.Json;
using Shared.EventSourcing;
using Shared.Exceptions;
using Shared.General;

namespace ManagementAPI.Tournament
{
    public class TournamentAggregate : Aggregate
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentAggregate"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentAggregate()
        {
            // Nothing here
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentAggregate"/> class.
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
        /// Gets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        public DateTime TournamentDate { get; private set; }

        /// <summary>
        /// Gets the club configuration identifier.
        /// </summary>
        /// <value>
        /// The club configuration identifier.
        /// </value>
        public Guid ClubConfigurationId { get; private set; }

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
        /// Gets the member category.
        /// </summary>
        /// <value>
        /// The member category.
        /// </value>
        public MemberCategory MemberCategory { get; private set; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public TournamentFormat Format { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCompleted { get; private set; }

        /// <summary>
        /// Gets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        public DateTime CompletedDateTime { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCancelled { get; private set; }

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
        /// Gets the adjustment.
        /// </summary>
        /// <value>
        /// The adjustment.
        /// </value>
        public Int32 Adjustment  { get; private set; }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public Int32 CSS  { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [CSS has been calculated].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CSS has been calculated]; otherwise, <c>false</c>.
        /// </value>
        public Boolean CSSHasBeenCalculated { get; private set; }
        
        #endregion

        #region Fields

        /// <summary>
        /// The member score records
        /// </summary>
        private List<MemberScoreRecord> MemberScoreRecords;

        /// <summary>
        /// The minimum hole number
        /// </summary>
        private const Int32 MinimumHoleNumber = 1;

        /// <summary>
        /// The maximum hole number
        /// </summary>
        private const Int32 MaximumHoleNumber = 18;

        /// <summary>
        /// The maximum playing handicap
        /// </summary>
        private const Int32 MaximumPlayingHandicap = 36;

        /// <summary>
        /// The handicap adjustments
        /// </summary>
        private List<HandicapAdjustment> HandicapAdjustments;

        #endregion

        #region Public Methods

        #region public static TournamentAggregate Create(Guid aggregateId)        
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static TournamentAggregate Create(Guid aggregateId)
        {
            return new TournamentAggregate(aggregateId);
        }
        #endregion

        #region public void CreateTournament()
        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="tournamentDate">The tournament date.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="measuredCourseSSS">The measured course SSS.</param>
        /// <param name="name">The name.</param>
        /// <param name="memberCategory">The member category.</param>
        /// <param name="tournamentFormat">The tournament format.</param>
        public void CreateTournament(DateTime tournamentDate, Guid clubConfigurationId, Guid measuredCourseId, Int32 measuredCourseSSS, String name, MemberCategory memberCategory, TournamentFormat tournamentFormat)
        {
            Guard.ThrowIfInvalidDate(tournamentDate, typeof(ArgumentNullException), " A tournament requires a valid date to be created");
            Guard.ThrowIfInvalidGuid(clubConfigurationId, typeof(ArgumentNullException), " A tournament requires a valid Club Configuration Id to be created");
            Guard.ThrowIfInvalidGuid(measuredCourseId, typeof(ArgumentNullException), " A tournament requires a valid Measured Course Id to be created");
            Guard.ThrowIfZero(measuredCourseSSS, typeof(ArgumentOutOfRangeException), "Measured Course SS must not be zero");
            Guard.ThrowIfNegative(measuredCourseSSS, typeof(ArgumentOutOfRangeException), "Measured Course SS must not be negative");
            Guard.ThrowIfNullOrEmpty(name, typeof(ArgumentNullException), " A tournament requires a valid Name to be created");
            Guard.ThrowIfInvalidEnum(typeof(MemberCategory), memberCategory, typeof(ArgumentOutOfRangeException), " A tournament requires a valid member category to be created");
            Guard.ThrowIfInvalidEnum(typeof(TournamentFormat), tournamentFormat, typeof(ArgumentOutOfRangeException), " A tournament requires a valid tournament format to be created");

            this.CheckTournamentNotAlreadyCreated();

            this.CheckTournamentNotAlreadyCompleted();
            
            this.CheckTournamentNotAlreadyCancelled();

            TournamentCreatedEvent tournamentCreatedEvent = TournamentCreatedEvent.Create(this.AggregateId,
                tournamentDate, clubConfigurationId, measuredCourseId, measuredCourseSSS,
                name, (Int32) memberCategory, (Int32) tournamentFormat);

            this.ApplyAndPend(tournamentCreatedEvent);
        }
        #endregion

        #region public void RecordMemberScore(Guid memberId, Dictionary<Int32, Int32> scores)        
        /// <summary>
        /// Records the member score.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        public void RecordMemberScore(Guid memberId, Int32 playingHandicap, Dictionary<Int32, Int32> holeScores)
        {
            Guard.ThrowIfInvalidGuid(memberId, typeof(ArgumentNullException), "Member Id must be provided to record a score");
            Guard.ThrowIfNull(holeScores, typeof(ArgumentNullException), "Hole Scores must be provided to record a score");

            // Check the members playing handicap is valid
            if (playingHandicap > 36)
            {
                throw new InvalidDataException($"{playingHandicap} is greater than the Maximum Playing handicap of {MaximumPlayingHandicap}");
            }

            // Check tournament has been created
            this.CheckTournamentHasBeenCreated();

            // Tournament is not completed
            this.CheckTournamentNotAlreadyCompleted();

            // Tournament is not cancelled
            this.CheckTournamentNotAlreadyCancelled();

            // Member must not have already entered a score
            this.CheckForDuplicateMemberScoreRecord(memberId);

            // Must have 18 hole scores
            this.ValidateHoleScores(holeScores);
            
            // Crete the event to record the score
            MemberScoreRecordedEvent memberScoreRecordedEvent = MemberScoreRecordedEvent.Create(this.AggregateId, memberId, playingHandicap, holeScores);

            // Apply and Pend
            this.ApplyAndPend(memberScoreRecordedEvent);
        }

        #endregion

        #region public void CompleteTournament(DateTime completedDateTime)
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

            TournamentCompletedEvent tournamentCompletedEvent = TournamentCompletedEvent.Create(this.AggregateId, completedDateTime);
            this.ApplyAndPend(tournamentCompletedEvent);
        }
        #endregion

        #region public void CancelTournament(DateTime cancelledDateTime, String cancellationReason)
        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="cancelledDateTime">The cancelled date time.</param>
        /// <param name="cancellationReason">The cancellation reason.</param>
        public void CancelTournament(DateTime cancelledDateTime, String cancellationReason)
        {
            Guard.ThrowIfInvalidDate(cancelledDateTime, typeof(ArgumentNullException), "A completed date time must be provided to cancel a tournament");
            Guard.ThrowIfNullOrEmpty(cancellationReason, typeof(ArgumentNullException), "A cancellation reason time must be provided to cancel a tournament");

            this.CheckTournamentHasBeenCreated();

            this.CheckTournamentNotAlreadyCompleted();

            this.CheckTournamentNotAlreadyCancelled();

            TournamentCancelledEvent tournamentCancelledEvent = TournamentCancelledEvent.Create(this.AggregateId, cancelledDateTime, cancellationReason);
            this.ApplyAndPend(tournamentCancelledEvent);
        }
        #endregion

        #region public void CalculateCSS()        
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
            var filteredScores = this.MemberScoreRecords.Where(m => m.PlayingHandicap <= 28).ToList();

            // Get the total score count 
            var totalScoreCount = filteredScores.Count();

            // Get a per category score count (including NRs and DQ's)
            var category1ScoreCount = filteredScores.Count(f => f.PlayingHandicap <= 5);
            var category2ScoreCount = filteredScores.Count(f => f.PlayingHandicap >= 6 && f.PlayingHandicap <= 12);
            var category3And4ScoreCount = filteredScores.Count(f => f.PlayingHandicap >= 13 && f.PlayingHandicap <= 28);

            // Now to get the number of buffers or better (by category excluding NRs)
            var cat1Scores = filteredScores.Where(s => s.PlayingHandicap <=5 && s.NetScore != 0);
            var cat2Scores = filteredScores.Where(s => s.PlayingHandicap >= 6 && s.PlayingHandicap <=12 && s.NetScore != 0);
            var cat3Scores = filteredScores.Where(s => s.PlayingHandicap >= 13 && s.PlayingHandicap <= 20 && s.NetScore != 0);
            var cat4Scores = filteredScores.Where(s => s.PlayingHandicap >= 21 && s.PlayingHandicap <= 28 && s.NetScore != 0);

            var cat1BufferorbetterCount = cat1Scores.Count(s => s.NetScore - MeasuredCourseSSS <= 1);
            var cat2BufferorbetterCount = cat2Scores.Count(s => s.NetScore - MeasuredCourseSSS <= 2);
            var cat3BufferorbetterCount = cat3Scores.Count(s => s.NetScore - MeasuredCourseSSS <= 3);
            var cat4BufferorbetterCount = cat4Scores.Count(s => s.NetScore - MeasuredCourseSSS <= 4);

            // Get count of total buffer or better
            var totalBufferOrBetter = cat1BufferorbetterCount + cat2BufferorbetterCount + cat3BufferorbetterCount +
                    cat4BufferorbetterCount;

            // Get percentage of category 1 scores (to nearest 10%)
            var cat1ScorePercentage = ((category1ScoreCount * 100) / totalScoreCount).RoundOff();

            // Get percentage of category 2 scores (to nearest 10%)
            var cat2ScorePercentage = ((category2ScoreCount * 100) / totalScoreCount).RoundOff();

            // Remainder is category 3 & 4
            var cat3ScorePercentage = 100 - (cat1ScorePercentage + cat2ScorePercentage);

            // Calculate percentage of scores at buffer or better
            var scoresAtBufferOrBetterPercentage =
                Math.Round(Convert.ToDecimal((totalBufferOrBetter * 100) / totalScoreCount));

            // Get the required table entry
            var cssScoreTableEntry =
                this.GetCSSScoreTableEntry(cat1ScorePercentage, cat2ScorePercentage, cat3ScorePercentage);

            Int32 adjustment = 0;
            // Get the adjustment based on net scores buffer or better
            foreach (var adjustmentRange in cssScoreTableEntry.AdjustmentRanges)
            {
                if (scoresAtBufferOrBetterPercentage >= adjustmentRange.RangeStart &&
                    scoresAtBufferOrBetterPercentage <= adjustmentRange.RangeEnd)
                {
                    adjustment = adjustmentRange.Adjustment;
                    break;
                }
            }

            // Record the CSS Calculated Event
            TournamentCSSCalculatedEvent tournamentCssCalculatedEvent= TournamentCSSCalculatedEvent.Create(this.AggregateId, adjustment, MeasuredCourseSSS + adjustment);
            
            this.ApplyAndPend(tournamentCssCalculatedEvent);
        }
        #endregion

        #region public void RecordHandicapAdjustment(Guid memberId, List<Decimal> adjustments)
        /// <summary>
        /// Records the handicap adjustment.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="adjustments">The adjustments.</param>
        public void RecordHandicapAdjustment(Guid memberId, List<Decimal> adjustments)
        {
            Guard.ThrowIfInvalidGuid(memberId, typeof(ArgumentNullException), "A valid member Id must be provided to record a handicap adjustment");

            if (!adjustments.Any())
            {
                throw new InvalidDataException("At least one adjustment has to be provided to record a handicap adjustment");
            }
            
            this.CheckTournamentHasBeenCreated();

            this.CheckTournamentHasBeenCompleted();

            this.CheckTournamentCSSHasBeenCalculated();

            this.CheckMemberHasRecordedScore(memberId);

            // Get the members score
            var memberScore = this.MemberScoreRecords.Single(s => s.MemberId == memberId);

            HandicapAdjustmentRecordedEvent handicapAdjustmentRecordedEvent = HandicapAdjustmentRecordedEvent.Create(this.AggregateId,memberId, memberScore.GrossScore, memberScore.NetScore, 
                this.CSS, memberScore.PlayingHandicap, adjustments, adjustments.Sum());

            this.ApplyAndPend(handicapAdjustmentRecordedEvent);
        }        

        #endregion

        #region public List<MemberScoreRecordDataTransferObject> GetScores()
        /// <summary>
        /// Gets the scores.
        /// </summary>
        /// <returns></returns>
        public List<MemberScoreRecordDataTransferObject> GetScores()
        {
            List<MemberScoreRecordDataTransferObject> result = new List<MemberScoreRecordDataTransferObject>();

            foreach (var memberScoreRecord in this.MemberScoreRecords)
            {
                result.Add(new MemberScoreRecordDataTransferObject
                {
                    PlayingHandicap = memberScoreRecord.PlayingHandicap,
                    HoleScores = memberScoreRecord.HoleScores,
                    NetScore = memberScoreRecord.NetScore,
                    MemberId = memberScoreRecord.MemberId,
                    GrossScore = memberScoreRecord.GrossScore,
                    HandicapCategory = memberScoreRecord.HandicapCategory
                });
            }

            return result;
        }
        #endregion
        
        #endregion

        #region Protected Methods

        #region protected override void PlayEvent(DomainEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic) domainEvent);
        }
        #endregion

        #endregion

        #region Private Methods (Play Event)

        #region private void PlayEvent(TournamentCreatedEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentCreatedEvent domainEvent)
        {
            this.Name = domainEvent.Name;
            this.ClubConfigurationId = domainEvent.ClubConfigurationId;
            this.Format = (TournamentFormat) domainEvent.Format;
            this.HasBeenCreated = true;
            this.MeasuredCourseId = domainEvent.MeasuredCourseId;
            this.MeasuredCourseSSS = domainEvent.MeasuredCourseSSS;
            this.MemberCategory = (MemberCategory) domainEvent.MemberCategory;
            this.TournamentDate = domainEvent.TournamentDate;
            this.MemberScoreRecords = new List<MemberScoreRecord>();
            this.HandicapAdjustments = new List<HandicapAdjustment>();
        }
        #endregion

        #region private void PlayEvent(MemberScoreRecordedEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(MemberScoreRecordedEvent domainEvent)
        {
            MemberScoreRecord memberScoreRecord = MemberScoreRecord.Create(domainEvent.MemberId, domainEvent.PlayingHandicap, domainEvent.HoleScores);    

            this.MemberScoreRecords.Add(memberScoreRecord);
        }
        #endregion

        #region private void PlayEvent(TournamentCompletedEvent domainEvent)
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentCompletedEvent domainEvent)
        {
            this.HasBeenCompleted = true;
            this.CompletedDateTime = domainEvent.CompletedDate;
        }
        #endregion

        #region private void PlayEvent(TournamentCancelledEvent domainEvent)
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
        #endregion

        #region private void PlayEvent(TournamentCSSCalculatedEvent domainEvent)
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
        #endregion

        #region private void PlayEvent(HandicapAdjustmentRecordedEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(HandicapAdjustmentRecordedEvent domainEvent)
        {
            HandicapAdjustment handicapAdjustment = HandicapAdjustment.Create(domainEvent.MemberId,domainEvent.Adjustments, domainEvent.TotalAdjustment);

            this.HandicapAdjustments.Add(handicapAdjustment);
        }
        #endregion

        #endregion

        #region Private Methods

        #region private void CheckTournamentHasBeenCreated()
        /// <summary>
        /// Checks the tournament has been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a tournament that has not been created</exception>
        private void CheckTournamentHasBeenCreated()
        {
            if (!this.HasBeenCreated)
            {
                throw  new InvalidOperationException("This operation cannot be performed on a tournament that has not been created");
            }
        }
        #endregion

        #region private void CheckTournamentNotAlreadyCreated()        
        /// <summary>
        /// Checks the tournament not already created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a tournament that has already been created</exception>
        private void CheckTournamentNotAlreadyCreated()
        {
            if (this.HasBeenCreated)
            {
                throw  new InvalidOperationException("This operation cannot be performed on a tournament that has already been created");
            }
        }
        #endregion

        #region private void CheckTournamentNotAlreadyCompleted()                
        /// <summary>
        /// Checks the tournament not already completed.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a tournament that has already been completed</exception>
        private void CheckTournamentNotAlreadyCompleted()
        {
            if (this.HasBeenCompleted)
            {
                throw  new InvalidOperationException("This operation cannot be performed on a tournament that has already been completed");
            }
        }
        #endregion

        #region private void CheckTournamentNotAlreadyCancelled()                
        /// <summary>
        /// Checks the tournament not already cancelled.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a tournament that has already been cancelled</exception>
        private void CheckTournamentNotAlreadyCancelled()
        {
            if (this.HasBeenCancelled)
            {
                throw  new InvalidOperationException("This operation cannot be performed on a tournament that has already been cancelled");
            }
        }
        #endregion

        #region private void CheckTournamentCSSNotAlreadyCalculated()                        
        /// <summary>
        /// Checks the tournament CSS not already calculated.
        /// </summary>
        private void CheckTournamentCSSNotAlreadyCalculated()
        {
            if (this.CSSHasBeenCalculated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has the CSS Calculated");
            }
        }
        #endregion

        #region private void CheckTournamentCSSHasBeenCalculated()   
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
        #endregion

        #region private void CheckTournamentHasBeenCompleted()                        
        /// <summary>
        /// Checks the tournament has been completed.
        /// </summary>
        private void CheckTournamentHasBeenCompleted()
        {
            if (!this.HasBeenCompleted)
            {
                throw new InvalidOperationException("This operation cannot be performed on a tournament that has not already been completed");
            }
        }
        #endregion

        #region private void CheckForDuplicateMemberScoreRecord(Guid memberId)
        /// <summary>
        /// Checks for duplicate member score record.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <exception cref="InvalidOperationException">Member Id {} has already recorded a score for this competition</exception>
        private void CheckForDuplicateMemberScoreRecord(Guid memberId)
        {
            if (this.MemberScoreRecords.Any(m => m.MemberId == memberId))
            {
                throw new InvalidOperationException("Member Id {} has already recorded a score for this competition");
            }
        }
        #endregion

        #region private void CheckMemberHasRecordedScore(Guid memberId)        
        /// <summary>
        /// Checks the member has recorded score.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <exception cref="NotFoundException"></exception>
        private void CheckMemberHasRecordedScore(Guid memberId)
        {
            if (!this.MemberScoreRecords.Any(m => m.MemberId == memberId))
            {
                throw new NotFoundException($"No score record found for {memberId}");
            }
        }
        #endregion

        #region private void ValidateHoleScores(Dictionary<Int32, Int32> holeScores)
        /// <summary>
        /// Validates the hole scores.
        /// </summary>
        /// <param name="holeScores">The hole scores.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void ValidateHoleScores(Dictionary<Int32, Int32> holeScores)
        {
            // Check we have 18 scores
            if (holeScores.Count != 18)
            {
                throw new InvalidDataException("A score to record must have 18 individual scores");
            }
            
            var missingHoleNumbers = Enumerable.Range(MinimumHoleNumber, MaximumHoleNumber- MinimumHoleNumber + 1).Except(holeScores.Keys).ToList();

            if (missingHoleNumbers.Count > 0)
            {
                // there are missing scores
                throw new InvalidDataException($"Hole numbers {String.Join(",", missingHoleNumbers)} are missing a score");
            }

            var holesWithNegativeScore = holeScores.Any(h => h.Value < 0);

            if (holesWithNegativeScore)
            {
                // there are negative scores
                throw new InvalidDataException($"Hole numbers {String.Join(",", missingHoleNumbers)} have a negative score");
            }
        }
        #endregion        
        
        #region private List<CSSScoreTableEntry> GetCSSScoreTable()        
        /// <summary>
        /// Gets the CSS score table.
        /// </summary>
        /// <returns></returns>
        private CSSScoreTableEntry GetCSSScoreTableEntry(Int32 category1Percentage, Int32 category2Percentage,Int32 category3And4Percentage)
        {
            List<CSSScoreTableEntry> cssScoreTableEntries = new List<CSSScoreTableEntry>();
            
            #region Category 1 Zero
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 0, Category3And4Percentage = 100, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 10, Category3And4Percentage = 90, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 20, Category3And4Percentage = 80, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 30, Category3And4Percentage = 70, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 40, Category3And4Percentage = 60, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 50, Category3And4Percentage = 50, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 60, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 70, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 80, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 90, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 0, Category2Percentage = 100, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion

            #region Category 1 Ten
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 0, Category3And4Percentage = 90, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 10, Category3And4Percentage = 80, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 20, Category3And4Percentage = 70, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 30, Category3And4Percentage = 60, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 40, Category3And4Percentage = 50, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 50, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 60, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 70, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 80, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 10, Category2Percentage = 90, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion

            #region Category 1 20
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 0, Category3And4Percentage = 80, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 10, Category3And4Percentage = 70, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 20, Category3And4Percentage = 60, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 30, Category3And4Percentage = 50, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 40, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 50, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 60, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 70, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 20, Category2Percentage = 80, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion
            
            #region Category 1 30
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 0, Category3And4Percentage = 70, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 10, Category3And4Percentage = 60, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 20, Category3And4Percentage = 50, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 30, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 40, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 50, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 60, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 30, Category2Percentage = 70, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion

            #region Category 1 40

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 0, Category3And4Percentage = 60, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 10, Category3And4Percentage = 50, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 20, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 30, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 40, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 50, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 40, Category2Percentage = 60, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            #endregion
            
            #region Category 1 50

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 50, Category2Percentage = 0, Category3And4Percentage = 50, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 50, Category2Percentage = 10, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 50, Category2Percentage = 20, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 50, Category2Percentage = 30, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 50, Category2Percentage = 40, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 50, Category2Percentage = 50, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            #endregion

            #region Category 1 60

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 60, Category2Percentage = 0, Category3And4Percentage = 40, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 60, Category2Percentage = 10, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 60, Category2Percentage = 20, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 60, Category2Percentage = 30, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 60, Category2Percentage = 40, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion

            #region Category 1 70

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 70, Category2Percentage = 0, Category3And4Percentage = 30, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 70, Category2Percentage = 10, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 70, Category2Percentage = 20, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 70, Category2Percentage = 30, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion
            
            #region Category 1 80

            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 80, Category2Percentage = 0, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 80, Category2Percentage = 10, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 80, Category2Percentage = 20, Category3And4Percentage = 20, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });

            #endregion

            #region Category 1 90
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 90, Category2Percentage = 0, Category3And4Percentage = 10, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 90, Category2Percentage = 10, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion

            #region Category 1 100
            cssScoreTableEntries.Add(new CSSScoreTableEntry
            {
                Category1Percentage = 100, Category2Percentage = 0, Category3And4Percentage = 0, AdjustmentRanges = new List<AdjustmentRange>()
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
                },                
            });
            #endregion

            // Get the required table entry
            var cssScoreTableEntry = cssScoreTableEntries.Where(e => e.Category1Percentage == category1Percentage
                                                                     && e.Category2Percentage == category2Percentage
                                                                     && e.Category3And4Percentage ==
                                                                     category3And4Percentage).Single();

            return cssScoreTableEntry;
        }
        #endregion
        
        #endregion
    }
}
