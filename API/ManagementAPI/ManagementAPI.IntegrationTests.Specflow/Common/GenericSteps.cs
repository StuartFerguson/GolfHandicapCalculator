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
        protected IContainerService SecurityServiceContainer;
        protected INetworkService TestNetwork;
        protected Int32 ManagementApiPort;
        protected Int32 EventStorePort;
        protected Int32 SecurityServicePort;


        protected GenericSteps(ScenarioContext scenarioContext) 
        {
            this.ScenarioContext = scenarioContext;
        }
        
        protected void RunSystem(String testFolder)
        {
            String managementAPIContainerName = $"managementApi{Guid.NewGuid():N}";
            String eventStoreContainerName = $"eventstore{Guid.NewGuid():N}";
            String securityServiceContainerName = $"auth{Guid.NewGuid():N}";
            
            // Build a network
            this.TestNetwork = new Builder().UseNetwork($"testnetwork{Guid.NewGuid()}").Build();
            
            String eventStoreConnectionString = $"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:1113;VerboseLogging=true;";
            String securityServiceAddress = $"AppSettings:OAuth2SecurityService=http://{securityServiceContainerName}:5001";

            // Security Service Container
            this.SecurityServiceContainer = new Builder()
                .UseContainer()
                .WithName(securityServiceContainerName)
                .WithEnvironment("SeedingType=IntegrationTest", "ASPNETCORE_ENVIRONMENT=IntegrationTest")
                .UseImage("oauth2securityserviceservice")
                .ExposePort(5001)
                .UseNetwork(this.TestNetwork)
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)                
                .Build()
                .Start()
                .WaitForPort("5001/tcp", 30000);

            // Management API Container
            this.ManagementAPIContainer = new Builder()
                .UseContainer()
                .WithName(managementAPIContainerName)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT=integrationtest",eventStoreConnectionString, securityServiceAddress)
                .UseImage("managementapiservice")
                .ExposePort(5000)
                .UseNetwork(this.TestNetwork)                
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start().WaitForPort("5000/tcp", 30000);

            // Event Store Container
            this.EventStoreContainer = new Builder()
                .UseContainer()
                .UseImage("eventstore/eventstore")
                .ExposePort(2113)
                .ExposePort(1113)
                .WithName(eventStoreContainerName)
                .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                .UseNetwork(this.TestNetwork)
                .Build()
                .Start().WaitForPort("2113/tcp", 30000);
            
            // Cache the ports
            this.ManagementApiPort = this.ManagementAPIContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.EventStorePort = this.EventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.SecurityServicePort = this.SecurityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;
       }

        protected void StopSystem()
        {
            if (this.ManagementAPIContainer != null)
            {
                this.ManagementAPIContainer.Stop();
                this.ManagementAPIContainer.Remove(true);
            }
            
            if (this.EventStoreContainer != null)
            {
                this.EventStoreContainer.Stop();
                this.EventStoreContainer.Remove(true);
            }

            if (this.SecurityServiceContainer != null)
            {
                this.SecurityServiceContainer.Stop();
                this.SecurityServiceContainer.Remove(true);
            }

            if (this.TestNetwork != null)
            {
                this.TestNetwork.Stop();
                this.TestNetwork.Remove(true);
            }
        }
    }
}
