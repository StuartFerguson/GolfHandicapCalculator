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
        
        #endregion
    }
}
