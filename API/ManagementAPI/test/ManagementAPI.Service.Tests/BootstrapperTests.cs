using System;
using System.Collections.Generic;
using ManagementtAPI.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StructureMap;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class BootstrapperTests
    {
        [Fact]
        public void VerifyBootstrapperIsValid_Development()
        {
            ServiceCollection servicesCollection = new ServiceCollection();
            Mock<IHostingEnvironment> hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Development");

            Startup.Configuration = SetupMemoryConfiguration();
            Startup.HostingEnvironment = hostingEnvironment.Object;

            IContainer container = Startup.GetConfiguredContainer(servicesCollection, hostingEnvironment.Object);

            //AddTestRegistrations(container);

            container.AssertConfigurationIsValid();
        }

        [Fact]
        public void VerifyBootstrapperIsValid_Production()
        {
            ServiceCollection servicesCollection = new ServiceCollection();
            Mock<IHostingEnvironment> hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("Production");

            Startup.Configuration = SetupMemoryConfiguration();
            Startup.HostingEnvironment = hostingEnvironment.Object;

            IContainer container = Startup.GetConfiguredContainer(servicesCollection, hostingEnvironment.Object);

            //AddTestRegistrations(container);

            container.AssertConfigurationIsValid();
        }

        private IConfigurationRoot SetupMemoryConfiguration()
        {
            Dictionary<String, String> configuration = new Dictionary<String, String>();

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(configuration);

            return builder.Build();
        }

        private void AddTestRegistrations(IContainer container)
        {

        }

    }
}
