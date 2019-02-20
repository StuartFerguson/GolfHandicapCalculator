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

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentController"/> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        public TournamentController(ICommandRouter commandRouter)
        {
            this.CommandRouter = commandRouter;
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
        [SwaggerResponse(200, type:typeof(CreateTournamentResponse))]
        [SwaggerResponseExample(200, typeof(CreateTournamentResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        [Authorize(Policy = PolicyNames.CreateTournamentPolicy)]
        public async Task<IActionResult> PostTournament([FromBody] CreateTournamentRequest request,
                                                        CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId);

            // Create the command
            CreateTournamentCommand command = CreateTournamentCommand.Create(Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok(command.Response);
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
        [Route("{tournamentId}/RecordMemberScore")]
        [Authorize(Policy = PolicyNames.RecordPlayerScoreForTournamentPolicy)]
        public async Task<IActionResult> PutTournament([FromRoute] Guid tournamentId,
                                                       [FromBody] RecordMemberTournamentScoreRequest request,
                                                       CancellationToken cancellationToken)
        {
            // Create the command
            RecordMemberTournamentScoreCommand command = RecordMemberTournamentScoreCommand.Create(tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok(command.Response);
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
        [Route("{tournamentId}/Complete")]
        [Authorize(Policy = PolicyNames.CompleteTournamentPolicy)]
        public async Task<IActionResult> PutTournament([FromRoute] Guid tournamentId,
                                                       CancellationToken cancellationToken)
        {
            // Create the command
            CompleteTournamentCommand command = CompleteTournamentCommand.Create(tournamentId);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok(command.Response);
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
        public async Task<IActionResult> PutTournament([FromRoute] Guid tournamentId,
                                                       [FromBody] CancelTournamentRequest request,
                                                       CancellationToken cancellationToken)
        {
            // Create the command
            CancelTournamentCommand command = CancelTournamentCommand.Create(tournamentId, request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok(command.Response);
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
        public async Task<IActionResult> PutTournamentProduceResult([FromRoute] Guid tournamentId,
                                                                    CancellationToken cancellationToken)
        {
            // Create the command
            ProduceTournamentResultCommand command = ProduceTournamentResultCommand.Create(tournamentId);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Ok(command.Response);
        }

        #endregion
    }
}