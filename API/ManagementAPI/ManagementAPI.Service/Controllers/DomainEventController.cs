using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI.Exceptions;
using ManagementAPI.ClubConfiguration.DomainEvents;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.Manager;
using Microsoft.AspNetCore.Mvc;
using Shared.EventSourcing;

namespace ManagementAPI.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class DomainEventController : Controller
    {
        #region Fields

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventController"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public DomainEventController(IManagmentAPIManager manager)
        {
            this.Manager = manager;
        }
        #endregion

        #region Public Methods

        #region public async Task<IActionResult> PostEvent([FromBody]DomainEvent @event, CancellationToken cancellationToken)
        /// <summary>
        /// Posts the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody]DomainEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                //Handle incoming events
                await this.HandleEvent((dynamic)@event, cancellationToken);
            }
            catch (WrongExpectedVersionException)
            {
                return BadRequest();
            }

            //TODO: Handle NAK scenarios
            return this.Ok();
        }
        #endregion

        #endregion

        #region Private Methods

        #region private async Task HandleEvent(ClubConfigurationCreatedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(ClubConfigurationCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.Manager.InsertClubInformationToReadModel(domainEvent, cancellationToken);
        }
        #endregion

        #region private async Task HandleEvent(ClubMembershipRequestedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(ClubMembershipRequestedEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.Manager.InsertClubMembershipRequestToReadModel(domainEvent, cancellationToken);
        }
        #endregion

        #region private async Task HandleEvent(ClubMembershipApprovedEvent domainEvent, CancellationToken cancellationToken)
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(ClubMembershipApprovedEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.Manager.RemoveClubMembershipRequestFromReadModel(domainEvent, cancellationToken);
        }
        #endregion

        #region private async Task HandleEvent(ClubMembershipRejectedEvent domainEvent, CancellationToken cancellationToken)
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(ClubMembershipRejectedEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.Manager.RemoveClubMembershipRequestFromReadModel(domainEvent, cancellationToken);
        }
        #endregion

        #endregion
    }
}
