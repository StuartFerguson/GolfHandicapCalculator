using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Common;
using ManagementAPI.Service.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubController"/> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        public PlayerController(ICommandRouter commandRouter)
        {
            this.CommandRouter = commandRouter;
        }

        #endregion

        #region Public Methods

        #region public async Task<IActionResult> PostPlayer([FromBody]RegisterPlayerRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Posts the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(RegisterPlayerResponse), 200)]
        [AllowAnonymous]
        public async Task<IActionResult> PostPlayer([FromBody]RegisterPlayerRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = RegisterPlayerCommand.Create(request);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> RequestClubMembership([FromRoute] Guid playerId, [FromRoute] Guid clubId, CancellationToken cancellationToken)
        [HttpPut]
        [Route("{playerId}/ClubMembershipRequest/{clubId}")]
        [Authorize(Policy = PolicyNames.RequestClubMembershipForPlayerPolicy)]
        public async Task<IActionResult> RequestClubMembershipForPlayer([FromRoute] Guid playerId, [FromRoute] Guid clubId, CancellationToken cancellationToken)
        {
            // Create the command
            var command = PlayerClubMembershipRequestCommand.Create(playerId, clubId);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok();
        }
        #endregion

        #region public async Task<IActionResult> ApprovePlayerMembershipRequest([FromRoute] Guid playerId, [FromRoute] Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Approves the player membership request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{playerId}/ClubMembershipRequest/{clubId}/Approve")]
        [Authorize(Policy = PolicyNames.ApprovePlayerMembershipRequestPolicy)]
        public async Task<IActionResult> ApprovePlayerMembershipRequest([FromRoute] Guid playerId, [FromRoute] Guid clubId, CancellationToken cancellationToken)
        {
            // Create the command
            var command = ApprovePlayerMembershipRequestCommand.Create(playerId, clubId);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok();
        }
        #endregion

        #region public async Task<IActionResult> ApprovePlayerMembershipRequest([FromRoute] Guid playerId, [FromRoute] Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Approves the player membership request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{playerId}/ClubMembershipRequest/{clubId}/Reject")]
        //[Authorize(Policy = PolicyNames.ApprovePlayerMembershipRequestPolicy)]
        public async Task<IActionResult> RejectPlayerMembershipRequest([FromRoute] Guid playerId, [FromRoute] Guid clubId, [FromBody] RejectMembershipRequestRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = RejectPlayerMembershipRequestCommand.Create(playerId, clubId, request);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok();
        }
        #endregion
        
        #endregion
    }
}
