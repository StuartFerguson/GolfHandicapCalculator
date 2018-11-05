using Shared.EventStore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ManagementAPI.Tournament.DomainEvents;
using Newtonsoft.Json;
using Shared.EventSourcing;
using Shared.General;

namespace ManagementAPI.TournamentAggregate
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
        /// Gets a value indicating whether this instance has been cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCancelled { get; private set; }

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

        #region public void CreateTournament(DateTime tournamentDate, Guid clubConfigurationId, Guid measuredCourseId,String name, MemberCategory memberCategory, TournamentFormat format)
        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="tournamentDate">The tournament date.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="memberCategory">The member category.</param>
        /// <param name="tournamentFormat">The tournament format.</param>
        public void CreateTournament(DateTime tournamentDate, Guid clubConfigurationId, Guid measuredCourseId,String name, MemberCategory memberCategory, TournamentFormat tournamentFormat)
        {
            Guard.ThrowIfInvalidDate(tournamentDate, typeof(ArgumentNullException), " A tournament requires a valid date to be created");
            Guard.ThrowIfInvalidGuid(clubConfigurationId, typeof(ArgumentNullException), " A tournament requires a valid Club Configuration Id to be created");
            Guard.ThrowIfInvalidGuid(measuredCourseId, typeof(ArgumentNullException), " A tournament requires a valid Measured Course Id to be created");
            Guard.ThrowIfNullOrEmpty(name, typeof(ArgumentNullException), " A tournament requires a valid Name to be created");
            Guard.ThrowIfInvalidEnum(typeof(MemberCategory), memberCategory, typeof(ArgumentNullException), " A tournament requires a valid member category to be created");
            Guard.ThrowIfInvalidEnum(typeof(TournamentFormat), tournamentFormat, typeof(ArgumentNullException), " A tournament requires a valid tournament format to be created");

            this.CheckTournamentNotAlreadyCreated();

            this.CheckTournamentNotAlreadyCompleted();
            
            this.CheckTournamentNotAlreadyCancelled();

            TournamentCreatedEvent tournamentCreatedEvent = TournamentCreatedEvent.Create(this.AggregateId,
                tournamentDate, clubConfigurationId, measuredCourseId,
                name, (Int32) memberCategory, (Int32) tournamentFormat);

            this.ApplyAndPend(tournamentCreatedEvent);
        }
        #endregion

        #region public void RecordMemberScore(Guid memberId, Dictionary<Int32, Int32> scores)        
        /// <summary>
        /// Records the member score.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="holeScores">The hole scores.</param>
        public void RecordMemberScore(Guid memberId, Dictionary<Int32, Int32> holeScores)
        {
            Guard.ThrowIfInvalidGuid(memberId, typeof(ArgumentNullException), "Member Id must be provided to record a score");
            Guard.ThrowIfNull(holeScores, typeof(ArgumentNullException), "Hole Scores must be provided to record a score");

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
            MemberScoreRecordedEvent memberScoreRecordedEvent = MemberScoreRecordedEvent.Create(this.AggregateId, memberId, holeScores);

            // Apply and Pend
            this.ApplyAndPend(memberScoreRecordedEvent);
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
            this.MemberCategory = (MemberCategory) domainEvent.MemberCategory;
            this.TournamentDate = domainEvent.TournamentDate;
            this.MemberScoreRecords = new List<MemberScoreRecord>();
        }
        #endregion

        #region private void PlayEvent(MemberScoreRecordedEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(MemberScoreRecordedEvent domainEvent)
        {
            MemberScoreRecord memberScoreRecord = MemberScoreRecord.Create(domainEvent.MemberId, domainEvent.HoleScores);    

            this.MemberScoreRecords.Add(memberScoreRecord);
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

        #endregion
    }
}
