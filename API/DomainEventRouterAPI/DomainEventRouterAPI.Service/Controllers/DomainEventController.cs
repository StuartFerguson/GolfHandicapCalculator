using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using EventHandling;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class DomainEventController : Controller
    {
        private readonly Func<String, IDomainEventHandler> DomainEventHandlerResolver;

        public DomainEventController(Func<String, IDomainEventHandler> domainEventHandlerResolver)
        {
            this.DomainEventHandlerResolver = domainEventHandlerResolver;
        }
    }
}
