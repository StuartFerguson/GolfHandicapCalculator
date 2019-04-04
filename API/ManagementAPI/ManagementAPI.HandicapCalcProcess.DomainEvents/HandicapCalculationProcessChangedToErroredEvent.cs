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
    public class HandicapCalculationProcessChangedToErroredEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessChangedToErroredEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapCalculationProcessChangedToErroredEvent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationProcessChangedToErroredEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="erroredDateTime">The errored date time.</param>
        /// <param name="errorMessage">The error message.</param>
        private HandicapCalculationProcessChangedToErroredEvent(Guid aggregateId,
                                                                Guid eventId,
                                                                DateTime erroredDateTime,
                                                                String errorMessage) : base(aggregateId, eventId)
        {
            this.ErroredDateTime = erroredDateTime;
            this.ErrorMessage = errorMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the errored date time.
        /// </summary>
        /// <value>
        /// The errored date time.
        /// </value>
        [JsonProperty]
        public DateTime ErroredDateTime { get; private set; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public String ErrorMessage { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="erroredDateTime">The errored date time.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static HandicapCalculationProcessChangedToErroredEvent Create(Guid aggregateId,
                                                                               DateTime erroredDateTime,
                                                                               String errorMessage)
        {
            return new HandicapCalculationProcessChangedToErroredEvent(aggregateId, Guid.NewGuid(), erroredDateTime, errorMessage);
        }

        #endregion
    }
}