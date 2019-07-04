namespace ManagementAPI.Player.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    [JsonObject]
    public class OpeningExactHandicapAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningExactHandicapAddedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public OpeningExactHandicapAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpeningExactHandicapAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="exactHandicap">The exact handicap.</param>
        private OpeningExactHandicapAddedEvent(Guid aggregateId, Guid eventId, Decimal exactHandicap) : base(aggregateId, eventId)
        {
            this.ExactHandicap = exactHandicap;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the exact handicap.
        /// </summary>
        /// <value>
        /// The exact handicap.
        /// </value>
        [JsonProperty]
        public Decimal ExactHandicap { get; private set; }

        #endregion

        #region Public Methods

        #region public static OpeningExactHandicapAddedEvent Create()
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <returns></returns>
        public static OpeningExactHandicapAddedEvent Create(Guid aggregateId, Decimal exactHandicap)
        {
            return new OpeningExactHandicapAddedEvent(aggregateId, Guid.NewGuid(), exactHandicap);
        }
        #endregion

        #endregion
    }
}