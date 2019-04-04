namespace ManagementAPI.HandicapCalculationProcess
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DomainEvents;
    using Shared.EventSourcing;
    using Shared.EventStore;
    using Shared.General;
    using Tournament;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.Aggregate" />
    public class HandicapCalculationProcessAggregate : Aggregate
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapCalculationProcessAggregate()
        {
            // Nothing here            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private HandicapCalculationProcessAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        public DateTime CompletedDateTime { get; private set; }

        /// <summary>
        /// Gets the errored date time.
        /// </summary>
        /// <value>
        /// The errored date time.
        /// </value>
        public DateTime ErroredDateTime { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public String ErrorMessage { get; private set; }

        /// <summary>
        /// Gets the running date time.
        /// </summary>
        /// <value>
        /// The running date time.
        /// </value>
        public DateTime RunningDateTime { get; private set; }

        /// <summary>
        /// Gets the started date time.
        /// </summary>
        /// <value>
        /// The started date time.
        /// </value>
        public DateTime StartedDateTime { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public HandicapProcessStatus Status { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static HandicapCalculationProcessAggregate Create(Guid aggregateId)
        {
            return new HandicapCalculationProcessAggregate(aggregateId);
        }

        /// <summary>
        /// Starts the handicap calculation process.
        /// </summary>
        /// <param name="tournament">The tournament.</param>
        /// <param name="startedDateTime">The started date time.</param>
        /// <exception cref="InvalidOperationException">All Tournament scores must be published to run a handicap calculation process
        /// or
        /// Tournament must be completed to run a handicap calculation process</exception>
        public void StartHandicapCalculationProcess(TournamentAggregate tournament,
                                                    DateTime startedDateTime)
        {
            // TODO: Business rules to protect reruns etc
            // TODO: handle resuming errored processes

            this.EnsureProcessCanBeStarted();

            if (!tournament.HasBeenCompleted)
            {
                throw new InvalidOperationException("Tournament must be completed to run a handicap calculation process");
            }

            if (!tournament.GetScores().All(s => s.IsPublished))
            {
                throw new InvalidOperationException("All Tournament scores must be published to run a handicap calculation process");
            }
            
            HandicapCalculationProcessStartedEvent handicapCalculationProcessStartedEvent =
                HandicapCalculationProcessStartedEvent.Create(this.AggregateId, startedDateTime);

            this.ApplyAndPend(handicapCalculationProcessStartedEvent);
        }

        private void EnsureProcessCanBeStarted()
        {
            if (this.Status != HandicapProcessStatus.Default)
            {
                throw  new InvalidOperationException($"Process cannot be started, current status is {this.Status}");
            }
        }

        private void EnsureProcessCanBeMarkedRunning()
        {
            if (this.Status != HandicapProcessStatus.Started)
            {
                throw new InvalidOperationException($"Process cannot be marked as running, current status is {this.Status}");
            }
        }

        private void EnsureProcessCanBeMarkedCompleted()
        {
            if (this.Status != HandicapProcessStatus.Running)
            {
                throw new InvalidOperationException($"Process cannot be marked as completed, current status is {this.Status}");
            }
        }

        private void EnsureProcessCanBeMarkedErrored()
        {
            if (this.Status != HandicapProcessStatus.Running)
            {
                throw new InvalidOperationException($"Process cannot be marked as errored, current status is {this.Status}");
            }
        }

        /// <summary>
        /// Updates the process to complete.
        /// </summary>
        /// <param name="completedDateTime">The completed date time.</param>
        public void UpdateProcessToComplete(DateTime completedDateTime)
        {
            this.EnsureProcessCanBeMarkedCompleted();

            HandicapCalculationProcessChangedToCompletedEvent handicapCalculationProcessChangedToCompletedEvent =
                HandicapCalculationProcessChangedToCompletedEvent.Create(this.AggregateId, completedDateTime);

            this.ApplyAndPend(handicapCalculationProcessChangedToCompletedEvent);
        }

        /// <summary>
        /// Updates the process to errored.
        /// </summary>
        /// <param name="erroredDateTime">The errored date time.</param>
        /// <param name="errorMessage">The error message.</param>
        public void UpdateProcessToErrored(DateTime erroredDateTime,
                                           String errorMessage)
        {
            this.EnsureProcessCanBeMarkedErrored();

            HandicapCalculationProcessChangedToErroredEvent handicapCalculationProcessChangedToErroredEvent =
                HandicapCalculationProcessChangedToErroredEvent.Create(this.AggregateId, erroredDateTime, errorMessage);

            this.ApplyAndPend(handicapCalculationProcessChangedToErroredEvent);
        }

        /// <summary>
        /// Updates the process to running.
        /// </summary>
        /// <param name="runningDateTime">The running date time.</param>
        public void UpdateProcessToRunning(DateTime runningDateTime)
        {
            this.EnsureProcessCanBeMarkedRunning();

            HandicapCalculationProcessChangedToRunningEvent handicapCalculationProcessChangedToRunningEvent =
                HandicapCalculationProcessChangedToRunningEvent.Create(this.AggregateId, runningDateTime);

            this.ApplyAndPend(handicapCalculationProcessChangedToRunningEvent);
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
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(HandicapCalculationProcessStartedEvent domainEvent)
        {
            this.Status = HandicapProcessStatus.Started;
            this.StartedDateTime = domainEvent.StartedDateTime;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(HandicapCalculationProcessChangedToRunningEvent domainEvent)
        {
            this.Status = HandicapProcessStatus.Running;
            this.RunningDateTime = domainEvent.RunningDateTime;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(HandicapCalculationProcessChangedToErroredEvent domainEvent)
        {
            this.Status = HandicapProcessStatus.Errored;
            this.ErrorMessage = domainEvent.ErrorMessage;
            this.ErroredDateTime = domainEvent.ErroredDateTime;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(HandicapCalculationProcessChangedToCompletedEvent domainEvent)
        {
            this.Status = HandicapProcessStatus.Completed;
            this.CompletedDateTime = domainEvent.CompletedDateTime;
        }

        #endregion
    }
}