using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Player.DomainEvents
{
    [JsonObject]
    public class ClubMembershipRejectedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipRejectedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ClubMembershipRejectedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipRejectedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipRejectedDateAndTime">The membership rejected date and time.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        private ClubMembershipRejectedEvent(Guid aggregateId, Guid eventId, Guid clubId, DateTime membershipRejectedDateAndTime, String rejectionReason) : base(aggregateId, eventId)
        {
            this.ClubId = clubId;
            this.MembershipRejectedDateAndTime = membershipRejectedDateAndTime;
            this.RejectionReason = rejectionReason;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the club identifier.
        /// </summary>
        /// <value>
        /// The club identifier.
        /// </value>
        [JsonProperty]
        public Guid ClubId { get; private set; }
        
        /// <summary>
        /// Gets the membership rejected date and time.
        /// </summary>
        /// <value>
        /// The membership rejected date and time.
        /// </value>
        [JsonProperty]
        public DateTime MembershipRejectedDateAndTime { get; private set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        [JsonProperty]
        public String RejectionReason { get; private set; }

        #endregion

        #region Public Methods

        #region public static ClubMembershipApprovedEvent Create()
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipRejectedDateAndTime">The membership rejected date and time.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <returns></returns>
        public static ClubMembershipRejectedEvent Create(Guid aggregateId, Guid clubId, DateTime membershipRejectedDateAndTime, String rejectionReason)
        {
            return new ClubMembershipRejectedEvent(aggregateId, Guid.NewGuid(), clubId, membershipRejectedDateAndTime, rejectionReason);
        }
        #endregion

        #endregion
    }
}
