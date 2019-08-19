namespace ManagementAPI.BusinessLogic.Manager
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Service.DataTransferObjects.Responses;

    /// <summary>
    /// 
    /// </summary>
    public interface IReportingManager
    {
        #region Methods

        /// <summary>
        /// Gets the number of members by age category report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersByAgeCategoryReportResponse> GetNumberOfMembersByAgeCategoryReport(Guid golfClubId,
                                                                                                  CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members by handicap category report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersByHandicapCategoryReportResponse> GetNumberOfMembersByHandicapCategoryReport(Guid golfClubId,
                                                                                                            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members by time period report.
        /// </summary>
        /// <param name="golfClubid">The golf clubid.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersByTimePeriodReportResponse> GetNumberOfMembersByTimePeriodReport(Guid golfClubid,
                                                                                                String timePeriod,
                                                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(Guid golfClubId,
                                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Gets the members handicap list report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetMembersHandicapListReportResponse> GetMembersHandicapListReport(Guid golfClubId,
                                                                        CancellationToken cancellationToken);

        #endregion
    }
}