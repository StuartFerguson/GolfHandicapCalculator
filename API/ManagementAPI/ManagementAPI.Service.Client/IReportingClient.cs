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
        /// Gets the number of members report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(String accessToken,
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

        #endregion
    }
}