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
        /// Gets the club identifier.
        /// </summary>
        /// <value>
        /// The club identifier.
        /// </value>
        public Guid ClubId { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovePlayerMembershipRequestCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club configuration identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private ApprovePlayerMembershipRequestCommand(Guid playerId, Guid clubId, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.ClubId = clubId;
        }
        #endregion

        #region public static ApprovePlayerMembershipRequestCommand Create(Guid playerId, Guid clubConfigurationId)
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club configuration identifier.</param>
        /// <returns></returns>
        public static ApprovePlayerMembershipRequestCommand Create(Guid playerId, Guid clubId)
        {
            return new ApprovePlayerMembershipRequestCommand(playerId, clubId, Guid.NewGuid());
        }
        #endregion
    }
}
