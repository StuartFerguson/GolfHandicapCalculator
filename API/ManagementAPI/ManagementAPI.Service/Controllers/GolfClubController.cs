namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Commands;
    using BusinessLogic.Manager;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using IdentityModel;
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
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{golfClubId}/AddMeasuredCourse")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> AddMeasuredCourseToGolfClub([FromRoute] Guid golfClubId,
                                                                     [FromBody] AddMeasuredCourseToClubRequest request,
                                                                     CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            AddMeasuredCourseToClubCommand command = AddMeasuredCourseToClubCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{golfClubId}/AddTournamentDivision")]
        public async Task<IActionResult> AddTournamentDivision([FromRoute] Guid golfClubId,
                                                               [FromBody] AddTournamentDivisionToGolfClubRequest request,
                                                               CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

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
        [Route("")]
        public async Task<IActionResult> CreateGolfClub([FromBody] CreateGolfClubRequest request,
                                                        CancellationToken cancellationToken)
        {
            if (ClaimsHelper.IsPasswordToken(this.User) == false)
            {
                return this.Forbid();
            }

            // Get the user id (subject) for the user
            Claim subjectIdClaim = ClaimsHelper.GetUserClaim(this.User, JwtClaimTypes.Subject);

            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            CreateGolfClubCommand command = CreateGolfClubCommand.Create(Guid.Parse(golfClubIdClaim.Value), Guid.Parse(subjectIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"api/golfclub/{command.Response}", command.Response);
        }

        [HttpPut]
        [Route("{golfClubId}/CreateMatchSecretary")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> CreateMatchSecretary([FromRoute] Guid golfClubId,
                                                              [FromBody] CreateMatchSecretaryRequest request,
                                                              CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            CreateMatchSecretaryCommand command = CreateMatchSecretaryCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            await this.CommandRouter.Route(command, cancellationToken);

            return this.NoContent();
        }

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type:typeof(GetGolfClubResponse))]
        [SwaggerResponseExample(200, typeof(GetGolfClubResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Route("{golfClubId}")]
        public async Task<IActionResult> GetGolfClub([FromRoute] Guid golfClubId,
                                                     CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            GetGolfClubResponse golfClub = await this.Manager.GetGolfClub(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(golfClub);
        }

        [HttpGet]
        [Route("{golfClubId}/MembersList")]
        [SwaggerResponse(200, type:typeof(List<GetGolfClubMembershipDetailsResponse>))]
        [SwaggerResponseExample(200, typeof(GolfClubMembershipListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetGolfClubMembersList([FromRoute] Guid golfClubId,
                                                                CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            List<GetGolfClubMembershipDetailsResponse> golfClubMembersList =
                await this.Manager.GetGolfClubMembersList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(golfClubMembersList);
        }

        [HttpGet]
        [Route("{golfClubId}/MeasuredCourses")]
        [SwaggerResponse(200, type:typeof(GetMeasuredCourseListResponse))]
        [SwaggerResponseExample(200, typeof(GetMeasuredCourseListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetMeasuredCourses([FromRoute] Guid golfClubId,
                                                            CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            GetMeasuredCourseListResponse measuredCourseList = await this.Manager.GetMeasuredCourseList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(measuredCourseList);
        }

        [HttpGet]
        [Route("{golfClubId}/Users")]
        [SwaggerResponse(200, type:typeof(GetGolfClubUserListResponse))]
        [SwaggerResponseExample(200, typeof(GetGolfClubUserListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        //[Authorize(Policy = PolicyNames.GetClubUsersListPolicy)]
        public async Task<IActionResult> GetUsers([FromRoute] Guid golfClubId,
                                                  CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            GetGolfClubUserListResponse users = await this.Manager.GetGolfClubUsers(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(users);
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
        
        #endregion
    }
}