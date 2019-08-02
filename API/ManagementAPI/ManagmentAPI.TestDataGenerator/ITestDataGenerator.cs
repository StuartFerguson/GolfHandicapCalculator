namespace ManagmentAPI.TestDataGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using ManagementAPI.Service.DataTransferObjects;
    using ManagementAPI.Service.DataTransferObjects.Requests;
    using ManagementAPI.Service.DataTransferObjects.Responses;

    /// <summary>
    /// 
    /// </summary>
    public interface ITestDataGenerator
    {
        #region Methods

        /// <summary>
        /// Adds the tournament division.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddTournamentDivision(String accessToken,
                                   Guid golfClubId,
                                   AddTournamentDivisionToGolfClubRequest request,
                                   CancellationToken cancellationToken);

        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMeasuredCourseToGolfClub(String accessToken,
                                         Guid golfClubId,
                                         AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="createGolfClubRequest">The create golf club request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateGolfClubResponse> CreateGolfClub(String accessToken,
                                                    CreateGolfClubRequest createGolfClubRequest,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="createTournamentRequest">The create tournament request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateTournamentResponse> CreateTournament(String accessToken,
                                                        Guid golfClubId,
                                                        CreateTournamentRequest createTournamentRequest,
                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns></returns>
        Task<String> GetToken(TokenType tokenType,
                              String clientId,
                              String clientSecret,
                              String userName = "",
                              String password = "",
                              List<String> scopes = null);

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        String GetToken(String userName);

        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordMemberTournamentScoreRequest">The record member tournament score request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RecordPlayerScore(String accessToken,
                               Guid playerId,
                               Guid tournamentId,
                               RecordPlayerTournamentScoreRequest recordMemberTournamentScoreRequest,
                               CancellationToken cancellationToken);

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="registerClubAdministratorRequest">The register club administrator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterGolfClubAdministrator(RegisterClubAdministratorRequest registerClubAdministratorRequest,
                                           CancellationToken cancellationToken);

        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="registerPlayerRequest">The register player request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest registerPlayerRequest,
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