using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.TournamentAggregate;
using Shared.CommandHandling;
using Shared.EventStore;


namespace ManagementAPI.Service.CommandHandlers
{
    public class TournamentCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The club configuration repository
        /// </summary>
        private readonly IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate> ClubConfigurationRepository;

        /// <summary>
        /// The tournament repository
        /// </summary>
        private readonly IAggregateRepository<TournamentAggregate.TournamentAggregate> TournamentRepository;

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCommandHandler"/> class.
        /// </summary>
        /// <param name="clubConfigurationRepository">The club configuration repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        public TournamentCommandHandler(IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate> clubConfigurationRepository,
                                        IAggregateRepository<TournamentAggregate.TournamentAggregate> tournamentRepository)
        {
            this.ClubConfigurationRepository = clubConfigurationRepository;
            this.TournamentRepository = tournamentRepository;
        }

        #endregion

        #region Public Methods

        #region public Task Handle(ICommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(ICommand command, CancellationToken cancellationToken)
        {
            await this.HandleCommand((dynamic)command, cancellationToken);
        }
        #endregion

        #endregion

        #region Private Methods (Command Handling)

        #region private async Task HandleCommand(CreateClubConfigurationCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(CreateTournamentCommand command, CancellationToken cancellationToken)
        {
            Guid tournamentAggregateId = Guid.NewGuid();

            // Rehydrate the aggregate
            var tournament = await this.TournamentRepository.GetLatestVersion(tournamentAggregateId, cancellationToken);

            // Get the club to validate the input
            var club = await this.ClubConfigurationRepository.GetLatestVersion(
                command.CreateTournamentRequest.ClubConfigurationId, cancellationToken);

            if (club.HasBeenCreated)
            {
                // Club is valid, now check the measured course, this will throw exception if not found
                club.GetMeasuredCourse(command.CreateTournamentRequest.MeasuredCourseId);                
            }

            tournament.CreateTournament(command.CreateTournamentRequest.TournamentDate, command.CreateTournamentRequest.ClubConfigurationId,
                command.CreateTournamentRequest.MeasuredCourseId, command.CreateTournamentRequest.Name,
                (MemberCategory)command.CreateTournamentRequest.MemberCategory,
                (TournamentFormat)command.CreateTournamentRequest.Format);

            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);

            // Setup the response
            command.Response = new CreateTournamentResponse {TournamentId= tournamentAggregateId } ;
        }
        #endregion

        #region private async Task HandleCommand(RecordMemberTournamentScoreCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(RecordMemberTournamentScoreCommand command, CancellationToken cancellationToken)
        {            
            // Rehydrate the aggregate
            var tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            tournament.RecordMemberScore(command.RecordMemberTournamentScoreRequest.MemberId, command.RecordMemberTournamentScoreRequest.HoleScores);
            
            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }
        #endregion

        #region private async Task HandleCommand(CompleteTournamentCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(CompleteTournamentCommand command, CancellationToken cancellationToken)
        {            
            // Rehydrate the aggregate
            var tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            DateTime completedDateTime = DateTime.Now;

            tournament.CompleteTournament(completedDateTime);
            
            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }
        #endregion

        #region private async Task HandleCommand(CompleteTournamentCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(CancelTournamentCommand command, CancellationToken cancellationToken)
        {            
            // Rehydrate the aggregate
            var tournament = await this.TournamentRepository.GetLatestVersion(command.TournamentId, cancellationToken);

            DateTime cancelledDateTime = DateTime.Now;

            tournament.CancelTournament(cancelledDateTime, command.CancelTournamentRequest.CancellationReason);
            
            // Save the changes
            await this.TournamentRepository.SaveChanges(tournament, cancellationToken);
        }
        #endregion

        #endregion
    }
}
