namespace ManagementAPI.Service.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using DataTransferObjects;
    using GolfClub;
    using Services;
    using Shared.CommandHandling;
    using Shared.EventStore;
    using HoleDataTransferObject = GolfClub.HoleDataTransferObject;

    public class GolfClubCommandHandler : ICommandHandler
    {
        #region Fields

        /// <summary>
        /// The golf club membership application service
        /// </summary>
        private readonly IGolfClubMembershipApplicationService GolfClubMembershipApplicationService;

        /// <summary>
        /// The golf club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The o auth2 security service
        /// </summary>
        private readonly IOAuth2SecurityService OAuth2SecurityService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubCommandHandler" /> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        /// <param name="golfClubMembershipApplicationService">The golf club membership application service.</param>
        public GolfClubCommandHandler(IAggregateRepository<GolfClubAggregate> golfClubRepository,
                                      IOAuth2SecurityService oAuth2SecurityService,
                                      IGolfClubMembershipApplicationService golfClubMembershipApplicationService)
        {
            this.GolfClubRepository = golfClubRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
            this.GolfClubMembershipApplicationService = golfClubMembershipApplicationService;
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
        private async Task HandleCommand(CreateGolfClubCommand command,
                                         CancellationToken cancellationToken)
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
            command.Response = new CreateGolfClubResponse
                               {
                                   GolfClubId = golfClubAggregateId
                               };
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(AddMeasuredCourseToClubCommand command,
                                         CancellationToken cancellationToken)
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

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(RequestClubMembershipCommand command,
                                         CancellationToken cancellationToken)
        {
            // Call the application service method
            await this.GolfClubMembershipApplicationService.RequestClubMembership(command.PlayerId, command.GolfClubId, cancellationToken);

            // No response to be set
        }

        /// <summary>
        /// Handles the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleCommand(AddTournamentDivisionToGolfClubCommand command,
                                         CancellationToken cancellationToken)
        {
            // Rehydrate the aggregate
            GolfClubAggregate golfClubAggregate = await this.GolfClubRepository.GetLatestVersion(command.GolfClubId, cancellationToken);

            // Create the dto from the request in the command
            TournamentDivisionDataTransferObject tournamentDivisionDataTransferObject = new TournamentDivisionDataTransferObject
                                                                                        {
                                                                                            Division = command.AddTournamentDivisionToGolfClubRequest.Division,
                                                                                            StartHandicap = command.AddTournamentDivisionToGolfClubRequest.StartHandicap,
                                                                                            EndHandicap = command.AddTournamentDivisionToGolfClubRequest.EndHandicap
                                                                                        };

            golfClubAggregate.AddTournamentDivision(tournamentDivisionDataTransferObject);

            await this.GolfClubRepository.SaveChanges(golfClubAggregate, cancellationToken);
        }

        #endregion
    }
}