using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.IntegrationTests.Common
{
    using System.Security.Claims;
    using System.Threading;
    using BusinessLogic.Manager;
    using Controllers;
    using DataTransferObjects.Requests;
    using IdentityModel;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public class ManagmentApiWebFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Setup my mocks in here
            Mock<IManagmentAPIManager> managmentApiManagerMock = this.CreateManagmentAPIManagerMock();
            
            builder.ConfigureServices((builderContext, services) =>
                                                            {
                                                                if (managmentApiManagerMock != null)
                                                                {
                                                                    services.AddSingleton(managmentApiManagerMock.Object);
                                                                }

                                                                services.AddMvc(options =>
                                                                                {
                                                                                    options.Filters.Add(new AllowAnonymousFilter());
                                                                                })
                                                                        .AddApplicationPart(typeof(Startup).Assembly);
                                                            }).UseStartup<Startup>();
            ;
        }

        private Mock<IManagmentAPIManager> CreateManagmentAPIManagerMock()
        {
            Mock<IManagmentAPIManager> managmentApiManagerMock = new Mock<IManagmentAPIManager>(MockBehavior.Strict);

            managmentApiManagerMock.Setup(m => m.RegisterClubAdministrator(It.IsAny<RegisterClubAdministratorRequest>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(TestData.GolfClubAdministratorUserId);

            return managmentApiManagerMock;
        }
    }
    
    /// <summary>
    /// </summary>
    /// <seealso cref="Startup" />
    [CollectionDefinition("TestCollection")]
    public class DatabaseCollection : ICollectionFixture<ManagmentApiWebFactory<Startup>>
    {
        // A class with no code, only used to define the collection
    }

    public class FakeGolfClubAdminUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                                                              {
                                                                                  new Claim(JwtClaimTypes.Subject, "194A2B1E-E10B-47D9-927F-A8A1CF3C9138"),
                                                                                  new Claim(ClaimTypes.Role, "Golf Club Administrator")
                                                                              }));

            await next();
        }
    }
}
