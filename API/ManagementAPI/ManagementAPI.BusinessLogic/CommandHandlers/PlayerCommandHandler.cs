namespace ManagementAPI.BusinessLogic.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using GolfClub;
    using Player;
    using Service.DataTransferObjects.Responses;
    using Services.ExternalServices;
    using Services.ExternalServices.DataTransferObjects;
    using Shared.CommandHandling;
    using Shared.EventStore;

    public class PlayerCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The club configuration repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The o auth2 security service
        /// </summary>
        private readonly ISecurityService OAuth2SecurityService;

        /// <summary>
        /// The golf club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerCommandHandler" /> class.
        /// </summary>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        /// <param name="golfClubRepository">The golf club repository.</param>
        public PlayerCommandHandler(IAggregateRepository<PlayerAggregate> playerRepository, ISecurityService oAuth2SecurityService, IAggregateRepository<GolfClubAggregate> golfClubRepository)
        {
            this.PlayerRepository = playerRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
            this.GolfClubRepository = golfClubRepository;
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

        #region private async Task HandleCommand(RegisterPlayerCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(RegisterPlayerCommand command, CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(command.PlayerId, cancellationToken);

            // Call the aggregate method
            player.Register(command.RegisterPlayerRequest.GivenName, 
                command.RegisterPlayerRequest.MiddleName,
                command.RegisterPlayerRequest.FamilyName,
                command.RegisterPlayerRequest.Gender,
                command.RegisterPlayerRequest.DateOfBirth,
                command.RegisterPlayerRequest.ExactHandicap,
                command.RegisterPlayerRequest.EmailAddress);

            // Now create a security user
            RegisterUserRequest request = new RegisterUserRequest
            {
                EmailAddress = command.RegisterPlayerRequest.EmailAddress,
                Claims = new Dictionary<String, String>
                {
                    {"PlayerId", command.PlayerId.ToString()}
                },
                Password = "123456",
                PhoneNumber = "123456789",
                MiddleName = command.RegisterPlayerRequest.MiddleName,
                FamilyName = command.RegisterPlayerRequest.FamilyName,
                GivenName = command.RegisterPlayerRequest.GivenName,
                Roles = new List<String>
                {
                    "Player"
                }
            };
            RegisterUserResponse createSecurityUserResponse = await this.OAuth2SecurityService.RegisterUser(request, cancellationToken);

            // Record this in the aggregate
            player.CreateSecurityUser(createSecurityUserResponse.UserId);

            // Save the changes
            await this.PlayerRepository.SaveChanges(player, cancellationToken);

            // Setup the response
            command.Response = new RegisterPlayerResponse {PlayerId = command.PlayerId };
        }
        #endregion
        
        #endregion
    }
}
