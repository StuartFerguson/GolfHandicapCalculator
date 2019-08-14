namespace ManagementAPI.BusinessLogic.Manager
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.EntityFrameworkCore;
    using Service.DataTransferObjects.Responses;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagementAPI.BusinessLogic.Manager.IReportingManager" />
    public class ReportingManager : IReportingManager
    {
        #region Fields

        /// <summary>
        /// The read model resolver
        /// </summary>
        private readonly Func<ManagementAPIReadModel> ReadModelResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingManager"/> class.
        /// </summary>
        /// <param name="readModelResolver">The read model resolver.</param>
        public ReportingManager(Func<ManagementAPIReadModel> readModelResolver)
        {
            this.ReadModelResolver = readModelResolver;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(Guid golfClubId,
                                                                                     CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, nameof(golfClubId));
            GetNumberOfMembersReportResponse response = new GetNumberOfMembersReportResponse();
            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                Int32 membersCount = await context.GolfClubMembershipReporting.CountAsync(g => g.GolfClubId == golfClubId, cancellationToken);
                response.NumberOfMembers = membersCount;
                response.GolfClubId = golfClubId;
            }

            return response;
        }

        #endregion
    }
}