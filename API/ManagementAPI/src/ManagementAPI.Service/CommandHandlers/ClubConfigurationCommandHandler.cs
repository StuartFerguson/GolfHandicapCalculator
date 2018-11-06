using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfigurationAggregate;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Localization;
using Shared.CommandHandling;
using Shared.EventStore;
using HoleDataTransferObject = ManagementAPI.ClubConfigurationAggregate.HoleDataTransferObject;

namespace ManagementAPI.Service.CommandHandlers
{
    public class ClubConfigurationCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The club configuration repository
        /// </summary>
        private readonly IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate> ClubConfigurationRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationCommandHandler"/> class.
        /// </summary>
        /// <param name="clubConfigurationRepository">The club configuration repository.</param>
        public ClubConfigurationCommandHandler(IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate> clubConfigurationRepository)
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

        #region private async Task HandleCommand(AddMeasuredCourseToClubCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(AddMeasuredCourseToClubCommand command, CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            var club = await this.ClubConfigurationRepository.GetLatestVersion(command.AddMeasuredCourseToClubRequest.ClubAggregateId, cancellationToken);

            // Translate the request to the input for AddMeasuredCourse
            MeasuredCourseDataTransferObject measuredCourse = new MeasuredCourseDataTransferObject
            {
                MeasuredCourseId = command.AddMeasuredCourseToClubRequest.MeasuredCourseId,
                Name = command.AddMeasuredCourseToClubRequest.Name,
                StandardScratchScore = command.AddMeasuredCourseToClubRequest.StandardScratchScore,
                TeeColour = command.AddMeasuredCourseToClubRequest.TeeColour,
                Holes = new List<HoleDataTransferObject>()
            };

            foreach (var holeDataTransferObject in command.AddMeasuredCourseToClubRequest.Holes)
            {
                measuredCourse.Holes.Add(new HoleDataTransferObject
                {
                    HoleNumber = holeDataTransferObject.HoleNumber,
                    Par = holeDataTransferObject.Par,
                    StrokeIndex = holeDataTransferObject.StrokeIndex,
                    LengthInYards = holeDataTransferObject.LengthInYards,
                    LengthInMeters = holeDataTransferObject.LengthInMeters
                });
            }

            // Add the measured course
            club.AddMeasuredCourse(measuredCourse);

            // Save the changes
            await this.ClubConfigurationRepository.SaveChanges(club, cancellationToken);

            // No Response to set
        }
        #endregion

        #endregion
    }
}
