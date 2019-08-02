namespace ManagementAPI.Service.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    public interface IPlayerClient
    {
        #region Methods

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubResponse>> GetGolfClubList(String accessToken,
                                                        Guid playerId,
                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetPlayerDetailsResponse> GetPlayer(String accessToken,
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
        /// Records the player score.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RecordPlayerScore(String accessToken,
                               Guid playerId,
                               Guid tournamentId,
                               RecordPlayerTournamentScoreRequest request,
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
        Task RequestClubMembership(String accessToken,
                                   Guid playerId,
                                   Guid golfClubId,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Signs up player for tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task SignUpPlayerForTournament(String accessToken,
                                       Guid playerId,
                                       Guid tournamentId,
                                       CancellationToken cancellationToken);

        #endregion
    }
}