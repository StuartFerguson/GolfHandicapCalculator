namespace ManagementAPI.Tournament.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    [JsonObject]
    public class TournamentResultProducedEvent : DomainEvent
    {
        [JsonProperty]
        public DateTime ResultDate { get; private set; }

        #region Constructors 

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPI.Tournament.DomainEvents.TournamentResultProducedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentResultProducedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPI.Tournament.DomainEvents.TournamentResultProducedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="resultDate">The result date.</param>
        private TournamentResultProducedEvent(Guid aggregateId, Guid eventId, DateTime resultDate) : base(aggregateId, eventId)
        {
            this.ResultDate = resultDate;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="resultDate">The result date.</param>
        /// <returns></returns>
        public static TournamentResultProducedEvent Create(Guid aggregateId, DateTime resultDate)
        {
            return new TournamentResultProducedEvent(aggregateId, Guid.NewGuid(), resultDate);
        }

        #endregion
    }
}