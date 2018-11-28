using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.Service.DataTransferObjects;
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

        #endregion

        #region Constructors

        public ManagmentAPIManager(IAggregateRepository<ClubConfigurationAggregate> clubRepository)
        {
            this.ClubRepository = clubRepository;
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

        #endregion
    }
}