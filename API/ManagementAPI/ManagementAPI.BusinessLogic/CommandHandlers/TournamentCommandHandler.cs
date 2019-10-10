namespace ManagementAPI.BusinessLogic.CommandHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using GolfClub;
    using Service.DataTransferObjects.Responses;
    using Services.ApplicationServices;
    using Shared.CommandHandling;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Tournament;
    using PlayerCategory = Tournament.PlayerCategory;
    using TournamentFormat = Tournament.TournamentFormat;

    public class TournamentCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The club configuration repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;
        
        /// <summary>
        /// The tournament application service
        /// </summary>
        private readonly ITournamentApplicationService TournamentApplicationService;

        /// <summary>
        /// The tournament repository
        /// </summary>
        private readonly IAggregateRepository<TournamentAggregate> TournamentRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCommandHandler" /> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        /// <param name="tournamentApplicationService">The tournament application service.</param>
        public TournamentCommandHandler(IAggregateRepository<GolfClubAggregate> golfClubRepository,
                                        IAggregateRepository<TournamentAggregate> tournamentRepository,
                                        ITournamentApplicationService tournamentApplicationService)
        {
            this.GolfClubRepository = golfClubRepository;
            this.TournamentRepository = tournamentRepository;
            this.TournamentApplicationService = tournamentApplicationService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
        private async Task HandleCommand(CreateTournamentCommand command,
                                         CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            // Get the club to validate the input
            GolfClubAggregate club = await this.GolfClubRepository.GetLatestVersion(command.GolfClubId, cancellationToken);

            // bug #29 fixes (throw exception if club not created)
            if (!club.HasBeenCreated)
            {
                throw new NotFoundException($"No created golf club found with Id {command.GolfClubId}");
            }

            // Club is valid, now check the measured course, this will throw exception if not found
            MeasuredCourseDataTransferObject measuredCourse = club.GetMeasuredCourse(command.CreateTournamentRequest.MeasuredCourseId);

            tournament.CreateTournament(command.CreateTournamentRequest.TournamentDate,
                                        command.GolfClubId,
                                        command.CreateTournamentRequest.MeasuredCourseId,
                                        measuredCourse.StandardScratchScore,
                                        command.CreateTournamentRequest.Name,
                                        (PlayerCategory)command.CreateTournamentRequest.MemberCategory,
                                        (TournamentFormat)command.CreateTournamentRequest.Format);

            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);

            // Setup the response
            command.Response = new CreateTournamentResponse
                               {
                                   TournamentId = command.TournamentId
                               };
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(RecordPlayerTournamentScoreCommand command,
                                         CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            tournament.RecordPlayerScore(command.PlayerId,
                                         command.RecordPlayerTournamentScoreRequest.PlayingHandicap,
                                         command.RecordPlayerTournamentScoreRequest.HoleScores);

            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(SignUpForTournamentCommand command,
                                         CancellationToken cancellationToken)
        {
            await this.TournamentApplicationService.SignUpPlayerForTournament(command.TournamentId, command.PlayerId, cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(CompleteTournamentCommand command,
                                         CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            DateTime completedDateTime = DateTime.Now;

            tournament.CompleteTournament(completedDateTime);

            tournament.CalculateCSS();

            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(CancelTournamentCommand command,
                                         CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            DateTime cancelledDateTime = DateTime.Now;

            tournament.CancelTournament(cancelledDateTime, command.CancelTournamentRequest.CancellationReason);

            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(ProduceTournamentResultCommand command,
                                         CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            // Produce the result
            tournament.ProduceResult();

            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }

        #endregion
    }
}