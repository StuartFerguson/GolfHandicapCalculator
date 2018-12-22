using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.ClubConfiguration.DomainEvents;
using ManagementAPI.Database;
using ManagementAPI.Database.Models;
using ManagementAPI.Service.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using Shared.EventStore;
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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagmentAPIManager"/> class.
        /// </summary>
        /// <param name="clubRepository">The club repository.</param>
        /// <param name="readModelResolver">The read model resolver.</param>
        public ManagmentAPIManager(IAggregateRepository<ClubConfigurationAggregate> clubRepository, Func<ManagementAPIReadModel> readModelResolver)
        {
            this.ClubRepository = clubRepository;
            this.ReadModelResolver = readModelResolver;
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

        #endregion
    }
}