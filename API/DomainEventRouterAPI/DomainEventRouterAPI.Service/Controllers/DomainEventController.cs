using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using EventHandling;
    using EventStore.ClientAPI.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Shared.EventSourcing;

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

        [HttpPost]
        [Route("GolfClubMembership")]
        public async Task<IActionResult> PostEventMembership([FromBody] DomainEvent @event,
                                                           CancellationToken cancellationToken)
        {
            try
            {
                IDomainEventHandler domainEventHandler = this.DomainEventHandlerResolver("GolfClubMembership");
                await domainEventHandler.Handle(@event, cancellationToken);
            }
            catch (WrongExpectedVersionException)
            {
                return this.BadRequest();
            }

            //TODO: Handle NAK scenarios
            return this.Ok();
        }
    }
}
