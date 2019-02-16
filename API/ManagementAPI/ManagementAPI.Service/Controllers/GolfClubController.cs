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
    using Shared.CommandHandling;

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
            return this.Ok(command.Response);
        }

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateGolfClubResponse), 200)]
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
            return this.Ok(command.Response);
        }

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetGolfClubResponse), 200)]
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
        [ProducesResponseType(typeof(List<GetGolfClubResponse>), 200)]
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
        [Route("RegisterGolfClubAdministrator")]
        public async Task<IActionResult> RegisterGolfClubAdministrator([FromBody] RegisterClubAdministratorRequest request,
                                                                       CancellationToken cancellationToken)
        {
            await this.Manager.RegisterClubAdministrator(request, cancellationToken);

            // return the result
            return this.Ok();
        }

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{golfClubId}/RequestClubMembership")]
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
            return this.Ok(command.Response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GolfClubMembershipDetails>), 200)]
        [Authorize(Policy = PolicyNames.GetGolfClubMembersListPolicy)]
        [Route("MembersList")]
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