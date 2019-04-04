namespace ManagementAPI.HandicapCalculationProcess.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventSourcing.DomainEvent" />
    [JsonObject]
    public class HandicapCalculationProcessChangedToRunningEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessChangedToRunningEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapCalculationProcessChangedToRunningEvent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessChangedToRunningEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="runningDateTime">The running date time.</param>
        private HandicapCalculationProcessChangedToRunningEvent(Guid aggregateId,
                                                                Guid eventId,
                                                                DateTime runningDateTime) : base(aggregateId, eventId)
        {
            this.RunningDateTime = runningDateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the running date time.
        /// </summary>
        /// <value>
        /// The running date time.
        /// </value>
        [JsonProperty]
        public DateTime RunningDateTime { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="runningDateTime">The running date time.</param>
        /// <returns></returns>
        public static HandicapCalculationProcessChangedToRunningEvent Create(Guid aggregateId,
                                                                             DateTime runningDateTime)
        {
            return new HandicapCalculationProcessChangedToRunningEvent(aggregateId, Guid.NewGuid(), runningDateTime);
        }

        #endregion
    }
}