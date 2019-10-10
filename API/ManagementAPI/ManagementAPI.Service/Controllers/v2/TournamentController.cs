using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Controllers.v2
{
    using System.Security.Claims;
    using System.Threading;
    using BusinessLogic.Commands;
    using BusinessLogic.Manager;
    using Common.v2;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.v2;
    using ManagementAPI.Service.Common;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Shared.CommandHandling;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using CreateTournamentResponse = DataTransferObjects.Responses.v2.CreateTournamentResponse;
    using PlayerCategory = DataTransferObjects.Responses.v2.PlayerCategory;
    using TournamentFormat = DataTransferObjects.Responses.v2.TournamentFormat;

    [Route(TournamentController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class TournamentController : ControllerBase
    {
        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "tournaments";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/golfclubs/{golfClubId}/" + TournamentController.ControllerName;

        #endregion

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
        /// Initializes a new instance of the <see cref="Controllers.TournamentController" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="manager">The manager.</param>
        public TournamentController(ICommandRouter commandRouter, IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
        }

        #endregion

        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(201, type: typeof(CreateTournamentResponse))]
        [SwaggerResponseExample(201, typeof(Common.v2.CreateTournamentResponseExample), jsonConverter: typeof(SwaggerJsonConverter))]
        [Route("")]
        public async Task<IActionResult> CreateTournament([FromRoute] Guid golfClubId,
                                                          [FromBody] CreateTournamentRequest request,
                                                          CancellationToken cancellationToken)
        {
            Guid tournamentId = Guid.NewGuid();

            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            Boolean validationResult = ClaimsHelper.ValidateRouteParameter(golfClubId, golfClubIdClaim);
            if (validationResult == false)
            {
                return this.Forbid();
            }

            // Create the command
            CreateTournamentCommand command = CreateTournamentCommand.Create( tournamentId,Guid.Parse(golfClubIdClaim.Value), request);

            // Route the command
            await this.CommandRouter.Route(command, cancellationToken);

            // return the result
            return this.Created($"{TournamentController.ControllerRoute}/{tournamentId}", new CreateTournamentResponse
                                                                                  {
                                                                                      TournamentId = tournamentId
                                                                                  });
        }

        /// <summary>
        /// Patches the tournament.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="tournamentPatchRequest">The tournament patch request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{tournamentId}")]
        public async Task<IActionResult> PatchTournament([FromRoute] Guid golfClubId, 
                                                         [FromRoute] Guid tournamentId,
                                                         TournamentPatchRequest tournamentPatchRequest,
                                                         CancellationToken cancellationToken)
        {
            // Get the Golf Club Id claim from the user            
            Claim golfClubIdClaim = ClaimsHelper.GetUserClaim(this.User, CustomClaims.GolfClubId, golfClubId.ToString());

            if (tournamentPatchRequest.Status == TournamentStatusUpdate.Complete)
            {
                // Create the command
                CompleteTournamentCommand command = CompleteTournamentCommand.Create(Guid.Parse(golfClubIdClaim.Value), tournamentId);

                // Route the command
                await this.CommandRouter.Route(command, cancellationToken);
            }
            else if (tournamentPatchRequest.Status == TournamentStatusUpdate.Cancel)
            {
                CancelTournamentRequest cancelTournamentRequest = new CancelTournamentRequest
                                                                  {
                                                                      CancellationReason = tournamentPatchRequest.CancellationReason
                                                                  };

                // Create the command
                CancelTournamentCommand command = CancelTournamentCommand.Create(Guid.Parse(golfClubIdClaim.Value), tournamentId, cancelTournamentRequest);

                // Route the command
                await this.CommandRouter.Route(command, cancellationToken);
            }
            else if (tournamentPatchRequest.Status == TournamentStatusUpdate.ProduceResult)
            {
                // Create the command
                ProduceTournamentResultCommand command = ProduceTournamentResultCommand.Create(Guid.Parse(golfClubIdClaim.Value), tournamentId);

                // Route the command
                await this.CommandRouter.Route(command, cancellationToken);
            }
            else
            {
                return this.BadRequest();
            }

            // return the result
            return this.Ok();
        }
    }
}
