using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.Player
{
    using BusinessLogic.Commands;

    public class PlayerCommandTests
    {
        [Fact]
        public void RegisterPlayerCommand_CanBeCreated_IsCreated()
        {
            RegisterPlayerCommand command = RegisterPlayerCommand.Create(PlayerTestData.RegisterPlayerRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.RegisterPlayerRequest.ShouldNotBeNull();
            command.RegisterPlayerRequest.ShouldBe(PlayerTestData.RegisterPlayerRequest);
        }
    }
}
