using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class AddAcceptedMembershipToPlayerCommand : Command<String>
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
        /// Gets or sets the accepted date time.
        /// </summary>
        /// <value>
        /// The accepted date time.
        /// </value>
        public DateTime AcceptedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        public String MembershipNumber { get; set; }

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
        /// Initializes a new instance of the <see cref="CreateGolfClubCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        /// <param name="commandId">The command identifier.</param>
        private AddAcceptedMembershipToPlayerCommand(Guid playerId, Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.GolfClubId = golfClubId;
            this.MembershipId = membershipId;
            this.MembershipNumber = membershipNumber;
            this.AcceptedDateTime = acceptedDateTime;
        }
        #endregion

        #region public static AddAcceptedMembershipToPlayerCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        /// <returns></returns>
        public static AddAcceptedMembershipToPlayerCommand Create(Guid playerId, Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        {
            return new AddAcceptedMembershipToPlayerCommand(playerId,golfClubId, membershipId, membershipNumber, acceptedDateTime, Guid.NewGuid());
        }
        #endregion
    }
}
