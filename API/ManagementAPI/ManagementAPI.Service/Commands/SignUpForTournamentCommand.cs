namespace ManagementAPI.Service.Commands
{
    using System;
    using Shared.CommandHandling;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.CommandHandling.Command{System.String}" />
    public class SignUpForTournamentCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpForTournamentCommand"/> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private SignUpForTournamentCommand(Guid tournamentId,
                                           Guid playerId,
                                           Guid commandId) : base(commandId)
        {
            this.TournamentId = tournamentId;
            this.PlayerId = playerId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; private set; }

        /// <summary>
        /// Gets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified tournament identifier.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <returns></returns>
        public static SignUpForTournamentCommand Create(Guid tournamentId,
                                                        Guid playerId)
        {
            return new SignUpForTournamentCommand(tournamentId, playerId, Guid.NewGuid());
        }

        #endregion
    }
}