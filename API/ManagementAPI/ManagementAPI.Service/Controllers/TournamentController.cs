namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Commands;
    using BusinessLogic.Manager;
    using Common;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    [Route("api/golfClub/{golfClubId}/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    [Authorize]
    [ApiVersion("1.0")]
    public class TournamentController : ControllerBase
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
        /// Initializes a new instance of the <see cref="TournamentController" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="manager">The manager.</param>
        public TournamentController(ICommandRouter commandRouter, IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(201, type:typeof(CreateTournamentResponse))]
        [SwaggerResponseExample(201, typeof(CreateTournamentResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> CreateTournament([FromRoute] Guid golfClubId, [FromBody] CreateTournamentRequest request,
                                                        CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            CreateTournamentCommand command = CreateTournamentCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created(String.Empty, command.Response);
        }



        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/Complete")]
        public async Task<IActionResult> Complete([FromRoute] Guid golfClubId, 
                                                  [FromRoute] Guid tournamentId,
                                                  CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            CompleteTournamentCommand command = CompleteTournamentCommand.Create(Guid.Parse(golfClubIdClaim.Value), tournamentId);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/Cancel")]
        public async Task<IActionResult> Cancel([FromRoute] Guid golfClubId, 
                                                [FromRoute] Guid tournamentId,
                                                       [FromBody] CancelTournamentRequest request,
                                                       CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            CancelTournamentCommand command = CancelTournamentCommand.Create(Guid.Parse(golfClubIdClaim.Value), tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/ProduceResult")]
        public async Task<IActionResult> ProduceResult([FromRoute] Guid golfClubId, 
                                                       [FromRoute] Guid tournamentId,
                                                                    CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            ProduceTournamentResultCommand command = ProduceTournamentResultCommand.Create(Guid.Parse(golfClubIdClaim.Value), tournamentId);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }
        
        [HttpGet]
        [SwaggerResponse(200, type:typeof(GetTournamentListResponse))]
        [SwaggerResponseExample(200, typeof(GetTournamentListResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("List")]
        public async Task<IActionResult> GetTournamentList([FromRoute] Guid golfClubId, CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Get the data 
            GetTournamentListResponse tournamentList = await this.Manager.GetTournamentList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(tournamentList);
        }
        #endregion
    }
}