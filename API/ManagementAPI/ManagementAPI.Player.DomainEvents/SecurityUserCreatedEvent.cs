using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Player.DomainEvents
{
    [JsonObject]
    public class SecurityUserCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUserCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SecurityUserCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUserCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        private SecurityUserCreatedEvent(Guid aggregateId, Guid eventId, Guid securityUserId) : base(aggregateId, eventId)
        {
            this.SecurityUserId = securityUserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [JsonProperty]
        public Guid SecurityUserId { get; private set; }

        #endregion

        #region Public Methods

        #region public static SecurityUserCreatedEvent Create(Guid aggregateId, Guid securityUserId)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <returns></returns>
        public static SecurityUserCreatedEvent Create(Guid aggregateId, Guid securityUserId)
        {
            return new SecurityUserCreatedEvent(aggregateId, Guid.NewGuid(), securityUserId);
        }
        #endregion

        #endregion
    }
}