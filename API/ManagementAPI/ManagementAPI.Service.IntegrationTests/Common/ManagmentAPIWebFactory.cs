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
    using DataTransferObjects.Responses;
    using IdentityModel;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Service.Common;
    using Shared.CommandHandling;
    using Xunit;
    using TimePeriod = BusinessLogic.Manager.TimePeriod;

    public class ManagmentApiWebFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Setup my mocks in here
            Mock<IManagmentAPIManager> managmentApiManagerMock = this.CreateManagmentAPIManagerMock();
            Mock<IReportingManager> reportingManagerMock = this.CreateReportingManagerMock();
            Mock<ICommandRouter> commandRouterMock = this.CreateCommandRouterMock();
            
            builder.ConfigureServices((builderContext, services) =>
                                                            {
                                                                if (managmentApiManagerMock != null)
                                                                {
                                                                    services.AddSingleton(managmentApiManagerMock.Object);
                                                                }

                                                                if (reportingManagerMock != null)
                                                                {
                                                                    services.AddSingleton(reportingManagerMock.Object);
                                                                }

                                                                if (commandRouterMock != null)
                                                                {
                                                                    services.AddSingleton(commandRouterMock.Object);
                                                                }

                                                                services.AddMvc(options =>
                                                                                {
                                                                                    options.Filters.Add(new AllowAnonymousFilter());
                                                                                })
                                                                        .AddApplicationPart(typeof(Startup).Assembly);
                                                            }).UseStartup<Startup>();
            ;
        }

        private Mock<IReportingManager> CreateReportingManagerMock()
        {
            Mock<IReportingManager> reportingManagerMock = new Mock<IReportingManager>(MockBehavior.Strict);

            reportingManagerMock.Setup(r => r.GetMembersHandicapListReport(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetMembersHandicapListReportResponse);
            reportingManagerMock.Setup(r => r.GetNumberOfMembersByAgeCategoryReport(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetNumberOfMembersByAgeCategoryReportResponse);
            reportingManagerMock.Setup(r => r.GetNumberOfMembersByHandicapCategoryReport(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetNumberOfMembersByHandicapCategoryReportResponse);
            reportingManagerMock.Setup(r => r.GetNumberOfMembersByTimePeriodReport(It.IsAny<Guid>(), TimePeriod.Day.ToString().ToLower(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetNumberOfMembersByTimePeriodReportDayResponse);
            reportingManagerMock.Setup(r => r.GetNumberOfMembersByTimePeriodReport(It.IsAny<Guid>(), TimePeriod.Month.ToString().ToLower(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetNumberOfMembersByTimePeriodReportMonthResponse);
            reportingManagerMock.Setup(r => r.GetNumberOfMembersByTimePeriodReport(It.IsAny<Guid>(), TimePeriod.Year.ToString().ToLower(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetNumberOfMembersByTimePeriodReportYearResponse);
            reportingManagerMock.Setup(r => r.GetNumberOfMembersReport(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetNumberOfMembersReportResponse);
            reportingManagerMock.Setup(r => r.GetPlayerScoresReport(It.IsAny<Guid>(), It.IsAny<Int32>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TestData.GetPlayerScoresResponse);

            return reportingManagerMock;
        }

        private Mock<ICommandRouter> CreateCommandRouterMock()
        {
            Mock<ICommandRouter> commandRouterMock=new Mock<ICommandRouter>(MockBehavior.Strict);

            commandRouterMock.Setup(c => c.Route(It.IsAny<ICommand>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            
            return commandRouterMock;
        }

        private Mock<IManagmentAPIManager> CreateManagmentAPIManagerMock()
        {
            Mock<IManagmentAPIManager> managmentApiManagerMock = new Mock<IManagmentAPIManager>(MockBehavior.Strict);

            managmentApiManagerMock.Setup(m => m.RegisterClubAdministrator(It.IsAny<RegisterClubAdministratorRequest>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(TestData.GolfClubAdministratorUserId);
            managmentApiManagerMock.Setup(m => m.GetGolfClub(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetGolfClubResponse);
            managmentApiManagerMock.Setup(m => m.GetGolfClubMembersList(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(TestData.GetGolfClubMembersListResponse);
            managmentApiManagerMock.Setup(m => m.GetMeasuredCourseList(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(TestData.GetMeasuredCourseListResponse);
            managmentApiManagerMock.Setup(m => m.GetGolfClubUsers(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetGolfClubUserListResponse);
            managmentApiManagerMock.Setup(m => m.GetGolfClubList(It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetGolfClubListResponse);
            managmentApiManagerMock.Setup(m => m.GetPlayerDetails(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetPlayerDetailsResponse);
            managmentApiManagerMock.Setup(m => m.GetPlayerSignedUpTournaments(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.PlayerSignedUpTournamentsResponse);
            managmentApiManagerMock.Setup(m => m.GetPlayersClubMemberships(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetPlayersClubMembershipsResponse);
            managmentApiManagerMock.Setup(m => m.GetTournamentList(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestData.GetTournamentListResponse);

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
                                                                                  new Claim(ClaimTypes.Role, "Golf Club Administrator"),
                                                                                  new Claim(CustomClaims.GolfClubId, TestData.GolfClubId.ToString())
                                                                              }));

            await next();
        }
    }

    public class FakePlayerUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                                                                              {
                                                                                  new Claim(JwtClaimTypes.Subject, "194A2B1E-E10B-47D9-927F-A8A1CF3C9138"),
                                                                                  new Claim(ClaimTypes.Role, "Player"),
                                                                                  new Claim(CustomClaims.PlayerId, TestData.PlayerId.ToString())
                                                                              }));

            await next();
        }
    }
}
