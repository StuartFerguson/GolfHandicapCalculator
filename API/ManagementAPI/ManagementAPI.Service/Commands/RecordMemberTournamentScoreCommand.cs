namespace ManagementAPI.Service.Commands
{
    using System;
    using DataTransferObjects;
    using Shared.CommandHandling;

    public class RecordMemberTournamentScoreCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordMemberTournamentScoreCommand" /> class.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordMemberTournamentScoreRequest">The record member tournament score request.</param>
        /// <param name="commandId">The command identifier.</param>
        private RecordMemberTournamentScoreCommand(Guid playerId,
                                                   Guid tournamentId,
                                                   RecordMemberTournamentScoreRequest recordMemberTournamentScoreRequest,
                                                   Guid commandId) : base(commandId)
        {
            this.RecordMemberTournamentScoreRequest = recordMemberTournamentScoreRequest;
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
        /// Gets or sets the record member tournament score request.
        /// </summary>
        /// <value>
        /// The record member tournament score request.
        /// </value>
        public RecordMemberTournamentScoreRequest RecordMemberTournamentScoreRequest { get; }

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
        /// <param name="recordMemberTournamentScoreRequest">The record member tournament score request.</param>
        /// <returns></returns>
        public static RecordMemberTournamentScoreCommand Create(Guid playerId,
                                                                Guid tournamentId,
                                                                RecordMemberTournamentScoreRequest recordMemberTournamentScoreRequest)
        {
            return new RecordMemberTournamentScoreCommand(playerId, tournamentId, recordMemberTournamentScoreRequest, Guid.NewGuid());
        }

        #endregion
    }
}