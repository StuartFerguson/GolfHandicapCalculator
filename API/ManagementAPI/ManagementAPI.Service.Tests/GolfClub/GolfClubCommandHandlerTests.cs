using System;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.GolfClub;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Services;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClub
{
    using Services.ExternalServices.DataTransferObjects;

    public class GolfClubCommandHandlerTests
    {
        [Fact]
        public void GolfClubCommandHandler_HandleCommand_CreateGolfClubCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            repository.Setup(r => r.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate());
            Mock<ISecurityService> oAuth2SecurityService = new Mock<ISecurityService>();
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
            Mock<ISecurityService> oAuth2SecurityService = new Mock<ISecurityService>();
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
            Mock<ISecurityService> oAuth2SecurityService = new Mock<ISecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetRegisterUserResponse());
            Mock<IGolfClubMembershipApplicationService> golfClubMembershipApplicationService = new Mock<IGolfClubMembershipApplicationService>();

            GolfClubCommandHandler handler = new GolfClubCommandHandler(repository.Object, oAuth2SecurityService.Object,
                                                                        golfClubMembershipApplicationService.Object);

            AddTournamentDivisionToGolfClubCommand command = GolfClubTestData.GetAddTournamentDivisionToGolfClubCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void GolfClubCommandHandler_HandleCommand_RequestClubMembershipCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> repository = new Mock<IAggregateRepository<GolfClubAggregate>>();            
            Mock<ISecurityService> oAuth2SecurityService = new Mock<ISecurityService>();
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
            Mock<ISecurityService> oAuth2SecurityService = new Mock<ISecurityService>();
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