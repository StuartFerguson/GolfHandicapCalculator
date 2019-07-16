namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using IdentityModel;
    using Manager;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class GolfClubController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubController" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="manager">The manager.</param>
        public GolfClubController(ICommandRouter commandRouter,
                                  IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Puts the club configuration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = PolicyNames.AddMeasuredCourseToGolfClubPolicy)]
        [Route("AddMeasuredCourse")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> AddMeasuredCourseToGolfClub([FromBody] AddMeasuredCourseToClubRequest request,
                                                                     CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            AddMeasuredCourseToClubCommand command = AddMeasuredCourseToClubCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        [HttpPut]
        [Authorize(Policy = PolicyNames.AddTournamentDivisionToGolfClubPolicy)]
        [SwaggerResponse(204)]
        [Route("AddTournamentDivision")]
        public async Task<IActionResult> AddTournamentDivision([FromBody] AddTournamentDivisionToGolfClubRequest request,
                                                               CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            AddTournamentDivisionToGolfClubCommand command = AddTournamentDivisionToGolfClubCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(201, type:typeof(CreateGolfClubResponse))]
        [SwaggerResponseExample(201, typeof(CreateGolfClubResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Route("Create")]
        public async Task<IActionResult> CreateGolfClub([FromBody] CreateGolfClubRequest request,
                                                        CancellationToken cancellationToken)
        {
            // Get the user id (subject) for the user
            Claim subjectIdClaim = ClaimsHelper.GetUserClaim(this.User, JwtClaimTypes.Subject);

            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            CreateGolfClubCommand command = CreateGolfClubCommand.Create(Guid.Parse(golfClubIdClaim.Value), Guid.Parse(subjectIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created(string.Empty, command.Response);
        }

        [HttpPut]
        [Authorize(Policy = PolicyNames.CreateMatchSecretaryPolicy)]
        [Route("CreateMatchSecretary")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> CreateMatchSecretary([FromBody] CreateMatchSecretaryRequest request,
                                                              CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            CreateMatchSecretaryCommand command = CreateMatchSecretaryCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            await this.CommandRouter.Route(command, cancellationToken);

            return this.NoContent();
        }

        [HttpGet]
        [Route("Users")]
        [SwaggerResponse(200, type: typeof(GetGolfClubUserListResponse))]
        [SwaggerResponseExample(200, typeof(GetGolfClubUserListResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.GetClubUsersListPolicy)]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            GetGolfClubUserListResponse users = await this.Manager.GetGolfClubUsers(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(users);
        }

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type:typeof(GetGolfClubResponse))]
        [SwaggerResponseExample(200, typeof(GetGolfClubResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.GetSingleGolfClubPolicy)]
        public async Task<IActionResult> GetGolfClub(CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            GetGolfClubResponse golfClub = await this.Manager.GetGolfClub(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(golfClub);
        }

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type:typeof(List<GetGolfClubResponse>))]
        [SwaggerResponseExample(200, typeof(GetGolfClubListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.GetGolfClubListPolicy)]
        [Route("List")]
        public async Task<IActionResult> GetGolfClubList(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            return this.Ok(golfClubList);
        }

        [HttpGet]
        [Authorize(Policy = PolicyNames.GetGolfClubMembersListPolicy)]
        [Route("MembersList")]
        [SwaggerResponse(200, type:typeof(List<GetGolfClubMembershipDetailsResponse>))]
        [SwaggerResponseExample(200, typeof(GolfClubMembershipListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetGolfClubMembersList(CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            List<GetGolfClubMembershipDetailsResponse> golfClubMembersList =
                await this.Manager.GetGolfClubMembersList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(golfClubMembersList);
        }

        [HttpGet]
        [Route("MeasuredCourses")]
        [SwaggerResponse(200, type: typeof(GetMeasuredCourseListResponse))]
        [SwaggerResponseExample(200, typeof(GetMeasuredCourseListResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.GetMeasuredCoursesPolicy)]
        public async Task<IActionResult> GetMeasuredCourses(CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            GetMeasuredCourseListResponse measuredCourseList = await this.Manager.GetMeasuredCourseList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(measuredCourseList);
        }

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse(204)]
        [Route("RegisterGolfClubAdministrator")]
        public async Task<IActionResult> RegisterGolfClubAdministrator([FromBody] RegisterClubAdministratorRequest request,
                                                                       CancellationToken cancellationToken)
        {
            await this.Manager.RegisterClubAdministrator(request, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = PolicyNames.RequestClubMembershipPolicy)]
        [Route("{golfClubId}/RequestClubMembership")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> RequestClubMembership([FromRoute] Guid golfClubId,
                                                               CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId);

            // Create the command
            RequestClubMembershipCommand command = RequestClubMembershipCommand.Create(Guid.Parse(playerIdClaim.Value), golfClubId);

            //Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        #endregion
    }
}