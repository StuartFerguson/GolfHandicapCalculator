using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.GolfClub.DomainEvents;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Manager
{
    public interface IManagmentAPIManager
    {
        /// <summary>
        /// Registers the club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterClubAdministrator(RegisterClubAdministratorRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubResponse> GetGolfClub(Guid golfClubId, CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the golf club to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertGolfClubToReadModel(GolfClubCreatedEvent domainEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubResponse>> GetGolfClubList(CancellationToken cancellationToken);

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
        /// <param name="golfClubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(Guid golfClubId, CancellationToken cancellationToken);

        /// <summary>
        /// Removes the club membership request from read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RemoveClubMembershipRequestFromReadModel(ClubMembershipApprovedEvent domainEvent, CancellationToken cancellationToken);

        /// <summary>
        /// Removes the club membership request from read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RemoveClubMembershipRequestFromReadModel(ClubMembershipRejectedEvent domainEvent, CancellationToken cancellationToken);
    }
}
