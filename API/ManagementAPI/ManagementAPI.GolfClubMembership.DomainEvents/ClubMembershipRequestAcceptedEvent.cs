using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.GolfClubMembership.DomainEvents
{
    [JsonObject]
    public class ClubMembershipRequestAcceptedEvent : DomainEvent
    {
        #region Constructors        

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipRequestAcceptedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ClubMembershipRequestAcceptedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembershipRequestAcceptedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="acceptedDateAndTime">The approval date and time.</param>
        /// <param name="membershipNumber">The membership number.</param>
        private ClubMembershipRequestAcceptedEvent(Guid aggregateId,Guid eventId, Guid membershipId, Guid playerId, String playerFullName, 
            DateTime playerDateOfBirth, String playerGender, DateTime acceptedDateAndTime, String membershipNumber) : base(aggregateId, eventId)
        {
            this.MembershipId = membershipId;
            this.PlayerId = playerId;
            this.PlayerFullName = playerFullName;
            this.PlayerDateOfBirth = playerDateOfBirth;
            this.PlayerGender = playerGender;
            this.AcceptedDateAndTime = acceptedDateAndTime;
            this.MembershipNumber = membershipNumber;
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
        [JsonProperty]
        public String PlayerFullName { get; private set; }

        /// <summary>
        /// Gets the player date of birth.
        /// </summary>
        /// <value>
        /// The player date of birth.
        /// </value>
        [JsonProperty]
        public DateTime PlayerDateOfBirth { get; private set; }

        /// <summary>
        /// Gets the player gender.
        /// </summary>
        /// <value>
        /// The player gender.
        /// </value>
        [JsonProperty]
        public String PlayerGender { get; private set; }

        /// <summary>
        /// Gets the accepted date and time.
        /// </summary>
        /// <value>
        /// The accepted date and time.
        /// </value>
        [JsonProperty]
        public DateTime AcceptedDateAndTime { get; private set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        [JsonProperty]
        public String MembershipNumber { get; private set; }

        #endregion

        #region Public Methods

        #region public static ClubMembershipRequestAcceptedEvent Create(Guid aggregateId, Guid membershipId, Guid playerId, String playerFullName, DateTime playerDateOfBirth, String playerGender, DateTime acceptedDateAndTime, String membershipNumber)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="acceptedDateAndTime">The accepted date and time.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <returns></returns>
        public static ClubMembershipRequestAcceptedEvent Create(Guid aggregateId, Guid membershipId, Guid playerId, String playerFullName, DateTime playerDateOfBirth, String playerGender, DateTime acceptedDateAndTime, String membershipNumber)
        {
            return new ClubMembershipRequestAcceptedEvent(aggregateId, Guid.NewGuid(), membershipId, playerId, playerFullName,playerDateOfBirth, playerGender, acceptedDateAndTime, membershipNumber);
        }
        #endregion

        #endregion
    }
}