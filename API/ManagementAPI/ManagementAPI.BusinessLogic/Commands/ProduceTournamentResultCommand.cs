namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Shared.CommandHandling;

    public class ProduceTournamentResultCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProduceTournamentResultCommand" /> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private ProduceTournamentResultCommand(Guid golfClubId,
                                               Guid tournamentId,
                                               Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.TournamentId = tournamentId;
        }

        #endregion

        #region Properties

        public Guid GolfClubId { get; }

        /// <summary>
        /// Gets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <returns></returns>
        public static ProduceTournamentResultCommand Create(Guid golfClubId,
                                                            Guid tournamentId)
        {
            return new ProduceTournamentResultCommand(golfClubId, tournamentId, Guid.NewGuid());
        }

        #endregion
    }
}