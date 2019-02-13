using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Player.DomainEvents
{
    [JsonObject]
    public class AcceptedMembershipAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptedMembershipAddedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public AcceptedMembershipAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptedMembershipAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        private AcceptedMembershipAddedEvent(Guid aggregateId, Guid eventId, Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime) : base(aggregateId, eventId)
        {
            this.GolfClubId = golfClubId;
            this.MembershipId = membershipId;
            this.MembershipNumber = membershipNumber;
            this.AcceptedDateTime = acceptedDateTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        [JsonProperty] 
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        [JsonProperty]
        public Guid MembershipId { get; private set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        [JsonProperty] 
        public String MembershipNumber { get; private set; }

        /// <summary>
        /// Gets the accepted date time.
        /// </summary>
        /// <value>
        /// The accepted date time.
        /// </value>
        [JsonProperty]
        public DateTime AcceptedDateTime { get; private set; }

        #endregion

        #region Public Methods

        #region public static AcceptedMembershipAddedEvent Create(Guid aggregateId, Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        /// <returns></returns>
        public static AcceptedMembershipAddedEvent Create(Guid aggregateId, Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        {
            return new AcceptedMembershipAddedEvent(aggregateId, Guid.NewGuid(), golfClubId, membershipId,membershipNumber,acceptedDateTime);
        }
        #endregion

        #endregion
    }
}