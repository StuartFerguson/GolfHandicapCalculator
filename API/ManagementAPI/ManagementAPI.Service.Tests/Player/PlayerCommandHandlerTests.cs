using System;
using System.Threading;
using ManagementAPI.Player;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
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

            PlayerCommandHandler handler = new PlayerCommandHandler(playerRepository.Object);

            RegisterPlayerCommand command = PlayerTestData.GetRegisterPlayerCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}