using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class ApprovePlayerMembershipRequestCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; private set; }

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
        /// Initializes a new instance of the <see cref="ApprovePlayerMembershipRequestCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private ApprovePlayerMembershipRequestCommand(Guid playerId, Guid golfClubId, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.GolfClubId = golfClubId;
        }
        #endregion

        #region public static ApprovePlayerMembershipRequestCommand Create(Guid playerId, Guid golfClubId)
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <returns></returns>
        public static ApprovePlayerMembershipRequestCommand Create(Guid playerId, Guid golfClubId)
        {
            return new ApprovePlayerMembershipRequestCommand(playerId, golfClubId, Guid.NewGuid());
        }
        #endregion
    }
}
