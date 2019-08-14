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
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(Guid golfClubId,
                                                                        CancellationToken cancellationToken);

        #endregion
    }
}