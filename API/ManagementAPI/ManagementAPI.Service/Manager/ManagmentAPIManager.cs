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

            using (ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.GolfClub.Where(c => c.GolfClubId == domainEvent.AggregateId)
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

            using (ManagementAPIReadModel context = ReadModelResolver())
            {
                List<Database.Models.GolfClub> golfClubs = await context.GolfClub.ToListAsync(cancellationToken);

                foreach (Database.Models.GolfClub golfClub in golfClubs)
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

        #endregion
    }
}