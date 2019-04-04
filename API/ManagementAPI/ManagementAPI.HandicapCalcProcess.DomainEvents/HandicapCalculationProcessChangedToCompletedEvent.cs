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
    public class HandicapCalculationProcessChangedToCompletedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessChangedToCompletedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapCalculationProcessChangedToCompletedEvent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessChangedToCompletedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="completedDateTime">The completed date time.</param>
        private HandicapCalculationProcessChangedToCompletedEvent(Guid aggregateId,
                                                                  Guid eventId,
                                                                  DateTime completedDateTime) : base(aggregateId, eventId)
        {
            this.CompletedDateTime = completedDateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        [JsonProperty]
        public DateTime CompletedDateTime { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="completedDateTime">The completed date time.</param>
        /// <returns></returns>
        public static HandicapCalculationProcessChangedToCompletedEvent Create(Guid aggregateId,
                                                                               DateTime completedDateTime)
        {
            return new HandicapCalculationProcessChangedToCompletedEvent(aggregateId, Guid.NewGuid(), completedDateTime);
        }

        #endregion
    }
}