namespace ManagementAPI.BusinessLogic.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClubMembership.DomainEvents;
    using Manager;
    using Player.DomainEvents;
    using Shared.EventSourcing;

    public class ReportingDomainEventHandler :IDomainEventHandler
    {
        /// <summary>
        /// The event types to silently handle
        /// </summary>
        private readonly IDomainEventTypesToSilentlyHandle EventTypesToSilentlyHandle;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        public ReportingDomainEventHandler(IManagmentAPIManager manager,
                                          IDomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle)
        {
            this.Manager = manager;
            this.EventTypesToSilentlyHandle = eventTypesToSilentlyHandle;
        }

        public async Task Handle(DomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
            await this.HandleSpecificDomainEvent((dynamic)domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleSpecificDomainEvent(ClubMembershipRequestAcceptedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.InsertPlayerMembershipToReporting(domainEvent, cancellationToken);
        }

        private async Task HandleSpecificDomainEvent(HandicapAdjustedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.UpdatePlayerMembershipToReporting(domainEvent, cancellationToken);
        }
    }
}
