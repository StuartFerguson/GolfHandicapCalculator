using System;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.EventHandling
{
    using Shared.EventSourcing;

    public interface IDomainEventTypesToSilentlyHandle
    {
        #region Methods

        /// <summary>
        /// Handles the silently.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
        /// <param name="domainEvent">The domain event.</param>
        /// <returns></returns>
        Boolean HandleSilently(String handlerName,
                               DomainEvent domainEvent);

        #endregion
    }
}
