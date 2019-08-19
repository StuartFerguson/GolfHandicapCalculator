namespace ManagementAPI.Service.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Responses;

    public interface IReportingClient
    {
        #region Methods

        /// <summary>
        /// Gets the members handicap list report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetMembersHandicapListReportResponse> GetMembersHandicapListReport(String accessToken,
                                                                                Guid golfClubId,
                                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members by age category report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersByAgeCategoryReportResponse> GetNumberOfMembersByAgeCategoryReport(String accessToken,
                                                                                                  Guid golfClubId,
                                                                                                  CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members by handicap category report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersByHandicapCategoryReportResponse> GetNumberOfMembersByHandicapCategoryReport(String accessToken,
                                                                                                            Guid golfClubId,
                                                                                                            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members by time period report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersByTimePeriodReportResponse> GetNumberOfMembersByTimePeriodReport(String accessToken,
                                                                                                Guid golfClubId,
                                                                                                String timePeriod,
                                                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(String accessToken,
                                                                        Guid golfClubId,
                                                                        CancellationToken cancellationToken);

        #endregion
    }
}