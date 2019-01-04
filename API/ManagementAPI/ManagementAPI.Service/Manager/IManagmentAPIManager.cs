using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfiguration.DomainEvents;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Manager
{
    public interface IManagmentAPIManager
    {
        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetClubConfigurationResponse> GetClubConfiguration(Guid clubId, CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the club information to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertClubInformationToReadModel(ClubConfigurationCreatedEvent domainEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetClubConfigurationResponse>> GetClubList(CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the club membership request to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertClubMembershipRequestToReadModel(ClubMembershipRequestedEvent domainEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the club membership request.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(Guid clubId, CancellationToken cancellationToken);
    }
}
