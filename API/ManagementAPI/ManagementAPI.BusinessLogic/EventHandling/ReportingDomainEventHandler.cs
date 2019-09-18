namespace ManagementAPI.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClubMembership.DomainEvents;
    using Manager;
    using Player.DomainEvents;
    using Shared.EventSourcing;
    using Shared.General;
    using Tournament.DomainEvents;

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

            await this.Manager.InsertPlayerHandicapRecordToReporting(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task HandleSpecificDomainEvent(HandicapAdjustedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.UpdatePlayerMembershipToReporting(domainEvent, cancellationToken);

            await this.Manager.UpdatePlayerHandicapRecordToReporting(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="Exception">No event handler for {domainEvent.GetType()}</exception>
        private Task HandleSpecificDomainEvent(DomainEvent domainEvent,
                                               CancellationToken cancellationToken)
        {
            if (this.EventTypesToSilentlyHandle.HandleSilently(this.GetType().Name, domainEvent))
            {
                // Silently handle this.
                return Task.CompletedTask;
            }

            Logger.LogWarning($"ReportingDomainEventHandler: No event handler for {domainEvent.GetType()}");

            // Not sure yet if/how we want to handle these events. Handler added so nothing is written to log file to prevent them filling up.
            throw new Exception($"No event handler for {domainEvent.GetType()}");
        }

        /// <summary>
        /// Handles the specific event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task HandleSpecificDomainEvent(PlayerScorePublishedEvent domainEvent,
                                         CancellationToken cancellationToken)
        {
            await this.Manager.InsertPlayerScoreToReadModel(domainEvent, cancellationToken);
        }
    }
}
