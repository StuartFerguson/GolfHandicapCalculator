namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manager;
    using DataTransferObjects.Responses;
    using Microsoft.AspNetCore.Mvc;
    using StructureMap.Diagnostics;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The reporting manager
        /// </summary>
        private readonly IReportingManager ReportingManager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportingController"/> class.
        /// </summary>
        /// <param name="reportingManager">The reporting manager.</param>
        public ReportingController(IReportingManager reportingManager)
        {
            this.ReportingManager = reportingManager;
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("GolfClub/{golfClubId}/membershandicaplist")]
        public async Task<IActionResult> GetMemberHandicapList(Guid golfClubId,
                                                               CancellationToken cancellationToken)
        {
            GetMembersHandicapListReportResponse response = await this.ReportingManager.GetMembersHandicapListReport(golfClubId, cancellationToken);

            return this.Ok(response);
        }

        [HttpGet]
        [Route("GolfClub/{golfClubId}/numberofmembersbyagecategory")]
        public async Task<IActionResult> GetNumberOfMembersByAgeCategoryReport(Guid golfClubId,
                                                                               CancellationToken cancellationToken)
        {
            GetNumberOfMembersByAgeCategoryReportResponse response = await this.ReportingManager.GetNumberOfMembersByAgeCategoryReport(golfClubId, cancellationToken);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GolfClub/{golfClubId}/numberofmembersbyhandicapcategory")]
        public async Task<IActionResult> GetNumberOfMembersByHandicapCategoryReport(Guid golfClubId,
                                                                                    CancellationToken cancellationToken)
        {
            GetNumberOfMembersByHandicapCategoryReportResponse reportData =
                await this.ReportingManager.GetNumberOfMembersByHandicapCategoryReport(golfClubId, cancellationToken);

            return this.Ok(reportData);
        }

        [HttpGet]
        [Route("GolfClub/{golfClubId}/numberofmembersbytimeperiod/{timeperiod}")]
        public async Task<IActionResult> GetNumberOfMembersByTimePeriodReport(Guid golfClubId,
                                                                              String timePeriod,
                                                                              CancellationToken cancellationToken)
        {
            GetNumberOfMembersByTimePeriodReportResponse response =
                await this.ReportingManager.GetNumberOfMembersByTimePeriodReport(golfClubId, timePeriod, cancellationToken);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GolfClub/{golfClubId}/numberofmembers")]
        public async Task<IActionResult> GetNumberOfMembersReport(Guid golfClubId,
                                                                  CancellationToken cancellationToken)
        {
            GetNumberOfMembersReportResponse reportData = await this.ReportingManager.GetNumberOfMembersReport(golfClubId, cancellationToken);

            return this.Ok(reportData);
        }

        #endregion
    }
}