using System;
using System.Threading;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class ClubConfigurationCommandHandlerTests
    {
        [Fact]
        public void ClubConfigurationCommandHandler_HandleCommand_CreateClubConfigurationCommand_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> repository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(ClubConfigurationTestData.GetEmptyClubConfigurationAggregate());
            
            ClubConfigurationCommandHandler handler = new ClubConfigurationCommandHandler(repository.Object);

            CreateClubConfigurationCommand command = ClubConfigurationTestData.GetCreateClubConfigurationCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void ClubConfigurationCommandHandler_HandleCommand_AddMeasuredCourseToClubCommand_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> repository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(ClubConfigurationTestData.GetCreatedClubConfigurationAggregate);
            
            ClubConfigurationCommandHandler handler = new ClubConfigurationCommandHandler(repository.Object);

            AddMeasuredCourseToClubCommand command = ClubConfigurationTestData.GetAddMeasuredCourseToClubCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}