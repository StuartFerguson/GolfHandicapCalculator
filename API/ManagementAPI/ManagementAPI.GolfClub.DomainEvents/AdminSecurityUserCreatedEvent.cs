using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.GolfClub.DomainEvents
{
    [JsonObject]
    public class AdminSecurityUserCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminSecurityUserCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public AdminSecurityUserCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminSecurityUserCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="adminSecurityUserId">The admin security user identifier.</param>
        private AdminSecurityUserCreatedEvent(Guid aggregateId, Guid eventId, Guid adminSecurityUserId) : base(aggregateId, eventId)
        {
            this.AdminSecurityUserId = adminSecurityUserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the admin security user identifier.
        /// </summary>
        /// <value>
        /// The admin security user identifier.
        /// </value>
        [JsonProperty]
        public Guid AdminSecurityUserId { get; private set; }

        #endregion

        #region Public Methods

        #region public static AdminSecurityUserCreatedEvent Create(Guid aggregateId, Guid adminSecurityUserId)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="adminSecurityUserId">The admin security user identifier.</param>
        /// <returns></returns>
        public static AdminSecurityUserCreatedEvent Create(Guid aggregateId, Guid adminSecurityUserId)
        {
            return new AdminSecurityUserCreatedEvent(aggregateId, Guid.NewGuid(), adminSecurityUserId);
        }
        #endregion

        #endregion
    }
}