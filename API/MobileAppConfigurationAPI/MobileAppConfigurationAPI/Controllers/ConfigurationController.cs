using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MobileAppConfigurationAPI.Controllers
{
    using System.Threading;
    using Database;
    using Database.Models;
    using Microsoft.EntityFrameworkCore;
    using Shared.Exceptions;

    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly Func<MobileConfiguration> MobileConfigurationResolver;

        public ConfigurationController(Func<MobileConfiguration> mobileConfigurationResolver)
        {
            this.MobileConfigurationResolver = mobileConfigurationResolver;
        }

        [HttpGet]
        [Route("/{IMEINumber}")]
        public async Task<IActionResult> GetConfiguration([FromRoute] String IMEINumber,
                                                          CancellationToken cancellationToken)
        {
            using(MobileConfiguration context = this.MobileConfigurationResolver())
            {
                ApplicationConfiguration config = await context.ApplicationConfiguration.SingleOrDefaultAsync(c => c.IMEINumber == IMEINumber, cancellationToken);

                if (config == null)
                {
                    throw new NotFoundException($"No configuration found for IMEI Number {IMEINumber}");
                }

                UrlModel model = new UrlModel();
                model.ManagementApiUrl = config.ManagementApiUri;
                model.SecurityServiceUrl = config.SecurityServiceUri;

                return this.Ok(model);
            }            
        }

        [HttpPost]
        [Route("/{IMEINumber}")]
        public async Task<IActionResult> PostConfiguration([FromRoute] String IMEINumber,
                                                           [FromBody] UrlModel urlModel,
                                                           CancellationToken cancellationToken)
        {
            using(MobileConfiguration context = this.MobileConfigurationResolver())
            {
                ApplicationConfiguration config = await context.ApplicationConfiguration.SingleOrDefaultAsync(c => c.IMEINumber == IMEINumber, cancellationToken);

                if (config != null)
                {
                    throw new InvalidOperationException($"Configuration found for IMEI Number {IMEINumber}, use the PUT method to update the config");
                }

                ApplicationConfiguration newConfig = new ApplicationConfiguration
                                                  {
                                                      IMEINumber = IMEINumber,
                                                      SecurityServiceUri = urlModel.SecurityServiceUrl,
                                                      ManagementApiUri = urlModel.ManagementApiUrl
                                                  };
                await context.ApplicationConfiguration.AddAsync(newConfig, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return this.NoContent();
            }
        }

        [HttpPut]
        [Route("/{IMEINumber}")]
        public async Task<IActionResult> PutConfiguration([FromRoute] String IMEINumber,
                                                          [FromBody] UrlModel urlModel,
                                                           CancellationToken cancellationToken)
        {
            using(MobileConfiguration context = this.MobileConfigurationResolver())
            {
                ApplicationConfiguration config = await context.ApplicationConfiguration.SingleOrDefaultAsync(c => c.IMEINumber == IMEINumber, cancellationToken);

                if (config == null)
                {
                    throw new NotFoundException($"No configuration found for IMEI Number {IMEINumber}");
                }

                config.ManagementApiUri = urlModel.ManagementApiUrl;
                config.SecurityServiceUri = urlModel.SecurityServiceUrl;

                await context.SaveChangesAsync(cancellationToken);

                return this.NoContent();
            }
        }
    }

    public class UrlModel
    {
        public String SecurityServiceUrl { get; set; }

        public String ManagementApiUrl { get; set; }
    }
}