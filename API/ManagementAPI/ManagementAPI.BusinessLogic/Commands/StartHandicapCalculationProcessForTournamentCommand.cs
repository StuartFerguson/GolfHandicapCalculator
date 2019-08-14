namespace ManagementAPI.BusinessLogic.Commands
{
    using System;
    using Shared.CommandHandling;

    public class StartHandicapCalculationProcessForTournamentCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartHandicapCalculationProcessForTournamentCommand"/> class.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="commandId">The command identifier.</param>
        public StartHandicapCalculationProcessForTournamentCommand(Guid tournamentId,
                                                                   Guid commandId) : base(commandId)
        {
            this.TournamentId = tournamentId;
        }

        #endregion

        #region Properties

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
        /// Creates the specified tournament identifier.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <returns></returns>
        public static StartHandicapCalculationProcessForTournamentCommand Create(Guid tournamentId)
        {
            return new StartHandicapCalculationProcessForTournamentCommand(tournamentId, Guid.NewGuid());
        }

        #endregion
    }
}