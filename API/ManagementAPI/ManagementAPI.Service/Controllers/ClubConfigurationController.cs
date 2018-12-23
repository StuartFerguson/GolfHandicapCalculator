using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
    [ExcludeFromCodeCoverage]
    public class ClubConfigurationController : ControllerBase
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
        /// Initializes a new instance of the <see cref="ClubConfigurationController"/> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        public ClubConfigurationController(ICommandRouter commandRouter,IManagmentAPIManager manager)
        {
            this.CommandRouter = commandRouter;
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
            await this.CommandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #region public async Task<IActionResult> GetClubConfiguration([FromQuery] Guid clubId,CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="clubConfigurationId">The clubconfiguration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetClubConfigurationResponse), 200)]
        [Route("{clubConfigurationId}")]
        public async Task<IActionResult> GetClubConfiguration([FromRoute] Guid clubConfigurationId,CancellationToken cancellationToken)
        {
            var clubConfiguration = await this.Manager.GetClubConfiguration(clubConfigurationId, cancellationToken);

            return this.Ok(clubConfiguration);
        }
        #endregion

        #region public async Task<IActionResult> GetClubConfigurationList(CancellationToken cancellationToken)              
        /// <summary>
        /// Gets the club configuration list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<GetClubConfigurationResponse>), 200)]
        public async Task<IActionResult> GetClubConfigurationList(CancellationToken cancellationToken)
        {
            var clubConfiguration = await this.Manager.GetClubList(cancellationToken);

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
            await this.CommandRouter.Route(command,CancellationToken.None);

            // return the result
            return this.Ok(command.Response);
        }
        #endregion

        #endregion
    }
}