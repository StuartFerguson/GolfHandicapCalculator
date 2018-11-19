using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Manager;
using ManagementAPI.Service.Services;
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
    [ExcludeFromCodeCoverage]
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {            
            var connString = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionString");            
            var connectionName = Startup.Configuration.GetValue<String>("EventStoreSettings:ConnectionName");
            var httpPort = Startup.Configuration.GetValue<Int32>("EventStoreSettings:HttpPort");

            EventStoreConnectionSettings settings = EventStoreConnectionSettings.Create(connString, connectionName, httpPort);

            For<IEventStoreContext>().Use<EventStoreContext>().Singleton().Ctor<EventStoreConnectionSettings>().Is(settings);

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

            For<Func<EventStoreConnectionSettings, IEventStoreConnection>>().Use(eventStoreConnectionFunc);

            For<ESLogger.ILogger>().Use<ESLogger.Common.Log.ConsoleLogger>().Singleton();
            For<ICommandRouter>().Use<CommandRouter>().Singleton();
            For<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>()
                .Use<AggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>().Singleton();
            For<IAggregateRepository<TournamentAggregate.TournamentAggregate>>()
                .Use<AggregateRepository<TournamentAggregate.TournamentAggregate>>().Singleton();
            For<IHandicapAdjustmentCalculatorService>().Use<HandicapAdjustmentCalculatorService>();
            For<IManagmentAPIManager>().Use<ManagmentAPIManager>().Singleton();
        }
    }
}
