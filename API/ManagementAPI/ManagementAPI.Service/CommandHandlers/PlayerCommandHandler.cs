using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Player;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using Shared.CommandHandling;
using Shared.EventStore;

namespace ManagementAPI.Service.CommandHandlers
{
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
        private readonly IOAuth2SecurityService OAuth2SecurityService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerCommandHandler" /> class.
        /// </summary>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        public PlayerCommandHandler(IAggregateRepository<PlayerAggregate> playerRepository, IOAuth2SecurityService oAuth2SecurityService)
        {
            this.PlayerRepository = playerRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
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
            Guid playerAggregateId = Guid.NewGuid();

            // Rehydrate the aggregate
            var player = await this.PlayerRepository.GetLatestVersion(playerAggregateId, cancellationToken);

            // Call the aggregate method
            player.Register(command.RegisterPlayerRequest.FirstName, 
                command.RegisterPlayerRequest.MiddleName,
                command.RegisterPlayerRequest.LastName,
                command.RegisterPlayerRequest.Gender,
                command.RegisterPlayerRequest.Age,
                command.RegisterPlayerRequest.ExactHandicap,
                command.RegisterPlayerRequest.EmailAddress);

            // Now create a security user
            RegisterUserRequest request = new RegisterUserRequest
            {
                EmailAddress = command.RegisterPlayerRequest.EmailAddress,
                Claims = new Dictionary<String, String>
                {
                    {"PlayerId", playerAggregateId.ToString()}
                },
                Password = "123456",
                PhoneNumber = "123456789",
                Roles = new List<String>()
            };
            var createSecurityUserResponse = await this.OAuth2SecurityService.RegisterUser(request, cancellationToken);

            // Record this in the aggregate
            player.CreateSecurityUser(createSecurityUserResponse.UserId);

            // Save the changes
            await this.PlayerRepository.SaveChanges(player, cancellationToken);

            // Setup the response
            command.Response = new RegisterPlayerResponse {PlayerId = playerAggregateId };
        }
        #endregion

        #endregion
    }
}
