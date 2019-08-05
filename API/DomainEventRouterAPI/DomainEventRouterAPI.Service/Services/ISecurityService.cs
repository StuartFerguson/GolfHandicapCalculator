using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.Services
{
    using System.Threading;

    public interface ISecurityServiceClient
    {
        Task<String> GetClientToken(String clientId,
                              String clientSecret,
                              CancellationToken cancellationToken,
                              List<String> scopes = null);
    }
}
