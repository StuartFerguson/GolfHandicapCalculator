using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Common;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Controllers
{
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
        /// Initializes a new instance of the <see cref="GolfClubController"/> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        public GolfClubController(ICommandRouter commandRouter,IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        #region Public Methods

        #region public async Task<IActionResult> PostGolfClub(CreateGolfClubRequest request, CancellationToken cancellationToken)                
        /// <summary>
        /// Posts the golf club.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateGolfClubResponse), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> PostGolfClub([FromBody]CreateGolfClubRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = CreateGolfClubCommand.Create(request);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> GetGolfClub([FromQuery] Guid golfClubId,CancellationToken cancellationToken)                
        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetGolfClubResponse), 200)]
        [Route("{golfClubId}")]
        [Authorize(Policy = PolicyNames.GetSingleClubPolicy)]
        public async Task<IActionResult> GetGolfClub([FromRoute] Guid golfClubId,CancellationToken cancellationToken)
        {
            var golfClub = await this.Manager.GetGolfClub(golfClubId, cancellationToken);

            return this.Ok(golfClub);
        }
        #endregion

        #region public async Task<IActionResult> GetGolfClubList(CancellationToken cancellationToken)                      
        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetGolfClubResponse>), 200)]        
        [Authorize(Policy = PolicyNames.GetClubListPolicy)]
        public async Task<IActionResult> GetGolfClubList(CancellationToken cancellationToken)
        {
            var golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            return this.Ok(golfClubList);
        }
        #endregion

        #region public async Task<IActionResult> PutGolfClub([FromRoute] Guid golfClubId, [FromBody] AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)
        /// <summary>
        /// Puts the club configuration.
        /// </summary>
        /// <param name="golfClubId">The club configuration identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = PolicyNames.AddMeasuredCourseToClubPolicy)]
        [Route("{golfClubId}")]
        public async Task<IActionResult> PutGolfClub([FromRoute] Guid golfClubId, [FromBody] AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = AddMeasuredCourseToClubCommand.Create(golfClubId, request);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> GetPendingMembershipRequests([FromRoute] Guid golfClubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the pending membership requests.
        /// </summary>
        /// <param name="golfClubId">The club configuration identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetClubMembershipRequestResponse>), 200)]
        [Route("{golfClubId}/PendingMembershipRequests")]
        [Authorize(Policy = PolicyNames.GetPendingMembershipRequestsPolicy)]
        public async Task<IActionResult> GetPendingMembershipRequests([FromRoute] Guid golfClubId, CancellationToken cancellationToken)
        {
            var pendingMembershipRequests = await this.Manager.GetPendingMembershipRequests(golfClubId, cancellationToken);

            return this.Ok(pendingMembershipRequests);
        }
        #endregion

        #endregion
    }
}