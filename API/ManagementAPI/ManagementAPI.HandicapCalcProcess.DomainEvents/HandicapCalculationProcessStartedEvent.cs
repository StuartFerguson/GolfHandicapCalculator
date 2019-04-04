namespace ManagementAPI.HandicapCalculationProcess.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Shared.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventSourcing.DomainEvent" />
    [JsonObject]
    public class HandicapCalculationProcessStartedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessStartedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapCalculationProcessStartedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessStartedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="startedDateTime">The started date time.</param>
        private HandicapCalculationProcessStartedEvent(Guid aggregateId,
                                                       Guid eventId,
                                                       DateTime startedDateTime) : base(aggregateId, eventId)
        {
            this.StartedDateTime = startedDateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the started date time.
        /// </summary>
        /// <value>
        /// The started date time.
        /// </value>
        [JsonProperty]
        public DateTime StartedDateTime { get; private set; }

        #endregion

        #region Methods

        public static HandicapCalculationProcessStartedEvent Create(Guid aggregateId,
                                                                    DateTime startedDateTime)
        {
            return new HandicapCalculationProcessStartedEvent(aggregateId, Guid.NewGuid(), startedDateTime);
        }

        #endregion
    }
}