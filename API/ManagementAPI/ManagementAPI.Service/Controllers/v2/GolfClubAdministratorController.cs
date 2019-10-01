using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAPI.Service.Controllers.v2
{
    using System.Threading;
    using BusinessLogic.Manager;
    using DataTransferObjects.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    
    [Route(GolfClubAdministratorController.ControllerRoute)]
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    public class GolfClubAdministratorController : ControllerBase
    {
        private readonly IManagmentAPIManager Manager;

        public GolfClubAdministratorController(IManagmentAPIManager manager)
        {
            this.Manager = manager;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> PostGolfClubAdministrator([FromBody] RegisterClubAdministratorRequest registerClubAdministratorRequest, CancellationToken cancellationToken)
        {
            Guid golfclubadministratorId = await this.Manager.RegisterClubAdministrator(registerClubAdministratorRequest, cancellationToken);

            return this.CreatedAtRoute($"{ControllerRoute}/{golfclubadministratorId}", null);
        }

        [HttpGet]
        [Route("{golfclubadministratorId}")]
        public async Task<IActionResult> GetGolfClubAdministrator([FromRoute] Guid golfclubadministratorId,
                                                                       CancellationToken cancellationToken)
        {
            return this.NotImplemented();
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetGolfClubAdministratorList(CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        [HttpPut]
        [Route("{golfclubadministratorId}")]
        public async Task<IActionResult> PutGolfClubAdministrator([FromRoute] Guid golfclubadministratorId, CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        [HttpPatch]
        [Route("{golfclubadministratorId}")]
        public async Task<IActionResult> PatchGolfClubAdministrator([FromRoute] Guid golfclubadministratorId, CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        [HttpDelete]
        [Route("{golfclubadministratorId}")]
        public async Task<IActionResult> DeleteGolfClubAdministrator([FromRoute] Guid golfclubadministratorId, CancellationToken cancellationToken)
        {
            return this.MethodNotAllowed();
        }

        /// <summary>
        /// The controller name
        /// </summary>
        public const String ControllerName = "golfclubadministrators";

        /// <summary>
        /// The controller route
        /// </summary>
        private const String ControllerRoute = "api/" + GolfClubAdministratorController.ControllerName;
    }
}