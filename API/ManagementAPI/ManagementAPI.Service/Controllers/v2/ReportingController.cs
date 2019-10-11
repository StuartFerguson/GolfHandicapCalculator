namespace ManagementAPI.Service.Controllers.v2
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manager;
    using Common;
    using Common.v2;
    using DataTransferObjects.Responses.v2;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using GetMembersHandicapListReportResponsev1 = DataTransferObjects.Responses.GetMembersHandicapListReportResponse;
    using GetNumberOfMembersByAgeCategoryReportResponsev1 = DataTransferObjects.Responses.GetNumberOfMembersByAgeCategoryReportResponse;
    using GetNumberOfMembersByHandicapCategoryReportResponsev1 = DataTransferObjects.Responses.GetNumberOfMembersByHandicapCategoryReportResponse;
    using GetNumberOfMembersByTimePeriodReportResponsev1 = DataTransferObjects.Responses.GetNumberOfMembersByTimePeriodReportResponse;
    using GetNumberOfMembersReportResponsev1 = DataTransferObjects.Responses.GetNumberOfMembersReportResponse;
    using GetPlayerScoresResponsev1 = DataTransferObjects.Responses.GetPlayerScoresResponse;
    using GetMembersHandicapListReportResponsev2 = DataTransferObjects.Responses.v2.GetMembersHandicapListReportResponse;
    using GetNumberOfMembersByAgeCategoryReportResponsev2 = DataTransferObjects.Responses.v2.GetNumberOfMembersByAgeCategoryReportResponse;
    using GetNumberOfMembersByHandicapCategoryReportResponsev2 = DataTransferObjects.Responses.v2.GetNumberOfMembersByHandicapCategoryReportResponse;
    using GetNumberOfMembersByTimePeriodReportResponsev2 = DataTransferObjects.Responses.v2.GetNumberOfMembersByTimePeriodReportResponse;
    using GetNumberOfMembersReportResponsev2 = DataTransferObjects.Responses.v2.GetNumberOfMembersReportResponse;
    using GetPlayerScoresResponsev2 = DataTransferObjects.Responses.v2.GetPlayerScoresResponse;
    using TimePeriod = DataTransferObjects.Responses.v2.TimePeriod;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route(ReportingController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
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
        /// Initializes a new instance of the <see cref="Controllers.ReportingController" /> class.
        /// </summary>
        /// <param name="reportingManager">The reporting manager.</param>
        public ReportingController(IReportingManager reportingManager)
        {
            this.ReportingManager = reportingManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the member handicap list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("golfclubs/{golfClubId}/membershandicaplist")]
        [SwaggerResponse(200, type:typeof(GetMembersHandicapListReportResponsev2))]
        [SwaggerResponseExample(200, typeof(GetMembersHandicapListReportResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetMemberHandicapList(Guid golfClubId,
                                                               CancellationToken cancellationToken)
        {
            GetMembersHandicapListReportResponsev1 managerResponse = await this.ReportingManager.GetMembersHandicapListReport(golfClubId, cancellationToken);

            GetMembersHandicapListReportResponsev2 response = this.ConvertGetMembersHandicapListReportResponse(managerResponse);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of members by age category report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("golfclubs/{golfClubId}/numberofmembersbyagecategory")]
        [SwaggerResponse(200, type:typeof(GetNumberOfMembersByAgeCategoryReportResponsev2))]
        [SwaggerResponseExample(200, typeof(GetNumberOfMembersByAgeCategoryReportResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetNumberOfMembersByAgeCategoryReport(Guid golfClubId,
                                                                               CancellationToken cancellationToken)
        {
            GetNumberOfMembersByAgeCategoryReportResponsev1 managerResponse =
                await this.ReportingManager.GetNumberOfMembersByAgeCategoryReport(golfClubId, cancellationToken);

            GetNumberOfMembersByAgeCategoryReportResponsev2 response = this.ConvertGetNumberOfMembersByAgeCategoryReportResponse(managerResponse);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("golfclubs/{golfClubId}/numberofmembersbyhandicapcategory")]
        [SwaggerResponse(200, type:typeof(GetNumberOfMembersByHandicapCategoryReportResponsev2))]
        [SwaggerResponseExample(200, typeof(GetNumberOfMembersByHandicapCategoryReportResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetNumberOfMembersByHandicapCategoryReport(Guid golfClubId,
                                                                                    CancellationToken cancellationToken)
        {
            GetNumberOfMembersByHandicapCategoryReportResponsev1 managerResponse =
                await this.ReportingManager.GetNumberOfMembersByHandicapCategoryReport(golfClubId, cancellationToken);

            GetNumberOfMembersByHandicapCategoryReportResponsev2 response = this.ConvertGetNumberOfMembersByHandicapCategoryReportResponse(managerResponse);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of members by time period report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="timePeriod">The time period.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("golfclubs/{golfClubId}/numberofmembersbytimeperiod/{timeperiod}")]
        [SwaggerResponse(200, type:typeof(GetNumberOfMembersByTimePeriodReportResponsev2))]
        [SwaggerResponseExample(200, typeof(GetNumberOfMembersByTimePeriodReportResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetNumberOfMembersByTimePeriodReport(Guid golfClubId,
                                                                              String timePeriod,
                                                                              CancellationToken cancellationToken)
        {
            GetNumberOfMembersByTimePeriodReportResponsev1 managerResponse =
                await this.ReportingManager.GetNumberOfMembersByTimePeriodReport(golfClubId, timePeriod, cancellationToken);

            GetNumberOfMembersByTimePeriodReportResponsev2 response = this.ConvertGetNumberOfMembersByTimePeriodReportResponse(managerResponse);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("golfclubs/{golfClubId}/numberofmembers")]
        [SwaggerResponse(200, type:typeof(GetNumberOfMembersReportResponsev2))]
        [SwaggerResponseExample(200, typeof(GetNumberOfMembersReportResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetNumberOfMembersReport(Guid golfClubId,
                                                                  CancellationToken cancellationToken)
        {
            GetNumberOfMembersReportResponsev1 managerResponse = await this.ReportingManager.GetNumberOfMembersReport(golfClubId, cancellationToken);

            GetNumberOfMembersReportResponsev2 response = this.ConvertGetNumberOfMembersReportResponse(managerResponse);

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the player scores.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="numberOfScores">The number of scores.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("players/{playerId}/scores")]
        [SwaggerResponse(200, type:typeof(GetPlayerScoresResponsev2))]
        [SwaggerResponseExample(200, typeof(GetPlayerScoresResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetPlayerScores(Guid playerId,
                                                         [FromQuery] Int32 numberOfScores,
                                                         CancellationToken cancellationToken)
        {
            GetPlayerScoresResponsev1 managerResponse = await this.ReportingManager.GetPlayerScoresReport(playerId, numberOfScores, cancellationToken);

            GetPlayerScoresResponsev2 response = this.ConvertGetPlayerScoresResponse(managerResponse);

            return this.Ok(response);
        }

        /// <summary>
        /// Converts the get members handicap list report response.
        /// </summary>
        /// <param name="managerResponse">The manager response.</param>
        /// <returns></returns>
        private GetMembersHandicapListReportResponsev2 ConvertGetMembersHandicapListReportResponse(GetMembersHandicapListReportResponsev1 managerResponse)
        {
            GetMembersHandicapListReportResponsev2 response = new GetMembersHandicapListReportResponsev2();

            response.GolfClubId = managerResponse.GolfClubId;
            response.MembersHandicapListReportResponse = new List<MembersHandicapListReportResponse>();

            foreach (DataTransferObjects.Responses.MembersHandicapListReportResponse membersHandicapListReportResponse in managerResponse
                .MembersHandicapListReportResponse)
            {
                response.MembersHandicapListReportResponse.Add(new MembersHandicapListReportResponse
                                                               {
                                                                   GolfClubId = membersHandicapListReportResponse.GolfClubId,
                                                                   PlayerId = membersHandicapListReportResponse.PlayerId,
                                                                   PlayingHandicap = membersHandicapListReportResponse.PlayingHandicap,
                                                                   ExactHandicap = membersHandicapListReportResponse.ExactHandicap,
                                                                   HandicapCategory = membersHandicapListReportResponse.HandicapCategory,
                                                                   PlayerName = membersHandicapListReportResponse.PlayerName
                                                               });
            }

            return response;
        }

        /// <summary>
        /// Converts the get number of members by age category report response.
        /// </summary>
        /// <param name="managerResponse">The manager response.</param>
        /// <returns></returns>
        private GetNumberOfMembersByAgeCategoryReportResponsev2 ConvertGetNumberOfMembersByAgeCategoryReportResponse(
            GetNumberOfMembersByAgeCategoryReportResponsev1 managerResponse)
        {
            GetNumberOfMembersByAgeCategoryReportResponsev2 response = new GetNumberOfMembersByAgeCategoryReportResponsev2();

            response.GolfClubId = managerResponse.GolfClubId;
            response.MembersByAgeCategoryResponse = new List<MembersByAgeCategoryResponse>();

            foreach (DataTransferObjects.Responses.MembersByAgeCategoryResponse membersByAgeCategoryResponse in managerResponse.MembersByAgeCategoryResponse)
            {
                response.MembersByAgeCategoryResponse.Add(new MembersByAgeCategoryResponse
                                                          {
                                                              AgeCategory = membersByAgeCategoryResponse.AgeCategory,
                                                              NumberOfMembers = membersByAgeCategoryResponse.NumberOfMembers
                                                          });
            }

            return response;
        }

        /// <summary>
        /// Converts the get number of members by handicap category report response.
        /// </summary>
        /// <param name="managerResponse">The manager response.</param>
        /// <returns></returns>
        private GetNumberOfMembersByHandicapCategoryReportResponsev2 ConvertGetNumberOfMembersByHandicapCategoryReportResponse(
            GetNumberOfMembersByHandicapCategoryReportResponsev1 managerResponse)
        {
            GetNumberOfMembersByHandicapCategoryReportResponsev2 response = new GetNumberOfMembersByHandicapCategoryReportResponsev2();

            response.GolfClubId = managerResponse.GolfClubId;
            response.MembersByHandicapCategoryResponse = new List<MembersByHandicapCategoryResponse>();

            foreach (DataTransferObjects.Responses.MembersByHandicapCategoryResponse membersByHandicapCategoryResponse in managerResponse
                .MembersByHandicapCategoryResponse)
            {
                response.MembersByHandicapCategoryResponse.Add(new MembersByHandicapCategoryResponse
                                                               {
                                                                   HandicapCategory = membersByHandicapCategoryResponse.HandicapCategory,
                                                                   NumberOfMembers = membersByHandicapCategoryResponse.NumberOfMembers
                                                               });
            }

            return response;
        }

        /// <summary>
        /// Converts the get number of members by time period report response.
        /// </summary>
        /// <param name="managerResponse">The manager response.</param>
        /// <returns></returns>
        private GetNumberOfMembersByTimePeriodReportResponsev2 ConvertGetNumberOfMembersByTimePeriodReportResponse(
            GetNumberOfMembersByTimePeriodReportResponsev1 managerResponse)
        {
            GetNumberOfMembersByTimePeriodReportResponsev2 response = new GetNumberOfMembersByTimePeriodReportResponsev2();

            response.GolfClubId = managerResponse.GolfClubId;
            response.TimePeriod = (TimePeriod)managerResponse.TimePeriod;
            response.MembersByTimePeriodResponse = new List<MembersByTimePeriodResponse>();

            foreach (DataTransferObjects.Responses.MembersByTimePeriodResponse membersByTimePeriodResponse in managerResponse.MembersByTimePeriodResponse)
            {
                response.MembersByTimePeriodResponse.Add(new MembersByTimePeriodResponse
                                                         {
                                                             NumberOfMembers = membersByTimePeriodResponse.NumberOfMembers,
                                                             Period = membersByTimePeriodResponse.Period
                                                         });
            }

            return response;
        }

        /// <summary>
        /// Converts the get number of members report response.
        /// </summary>
        /// <param name="managerResponse">The manager response.</param>
        /// <returns></returns>
        private GetNumberOfMembersReportResponsev2 ConvertGetNumberOfMembersReportResponse(GetNumberOfMembersReportResponsev1 managerResponse)
        {
            GetNumberOfMembersReportResponsev2 response = new GetNumberOfMembersReportResponsev2();

            response.GolfClubId = managerResponse.GolfClubId;
            response.NumberOfMembers = managerResponse.NumberOfMembers;

            return response;
        }

        /// <summary>
        /// Converts the get player scores response.
        /// </summary>
        /// <param name="managerResponse">The manager response.</param>
        /// <returns></returns>
        private GetPlayerScoresResponsev2 ConvertGetPlayerScoresResponse(GetPlayerScoresResponsev1 managerResponse)
        {
            GetPlayerScoresResponsev2 response = new GetPlayerScoresResponsev2();

            response.Scores = new List<PlayerScoreResponse>();

            foreach (DataTransferObjects.Responses.PlayerScoreResponse managerResponseScore in managerResponse.Scores)
            {
                response.Scores.Add(new PlayerScoreResponse
                                    {
                                        GolfClubId = managerResponseScore.GolfClubId,
                                        MeasuredCourseId = managerResponseScore.MeasuredCourseId,
                                        TournamentFormat = (TournamentFormat)managerResponseScore.TournamentFormat,
                                        TournamentDate = managerResponseScore.TournamentDate,
                                        TournamentName = managerResponseScore.TournamentName,
                                        MeasuredCourseName = managerResponseScore.MeasuredCourseName,
                                        TournamentId = managerResponseScore.TournamentId,
                                        PlayerId = managerResponseScore.PlayerId,
                                        MeasuredCourseTeeColour = managerResponseScore.MeasuredCourseTeeColour,
                                        GolfClubName = managerResponseScore.GolfClubName,
                                        PlayingHandicap = managerResponseScore.PlayingHandicap,
                                        CSS = managerResponseScore.CSS,
                                        NetScore = managerResponseScore.NetScore,
                                        GrossScore = managerResponseScore.GrossScore
                                    });
            }

            return response;
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "reports";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + ReportingController.ControllerName;

        #endregion
    }
}