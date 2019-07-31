using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.EventHandling
{
    using System.Threading;
    using Shared.EventSourcing;

    public interface IDomainEventHandler
    {
        /// <summary>
        /// Handles the specified domain event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task Handle(DomainEvent domainEvent,
                    CancellationToken cancellationToken);
    }

    public class PlayerDomainEventHandler : IDomainEventHandler
    {
        public async Task Handle(DomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
        }
    }

    public class TournamentDomainEventHandler : IDomainEventHandler
    {
        public async Task Handle(DomainEvent domainEvent,
                                 CancellationToken cancellationToken)
        {
        }
    }
}
