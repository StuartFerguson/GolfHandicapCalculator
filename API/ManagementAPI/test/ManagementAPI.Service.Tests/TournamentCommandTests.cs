using System;
using System.Linq;
using ManagementAPI.Service.Commands;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class TournamentCommandTests
    {
        [Fact]
        public void CreateTournamentCommand_CanBeCreated_IsCreated()
        {
            CreateTournamentCommand command = CreateTournamentCommand.Create(TournamentTestData.CreateTournamentRequest);

            command.ShouldNotBeNull();
            command.CommandId.ShouldNotBe(Guid.Empty);
            command.CreateTournamentRequest.ShouldNotBeNull();
            command.CreateTournamentRequest.ShouldBe(TournamentTestData.CreateTournamentRequest); 
        }
    }
}
