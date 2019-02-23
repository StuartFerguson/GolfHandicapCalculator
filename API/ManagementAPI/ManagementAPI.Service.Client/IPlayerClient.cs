namespace ManagementAPI.Service.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects;

    public interface IPlayerClient
    {
        #region Methods

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetPlayerDetailsResponse> GetPlayer(String passwordToken,
                                                 CancellationToken cancellationToken);

        /// <summary>
        /// Gets the player memberships.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<ClubMembershipResponse>> GetPlayerMemberships(String passwordToken,
                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest request,
                                                    CancellationToken cancellationToken);

        #endregion
    }
}