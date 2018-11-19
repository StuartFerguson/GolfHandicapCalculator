using System;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Commands
{
    public class ProduceTournamentResultCommand : Command<String>
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
        /// Initializes a new instance of the <see cref="ProduceTournamentResultCommand" /> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        private ProduceTournamentResultCommand(Guid tournamentId, Guid commandId) : base(commandId)
        {
            this.TournamentId = tournamentId;
        }
        #endregion

        #region public static ProduceTournamentResultCommand Create(Guid tournamentId)                        
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <returns></returns>
        public static ProduceTournamentResultCommand Create(Guid tournamentId)
        {
            return new ProduceTournamentResultCommand(tournamentId, Guid.NewGuid());
        }
        #endregion
    }
}