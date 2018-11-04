using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Manager;
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

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationController"/> class.
        /// </summary>
        /// <param name="commmandRouter">The commmand router.</param>
        public ClubConfigurationController(ICommandRouter commmandRouter,IManagmentAPIManager manager)
        {
            this.CommmandRouter = commmandRouter;
            this.Manager = manager;
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
        [ProducesResponseType(typeof(CreateClubConfigurationResponse), 200)]
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

        #region public async Task<IActionResult> GetClubConfiguration([FromQuery] Guid clubId,CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetClubConfigurationResponse), 200)]
        public async Task<IActionResult> GetClubConfiguration([FromQuery] Guid clubId,CancellationToken cancellationToken)
        {
            var clubConfiguration = await this.Manager.GetClubConfiguration(clubId, cancellationToken);

            return this.Ok(clubConfiguration);
        }
        #endregion

        #region public async Task<IActionResult> PutClubConfiguration(AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Puts the club configuration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> PutClubConfiguration(AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)
        {
            // Create the command
            var command = AddMeasuredCourseToClubCommand.Create(request);

            // Route the command
            await this.CommmandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #endregion
    }
}