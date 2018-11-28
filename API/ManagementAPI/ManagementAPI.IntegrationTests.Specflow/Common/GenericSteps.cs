using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Specflow.Common
{
    [Binding]    
    public class GenericSteps
    {
        protected ScenarioContext ScenarioContext;
        protected IContainerService ManagementAPIContainer;
        protected IContainerService EventStoreContainer;
        protected INetworkService TestNetwork;
        protected Int32 ManagementApiPort;
        protected Int32 EventStorePort;


        protected GenericSteps(ScenarioContext scenarioContext) 
        {
            this.ScenarioContext = scenarioContext;
        }
        
        protected void RunSystem(String testFolder)
        {
            String managementAPIContainerName = $"managementApi{Guid.NewGuid()}";
            String eventStoreContainerName = $"eventstore{Guid.NewGuid()}";
            
            // Build a network
            this.TestNetwork = new Builder().UseNetwork($"test-network-{Guid.NewGuid()}").Build();

            String eventStoreConnectionString = $"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:1113;VerboseLogging=true;";

            // Management API Container
            this.ManagementAPIContainer = new Builder()
                .UseContainer()
                .WithName(managementAPIContainerName)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT=integrationtest",eventStoreConnectionString)
                .UseImage("managementapiservice")
                .ExposePort(5000)
                .UseNetwork(this.TestNetwork)
                .WaitForPort("5000/tcp", 30000)
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start();

            this.EventStoreContainer = new Builder()
                .UseContainer()
                .UseImage("eventstore/eventstore")
                .ExposePort(2113)
                .ExposePort(1113)
                .WithName(eventStoreContainerName)
                .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                .UseNetwork(this.TestNetwork).WaitForPort("2113/tcp", 30000)
                .Build()
                .Start();

            // Cache the ports
            this.ManagementApiPort = this.ManagementAPIContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.EventStorePort = this.EventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
        }

        protected void StopSystem()
        {
            this.ManagementAPIContainer.Stop();
            this.ManagementAPIContainer.Remove(true);
            this.EventStoreContainer.Stop();
            this.EventStoreContainer.Remove(true);
            this.TestNetwork.Stop();
            this.TestNetwork.Remove(true);
        }
    }
}
