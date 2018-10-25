using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagementAPI.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubConfigurationController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> PostClubConfiguration(CreateClubConfigurationRequest request, CancellationToken cancellationToken)
        {
            return this.StatusCode((Int32) HttpStatusCode.NotImplemented);
        }
    }
}