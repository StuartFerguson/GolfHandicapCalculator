using System;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    using DataTransferObjects.Requests;

    public class CancelTournamentCommand : Command<String>
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
        /// Gets the cancel tournament request.
        /// </summary>
        /// <value>
        /// The cancel tournament request.
        /// </value>
        public CancelTournamentRequest CancelTournamentRequest { get; private set; }

        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelTournamentCommand" /> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancelTournamentRequest">The cancel tournament request.</param>
        /// <param name="commandId">The command identifier.</param>
        private CancelTournamentCommand(Guid tournamentId, CancelTournamentRequest cancelTournamentRequest, Guid commandId) : base(commandId)
        {
            this.CancelTournamentRequest = cancelTournamentRequest;
            this.TournamentId = tournamentId;
        }
        #endregion

        #region public static CancelTournamentCommand Create()                        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancelTournamentRequest">The cancel tournament request.</param>
        /// <returns></returns>
        public static CancelTournamentCommand Create(Guid tournamentId, CancelTournamentRequest cancelTournamentRequest)
        {
            return new CancelTournamentCommand(tournamentId, cancelTournamentRequest, Guid.NewGuid());
        }
        #endregion
    }
}