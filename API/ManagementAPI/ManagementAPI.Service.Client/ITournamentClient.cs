namespace ManagementAPI.Service.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    /// <summary>
    /// 
    /// </summary>
    public interface ITournamentClient
    {
        #region Methods

        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CancelTournament(String accessToken,
                              Guid golfClubId,
                              Guid tournamentId,
                              CancelTournamentRequest request,
                              CancellationToken cancellationToken);

        /// <summary>
        /// Completes the tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CompleteTournament(String accessToken,
                                Guid golfClubId,
                                Guid tournamentId,
                                CancellationToken cancellationToken);

        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateTournamentResponse> CreateTournament(String accessToken,
                                                        Guid golfClubId,
                                                        CreateTournamentRequest request,
                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Gets the tournament list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetTournamentListResponse> GetTournamentList(String accessToken,
                                                          Guid golfClubId,
                                                          CancellationToken cancellationToken);

        /// <summary>
        /// Produces the tournament result.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task ProduceTournamentResult(String accessToken,
                                     Guid golfClubId,
                                     Guid tournamentId,
                                     CancellationToken cancellationToken);

        #endregion
    }
}