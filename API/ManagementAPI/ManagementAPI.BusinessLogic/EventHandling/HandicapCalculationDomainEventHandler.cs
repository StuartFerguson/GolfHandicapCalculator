namespace ManagementAPI.BusinessLogic.EventHandling
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using HandicapCalculationProcess.DomainEvents;
    using Services;
    using Shared.EventSourcing;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IDomainEventHandler" />
    public class HandicapCalculationDomainEventHandler : IDomainEventHandler
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapCalculationDomainEventHandler"/> class.
        /// </summary>
        /// <param name="handicapCalculationProcessor">The handicap calculation processor.</param>
        /// <param name="eventTypesToSilentlyHandle">The event types to silently handle.</param>
        public HandicapCalculationDomainEventHandler(IHandicapCalculationProcessorService handicapCalculationProcessor,
                                                     IDomainEventTypesToSilentlyHandle eventTypesToSilentlyHandle)
        {
            this.HandicapCalculationProcessor = handicapCalculationProcessor;
            this.EventTypesToSilentlyHandle = eventTypesToSilentlyHandle;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the event types to silently handle.
        /// </summary>
        /// <value>
        /// The event types to silently handle.
        /// </value>
        public IDomainEventTypesToSilentlyHandle EventTypesToSilentlyHandle { get; }

        /// <summary>
        /// Gets the handicap calculation processor.
        /// </summary>
        /// <value>
        /// The handicap calculation processor.
        /// </value>
        public IHandicapCalculationProcessorService HandicapCalculationProcessor { get; }

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
        /// Handles the specific domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task HandleSpecificDomainEvent(HandicapCalculationProcessStartedEvent domainEvent,
                                                     CancellationToken cancellationToken)
        {
            await this.HandicapCalculationProcessor.Start(Guid.NewGuid(), domainEvent.AggregateId, domainEvent.StartedDateTime, cancellationToken);
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

            Logger.LogWarning($"HandicapCalculationDomainEventHandler: No event handler for {domainEvent.GetType()}");

            // Not sure yet if/how we want to handle these events. Handler added so nothing is written to log file to prevent them filling up.
            throw new Exception($"No event handler for {domainEvent.GetType()}");
        }

        #endregion
    }
}