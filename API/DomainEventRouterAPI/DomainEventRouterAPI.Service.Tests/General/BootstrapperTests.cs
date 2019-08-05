using System;
using System.Collections.Generic;
using System.Text;

namespace DomainEventRouterAPI.Service.Tests.General
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using Shared.EventStore;
    using Shared.General;
    using StructureMap;
    using Xunit;

    public class BootstrapperTests
    {
        [Fact]
        public void VerifyBootstrapperIsValid_Development()
        {
            ServiceCollection servicesCollection = new ServiceCollection();
            Mock<IHostingEnvironment> hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Development");

            Startup.Configuration = this.SetupMemoryConfiguration();
            Startup.HostingEnvironment = hostingEnvironment.Object;

            ConfigurationReader.Initialise(Startup.Configuration);

            IContainer container = Startup.GetConfiguredContainer(servicesCollection, hostingEnvironment.Object);

            this.AddTestRegistrations(container);

            container.AssertConfigurationIsValid();
        }

        [Fact]
        public void VerifyBootstrapperIsValid_Production()
        {
            ServiceCollection servicesCollection = new ServiceCollection();
            Mock<IHostingEnvironment> hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Production");

            Startup.Configuration = this.SetupMemoryConfiguration();
            Startup.HostingEnvironment = hostingEnvironment.Object;

            ConfigurationReader.Initialise(Startup.Configuration);

            IContainer container = Startup.GetConfiguredContainer(servicesCollection, hostingEnvironment.Object);

            this.AddTestRegistrations(container);

            container.AssertConfigurationIsValid();
        }

        private IConfigurationRoot SetupMemoryConfiguration()
        {
            Dictionary<String, String> configuration = new Dictionary<String, String>();

            IConfigurationBuilder builder = new ConfigurationBuilder();

            configuration.Add("AppSettings:ManagementAPI", "http://localhost:5000");
            configuration.Add("AppSettings:SecurityService", "http://localhost:5001");
            configuration.Add("AppSettings:ElasticServer", "http://localhost:9200");
            configuration.Add("AppSettings:ElasticUserName", String.Empty);
            configuration.Add("AppSettings:ElasticPassword", String.Empty);
            configuration.Add("AppSettings:HandlerEventTypesToSilentlyHandle", "\"GolfClubDomainEventHandler\": []");
            
            builder.AddInMemoryCollection(configuration);

            return builder.Build();
        }

        private void AddTestRegistrations(IContainer container)
        {
            container.Configure(c => c.For<IOptions<EventStoreConnectionSettings>>().Use<OptionsManager<EventStoreConnectionSettings>>());
            container.Configure(c => c.For<IOptionsFactory<EventStoreConnectionSettings>>().Use<OptionsFactory<EventStoreConnectionSettings>>());
        }

    }
}
