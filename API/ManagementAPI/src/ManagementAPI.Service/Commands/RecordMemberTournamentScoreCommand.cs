using System;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class RecordMemberTournamentScoreCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; private set; }

        /// <summary>
        /// Gets or sets the record member tournament score request.
        /// </summary>
        /// <value>
        /// The record member tournament score request.
        /// </value>
        public RecordMemberTournamentScoreRequest RecordMemberTournamentScoreRequest { private set; get; }

        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordMemberTournamentScoreCommand" /> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordMemberTournamentScoreRequest">The record member tournament score request.</param>
        /// <param name="commandId">The command identifier.</param>
        private RecordMemberTournamentScoreCommand(Guid tournamentId, RecordMemberTournamentScoreRequest recordMemberTournamentScoreRequest, Guid commandId) : base(commandId)
        {
            this.RecordMemberTournamentScoreRequest = recordMemberTournamentScoreRequest;
            this.TournamentId = tournamentId;
        }
        #endregion

        #region public static CreateTournamentCommand Create()                
        /// <summary>
        /// Creates the specified record member tournament score request.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordMemberTournamentScoreRequest">The record member tournament score request.</param>
        /// <returns></returns>
        public static RecordMemberTournamentScoreCommand Create(Guid tournamentId, RecordMemberTournamentScoreRequest recordMemberTournamentScoreRequest)
        {
            return new RecordMemberTournamentScoreCommand(tournamentId, recordMemberTournamentScoreRequest, Guid.NewGuid());
        }
        #endregion
    }
}