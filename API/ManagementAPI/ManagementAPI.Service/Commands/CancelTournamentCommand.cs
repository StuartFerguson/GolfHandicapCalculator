namespace ManagementAPI.Service.Commands
{
    using System;
    using DataTransferObjects.Requests;
    using Shared.CommandHandling;

    public class CancelTournamentCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelTournamentCommand" /> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancelTournamentRequest">The cancel tournament request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CancelTournamentCommand(Guid golfClubId,
                                        Guid tournamentId,
                                        CancelTournamentRequest cancelTournamentRequest,
                                        Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.CancelTournamentRequest = cancelTournamentRequest;
            this.TournamentId = tournamentId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the cancel tournament request.
        /// </summary>
        /// <value>
        /// The cancel tournament request.
        /// </value>
        public CancelTournamentRequest CancelTournamentRequest { get; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
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
        /// <param name="cancelTournamentRequest">The cancel tournament request.</param>
        /// <returns></returns>
        public static CancelTournamentCommand Create(Guid golfClubId,
                                                     Guid tournamentId,
                                                     CancelTournamentRequest cancelTournamentRequest)
        {
            return new CancelTournamentCommand(golfClubId, tournamentId, cancelTournamentRequest, Guid.NewGuid());
        }

        #endregion
    }
}