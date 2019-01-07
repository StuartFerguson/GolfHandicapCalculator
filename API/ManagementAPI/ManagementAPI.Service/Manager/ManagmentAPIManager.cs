using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.ClubConfiguration.DomainEvents;
using ManagementAPI.Database;
using ManagementAPI.Database.Models;
using ManagementAPI.Player;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.DataTransferObjects;
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
        private readonly IAggregateRepository<ClubConfigurationAggregate> ClubRepository;

        /// <summary>
        /// The read model resolver
        /// </summary>
        private readonly Func<ManagementAPIReadModel> ReadModelResolver;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagmentAPIManager"/> class.
        /// </summary>
        /// <param name="clubRepository">The club repository.</param>
        /// <param name="readModelResolver">The read model resolver.</param>
        public ManagmentAPIManager(IAggregateRepository<ClubConfigurationAggregate> clubRepository, Func<ManagementAPIReadModel> readModelResolver,
            IAggregateRepository<PlayerAggregate> playerRepository)
        {
            this.ClubRepository = clubRepository;
            this.ReadModelResolver = readModelResolver;
            this.PlayerRepository = playerRepository;
        }

        #endregion

        #region Public Methods

        #region public async Task<GetClubConfigurationResponse> GetClubConfiguration(Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetClubConfigurationResponse> GetClubConfiguration(Guid clubId, CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(clubId, typeof(ArgumentNullException), "A Club Id must be provided to retrieve a club configuration");

            GetClubConfigurationResponse result = null;

            // Find the club configuration by id
            var clubConfiguration = await this.ClubRepository.GetLatestVersion(clubId, cancellationToken);

            // Check we have found the club configuration
            if (!clubConfiguration.HasBeenCreated)
            {
                throw new NotFoundException($"Club Configuration not found for Club Id [{clubId}]");
            }

            // We have found the club configuration
            result = new GetClubConfigurationResponse
            {
                AddressLine1 = clubConfiguration.AddressLine1,
                EmailAddress = clubConfiguration.EmailAddress,
                PostalCode = clubConfiguration.PostalCode,
                Name = clubConfiguration.Name,
                Town = clubConfiguration.Town,
                Website = clubConfiguration.Website,
                Region = clubConfiguration.Region,
                TelephoneNumber = clubConfiguration.TelephoneNumber,
                AddressLine2 = clubConfiguration.AddressLine2,
                Id = clubConfiguration.AggregateId
            };

            return result;
        }
        #endregion

        #region public async Task InsertClubInformationToReadModel(ClubConfigurationCreatedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Inserts the club information to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertClubInformationToReadModel(ClubConfigurationCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using (var context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                var isDuplicate = await context.ClubInformation.Where(c => c.ClubConfigurationId == domainEvent.AggregateId)
                    .AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    ClubInformation clubInformation = new ClubInformation
                    {
                        AddressLine1 = domainEvent.AddressLine1,
                        EmailAddress = domainEvent.EmailAddress,
                        Name = domainEvent.Name,
                        Town = domainEvent.Town,
                        Region = domainEvent.Region,
                        TelephoneNumber = domainEvent.TelephoneNumber,
                        AddressLine2 = domainEvent.AddressLine2,
                        ClubConfigurationId = domainEvent.AggregateId,
                        PostalCode = domainEvent.PostalCode,
                        WebSite = domainEvent.Website
                    };

                    context.ClubInformation.Add(clubInformation);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }
        #endregion

        #region public async Task<List<GetClubConfigurationResponse>> GetClubList(CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetClubConfigurationResponse>> GetClubList(CancellationToken cancellationToken)
        {
            List<GetClubConfigurationResponse> result = new List<GetClubConfigurationResponse>();

            using (var context = ReadModelResolver())
            {
                var clubInformationList = await context.ClubInformation.ToListAsync(cancellationToken);

                foreach (var clubInformation in clubInformationList)
                {
                    result.Add(new GetClubConfigurationResponse
                    {
                        AddressLine1 = clubInformation.AddressLine1,
                        EmailAddress = clubInformation.EmailAddress,
                        Name = clubInformation.Name,
                        Town = clubInformation.Town,
                        Website = clubInformation.WebSite,
                        Region = clubInformation.Region,
                        TelephoneNumber = clubInformation.TelephoneNumber,
                        PostalCode = clubInformation.PostalCode,
                        AddressLine2 = clubInformation.AddressLine2,
                        Id = clubInformation.ClubConfigurationId
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
            var club = await this.ClubRepository.GetLatestVersion(domainEvent.ClubId, cancellationToken);

            if (!club.HasBeenCreated)
            {
                throw new InvalidOperationException($"Unable to find club with Id {domainEvent.ClubId}");
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