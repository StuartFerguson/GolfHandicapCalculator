namespace ManagementAPI.GolfClub.DomainEvents
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
    public class MatchSecretarySecurityUserCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSecretarySecurityUserCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MatchSecretarySecurityUserCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchSecretarySecurityUserCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="matchSecretarySecurityUserId">The match secretary security user identifier.</param>
        private MatchSecretarySecurityUserCreatedEvent(Guid aggregateId,
                                                       Guid eventId,
                                                       Guid matchSecretarySecurityUserId) : base(aggregateId, eventId)
        {
            this.MatchSecretarySecurityUserId = matchSecretarySecurityUserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the match secretary security user identifier.
        /// </summary>
        /// <value>
        /// The match secretary security user identifier.
        /// </value>
        [JsonProperty]
        public Guid MatchSecretarySecurityUserId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="matchSecretarySecurityUserId">The match secretary security user identifier.</param>
        /// <returns></returns>
        public static MatchSecretarySecurityUserCreatedEvent Create(Guid aggregateId,
                                                                    Guid matchSecretarySecurityUserId)
        {
            return new MatchSecretarySecurityUserCreatedEvent(aggregateId, Guid.NewGuid(), matchSecretarySecurityUserId);
        }

        #endregion
    }
}