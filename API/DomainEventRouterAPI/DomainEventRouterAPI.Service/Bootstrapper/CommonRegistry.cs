using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.Bootstrapper
{
    using EventHandling;
    using StructureMap;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            For<IDomainEventHandler>().Use<PlayerDomainEventHandler>().Named("Player");
            For<IDomainEventHandler>().Use<GolfClubMembershipDomainEventHandler>().Named("GolfClubMembership");
            For<IDomainEventHandler>().Use<TournamentDomainEventHandler>().Named("Tournament");

            Func<String, IDomainEventHandler> domainEventHanderFunc = (name) => Startup.Container.GetInstance<IDomainEventHandler>(name);
        }
    }
}
