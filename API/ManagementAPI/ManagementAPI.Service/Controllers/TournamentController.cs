namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using Common;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
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
    public class TournamentController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentController"/> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
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
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(201, type:typeof(CreateTournamentResponse))]
        [SwaggerResponseExample(201, typeof(CreateTournamentResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.CreateTournamentPolicy)]
        public async Task<IActionResult> CreateTournament([FromBody] CreateTournamentRequest request,
                                                        CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

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
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/RecordPlayerScore")]
        [Authorize(Policy = PolicyNames.RecordPlayerScoreForTournamentPolicy)]
        public async Task<IActionResult> RecordPlayerScore([FromRoute] Guid tournamentId,
                                                       [FromBody] RecordPlayerTournamentScoreRequest request,
                                                       CancellationToken cancellationToken)
        {
            // Get the Player Id claim from the user            
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId);

            // Create the command
            RecordPlayerTournamentScoreCommand command = RecordPlayerTournamentScoreCommand.Create( Guid.Parse(playerIdClaim.Value), tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/Complete")]
        [Authorize(Policy = PolicyNames.CompleteTournamentPolicy)]
        public async Task<IActionResult> Complete([FromRoute] Guid tournamentId,
                                                       CancellationToken cancellationToken)
        {
            // Create the command
            CompleteTournamentCommand command = CompleteTournamentCommand.Create(tournamentId);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/Cancel")]
        [Authorize(Policy = PolicyNames.CancelTournamentPolicy)]
        public async Task<IActionResult> Cancel([FromRoute] Guid tournamentId,
                                                       [FromBody] CancelTournamentRequest request,
                                                       CancellationToken cancellationToken)
        {
            // Create the command
            CancelTournamentCommand command = CancelTournamentCommand.Create(tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/ProduceResult")]
        [Authorize(Policy = PolicyNames.ProduceTournamentResultPolicy)]
        public async Task<IActionResult> ProduceResult([FromRoute] Guid tournamentId,
                                                                    CancellationToken cancellationToken)
        {
            // Create the command
            ProduceTournamentResultCommand command = ProduceTournamentResultCommand.Create(tournamentId);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        [HttpPut]
        [SwaggerResponse(204)]
        [Route("{tournamentId}/SignUp")]
        [Authorize(Policy = PolicyNames.PlayerTournamentSignUpPolicy)]
        public async Task<IActionResult> SignupPlayer([FromRoute] Guid tournamentId,
                                                      CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user
            Claim playerIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.PlayerId);

            // Create the command
            SignUpForTournamentCommand command = SignUpForTournamentCommand.Create(tournamentId, Guid.Parse(playerIdClaim.Value));

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.NoContent();
        }

        [HttpGet]
        [SwaggerResponse(200, type:typeof(GetTournamentListResponse))]
        [SwaggerResponseExample(200, typeof(GetTournamentListResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("List")]
        [Authorize(Policy = PolicyNames.GetTournamentListPolicy)]
        public async Task<IActionResult> GetTournamentList(CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Get the data 
            GetTournamentListResponse tournamentList = await this.Manager.GetTournamentList(Guid.Parse(golfClubIdClaim.Value), cancellationToken);

            return this.Ok(tournamentList);
        }
        #endregion
    }
}