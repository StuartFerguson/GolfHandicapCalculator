namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using EventHandling;
    using EventStore.ClientAPI.Exceptions;
    using Microsoft.AspNetCore.Mvc;
    using Shared.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class DomainEventController : Controller
    {
        #region Fields

        /// <summary>
        /// The domian event handler resolver
        /// </summary>
        private readonly Func<String, IDomainEventHandler> DomianEventHandlerResolver;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventController" /> class.
        /// </summary>
        /// <param name="domianEventHandlerResolver">The domian event handler resolver.</param>
        public DomainEventController(Func<String, IDomainEventHandler> domianEventHandlerResolver)
        {
            this.DomianEventHandlerResolver = domianEventHandlerResolver;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Posts the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GolfClub")]
        public async Task<IActionResult> PostEventGolfClub([FromBody] DomainEvent @event,
                                                           CancellationToken cancellationToken)
        {
            try
            {
                IDomainEventHandler domainEventHandler = this.DomianEventHandlerResolver("GolfClub");
                await domainEventHandler.Handle(@event, cancellationToken);
            }
            catch(WrongExpectedVersionException)
            {
                return this.BadRequest();
            }

            //TODO: Handle NAK scenarios
            return this.Ok();
        }

        /// <summary>
        /// Posts the event golf club membership.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("GolfClubMembership")]
        public async Task<IActionResult> PostEventGolfClubMembership([FromBody] DomainEvent @event,
                                                                     CancellationToken cancellationToken)
        {
            try
            {
                IDomainEventHandler domainEventHandler = this.DomianEventHandlerResolver("GolfClubMembership");
                await domainEventHandler.Handle(@event, cancellationToken);
            }
            catch(WrongExpectedVersionException)
            {
                return this.BadRequest();
            }

            //TODO: Handle NAK scenarios
            return this.Ok();
        }

        #endregion
    }
}