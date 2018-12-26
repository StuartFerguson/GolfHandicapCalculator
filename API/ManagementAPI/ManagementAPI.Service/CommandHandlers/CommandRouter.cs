﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.Player;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Services;
using ManagementAPI.Tournament;
using Shared.CommandHandling;
using Shared.EventStore;
namespace ManagementAPI.Service.CommandHandlers
{
    public class CommandRouter : ICommandRouter
    {
        #region Fields

        /// <summary>
        /// The club aggregate repository
        /// </summary>
        private readonly IAggregateRepository<ClubConfigurationAggregate> ClubAggregateRepository;

        /// <summary>
        /// The tournament repository
        /// </summary>
        private readonly IAggregateRepository<TournamentAggregate> TournamentRepository;

        /// <summary>
        /// The handicap adjustment calculator service
        /// </summary>
        private readonly IHandicapAdjustmentCalculatorService HandicapAdjustmentCalculatorService;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The o auth2 security service
        /// </summary>
        private readonly IOAuth2SecurityService OAuth2SecurityService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRouter" /> class.
        /// </summary>
        /// <param name="clubAggregateRepository">The club aggregate repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        /// <param name="handicapAdjustmentCalculatorService">The handicap adjustment calculator service.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        public CommandRouter(IAggregateRepository<ClubConfigurationAggregate> clubAggregateRepository,
            IAggregateRepository<TournamentAggregate> tournamentRepository,
            IHandicapAdjustmentCalculatorService handicapAdjustmentCalculatorService,
            IAggregateRepository<PlayerAggregate> playerRepository,
            IOAuth2SecurityService oAuth2SecurityService)
        {
            this.ClubAggregateRepository = clubAggregateRepository;
            this.TournamentRepository = tournamentRepository;
            this.HandicapAdjustmentCalculatorService = handicapAdjustmentCalculatorService;
            this.PlayerRepository = playerRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Routes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task Route(ICommand command, CancellationToken cancellationToken)
        {
            ICommandHandler commandHandler = CreateHandler((dynamic)command);

            await commandHandler.Handle(command, cancellationToken);
        }

        #endregion

        #region Private Methods

        #region private ICommandHandler CreateHandler(CreateClubConfigurationCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateClubConfigurationCommand command)
        {
            return new ClubConfigurationCommandHandler(this.ClubAggregateRepository, this.OAuth2SecurityService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(AddMeasuredCourseToClubCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(AddMeasuredCourseToClubCommand command)
        {
            return new ClubConfigurationCommandHandler(this.ClubAggregateRepository, this.OAuth2SecurityService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(CreateTournamentCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CreateTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubAggregateRepository, this.TournamentRepository,this.HandicapAdjustmentCalculatorService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(RecordMemberTournamentScoreCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(RecordMemberTournamentScoreCommand command)
        {
            return new TournamentCommandHandler(this.ClubAggregateRepository, this.TournamentRepository,this.HandicapAdjustmentCalculatorService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(CompleteTournamentCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CompleteTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubAggregateRepository, this.TournamentRepository,this.HandicapAdjustmentCalculatorService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(CancelTournamentCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(CancelTournamentCommand command)
        {
            return new TournamentCommandHandler(this.ClubAggregateRepository, this.TournamentRepository,this.HandicapAdjustmentCalculatorService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(ProduceTournamentResultCommand command)        
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(ProduceTournamentResultCommand command)
        {
            return new TournamentCommandHandler(this.ClubAggregateRepository, this.TournamentRepository,this.HandicapAdjustmentCalculatorService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(RegisterPlayerCommand command)
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(RegisterPlayerCommand command)
        {
            return new PlayerCommandHandler(this.PlayerRepository, this.OAuth2SecurityService);
        }
        #endregion

        #region private ICommandHandler CreateHandler(PlayerClubMembershipRequestCommand command)
        /// <summary>
        /// Creates the handler.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private ICommandHandler CreateHandler(PlayerClubMembershipRequestCommand command)
        {
            return new PlayerCommandHandler(this.PlayerRepository, this.OAuth2SecurityService);
        }
        #endregion

        #endregion
    }
}
