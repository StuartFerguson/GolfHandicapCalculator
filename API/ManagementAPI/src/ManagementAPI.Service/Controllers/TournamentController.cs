using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.CommandHandling;

namespace ManagementAPI.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The commmand router
        /// </summary>
        private readonly ICommandRouter CommmandRouter;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentController"/> class.
        /// </summary>
        /// <param name="commmandRouter">The commmand router.</param>
        public TournamentController(ICommandRouter commmandRouter)
        {
            this.CommmandRouter = commmandRouter;
        }

        #endregion

        #region Public Methods

        #region public async Task<IActionResult> PostTournament([FromBody]CreateTournamentRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CreateTournamentResponse), 200)]
        public async Task<IActionResult> PostTournament([FromBody]CreateTournamentRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = CreateTournamentCommand.Create(request);

            // Route the command
            await this.CommmandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> PostTournament([FromRoute] Guid tournamentId, [FromBody]RecordMemberTournamentScoreRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]      
        [ProducesResponseType(204)]
        [Route("{tournamentId}/RecordMemberScore")]
        public async Task<IActionResult> PutTournament([FromRoute] Guid tournamentId, [FromBody]RecordMemberTournamentScoreRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = RecordMemberTournamentScoreCommand.Create(tournamentId, request);

            // Route the command
            await this.CommmandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> PostTournament([FromRoute] Guid tournamentId, CancellationToken cancellationToken)        
        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]      
        [ProducesResponseType(204)]
        [Route("{tournamentId}/Complete")]
        public async Task<IActionResult> PutTournament([FromRoute] Guid tournamentId, CancellationToken cancellationToken)
        {
            // Create the command
            var command = CompleteTournamentCommand.Create(tournamentId);

            // Route the command
            await this.CommmandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> PostTournament([FromRoute] Guid tournamentId, [FromBody]CancelTournamentRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Posts the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]      
        [ProducesResponseType(204)]
        [Route("{tournamentId}/Cancel")]
        public async Task<IActionResult> PutTournament([FromRoute] Guid tournamentId, [FromBody]CancelTournamentRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = CancelTournamentCommand.Create(tournamentId, request);

            // Route the command
            await this.CommmandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #endregion
    }
}