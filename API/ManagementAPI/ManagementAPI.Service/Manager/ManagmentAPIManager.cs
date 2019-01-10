using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Database;
using ManagementAPI.Database.Models;
using ManagementAPI.GolfClub;
using ManagementAPI.GolfClub.DomainEvents;
using ManagementAPI.Player;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Shared.EventStore;
using Shared.Exceptions;
using Shared.General;

namespace ManagementAPI.Service.Manager
{
    public class ManagmentAPIManager : IManagmentAPIManager
    {
        #region Fields
        
        /// <summary>
        /// The club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The read model resolver
        /// </summary>
        private readonly Func<ManagementAPIReadModel> ReadModelResolver;

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
        /// Initializes a new instance of the <see cref="ManagmentAPIManager" /> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="readModelResolver">The read model resolver.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="oAuth2SecurityService">The o auth2 security service.</param>
        public ManagmentAPIManager(IAggregateRepository<GolfClubAggregate> golfClubRepository, Func<ManagementAPIReadModel> readModelResolver,
            IAggregateRepository<PlayerAggregate> playerRepository, IOAuth2SecurityService oAuth2SecurityService)
        {
            this.GolfClubRepository = golfClubRepository;
            this.ReadModelResolver = readModelResolver;
            this.PlayerRepository = playerRepository;
            this.OAuth2SecurityService = oAuth2SecurityService;
        }

        #endregion

        #region Public Methods

        #region public async Task RegisterClubAdministrator(RegisterClubAdministratorRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Registers the club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RegisterClubAdministrator(RegisterClubAdministratorRequest request, CancellationToken cancellationToken)
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

        #region public async Task<GetGolfClubResponse> GetGolfClub(Guid golfClubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club not found for Golf Club Id [{golfClubId}</exception>
        public async Task<GetGolfClubResponse> GetGolfClub(Guid golfClubId, CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "A Golf Club Id must be provided to retrieve a golf club");

            GetGolfClubResponse result = null;

            // Find the club configuration by id
            var golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

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
        #endregion

        #region public async Task InsertClubInformationToReadModel(GolfClubCreatedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Inserts the club information to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertGolfClubToReadModel(GolfClubCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using (var context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                var isDuplicate = await context.GolfClub.Where(c => c.GolfClubId == domainEvent.AggregateId)
                    .AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    Database.Models.GolfClub golfClub = new Database.Models.GolfClub
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
        #endregion

        #region public async Task<List<GetGolfClubResponse>> GetGolfClubList(CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubResponse>> GetGolfClubList(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> result = new List<GetGolfClubResponse>();

            using (var context = ReadModelResolver())
            {
                var golfClubs = await context.GolfClub.ToListAsync(cancellationToken);

                foreach (var golfClub in golfClubs)
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
            }

            return result;
        }
        #endregion

        #region public async Task InsertClubMembershipRequestToReadModel(ClubMembershipRequestedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Inserts the club membership request to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertClubMembershipRequestToReadModel(ClubMembershipRequestedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            // Get the club
            var club = await this.GolfClubRepository.GetLatestVersion(domainEvent.ClubId, cancellationToken);

            if (!club.HasBeenCreated)
            {
                throw new InvalidOperationException($"Unable to find golf club with Id {domainEvent.ClubId}");
            }

            // Get the player
            var player = await this.PlayerRepository.GetLatestVersion(domainEvent.AggregateId, cancellationToken);

            if (!player.HasBeenRegistered)
            {
                throw new InvalidOperationException($"Unable to find player with Id {domainEvent.AggregateId}");
            }

            using (var context = this.ReadModelResolver())
            {
                ClubMembershipRequest clubMembershipRequest = new ClubMembershipRequest
                {
                    ClubId = domainEvent.ClubId,
                    Age = player.Age,
                    ExactHandicap = player.ExactHandicap,
                    FirstName = player.FirstName,
                    Gender = player.Gender,
                    HandicapCategory = player.HandicapCategory,
                    LastName = player.LastName,
                    MembershipRequestId = domainEvent.EventId,
                    MembershipRequestedDateAndTime = domainEvent.MembershipRequestedDateAndTime,
                    MiddleName = player.MiddleName,
                    PlayerId = domainEvent.AggregateId,
                    PlayingHandicap = player.PlayingHandicap,
                    Status = 0 // Pending
                };

                context.ClubMembershipRequest.Add(clubMembershipRequest);

                await context.SaveChangesAsync(cancellationToken);                
            }
        }
        #endregion

        #region public async Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club membership request.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(Guid clubId, CancellationToken cancellationToken)
        {
            List<GetClubMembershipRequestResponse> result = new List<GetClubMembershipRequestResponse>();

            using (var context = this.ReadModelResolver())
            {
                var pendingMembershipRequests = await context.ClubMembershipRequest.Where(c => c.ClubId == clubId && c.Status == 0).ToListAsync(cancellationToken);

                foreach (var pendingMembershipRequest in pendingMembershipRequests)
                {
                    result.Add(new GetClubMembershipRequestResponse
                    {
                        ClubId = pendingMembershipRequest.ClubId,
                        MembershipRequestedDateAndTime = pendingMembershipRequest.MembershipRequestedDateAndTime,
                        PlayerId = pendingMembershipRequest.PlayerId,
                        MembershipRequestId = pendingMembershipRequest.MembershipRequestId,
                        HandicapCategory = pendingMembershipRequest.HandicapCategory,
                        Age = pendingMembershipRequest.Age,
                        Gender = pendingMembershipRequest.Gender,
                        FirstName = pendingMembershipRequest.FirstName,
                        MiddleName = pendingMembershipRequest.MiddleName,
                        PlayingHandicap = pendingMembershipRequest.PlayingHandicap,
                        LastName = pendingMembershipRequest.LastName,
                        ExactHandicap = pendingMembershipRequest.ExactHandicap,
                    });
                }
            }

            return result;
        }
        #endregion

        #region public async Task RemoveClubMembershipRequestFromReadModel(ClubMembershipApprovedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Removes the club membership request from read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No pending membership request found for Club Id {domainEvent.ClubId} and Player Id {domainEvent.AggregateId}</exception>
        public async Task RemoveClubMembershipRequestFromReadModel(ClubMembershipApprovedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using (var context = this.ReadModelResolver())
            {
                // find the pending membership request
                var pendingRequest = await context.ClubMembershipRequest.Where(c => c.ClubId == domainEvent.ClubId && 
                                                                              c.PlayerId == domainEvent.AggregateId &&
                                                                              c.Status == 0).SingleOrDefaultAsync(cancellationToken); // Pending
                
                if (pendingRequest == null)
                {
                    throw new NotFoundException($"No pending membership request found for Club Id {domainEvent.ClubId} and Player Id {domainEvent.AggregateId}");
                }

                context.ClubMembershipRequest.Remove(pendingRequest);

                await context.SaveChangesAsync(cancellationToken);                
            }
        }
        #endregion

        #region public async Task RemoveClubMembershipRequestFromReadModel(ClubMembershipRejectedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Removes the club membership request from read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No pending membership request found for Club Id {domainEvent.ClubId} and Player Id {domainEvent.AggregateId}</exception>
        public async Task RemoveClubMembershipRequestFromReadModel(ClubMembershipRejectedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using (var context = this.ReadModelResolver())
            {
                // find the pending membership request
                var pendingRequest = await context.ClubMembershipRequest.Where(c => c.ClubId == domainEvent.ClubId && 
                                                                                    c.PlayerId == domainEvent.AggregateId &&
                                                                                    c.Status == 0).SingleOrDefaultAsync(cancellationToken); // Pending
                
                if (pendingRequest == null)
                {
                    throw new NotFoundException($"No pending membership request found for Club Id {domainEvent.ClubId} and Player Id {domainEvent.AggregateId}");
                }

                context.ClubMembershipRequest.Remove(pendingRequest);

                await context.SaveChangesAsync(cancellationToken);                
            }
        }
        #endregion

        #endregion
    }
}