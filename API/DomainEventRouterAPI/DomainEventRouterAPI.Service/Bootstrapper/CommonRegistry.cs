using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainEventRouterAPI.Service.Bootstrapper
{
    using Common;
    using EventHandling;
    using ManagementAPI.Service.Client;
    using Microsoft.Extensions.Configuration;
    using Services;
    using StructureMap;

    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            Func<String, String> baseAddressResolver = address => { return Startup.Configuration.GetValue<String>($"AppSettings:{address}"); ; };
            this.For<Func<String, String>>().Use(baseAddressResolver);

            this.For<IDomainEventHandler>().Use<PlayerDomainEventHandler>().Named("Player").Singleton();
            this.For<IDomainEventHandler>().Use<GolfClubMembershipDomainEventHandler>().Named("GolfClubMembership").Singleton();
            this.For<IDomainEventHandler>().Use<TournamentDomainEventHandler>().Named("Tournament").Singleton();

            Func<String, IDomainEventHandler> domainEventHanderFunc = (name) =>
                                                                      {
                                                                          return Startup.Container.GetInstance<IDomainEventHandler>(name);
                                                                      };
            this.For<Func<String, IDomainEventHandler>>().Use(domainEventHanderFunc);

            this.For<ISecurityServiceClient>().Use<SecurityServiceClient>().Singleton();
            this.For<IGolfClubClient>().Use<GolfClubClient>().Singleton();
            this.For<IPlayerClient>().Use<PlayerClient>().Singleton();
            this.For<IElasticService>().Use<ElasticService>().Singleton();
            this.For<IModelFactory>().Use<ModelFactory>().Singleton();


        }
    }
}
