namespace ManagementAPI.BusinessLogic.Services.ApplicationServices
{
    using ManagementAPI.GolfClub;
    using ManagementAPI.GolfClubMembership;
    using ManagementAPI.Player;
    using Shared.EventStore;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class GolfClubMembershipApplicationService : IGolfClubMembershipApplicationService
    {
        #region Fields

        /// <summary>
        /// The golf club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The golf club membership repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubMembershipAggregate> GolfClubMembershipRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubMembershipApplicationService"/> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="golfClubMembershipRepository">The golf club membership repository.</param>
        public GolfClubMembershipApplicationService(IAggregateRepository<GolfClubAggregate> golfClubRepository, IAggregateRepository<PlayerAggregate> playerRepository,
            IAggregateRepository<GolfClubMembershipAggregate> golfClubMembershipRepository)
        {
            this.GolfClubRepository = golfClubRepository;
            this.PlayerRepository = playerRepository;
            this.GolfClubMembershipRepository = golfClubMembershipRepository;
        }

        #endregion

        #region Public Methods

        #region public Task RequestClubMembership(Guid playerId, Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="InvalidDataException">
        /// Unable to request club membership for a player that has not been registered
        /// or
        /// Unable to request club membership for a club that has not been created
        /// </exception>
        public async Task RequestClubMembership(Guid playerId, Guid golfClubId, CancellationToken cancellationToken)
        {
            // Rehydrate the golf club membership aggregate
            GolfClubMembershipAggregate golfClubMembershipAggregate =
                await this.GolfClubMembershipRepository.GetLatestVersion(golfClubId, cancellationToken);

            // Validate the player id firstly
            PlayerAggregate playerAggregate = await this.PlayerRepository.GetLatestVersion(playerId, cancellationToken);

            if (!playerAggregate.HasBeenRegistered)
            {
                throw new InvalidDataException("Unable to request club membership for a player that has not been registered");
            }

            // Now validate the club
            GolfClubAggregate golfClubAggregate =
                await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            if (!golfClubAggregate.HasBeenCreated)
            {
                throw new InvalidDataException("Unable to request club membership for a club that has not been created");
            }

            // Ok all the data has been validated, now run through the aggregate rules
            golfClubMembershipAggregate.RequestMembership(playerId, 
                playerAggregate.FullName, 
                playerAggregate.DateOfBirth,
                playerAggregate.Gender,
                DateTime.Now);

            // Save any pending changes
            await this.GolfClubMembershipRepository.SaveChanges(golfClubMembershipAggregate, cancellationToken);
        }
        #endregion

        #endregion
    }
}