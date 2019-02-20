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
    public class PlayerController : ControllerBase
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
        public PlayerController(ICommandRouter commandRouter,
                                IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the player memberships.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type:typeof(List<ClubMembershipResponse>))]
        [SwaggerResponseExample(200, typeof(ClubMembershipListResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Route("Memberships")]
        public async Task<IActionResult> GetPlayerMemberships(CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId);

            List<ClubMembershipResponse> membershipList = await this.Manager.GetPlayersClubMemberships(Guid.Parse(playerIdClaim.Value), cancellationToken);

            return this.Ok(membershipList);
        }

        /// <summary>
        /// Posts the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, type:typeof(RegisterPlayerResponse))]
        [SwaggerResponseExample(200, typeof(RegisterPlayerResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> PostPlayer([FromBody] RegisterPlayerRequest request,
                                                    CancellationToken cancellationToken)
        {
            // Create the command
            RegisterPlayerCommand command = RegisterPlayerCommand.Create(request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok(command.Response);
        }

        #endregion
    }
}