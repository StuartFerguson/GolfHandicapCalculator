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
        Task<AddMeasuredCourseToClubResponse> AddMeasuredCourseToGolfClub(String accessToken,
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
        Task<AddTournamentDivisionToGolfClubResponse> AddTournamentDivision(String accessToken,
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
        Task<CreateMatchSecretaryResponse> CreateMatchSecretary(String accessToken,
                                                                Guid golfClubId,
                                                                CreateMatchSecretaryRequest request,
                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the single golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubResponse> GetGolfClub(String accessToken,
                                              Guid golfClubId,
                                              CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="includeMembers">if set to <c>true</c> [include members].</param>
        /// <param name="includeMeasuredCourses">if set to <c>true</c> [include measured courses].</param>
        /// <param name="includeUsers">if set to <c>true</c> [include users].</param>
        /// <returns></returns>
        Task<List<GetGolfClubResponse>> GetGolfClubList(String accessToken,
                                                        CancellationToken cancellationToken,
                                                        Boolean includeMembers = false,
                                                        Boolean includeMeasuredCourses = false,
                                                        Boolean includeUsers = false);

        /// <summary>
        /// Gets the golf club membership list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GolfClubMembershipDetailsResponse>> GetGolfClubMembershipList(String accessToken,
                                                                                Guid golfClubId,
                                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club user list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GolfClubUserResponse>> GetGolfClubUserList(String accessToken,
                                                             Guid golfClubId,
                                                             CancellationToken cancellationToken);

        /// <summary>
        /// Gets the measured courses.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<MeasuredCourseListResponse>> GetMeasuredCourses(String accessToken,
                                                                  Guid golfClubId,
                                                                  CancellationToken cancellationToken);

        #endregion
    }
}