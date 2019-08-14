using System;
using System.Threading;
using ManagementAPI.GolfClub;
using ManagementAPI.Player;
using ManagementAPI.Service.Tests.GolfClub;
using Moq;
using Shared.EventStore;
using Shared.Exceptions;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Player
{
    using BusinessLogic.CommandHandlers;
    using BusinessLogic.Commands;
    using BusinessLogic.Services.ExternalServices;
    using BusinessLogic.Services.ExternalServices.DataTransferObjects;

    public class PlayerCommandHandlerTests
    {
        [Fact]
        public void PlayerCommandHandler_HandleCommand_RegisterPlayerCommand_CommandHandled()
        {
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate());
            Mock<ISecurityService> oAuth2SecurityService = new Mock<ISecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetRegisterUserResponse());
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetGolfClubAggregateWithMeasuredCourse());

            PlayerCommandHandler handler = new PlayerCommandHandler(playerRepository.Object, oAuth2SecurityService.Object, clubRepository.Object);

            RegisterPlayerCommand command = PlayerTestData.GetRegisterPlayerCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}