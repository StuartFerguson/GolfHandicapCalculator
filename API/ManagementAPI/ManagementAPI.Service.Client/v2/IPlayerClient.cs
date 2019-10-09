namespace ManagementAPI.Service.Client.v2
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses.v2;

    /// <summary>
    /// 
    /// </summary>
    public interface IPlayerClient
    {
        #region Methods

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetPlayerResponse> GetPlayer(String accessToken,
                                          Guid playerId,
                                          CancellationToken cancellationToken);

        /// <summary>
        /// Gets the player memberships.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<ClubMembershipResponse>> GetPlayerMemberships(String accessToken,
                                                                Guid playerId,
                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the tournaments signed up for.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<SignedUpTournamentResponse>> GetTournamentsSignedUpFor(String accessToken,
                                                                         Guid playerId,
                                                                         CancellationToken cancellationToken);

        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest request,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RequestClubMembershipResponse> RequestClubMembership(String accessToken,
                                                                  Guid playerId,
                                                                  Guid golfClubId,
                                                                  CancellationToken cancellationToken);

        #endregion
    }
}