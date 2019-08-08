using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.Tests.HandicapCalculationProcess
{
    using System.Threading;
    using BusinessLogic.CommandHandlers;
    using BusinessLogic.Commands;
    using ManagementAPI.HandicapCalculationProcess;
    using ManagementAPI.Tournament;
    using Moq;
    using Shared.EventStore;
    using Shouldly;
    using Tournament;
    using Xunit;

    public class HandicapCalculationCommandHandlerTests
    {
        [Fact]
        public void HandicapCalculationCommandHandler_HandleCommand_StartHandicapCalculationProcessForTournamentCommand_CommandHandled()
        {
            Mock<IAggregateRepository<HandicapCalculationProcessAggregate>> handicapCalculationProcessRepository = new Mock<IAggregateRepository<HandicapCalculationProcessAggregate>>();
            handicapCalculationProcessRepository.Setup(h => h.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                                .ReturnsAsync(HandicapCalculationProcessTestData.GetEmptyHandicapCalculationProcessAggregate());
            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                .ReturnsAsync(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate());

            HandicapCalculationCommandHandler handler = new HandicapCalculationCommandHandler(handicapCalculationProcessRepository.Object,
                                                                                              tournamentRepository.Object);

            StartHandicapCalculationProcessForTournamentCommand command = HandicapCalculationProcessTestData.GetStartHandicapCalculationProcessForTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }
    }
}
