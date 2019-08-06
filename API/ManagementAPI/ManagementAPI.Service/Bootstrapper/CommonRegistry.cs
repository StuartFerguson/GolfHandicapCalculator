using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using ManagementAPI.GolfClub;
using ManagementAPI.GolfClubMembership;
using ManagementAPI.Player;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Manager;
using ManagementAPI.Service.Services;
using ManagementAPI.Tournament;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Shared.CommandHandling;
using Shared.EventStore;
using StructureMap;
using StructureMap.Pipeline;
using ESLogger = EventStore.ClientAPI;

namespace ManagementAPI.Service.Bootstrapper
{
    using EventHandling;
    using HandicapCalculationProcess;
    using Services.DomainServices;

    [ExcludeFromCodeCoverage]
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {            
            String connString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");            
            String connectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            Int32 httpPort = Startup.Configuration.GetValue<Int32>("EventStoreSettings:HttpPort");

            EventStoreConnectionSettings settings = EventStoreConnectionSettings.Create(connString, connectionName, httpPort);

            this.For<IEventStoreContext>().Use<EventStoreContext>().Singleton().Ctor<EventStoreConnectionSettings>().Is(settings);

            Func<String, IEventStoreContext> eventStoreContextFunc = (connectionString) =>
            {  
                EventStoreConnectionSettings connectionSettings = EventStoreConnectionSettings.Create(connectionString,connectionName, httpPort);

                ExplicitArguments args = new ExplicitArguments().Set(connectionSettings);

                return Startup.Container.GetInstance<IEventStoreContext>(args);
            };

            Func<EventStoreConnectionSettings, IEventStoreConnection> eventStoreConnectionFunc = (connectionSettings) =>
            {  
                return EventStoreConnection.Create(connectionSettings.ConnectionString);                
            };

            this.For<IDomainEventHandler>().Use<GolfClubDomainEventHandler>().Named("GolfClub");
            this.For<IDomainEventHandler>().Use<GolfClubMembershipDomainEventHandler>().Named("GolfClubMembership");
            this.For<IDomainEventHandler>().Use<TournamentDomainEventHandler>().Named("Tournament");
            this.For<IDomainEventHandler>().Use<HandicapCalculationDomainEventHandler>().Named("HandicapCalculator");
            this.For<IDomainEventHandler>().Use<ReportingDomainEventHandler>().Named("Reporting");

            Func<String, IDomainEventHandler> domainEventHanderFunc = (name) => Startup.Container.GetInstance<IDomainEventHandler>(name);

            this.For<Func<EventStoreConnectionSettings, IEventStoreConnection>>().Use(eventStoreConnectionFunc);

            this.For<ESLogger.ILogger>().Use<ESLogger.Common.Log.ConsoleLogger>().Singleton();
            this.For<ICommandRouter>().Use<CommandRouter>().Singleton();
            this.For<IAggregateRepository<GolfClubAggregate>>()
                .Use<AggregateRepository<GolfClubAggregate>>().Singleton();
            this.For<IAggregateRepository<GolfClubMembershipAggregate>>()
                .Use<AggregateRepository<GolfClubMembershipAggregate>>().Singleton();
            this.For<IAggregateRepository<TournamentAggregate>>()
                .Use<AggregateRepository<TournamentAggregate>>().Singleton();
            this.For<IAggregateRepository<PlayerAggregate>>()
                .Use<AggregateRepository<PlayerAggregate>>().Singleton();
            this.For<IAggregateRepository<HandicapCalculationProcessAggregate>>()
                .Use<AggregateRepository<HandicapCalculationProcessAggregate>>().Singleton();

            this.For<IHandicapAdjustmentCalculatorService>().Use<HandicapAdjustmentCalculatorService>();
            this.For<IManagmentAPIManager>().Use<ManagementAPIManager>().Singleton();
            this.For<ISecurityService>().Use<SecurityService>().Singleton();
            this.For<IGolfClubMembershipApplicationService>().Use<GolfClubMembershipApplicationService>().Singleton();
            this.For<ITournamentApplicationService>().Use<TournamentApplicationService>().Singleton();

            this.For<IHandicapCalculationProcessorService>().Use<HandicapCalculationProcessorService>().Transient();
        }
    }
}
