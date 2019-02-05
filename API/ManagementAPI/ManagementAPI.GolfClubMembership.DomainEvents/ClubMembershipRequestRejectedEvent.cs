using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.GolfClubMembership.DomainEvents
{
    [JsonObject]
    public class ClubMembershipRequestRejectedEvent : DomainEvent
    {
        #region Constructors        

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipRequestRejectedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ClubMembershipRequestRejectedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipRequestRejectedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectionDateAndTime">The rejection date and time.</param>
        private ClubMembershipRequestRejectedEvent(Guid aggregateId,Guid eventId, Guid membershipId, Guid playerId, String playerFullName, 
            DateTime playerDateOfBirth, String playerGender, String rejectionReason, DateTime rejectionDateAndTime) : base(aggregateId, eventId)
        {
            this.MembershipId = membershipId;
            this.PlayerId = playerId;
            this.PlayerFullName = playerFullName;
            this.PlayerDateOfBirth = playerDateOfBirth;
            this.PlayerGender = playerGender;
            this.RejectionReason = rejectionReason;
            this.RejectionDateAndTime = rejectionDateAndTime;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        [JsonProperty]
        public Guid MembershipId { get; private set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        [JsonProperty]
        public Guid PlayerId { get; private set; }

        /// <summary>
        /// Gets the full name of the player.
        /// </summary>
        /// <value>
        /// The full name of the player.
        /// </value>
        public String PlayerFullName { get; private set; }

        /// <summary>
        /// Gets the player date of birth.
        /// </summary>
        /// <value>
        /// The player date of birth.
        /// </value>
        public DateTime PlayerDateOfBirth { get; private set; }

        /// <summary>
        /// Gets the player gender.
        /// </summary>
        /// <value>
        /// The player gender.
        /// </value>
        public String PlayerGender { get; private set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        [JsonProperty]
        public String RejectionReason { get; private set; }

        /// <summary>
        /// Gets the rejection date and time.
        /// </summary>
        /// <value>
        /// The rejection date and time.
        /// </value>
        [JsonProperty]
        public DateTime RejectionDateAndTime { get; private set; }

        #endregion

        #region Public Methods

        #region public static ClubMembershipRequestRejectedEvent Create(Guid aggregateId, Guid membershipId, Guid playerId, String playerFullName, DateTime playerDateOfBirth, String playerGender, String rejectionReason, DateTime rejectionDateAndTime)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectionDateAndTime">The rejection date and time.</param>
        /// <returns></returns>
        public static ClubMembershipRequestRejectedEvent Create(Guid aggregateId, Guid membershipId, Guid playerId, String playerFullName, DateTime playerDateOfBirth, String playerGender, String rejectionReason, DateTime rejectionDateAndTime)
        {
            return new ClubMembershipRequestRejectedEvent(aggregateId, Guid.NewGuid(), membershipId, playerId, playerFullName,playerDateOfBirth, playerGender, rejectionReason, rejectionDateAndTime);
        }
        #endregion

        #endregion
    }
}