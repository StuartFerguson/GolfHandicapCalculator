using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Player.DomainEvents
{
    [JsonObject]
    public class ClubMembershipApprovedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipApprovedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ClubMembershipApprovedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipApprovedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipApprovedDateAndTime">The membership approved date and time.</param>
        private ClubMembershipApprovedEvent(Guid aggregateId, Guid eventId, Guid clubId, DateTime membershipApprovedDateAndTime) : base(aggregateId, eventId)
        {
            this.ClubId = clubId;
            this.MembershipApprovedDateAndTime = membershipApprovedDateAndTime;
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
        /// Gets the membership approved date and time.
        /// </summary>
        /// <value>
        /// The membership approved date and time.
        /// </value>
        [JsonProperty]
        public DateTime MembershipApprovedDateAndTime { get; private set; }

        #endregion

        #region Public Methods

        #region public static ClubMembershipApprovedEvent Create()
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipApprovedDateAndTime">The membership approved date and time.</param>
        /// <returns></returns>
        public static ClubMembershipApprovedEvent Create(Guid aggregateId, Guid clubId, DateTime membershipApprovedDateAndTime)
        {
            return new ClubMembershipApprovedEvent(aggregateId, Guid.NewGuid(), clubId, membershipApprovedDateAndTime);
        }
        #endregion

        #endregion
    }
}
