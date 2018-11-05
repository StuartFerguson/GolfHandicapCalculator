using Shared.EventStore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        }
        #endregion

        #endregion

        #region Private Methods

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
            if (this.HasBeenCompleted)
            {
                throw  new InvalidOperationException("This operation cannot be performed on a tournament that has already been cancelled");
            }
        }
        #endregion

        #endregion
    }
}
