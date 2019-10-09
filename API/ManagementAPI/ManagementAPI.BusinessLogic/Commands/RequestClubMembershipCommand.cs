namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Shared.CommandHandling;

    public class RequestClubMembershipCommand : Command<String>
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
        /// Initializes a new instance of the <see cref="RequestClubMembershipCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private RequestClubMembershipCommand(Guid playerId, Guid golfClubId, Guid commandId) : base(commandId)
        {
            this.PlayerId = playerId;
            this.GolfClubId = golfClubId;
        }
        #endregion

        #region public static RequestClubMembershipCommand Create()        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <returns></returns>
        public static RequestClubMembershipCommand Create(Guid playerId, Guid golfClubId)
        {
            return new RequestClubMembershipCommand(playerId, golfClubId, Guid.NewGuid());
        }
        #endregion
    }
}