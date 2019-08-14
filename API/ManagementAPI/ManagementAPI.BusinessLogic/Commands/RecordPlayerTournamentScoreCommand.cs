namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Service.DataTransferObjects.Requests;
    using Shared.CommandHandling;

    public class RecordPlayerTournamentScoreCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordPlayerTournamentScoreCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordPlayerTournamentScoreRequest">The record player tournament score request.</param>
        /// <param name="commandId">The command identifier.</param>
        private RecordPlayerTournamentScoreCommand(Guid playerId,
                                                   Guid tournamentId,
                                                   RecordPlayerTournamentScoreRequest recordPlayerTournamentScoreRequest,
                                                   Guid commandId) : base(commandId)
        {
            this.RecordPlayerTournamentScoreRequest = recordPlayerTournamentScoreRequest;
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
        public Guid PlayerId { get; }

        /// <summary>
        /// Gets the record player tournament score request.
        /// </summary>
        /// <value>
        /// The record member tournament score request.
        /// </value>
        public RecordPlayerTournamentScoreRequest RecordPlayerTournamentScoreRequest { get; }

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
        /// Creates the specified record member tournament score request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordPlayerTournamentScoreRequest">The record player tournament score request.</param>
        /// <returns></returns>
        public static RecordPlayerTournamentScoreCommand Create(Guid playerId,
                                                                Guid tournamentId,
                                                                RecordPlayerTournamentScoreRequest recordPlayerTournamentScoreRequest)
        {
            return new RecordPlayerTournamentScoreCommand(playerId, tournamentId, recordPlayerTournamentScoreRequest, Guid.NewGuid());
        }

        #endregion
    }
}