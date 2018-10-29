using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class ClubConfigurationController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The commmand router
        /// </summary>
        private readonly ICommandRouter CommmandRouter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationController"/> class.
        /// </summary>
        /// <param name="commmandRouter">The commmand router.</param>
        public ClubConfigurationController(ICommandRouter commmandRouter)
        {
            this.CommmandRouter = commmandRouter;
        }

        #endregion

        #region Public Methods

        #region public async Task<IActionResult> PostClubConfiguration(CreateClubConfigurationRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Posts the club configuration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostClubConfiguration([FromBody]CreateClubConfigurationRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = CreateClubConfigurationCommand.Create(request);

            // Route the command
            await this.CommmandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #endregion
    }
}