namespace DomainEventRouterAPI.Service.EventHandling
{
    using System.Threading;
    using System.Threading.Tasks;
    using Shared.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DomainEventRouterAPI.Service.EventHandling.IDomainEventHandler" />
    public class PlayerDomainEventHandler : IDomainEventHandler
    {
        #region Methods

        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task Handle(DomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
        }

        #endregion
    }
}