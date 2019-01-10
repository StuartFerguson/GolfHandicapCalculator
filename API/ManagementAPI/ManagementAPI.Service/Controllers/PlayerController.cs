using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
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

        #region public async Task<IActionResult> RequestClubMembership([FromRoute] Guid clubId, CancellationToken cancellationToken)
        /// <summary>
        /// Requests the club membership for player.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("ClubMembershipRequest/{clubId}")]
        [Authorize(Policy = PolicyNames.RequestGolfClubMembershipForPlayerPolicy)]
        public async Task<IActionResult> RequestClubMembershipForPlayer([FromRoute] Guid clubId, CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId);

            // Create the command
            var command = PlayerClubMembershipRequestCommand.Create(Guid.Parse(playerIdClaim.Value), clubId);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok();
        }
        #endregion

        #region public async Task<IActionResult> ApprovePlayerMembershipRequest([FromRoute] Guid playerId, CancellationToken cancellationToken)        
        /// <summary>
        /// Approves the player membership request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{playerId}/ClubMembershipRequest/Approve")]
        [Authorize(Policy = PolicyNames.ApprovePlayerMembershipRequestPolicy)]
        public async Task<IActionResult> ApprovePlayerMembershipRequest([FromRoute] Guid playerId, CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            var command = ApprovePlayerMembershipRequestCommand.Create(playerId, Guid.Parse(golfClubIdClaim.Value));

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok();
        }
        #endregion

        #region public async Task<IActionResult> RejectPlayerMembershipRequest([FromRoute] Guid playerId, [FromBody] RejectMembershipRequestRequest request, CancellationToken cancellationToken)
        /// <summary>
        /// Approves the player membership request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{playerId}/ClubMembershipRequest/Reject")]
        //[Authorize(Policy = PolicyNames.ApprovePlayerMembershipRequestPolicy)]
        public async Task<IActionResult> RejectPlayerMembershipRequest([FromRoute] Guid playerId, [FromBody] RejectMembershipRequestRequest request, CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            var command = RejectPlayerMembershipRequestCommand.Create(playerId, Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command,cancellationToken);

            // return the result
            return this.Ok();
        }
        #endregion
        
        #endregion
    }
}
