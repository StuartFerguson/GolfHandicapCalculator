using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Common;
using Ductus.FluentDocker.Model.Builders;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Specflow.Common
{
    [Binding]    
    public abstract class GenericSteps
    {
        protected ScenarioContext ScenarioContext;
        protected IContainerService ManagementAPIContainer;
        protected IContainerService SubscriptionServiceContainer;
        protected IContainerService EventStoreContainer;
        protected IContainerService SecurityServiceContainer;
        protected INetworkService TestNetwork;
        protected Int32 ManagementApiPort;
        protected Int32 EventStorePort;
        protected Int32 SecurityServicePort;
        protected Guid SubscriberServiceId;
        
        protected GenericSteps(ScenarioContext scenarioContext) 
        {
            this.ScenarioContext = scenarioContext;
        }
        
        protected void RunSystem(String testFolder)
        {
            Logging.Enabled();

            String managementAPIContainerName = $"rest{Guid.NewGuid():N}";
            String eventStoreContainerName = $"eventstore{Guid.NewGuid():N}";
            String securityServiceContainerName = $"auth{Guid.NewGuid():N}";
            this.SubscriberServiceId = Guid.NewGuid();
            String subscriptionServiceContainerName = $"subService{this.SubscriberServiceId:N}";
            
            // Build a network
            this.TestNetwork = new Builder().UseNetwork($"testnetwork{Guid.NewGuid()}").Build();
            
            String eventStoreConnectionString = $"EventStoreSettings:ConnectionString=ConnectTo=tcp://admin:changeit@{eventStoreContainerName}:1113;VerboseLogging=true;";
            String securityServiceAddress = $"AppSettings:OAuth2SecurityService=http://{securityServiceContainerName}:5001";
            String subscriptionServiceConnectionString = $"\"ConnectionStrings:SubscriptionServiceConfigurationContext={Setup.GetConnectionString("SubscriptionServiceConfiguration")}\"";
            String managementAPISeedingType = "SeedingType=IntegrationTest";

            #region Security Service Container
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
            #endregion

            #region Management API Container
            // Management API Container
            this.ManagementAPIContainer = new Builder()
                .UseContainer()
                .WithName(managementAPIContainerName)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT=IntegrationTest",eventStoreConnectionString, securityServiceAddress, 
                    //managementAPIConnectionString,
                    managementAPISeedingType)
                .UseImage("managementapiservice")
                .ExposePort(5000)
                .UseNetwork(new List<INetworkService> {this.TestNetwork, Setup.DatabaseServerNetwork}.ToArray())     
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start().WaitForPort("5000/tcp", 30000);
            #endregion           
            
            #region Event Store Container
            // Event Store Container
            this.EventStoreContainer = new Builder()
                .UseContainer()
                .UseImage("eventstore/eventstore")
                .ExposePort(2113)
                .ExposePort(1113)
                .WithName(eventStoreContainerName)
                .WithEnvironment("EVENTSTORE_RUN_PROJECTIONS=all", "EVENTSTORE_START_STANDARD_PROJECTIONS=true")
                .UseNetwork(this.TestNetwork)
                .Mount($"D:\\temp\\docker\\{testFolder}\\eventstore", "/var/log/eventstore", MountType.ReadWrite)
                .Build()
                .Start().WaitForPort("2113/tcp", 30000);
            #endregion
            
            // Cache the ports
            this.ManagementApiPort = this.ManagementAPIContainer.ToHostExposedEndpoint("5000/tcp").Port;
            this.EventStorePort = this.EventStoreContainer.ToHostExposedEndpoint("2113/tcp").Port;
            this.SecurityServicePort = this.SecurityServiceContainer.ToHostExposedEndpoint("5001/tcp").Port;

            this.SetupSubscriptionServiceConfig();

            #region Subscriber Service Container
            // Subscriber Service Container
            this.SubscriptionServiceContainer = new Builder()
                .UseContainer()
                .WithName(subscriptionServiceContainerName)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT=Development", 
                    subscriptionServiceConnectionString, 
                    eventStoreConnectionString, 
                    "EventStoreSettings:HttpPort=2113",
                    $"ServiceSettings:SubscriptionServiceId={this.SubscriberServiceId}")
                .UseImage("subscriptionserviceservice")
                .UseNetwork(new List<INetworkService> {this.TestNetwork, Setup.DatabaseServerNetwork}.ToArray()) 
                .Mount($"D:\\temp\\docker\\{testFolder}", "/home", MountType.ReadWrite)
                .Build()
                .Start();
            #endregion
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

            if (this.SubscriptionServiceContainer != null)
            {
                this.SubscriptionServiceContainer.Stop();
                this.SubscriptionServiceContainer.Remove(true);
            }

            if (this.TestNetwork != null)
            {
                this.TestNetwork.Stop();
                this.TestNetwork.Remove(true);
            }
        }

        protected abstract void SetupSubscriptionServiceConfig();
    }
}
