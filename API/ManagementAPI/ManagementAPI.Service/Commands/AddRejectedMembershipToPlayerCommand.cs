using System;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class AddRejectedMembershipToPlayerCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        public Guid MembershipId { get; private set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; private set; }

        /// <summary>
        /// Gets or sets the rejected date time.
        /// </summary>
        /// <value>
        /// The rejected date time.
        /// </value>
        public DateTime RejectedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        public String RejectionReason { get; set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AddRejectedMembershipToPlayerCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectedDateTime">The rejected date time.</param>
        /// <param name="commandId">The command identifier.</param>
        private AddRejectedMembershipToPlayerCommand(Guid playerId, Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.GolfClubId = golfClubId;
            this.MembershipId = membershipId;
            this.RejectionReason = rejectionReason;
            this.RejectedDateTime = rejectedDateTime;
        }
        #endregion

        #region public static AddRejectedMembershipToPlayerCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectedDateTime">The rejected date time.</param>
        /// <returns></returns>
        public static AddRejectedMembershipToPlayerCommand Create(Guid playerId, Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime)
        {
            return new AddRejectedMembershipToPlayerCommand(playerId,golfClubId, membershipId, rejectionReason, rejectedDateTime, Guid.NewGuid());
        }
        #endregion
    }
}