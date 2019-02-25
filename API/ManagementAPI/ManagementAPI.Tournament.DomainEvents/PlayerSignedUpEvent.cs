namespace ManagementAPI.Tournament.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    [JsonObject]
    public class PlayerSignedUpEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberScoreRecordedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerSignedUpEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberScoreRecordedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        private PlayerSignedUpEvent(Guid aggregateId,
                                    Guid eventId,
                                    Guid playerId) : base(aggregateId, eventId)
        {
            this.PlayerId = playerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        [JsonProperty]
        public Guid PlayerId { get; private set; }

        #endregion

        #region Methods

        public static PlayerSignedUpEvent Create(Guid aggregateId,
                                                 Guid playerId)
        {
            return new PlayerSignedUpEvent(aggregateId, Guid.NewGuid(), playerId);
        }

        #endregion
    }
}