using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Commands;
using ManagementAPI.Service.Services;
using ManagementAPI.Tournament;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class TournamentCommandHandlerTests
    {
        [Fact]
        public void TournamentCommandHandler_HandleCommand_CreateTournamentCommand_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            clubConfigurationRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ClubConfigurationTestData.GetClubConfigurationAggregateWithMeasuredCourse());

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CreateTournamentCommand_ClubNotFound_ErrorThrown()
        {
            // test added as part of bug #29 fixes
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            clubConfigurationRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ClubConfigurationTestData.GetEmptyClubConfigurationAggregate);

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.Throw<NotFoundException>(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CreateTournamentCommand_MeasuredCourseNotFound_ErrorThrown()
        {
            // test added as part of bug #29 fixes
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            clubConfigurationRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ClubConfigurationTestData.GetCreatedClubConfigurationAggregate);

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.Throw<NotFoundException>(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_RecordMemberTournamentScoreCommand_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetCreatedTournamentAggregate);

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            RecordMemberTournamentScoreCommand command = TournamentTestData.GetRecordMemberTournamentScoreCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CompleteTournamentCommand_WithScores_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate);

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            CompleteTournamentCommand command = TournamentTestData.GetCompleteTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CancelTournamentCommand_WithScores_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate);

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            CancelTournamentCommand command = TournamentTestData.GetCancelTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_ProduceTournamentResultCommand_CommandHandled()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubConfigurationRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(1,2,2,2,5,3));

            Mock<IHandicapAdjustmentCalculatorService> handicapAdjustmentCalculatorService = new Mock<IHandicapAdjustmentCalculatorService>();
            handicapAdjustmentCalculatorService.SetupSequence(h =>
                    h.CalculateHandicapAdjustment(It.IsAny<Decimal>(), It.IsAny<Int32>(),
                        It.IsAny<Dictionary<Int32, Int32>>()))
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {-0.1m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {-0.2m, -0.2m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {-0.4m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {-0.2m})
                .Returns(new List<Decimal> {0.1m})
                .Returns(new List<Decimal> {0.1m});

            TournamentCommandHandler handler = new TournamentCommandHandler(clubConfigurationRepository.Object, tournamentRepository.Object, handicapAdjustmentCalculatorService.Object);

            ProduceTournamentResultCommand command = TournamentTestData.GetProduceTournamentResultCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }


    }
}
