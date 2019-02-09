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
    }
}
