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
    using DataTransferObjects;
    using IdentityModel;
    using Manager;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
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
            return this.Created(String.Empty, command.Response);
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
        [SwaggerResponse(200, type:typeof(List<CreateGolfClubResponse>))]
        [SwaggerResponseExample(200, typeof(GetGolfClubListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.GetGolfClubListPolicy)]
        [Route("List")]
        public async Task<IActionResult> GetGolfClubList(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            return this.Ok(golfClubList);
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

        [HttpGet]
        [Authorize(Policy = PolicyNames.GetGolfClubMembersListPolicy)]
        [Route("MembersList")]
        [SwaggerResponse(200, type:typeof(List<GolfClubMembershipDetails>))]
        [SwaggerResponseExample(200, typeof(GolfClubMembershipListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> GetGolfClubMembersList(CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            List<GolfClubMembershipDetails> golfClubMembersList = await this.Manager.GetGolfClubMembersList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(golfClubMembersList);
        }

        #endregion
    }
}