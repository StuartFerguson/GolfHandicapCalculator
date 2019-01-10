using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;

namespace ManagementAPI.Service.Client
{
    public interface IPlayerClient
    {
        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RequestClubMembership(String passwordToken, Guid clubId, CancellationToken cancellationToken);

        /// <summary>
        /// Approves the club membership request.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ApproveClubMembershipRequest(String passwordToken, Guid playerId, CancellationToken cancellationToken);

        /// <summary>
        /// Rejects the club membership request.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RejectClubMembershipRequest(String passwordToken, Guid playerId, RejectMembershipRequestRequest request, CancellationToken cancellationToken);
    }
}