using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.GolfClub.DomainEvents
{
    [JsonObject]
    public class GolfClubAdministratorSecurityUserCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAdministratorSecurityUserCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public GolfClubAdministratorSecurityUserCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAdministratorSecurityUserCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="golfClubAdministratorSecurityUserId">The golf club administrator security user identifier.</param>
        private GolfClubAdministratorSecurityUserCreatedEvent(Guid aggregateId, Guid eventId, Guid golfClubAdministratorSecurityUserId) : base(aggregateId, eventId)
        {
            this.GolfClubAdministratorSecurityUserId = golfClubAdministratorSecurityUserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the golf club administrator security user identifier.
        /// </summary>
        /// <value>
        /// The golf club administrator security user identifier.
        /// </value>
        [JsonProperty]
        public Guid GolfClubAdministratorSecurityUserId { get; private set; }

        #endregion

        #region Public Methods

        #region public static GolfClubAdministratorSecurityUserCreatedEvent Create(Guid aggregateId, Guid golfClubAdministratorSecurityUserId)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="golfClubAdministratorSecurityUserId">The golf club administrator security user identifier.</param>
        /// <returns></returns>
        public static GolfClubAdministratorSecurityUserCreatedEvent Create(Guid aggregateId, Guid golfClubAdministratorSecurityUserId)
        {
            return new GolfClubAdministratorSecurityUserCreatedEvent(aggregateId, Guid.NewGuid(), golfClubAdministratorSecurityUserId);
        }
        #endregion

        #endregion
    }
}