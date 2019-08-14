namespace ManagementAPI.BusinessLogic.CommandHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using HandicapCalculationProcess;
    using Shared.CommandHandling;
    using Shared.EventStore;
    using Tournament;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.CommandHandling.ICommandHandler" />
    public class HandicapCalculationCommandHandler : ICommandHandler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationCommandHandler" /> class.
        /// </summary>
        /// <param name="handicapCalculationProcessRepository">The handicap calculation process repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        public HandicapCalculationCommandHandler(IAggregateRepository<HandicapCalculationProcessAggregate> handicapCalculationProcessRepository,
                                                 IAggregateRepository<TournamentAggregate> tournamentRepository)
        {
            this.HandicapCalculationProcessRepository = handicapCalculationProcessRepository;
            this.TournamentRepository = tournamentRepository;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the handicap calculation process repository.
        /// </summary>
        /// <value>
        /// The handicap calculation process repository.
        /// </value>
        public IAggregateRepository<HandicapCalculationProcessAggregate> HandicapCalculationProcessRepository { get; }

        /// <summary>
        /// Gets the tournament repository.
        /// </summary>
        /// <value>
        /// The tournament repository.
        /// </value>
        public IAggregateRepository<TournamentAggregate> TournamentRepository { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task Handle(ICommand command,
                                 CancellationToken cancellationToken)
        {
            await this.HandleCommand((dynamic)command, cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(StartHandicapCalculationProcessForTournamentCommand command,
                                         CancellationToken cancellationToken)
        {
            // Get the tournament for validation
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            // Rehydrate the aggregate
            HandicapCalculationProcessAggregate handicapCalculationProcessAggregate =
                await this.HandicapCalculationProcessRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            // Call the aggregate method
            handicapCalculationProcessAggregate.StartHandicapCalculationProcess(tournament, DateTime.Now);

            // Save the changes
            await this.HandicapCalculationProcessRepository.SaveChanges(handicapCalculationProcessAggregate, cancellationToken);
        }

        #endregion
    }
}