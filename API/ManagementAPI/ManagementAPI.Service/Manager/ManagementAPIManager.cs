namespace ManagementAPI.Service.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using Database.Models;
    using DataTransferObjects;
    using GolfClub;
    using GolfClub.DomainEvents;
    using GolfClubMembership;
    using GolfClubMembership.DomainEvents;
    using Microsoft.EntityFrameworkCore;
    using Player;
    using Services;
    using Services.DataTransferObjects;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;

    public class ManagementAPIManager : IManagmentAPIManager
    {
        #region Fields

        /// <summary>
        /// The golf club membership repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubMembershipAggregate> GolfClubMembershipRepository;

        /// <summary>
        /// The club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The o auth2 security service
        /// </summary>
        private readonly IOAuth2SecurityService OAuth2SecurityService;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The read model resolver
        /// </summary>
        private readonly Func<ManagementAPIReadModel> ReadModelResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPIManager" /> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="readModelResolver">The read model resolver.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        /// <param name="golfClubMembershipRepository">The golf club membership repository.</param>
        public ManagementAPIManager(IAggregateRepository<GolfClubAggregate> golfClubRepository,
                                    Func<ManagementAPIReadModel> readModelResolver,
                                    IAggregateRepository<PlayerAggregate> playerRepository,
                                    IOAuth2SecurityService oAuth2SecurityService,
                                    IAggregateRepository<GolfClubMembershipAggregate> golfClubMembershipRepository)
        {
            this.GolfClubRepository = golfClubRepository;
            this.ReadModelResolver = readModelResolver;
            this.PlayerRepository = playerRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
            this.GolfClubMembershipRepository = golfClubMembershipRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club not found for Golf Club Id [{golfClubId}</exception>
        public async Task<GetGolfClubResponse> GetGolfClub(Guid golfClubId,
                                                           CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "A Golf Club Id must be provided to retrieve a golf club");

            GetGolfClubResponse result = null;

            // Find the club configuration by id
            GolfClubAggregate golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            // Check we have found the club configuration
            if (!golfClub.HasBeenCreated)
            {
                throw new NotFoundException($"Golf Club not found for Golf Club Id [{golfClubId}]");
            }

            // We have found the club configuration
            result = new GetGolfClubResponse
                     {
                         AddressLine1 = golfClub.AddressLine1,
                         EmailAddress = golfClub.EmailAddress,
                         PostalCode = golfClub.PostalCode,
                         Name = golfClub.Name,
                         Town = golfClub.Town,
                         Website = golfClub.Website,
                         Region = golfClub.Region,
                         TelephoneNumber = golfClub.TelephoneNumber,
                         AddressLine2 = golfClub.AddressLine2,
                         Id = golfClub.AggregateId
                     };

            return result;
        }

        /// <summary>
        /// Gets the club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubResponse>> GetGolfClubList(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> result = new List<GetGolfClubResponse>();

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<GolfClub> golfClubs = await context.GolfClub.ToListAsync(cancellationToken);

                foreach (GolfClub golfClub in golfClubs)
                {
                    result.Add(new GetGolfClubResponse
                               {
                                   AddressLine1 = golfClub.AddressLine1,
                                   EmailAddress = golfClub.EmailAddress,
                                   Name = golfClub.Name,
                                   Town = golfClub.Town,
                                   Website = golfClub.WebSite,
                                   Region = golfClub.Region,
                                   TelephoneNumber = golfClub.TelephoneNumber,
                                   PostalCode = golfClub.PostalCode,
                                   AddressLine2 = golfClub.AddressLine2,
                                   Id = golfClub.GolfClubId
                               });
                }

                // Order the result by name
                result = result.OrderBy(g => g.Name).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets the golf club members list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubMembershipDetailsResponse>> GetGolfClubMembersList(Guid golfClubId,
                                                                                             CancellationToken cancellationToken)
        {
            List<GetGolfClubMembershipDetailsResponse> result = new List<GetGolfClubMembershipDetailsResponse>();

            // Rehydrate the Golf Club
            GolfClubAggregate golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            if (!golfClub.HasBeenCreated)
            {
                throw new NotFoundException($"Golf Club not found with Id {golfClubId}");
            }

            // Rehydrate the Golf Club Membership
            GolfClubMembershipAggregate golfClubMembership = await this.GolfClubMembershipRepository.GetLatestVersion(golfClubId, cancellationToken);

            // Translate
            List<MembershipDataTransferObject> membershipList = golfClubMembership.GetMemberships();

            foreach (MembershipDataTransferObject membership in membershipList)
            {
                result.Add(new GetGolfClubMembershipDetailsResponse
                           {
                               GolfClubId = golfClub.AggregateId,
                               PlayerId = membership.PlayerId,
                               PlayerGender = membership.PlayerGender,
                               PlayerDateOfBirth = membership.PlayerDateOfBirth.ToString("dd/MM/yyyy"),
                               PlayerFullName = membership.PlayerFullName,
                               MembershipNumber = membership.MembershipNumber,
                               MembershipStatus = (MembershipStatus)membership.Status,
                               Name = golfClub.Name
                           });
            }

            // Return
            return result;
        }

        /// <summary>
        /// Gets the player details.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Player not found with Id {playerId}</exception>
        public async Task<GetPlayerDetailsResponse> GetPlayerDetails(Guid playerId,
                                                                     CancellationToken cancellationToken)
        {
            GetPlayerDetailsResponse result = null;

            // Rehydrate the player
            PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(playerId, cancellationToken);

            if (!player.HasBeenRegistered)
            {
                throw new NotFoundException($"Player not found with Id {playerId}");
            }

            // Translate 
            result = new GetPlayerDetailsResponse
                     {
                         DateOfBirth = player.DateOfBirth,
                         FullName = player.FullName,
                         EmailAddress = player.EmailAddress,
                         Gender = player.Gender,
                         HasBeenRegistered = player.HasBeenRegistered,
                         FirstName = player.FirstName,
                         LastName = player.LastName,
                         MiddleName = player.MiddleName,
                         ExactHandicap = player.ExactHandicap,
                         HandicapCategory = player.HandicapCategory,
                         PlayingHandicap = player.PlayingHandicap
                     };

            return result;
        }

        /// <summary>
        /// Gets the players club memberships.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<ClubMembershipResponse>> GetPlayersClubMemberships(Guid playerId,
                                                                                  CancellationToken cancellationToken)
        {
            List<ClubMembershipResponse> result = new List<ClubMembershipResponse>();

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<PlayerClubMembership> debugList = await context.PlayerClubMembership.ToListAsync(cancellationToken);
                foreach (PlayerClubMembership playerClubMembership in debugList)
                {
                    Logger.LogDebug($"Player Id {playerClubMembership.PlayerId} Golf Club Id {playerClubMembership.GolfClubId}");
                }
                
                List<PlayerClubMembership> membershipList = await context.PlayerClubMembership.Where(p => p.PlayerId == playerId).ToListAsync(cancellationToken);

                if (membershipList.Count == 0)
                {
                    throw new NotFoundException($"No club memberships found for Player Id {playerId}");
                }

                foreach (PlayerClubMembership playerClubMembership in membershipList)
                {
                    result.Add(new ClubMembershipResponse
                               {
                                   MembershipNumber = playerClubMembership.MembershipNumber,
                                   MembershipId = playerClubMembership.MembershipId,
                                   GolfClubId = playerClubMembership.GolfClubId,
                                   RejectionReason = playerClubMembership.RejectionReason,
                                   Status = (MembershipStatus)playerClubMembership.Status,
                                   RejectedDateTime = playerClubMembership.RejectedDateTime,
                                   AcceptedDateTime = playerClubMembership.AcceptedDateTime,
                                   GolfClubName = playerClubMembership.GolfClubName
                               });
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts the club information to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertGolfClubToReadModel(GolfClubCreatedEvent domainEvent,
                                                    CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.GolfClub.Where(c => c.GolfClubId == domainEvent.AggregateId).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    GolfClub golfClub = new GolfClub
                                        {
                                            AddressLine1 = domainEvent.AddressLine1,
                                            EmailAddress = domainEvent.EmailAddress,
                                            Name = domainEvent.Name,
                                            Town = domainEvent.Town,
                                            Region = domainEvent.Region,
                                            TelephoneNumber = domainEvent.TelephoneNumber,
                                            AddressLine2 = domainEvent.AddressLine2,
                                            GolfClubId = domainEvent.AggregateId,
                                            PostalCode = domainEvent.PostalCode,
                                            WebSite = domainEvent.Website
                                        };

                    context.GolfClub.Add(golfClub);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player membership to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club with Id {domainEvent.AggregateId}</exception>
        public async Task InsertPlayerMembershipToReadModel(ClubMembershipRequestAcceptedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.PlayerClubMembership.Where(p => p.PlayerId == domainEvent.PlayerId &&
                                                                                    p.GolfClubId == domainEvent.AggregateId &&
                                                                                    p.Status == (Int32)MembershipStatus.Accepted).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                    if (golfClub == null)
                    {
                        throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                    }

                    PlayerClubMembership playerClubMembership = new PlayerClubMembership
                                                                {
                                                                    PlayerId = domainEvent.PlayerId,
                                                                    MembershipNumber = domainEvent.MembershipNumber,
                                                                    GolfClubId = domainEvent.AggregateId,
                                                                    MembershipId = domainEvent.MembershipId,
                                                                    RejectionReason = null,
                                                                    Status = (Int32)MembershipStatus.Accepted,
                                                                    AcceptedDateTime = domainEvent.AcceptedDateAndTime,
                                                                    RejectedDateTime = null,
                                                                    GolfClubName = golfClub.Name
                                                                };

                    context.PlayerClubMembership.Add(playerClubMembership);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player membership to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club with Id {domainEvent.AggregateId}</exception>
        public async Task InsertPlayerMembershipToReadModel(ClubMembershipRequestRejectedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.PlayerClubMembership.Where(p => p.PlayerId == domainEvent.PlayerId &&
                                                                                    p.GolfClubId == domainEvent.AggregateId &&
                                                                                    p.Status == (Int32)MembershipStatus.Rejected).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                    if (golfClub == null)
                    {
                        throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                    }

                    PlayerClubMembership playerClubMembership = new PlayerClubMembership
                                                                {
                                                                    PlayerId = domainEvent.PlayerId,
                                                                    MembershipNumber = null,
                                                                    GolfClubId = domainEvent.AggregateId,
                                                                    MembershipId = domainEvent.MembershipId,
                                                                    RejectionReason = domainEvent.RejectionReason,
                                                                    Status = (Int32)MembershipStatus.Rejected,
                                                                    AcceptedDateTime = null,
                                                                    RejectedDateTime = domainEvent.RejectionDateAndTime,
                                                                    GolfClubName = golfClub.Name
                                                                };

                    context.PlayerClubMembership.Add(playerClubMembership);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Registers the club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RegisterClubAdministrator(RegisterClubAdministratorRequest request,
                                                    CancellationToken cancellationToken)
        {
            // Allocate a new club Id 
            Guid golfClubAggregateId = Guid.NewGuid();

            // Now create a club admin security user
            RegisterUserRequest registerUserRequest = new RegisterUserRequest
                                                      {
                                                          EmailAddress = request.EmailAddress,
                                                          Claims = new Dictionary<String, String>
                                                                   {
                                                                       {"GolfClubId", golfClubAggregateId.ToString()}
                                                                   },
                                                          Password = "123456",
                                                          PhoneNumber = request.TelephoneNumber,
                                                          Roles = new List<String>
                                                                  {
                                                                      "Club Administrator"
                                                                  }
                                                      };

            // Create the user
            await this.OAuth2SecurityService.RegisterUser(registerUserRequest, cancellationToken);
        }

        #endregion
    }
}