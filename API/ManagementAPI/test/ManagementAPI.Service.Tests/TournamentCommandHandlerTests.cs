using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class TournamentCommandHandlerTests
    {
        [Fact]
        public void ClubConfigurationCommandHandler_HandleCommand_CreateTournamentCommand_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>();
            clubConfigurationRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ClubConfigurationTestData.GetClubConfigurationAggregateWithMeasuredCourse());

            Mock<IAggregateRepository<TournamentAggregate.TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate.TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}
