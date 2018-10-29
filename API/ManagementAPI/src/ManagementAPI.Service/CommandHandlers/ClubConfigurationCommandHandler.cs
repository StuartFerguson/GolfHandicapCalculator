using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClubConfigAggregate = ManagementAPI.ClubConfigurationAggregate.ClubConfigurationAggregate;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Localization;
using Shared.CommandHandling;
using Shared.EventStore;

namespace ManagementAPI.Service.CommandHandlers
{
    public class ClubConfigurationCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The club configuration repository
        /// </summary>
        private readonly IAggregateRepository<ClubConfigAggregate> ClubConfigurationRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationCommandHandler"/> class.
        /// </summary>
        /// <param name="clubConfigurationRepository">The club configuration repository.</param>
        public ClubConfigurationCommandHandler(IAggregateRepository<ClubConfigAggregate> clubConfigurationRepository)
        {
            this.ClubConfigurationRepository = clubConfigurationRepository;
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
        private async Task HandleCommand(CreateClubConfigurationCommand command, CancellationToken cancellationToken)
        {
            Guid clubAggregateId = Guid.NewGuid();

            // Rehydrate the aggregate
            var club = await this.ClubConfigurationRepository.GetLatestVersion(clubAggregateId, cancellationToken);

            // Call the aggregate method
            club.CreateClubConfiguration(command.CreateClubConfigurationRequest.Name,
                command.CreateClubConfigurationRequest.AddressLine1,
                command.CreateClubConfigurationRequest.AddressLine2,
                command.CreateClubConfigurationRequest.Town,
                command.CreateClubConfigurationRequest.Region,
                command.CreateClubConfigurationRequest.PostalCode,
                command.CreateClubConfigurationRequest.TelephoneNumber,
                command.CreateClubConfigurationRequest.Website,
                command.CreateClubConfigurationRequest.EmailAddress);

            // Save the changes
            await this.ClubConfigurationRepository.SaveChanges(club, cancellationToken);

            // Setup the response
            command.Response = new CreateClubConfigurationResponse {ClubConfigurationId = clubAggregateId } ;
        }
        #endregion

        #endregion
    }
}
