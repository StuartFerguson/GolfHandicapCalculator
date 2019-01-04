using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Client
{
    public interface IPlayerClient
    {
        /// <summary>
        /// Creates the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterPlayerResponse> CreatePlayer(RegisterPlayerRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RequestClubMembership(String passwordToken, Guid playerId, Guid clubId, CancellationToken cancellationToken);
    }
}