using System;
using ManagementAPI.Service.DataTransferObjects;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class CompleteTournamentCommand : Command<String>
    {
        #region Properties

        /// <summary>
        /// Gets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; private set; }

        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="CompleteTournamentCommand" /> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private CompleteTournamentCommand(Guid tournamentId, Guid commandId) : base(commandId)
        {
            this.TournamentId = tournamentId;
        }
        #endregion

        #region public static CompleteTournamentCommand Create(CompleteTournamentRequest completeTournamentRequest)                        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <returns></returns>
        public static CompleteTournamentCommand Create(Guid tournamentId)
        {
            return new CompleteTournamentCommand(tournamentId, Guid.NewGuid());
        }
        #endregion
    }
}