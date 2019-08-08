namespace ManagementAPI.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClubMembership.DomainEvents;
    using Manager;
    using Shared.CommandHandling;
    using Shared.EventSourcing;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IDomainEventHandler" />
    public class GolfClubMembershipDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The command router
        /// </summary>
        private readonly ICommandRouter CommandRouter;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        /// <summary>
        /// The event types to silently handle
        /// </summary>
        private readonly IDomainEventTypesToSilentlyHandle EventTypesToSilentlyHandle;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubMembershipDomainEventHandler" /> class.
        /// </summary>
        /// <param name="commandRouter">The command router.</param>
        /// <param name="manager">The manager.</param>
        /// <param name="eventTypesToSilentlyHandle">The event types to silently handle.</param>
        public GolfClubMembershipDomainEventHandler(ICommandRouter commandRouter,
                                                    IManagmentAPIManager manager,
                                                    IDomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle)
        {
            this.CommandRouter = commandRouter;
            this.Manager = manager;
            this.EventTypesToSilentlyHandle = eventTypesToSilentlyHandle;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
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
            await this.Manager.InsertPlayerMembershipToReadModel(domainEvent, cancellationToken);

            Logger.LogDebug($"Added Player Id {domainEvent.PlayerId} to Golf Club {domainEvent.AggregateId}");
        }

        /// <summary>
        /// Handles the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleSpecificDomainEvent(ClubMembershipRequestRejectedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.InsertPlayerMembershipToReadModel(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No event handler for {domainEvent.GetType()}</exception>
        private Task HandleSpecificDomainEvent(DomainEvent domainEvent,
                                               CancellationToken cancellationToken)
        {
            if (this.EventTypesToSilentlyHandle.HandleSilently(this.GetType().Name, domainEvent))
            {
                // Silently handle this.
                return Task.CompletedTask;
            }

            Logger.LogWarning($"GolfClubMembershipDomainEventHandler: No event handler for {domainEvent.GetType()}");

            // Not sure yet if/how we want to handle these events. Handler added so nothing is written to log file to prevent them filling up.
            throw new Exception($"No event handler for {domainEvent.GetType()}");
        }

        #endregion
    }
}