using System;
using System.Threading;
using ManagementAPI.Player;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Player
{
    public class PlayerCommandHandlerTests
    {
        [Fact]
        public void PlayerCommandHandler_HandleCommand_RegisterPlayerCommand_CommandHandled()
        {
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate());
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            oAuth2SecurityService
                .Setup(o => o.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetRegisterUserResponse());

            PlayerCommandHandler handler = new PlayerCommandHandler(playerRepository.Object, oAuth2SecurityService.Object);

            RegisterPlayerCommand command = PlayerTestData.GetRegisterPlayerCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void PlayerCommandHandler_HandleCommand_PlayerClubMembershipRequestCommand_CommandHandled()
        {
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregateWithSecurityUserCreated);
            Mock<IOAuth2SecurityService> oAuth2SecurityService = new Mock<IOAuth2SecurityService>();
            
            PlayerCommandHandler handler = new PlayerCommandHandler(playerRepository.Object, oAuth2SecurityService.Object);

            PlayerClubMembershipRequestCommand command = PlayerTestData.GetPlayerClubMembershipRequestCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}