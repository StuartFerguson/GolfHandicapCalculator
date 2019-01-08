using System;
using System.Threading;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.ClubConfiguration
{
    public class ClubConfigurationCommandHandlerTests
    {
        [Fact]
        public void ClubConfigurationCommandHandler_HandleCommand_CreateClubConfigurationCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(ClubConfigurationTestData.GetEmptyClubConfigurationAggregate());
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(ClubConfigurationTestData.GetRegisterUserResponse());

            ClubConfigurationCommandHandler handler = new ClubConfigurationCommandHandler(repository.Object, oAuth2SecurityService.Object);

            CreateClubConfigurationCommand command = ClubConfigurationTestData.GetCreateClubConfigurationCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void ClubConfigurationCommandHandler_HandleCommand_AddMeasuredCourseToClubCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(ClubConfigurationTestData.GetCreatedClubConfigurationAggregate);
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(ClubConfigurationTestData.GetRegisterUserResponse());

            ClubConfigurationCommandHandler handler = new ClubConfigurationCommandHandler(repository.Object, oAuth2SecurityService.Object);

            AddMeasuredCourseToClubCommand command = ClubConfigurationTestData.GetAddMeasuredCourseToClubCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}