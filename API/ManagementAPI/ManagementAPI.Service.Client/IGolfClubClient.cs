namespace ManagementAPI.Service.Client
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;

    /// <summary>
    /// 
    /// </summary>
    public interface IGolfClubClient
    {
        #region Methods

        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task AddMeasuredCourseToGolfClub(String accessToken,
                                         Guid golfClubId,
                                         AddMeasuredCourseToClubRequest request,
                                         CancellationToken cancellationToken);

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
        /// Creates the golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<CreateGolfClubResponse> CreateGolfClub(String accessToken,
                                                    CreateGolfClubRequest request,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Creates the match secretary.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task CreateMatchSecretary(String accessToken,
                                  Guid golfClubId,
                                  CreateMatchSecretaryRequest request,
                                  CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club membership list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubMembershipDetailsResponse>> GetGolfClubMembershipList(String accessToken,
                                                                                   Guid golfClubId,
                                                                                   CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club user list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubUserListResponse> GetGolfClubUserList(String accessToken,
                                                              Guid golfClubId,
                                                              CancellationToken cancellationToken);

        /// <summary>
        /// Gets the measured courses.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetMeasuredCourseListResponse> GetMeasuredCourses(String accessToken,
                                                               Guid golfClubId,
                                                               CancellationToken cancellationToken);

        /// <summary>
        /// Gets the single golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubResponse> GetSingleGolfClub(String accessToken,
                                                    Guid golfClubId,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterGolfClubAdministrator(RegisterClubAdministratorRequest request,
                                           CancellationToken cancellationToken);

        #endregion
    }
}