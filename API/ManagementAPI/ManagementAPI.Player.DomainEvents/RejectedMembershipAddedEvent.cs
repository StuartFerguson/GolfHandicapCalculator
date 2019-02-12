using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Player.DomainEvents
{
    [JsonObject]
    public class RejectedMembershipAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RejectedMembershipAddedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public RejectedMembershipAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RejectedMembershipAddedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectedDateTime">The rejected date time.</param>
        private RejectedMembershipAddedEvent(Guid aggregateId, Guid eventId, Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime) : base(aggregateId, eventId)
        {
            this.GolfClubId = golfClubId;
            this.MembershipId = membershipId;
            this.RejectionReason = rejectionReason;
            this.RejectedDateTime = rejectedDateTime;
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
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        [JsonProperty] 
        public String RejectionReason { get; private set; }

        /// <summary>
        /// Gets the rejected date time.
        /// </summary>
        /// <value>
        /// The rejected date time.
        /// </value>
        [JsonProperty]
        public DateTime RejectedDateTime { get; private set; }

        #endregion

        #region Public Methods

        #region public static RejectedMembershipAddedEvent Create(Guid aggregateId, Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectedDateTime">The rejected date time.</param>
        /// <returns></returns>
        public static RejectedMembershipAddedEvent Create(Guid aggregateId, Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime)
        {
            return new RejectedMembershipAddedEvent(aggregateId, Guid.NewGuid(), golfClubId, membershipId,rejectionReason,rejectedDateTime);
        }
        #endregion

        #endregion
    }
}