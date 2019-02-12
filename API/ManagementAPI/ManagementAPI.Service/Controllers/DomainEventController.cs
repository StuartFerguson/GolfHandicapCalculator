using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI.Exceptions;
using ManagementAPI.GolfClub.DomainEvents;
using ManagementAPI.GolfClubMembership.DomainEvents;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Manager;
using Microsoft.AspNetCore.Mvc;
using Shared.CommandHandling;
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

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomainEventController" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="commandRouter">The command router.</param>
        public DomainEventController(IManagmentAPIManager manager,ICommandRouter commandRouter)
        {
            this.Manager = manager;
            this.CommandRouter = commandRouter;
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

        #region private async Task HandleEvent(GolfClubCreatedEvent domainEvent, CancellationToken cancellationToken)        
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(GolfClubCreatedEvent domainEvent, CancellationToken cancellationToken)
        {
            await this.Manager.InsertGolfClubToReadModel(domainEvent, cancellationToken);
        }
        #endregion

        #region private async Task HandleEvent(ClubMembershipRequestAcceptedEvent domainEvent, CancellationToken cancellationToken)
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(ClubMembershipRequestAcceptedEvent domainEvent, CancellationToken cancellationToken)
        {
            AddAcceptedMembershipToPlayerCommand command = AddAcceptedMembershipToPlayerCommand.Create(domainEvent.PlayerId, 
                domainEvent.AggregateId,
                domainEvent.MembershipId, domainEvent.MembershipNumber, domainEvent.AcceptedDateAndTime);

            await this.CommandRouter.Route(command, cancellationToken);
        }
        #endregion

        #region private async Task HandleEvent(ClubMembershipRequestRejectedEvent domainEvent, CancellationToken cancellationToken)
        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleEvent(ClubMembershipRequestRejectedEvent domainEvent, CancellationToken cancellationToken)
        {
            AddRejectedMembershipToPlayerCommand command = AddRejectedMembershipToPlayerCommand.Create(
                domainEvent.PlayerId,
                domainEvent.AggregateId, domainEvent.MembershipId, domainEvent.RejectionReason,
                domainEvent.RejectionDateAndTime);

            await this.CommandRouter.Route(command, cancellationToken);
        }
        #endregion

        #endregion
    }
}
