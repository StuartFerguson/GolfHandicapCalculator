namespace ManagementAPI.Service.Controllers
{
    using Commands;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Manager;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;

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

        [HttpGet]
        [SwaggerResponse(200, type: typeof(GetPlayerDetailsResponse))]
        [SwaggerResponseExample(200, typeof(GetPlayerDetailsResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("{playerId}")]
        public async Task<IActionResult> GetPlayer([FromRoute] Guid playerId, CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            GetPlayerDetailsResponse player = await this.Manager.GetPlayerDetails(Guid.Parse(playerIdClaim.Value), cancellationToken);

            return this.Ok(player);
        }

        /// <summary>
        /// Gets the player memberships.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type: typeof(List<ClubMembershipResponse>))]
        [SwaggerResponseExample(200, typeof(ClubMembershipListResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("{playerId}/Memberships")]
        public async Task<IActionResult> GetPlayerMemberships([FromRoute] Guid playerId, CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

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
        [SwaggerResponse(201, type: typeof(RegisterPlayerResponse))]
        [SwaggerResponseExample(201, typeof(RegisterPlayerResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [AllowAnonymous]
        public async Task<IActionResult> PostPlayer([FromBody] RegisterPlayerRequest request,
                                                    CancellationToken cancellationToken)
        {
            // Create the command
            RegisterPlayerCommand command = RegisterPlayerCommand.Create(request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created(String.Empty, command.Response);
        }

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{playerId}/GolfClub/{golfClubId}/RequestMembership")]
        [SwaggerResponse(204)]
        public async Task<IActionResult> RequestClubMembership([FromRoute] Guid playerId, 
                                                               [FromRoute] Guid golfClubId,
                                                               CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            RequestClubMembershipCommand command = RequestClubMembershipCommand.Create(Guid.Parse(playerIdClaim.Value), golfClubId);

            //Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{playerId}/Tournament/{tournamentId}/RecordScore")]
        public async Task<IActionResult> RecordPlayerScore([FromRoute] Guid playerId,
                                                           [FromRoute] Guid tournamentId,
                                                           [FromBody] RecordPlayerTournamentScoreRequest request,
                                                           CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId);

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(playerId, playerIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            RecordPlayerTournamentScoreCommand command = RecordPlayerTournamentScoreCommand.Create(Guid.Parse(playerIdClaim.Value), tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{playerId}/Tournament/{tournamentId}/SignUp")]
        public async Task<IActionResult> SignupPlayer([FromRoute] Guid playerId,
                                                      [FromRoute] Guid tournamentId,
                                                      CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId, playerId);

            // Create the command
            SignUpForTournamentCommand command = SignUpForTournamentCommand.Create(tournamentId, Guid.Parse(playerIdClaim.Value));

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(200, type: typeof(List<GetGolfClubResponse>))]
        [SwaggerResponseExample(200, typeof(GetGolfClubListResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("GolfClubList")]
        public async Task<IActionResult> GetGolfClubList(CancellationToken cancellationToken)
        {
            // TODO: Revisit this one :| not sure where this lives, only required by the player as golf clubs cant see each other :(

            List<GetGolfClubResponse> golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            return this.Ok(golfClubList);
        }

        #endregion
    }
}