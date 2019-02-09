using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.GolfClub;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Localization;
using Shared.CommandHandling;
using Shared.EventStore;
using HoleDataTransferObject = ManagementAPI.GolfClub.HoleDataTransferObject;

namespace ManagementAPI.Service.CommandHandlers
{
    public class GolfClubCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The golf club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The o auth2 security service
        /// </summary>
        private readonly IOAuth2SecurityService OAuth2SecurityService;

        /// <summary>
        /// The golf club membership application service
        /// </summary>
        private readonly IGolfClubMembershipApplicationService GolfClubMembershipApplicationService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubCommandHandler" /> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        /// <param name="golfClubMembershipApplicationService">The golf club membership application service.</param>
        public GolfClubCommandHandler(IAggregateRepository<GolfClubAggregate> golfClubRepository, IOAuth2SecurityService oAuth2SecurityService,
            IGolfClubMembershipApplicationService golfClubMembershipApplicationService)
        {
            this.GolfClubRepository = golfClubRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
            this.GolfClubMembershipApplicationService = golfClubMembershipApplicationService;
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

        #region private async Task HandleCommand(CreateGolfClubCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(CreateGolfClubCommand command, CancellationToken cancellationToken)
        {
            Guid golfClubAggregateId = command.GolfClubId;

            // Rehydrate the aggregate
            GolfClubAggregate club = await this.GolfClubRepository.GetLatestVersion(golfClubAggregateId, cancellationToken);

            // Call the aggregate method
            club.CreateGolfClub(command.CreateGolfClubRequest.Name,
                command.CreateGolfClubRequest.AddressLine1,
                command.CreateGolfClubRequest.AddressLine2,
                command.CreateGolfClubRequest.Town,
                command.CreateGolfClubRequest.Region,
                command.CreateGolfClubRequest.PostalCode,
                command.CreateGolfClubRequest.TelephoneNumber,
                command.CreateGolfClubRequest.Website,
                command.CreateGolfClubRequest.EmailAddress);
            
            // TODO: Record club admin user against aggregate

            // Save the changes
            await this.GolfClubRepository.SaveChanges(club, cancellationToken);

            // Setup the response
            command.Response = new CreateGolfClubResponse {GolfClubId = golfClubAggregateId };
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
            GolfClubAggregate club = await this.GolfClubRepository.GetLatestVersion(command.GolfClubId, cancellationToken);

            // Translate the request to the input for AddMeasuredCourse
            MeasuredCourseDataTransferObject measuredCourse = new MeasuredCourseDataTransferObject
            {
                MeasuredCourseId = command.AddMeasuredCourseToClubRequest.MeasuredCourseId,
                Name = command.AddMeasuredCourseToClubRequest.Name,
                StandardScratchScore = command.AddMeasuredCourseToClubRequest.StandardScratchScore,
                TeeColour = command.AddMeasuredCourseToClubRequest.TeeColour,
                Holes = new List<HoleDataTransferObject>()
            };

            foreach (DataTransferObjects.HoleDataTransferObject holeDataTransferObject in command.AddMeasuredCourseToClubRequest.Holes)
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
            await this.GolfClubRepository.SaveChanges(club, cancellationToken);

            // No Response to set
        }
        #endregion

        #region private async Task HandleCommand(RequestClubMembershipCommand command, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(RequestClubMembershipCommand command, CancellationToken cancellationToken)
        {
            // Call the applciation service method
            await this.GolfClubMembershipApplicationService.RequestClubMembership(command.PlayerId, command.GolfClubId,
                cancellationToken);

            // No response to be set
        }
        #endregion

        #endregion
    }
}
