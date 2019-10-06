namespace ManagementAPI.Service.Controllers.v2
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manager;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses.v2;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route(GolfClubAdministratorController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class GolfClubAdministratorController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAdministratorController" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public GolfClubAdministratorController(IManagmentAPIManager manager)
        {
            this.Manager = manager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes the golf club administrator.
        /// </summary>
        /// <param name="golfclubadministratorId">The golfclubadministrator identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{golfclubadministratorId}")]
        [SwaggerResponse(405)]
        public async Task<IActionResult> DeleteGolfClubAdministrator([FromRoute] Guid golfclubadministratorId,
                                                                     CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        /// <summary>
        /// Gets the golf club administrator.
        /// </summary>
        /// <param name="golfclubadministratorId">The golfclubadministrator identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{golfclubadministratorId}")]
        [SwaggerResponse(501)]
        public async Task<IActionResult> GetGolfClubAdministrator([FromRoute] Guid golfclubadministratorId,
                                                                  CancellationToken cancellationToken)
        {
            return this.NotImplemented();
        }

        /// <summary>
        /// Gets the golf club administrator list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(405)]
        public async Task<IActionResult> GetGolfClubAdministratorList(CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        /// <summary>
        /// Patches the golf club administrator.
        /// </summary>
        /// <param name="golfclubadministratorId">The golfclubadministrator identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("{golfclubadministratorId}")]
        [SwaggerResponse(405)]
        public async Task<IActionResult> PatchGolfClubAdministrator([FromRoute] Guid golfclubadministratorId,
                                                                    CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        /// <summary>
        /// Posts the golf club administrator.
        /// </summary>
        /// <param name="registerClubAdministratorRequest">The register club administrator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        [SwaggerResponse(201, type:typeof(RegisterClubAdministratorResponseExample))]
        [SwaggerResponseExample(201, typeof(RegisterClubAdministratorResponseExample), jsonConverter:typeof(SwaggerJsonConverter))]
        public async Task<IActionResult> PostGolfClubAdministrator([FromBody] RegisterClubAdministratorRequest registerClubAdministratorRequest,
                                                                   CancellationToken cancellationToken)
        {
            Guid golfClubAdministratorId = await this.Manager.RegisterClubAdministrator(registerClubAdministratorRequest, cancellationToken);

            return this.Created($"api/{GolfClubAdministratorController.ControllerName}/{golfClubAdministratorId}",
                                new RegisterClubAdministratorResponse
                                {
                                    GolfClubAdministratorId = golfClubAdministratorId
                                });
        }

        /// <summary>
        /// Puts the golf club administrator.
        /// </summary>
        /// <param name="golfclubadministratorId">The golfclubadministrator identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{golfclubadministratorId}")]
        [SwaggerResponse(405)]
        public async Task<IActionResult> PutGolfClubAdministrator([FromRoute] Guid golfclubadministratorId,
                                                                  CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        #endregion

        #region Others

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "golfclubadministrators";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + GolfClubAdministratorController.ControllerName;

        #endregion
    }
}