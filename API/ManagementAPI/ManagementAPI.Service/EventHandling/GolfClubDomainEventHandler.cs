namespace ManagementAPI.Service.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClub.DomainEvents;
    using Manager;
    using Shared.EventSourcing;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagementAPI.Service.EventHandling.IDomainEventHandler" />
    public class GolfClubDomainEventHandler : IDomainEventHandler
    {
        #region Fields

        /// <summary>
        /// The event types to silently handle
        /// </summary>
        private readonly IDomainEventTypesToSilentlyHandle EventTypesToSilentlyHandle;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubDomainEventHandler" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="eventTypesToSilentlyHandle">The event types to silently handle.</param>
        public GolfClubDomainEventHandler(IManagmentAPIManager manager,
                                          IDomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle)
        {
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
        private async Task HandleSpecificDomainEvent(GolfClubCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.InsertGolfClubToReadModel(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleSpecificDomainEvent(GolfClubAdministratorSecurityUserCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.InsertUserRecordToReadModel(domainEvent, cancellationToken);
        }

        /// <summary>
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleSpecificDomainEvent(MatchSecretarySecurityUserCreatedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.Manager.InsertUserRecordToReadModel(domainEvent, cancellationToken);
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

            Logger.LogWarning($"GolfClubDomainEventHandler: No event handler for {domainEvent.GetType()}");

            // Not sure yet if/how we want to handle these events. Handler added so nothing is written to log file to prevent them filling up.
            throw new Exception($"No event handler for {domainEvent.GetType()}");
        }

        #endregion
    }
}