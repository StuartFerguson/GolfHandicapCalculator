using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.GolfClub;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClub
{
    public class GolfClubCommandHandlerTests
    {
        [Fact]
        public void GolfClubCommandHandler_HandleCommand_CreateGolfClubCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate());
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetRegisterUserResponse());
            Mock<IGolfClubMembershipApplicationService> golfClubMembershipApplicationService = new Mock<IGolfClubMembershipApplicationService>();

            GolfClubCommandHandler handler = new GolfClubCommandHandler(repository.Object, oAuth2SecurityService.Object, 
                golfClubMembershipApplicationService.Object);

            CreateGolfClubCommand command = GolfClubTestData.GetCreateGolfClubCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void GolfClubCommandHandler_HandleCommand_AddMeasuredCourseToClubCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetRegisterUserResponse());
            Mock<IGolfClubMembershipApplicationService> golfClubMembershipApplicationService = new Mock<IGolfClubMembershipApplicationService>();

            GolfClubCommandHandler handler = new GolfClubCommandHandler(repository.Object, oAuth2SecurityService.Object, 
                golfClubMembershipApplicationService.Object);

            AddMeasuredCourseToClubCommand command = GolfClubTestData.GetAddMeasuredCourseToClubCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void GolfClubCommandHandler_HandleCommand_AddTournamentDivisionToGolfClubCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetRegisterUserResponse());
            Mock<IGolfClubMembershipApplicationService> golfClubMembershipApplicationService = new Mock<IGolfClubMembershipApplicationService>();

            GolfClubCommandHandler handler = new GolfClubCommandHandler(repository.Object, oAuth2SecurityService.Object,
                                                                        golfClubMembershipApplicationService.Object);

            AddMeasuredCourseToClubCommand command = GolfClubTestData.GetAddMeasuredCourseToClubCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void GolfClubCommandHandler_HandleCommand_RequestClubMembershipCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();            
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            Mock<IGolfClubMembershipApplicationService> golfClubMembershipApplicationService = new Mock<IGolfClubMembershipApplicationService>();
            golfClubMembershipApplicationService.Setup(x =>
                    x.RequestClubMembership(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken.None))
                .Returns(Task.CompletedTask);
            GolfClubCommandHandler handler = new GolfClubCommandHandler(repository.Object, oAuth2SecurityService.Object, 
                golfClubMembershipApplicationService.Object);

            RequestClubMembershipCommand command = GolfClubTestData.GetRequestClubMembershipCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void GolfClubCommandHandler_HandleCommand_CreateMatchSecretaryCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            Mock<IGolfClubMembershipApplicationService> golfClubMembershipApplicationService = new Mock<IGolfClubMembershipApplicationService>();
            GolfClubCommandHandler handler = new GolfClubCommandHandler(repository.Object, oAuth2SecurityService.Object,
                                                                        golfClubMembershipApplicationService.Object);

            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetRegisterUserResponse());

            CreateMatchSecretaryCommand command = GolfClubTestData.GetCreateMatchSecretaryCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}