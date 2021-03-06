﻿namespace ManagementAPI.Service.Tests.Tournament
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using BusinessLogic.CommandHandlers;
    using BusinessLogic.Commands;
    using BusinessLogic.Services.ApplicationServices;
    using GolfClub;
    using ManagementAPI.GolfClub;
    using ManagementAPI.Tournament;
    using Moq;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shouldly;
    using Xunit;

    public class TournamentCommandHandlerTests
    {
        #region Methods

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CancelTournamentCommand_WithScores_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate);
            
            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            CancelTournamentCommand command = TournamentTestData.GetCancelTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CompleteTournamentCommand_WithScores_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetCreatedTournamentWithScoresRecordedAggregate);

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            CompleteTournamentCommand command = TournamentTestData.GetCompleteTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CreateTournamentCommand_ClubNotFound_ErrorThrown()
        {
            // test added as part of bug #29 fixes
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            golfClubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate);

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.Throw<NotFoundException>(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CreateTournamentCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            golfClubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(GolfClubTestData.GetGolfClubAggregateWithMeasuredCourse());

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_CreateTournamentCommand_MeasuredCourseNotFound_ErrorThrown()
        {
            // test added as part of bug #29 fixes
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            golfClubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetEmptyTournamentAggregate);

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            CreateTournamentCommand command = TournamentTestData.GetCreateTournamentCommand();

            Should.Throw<NotFoundException>(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_ProduceTournamentResultCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetCompletedTournamentAggregateWithCSSCalculatedAggregate(1, 2, 2, 2, 5, 3));

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            ProduceTournamentResultCommand command = TournamentTestData.GetProduceTournamentResultCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_RecordMemberTournamentScoreCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetCreatedTournamentAggregateWithPlayerSignedUp);

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            RecordPlayerTournamentScoreCommand command = TournamentTestData.GetRecordPlayerTournamentScoreCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        [Fact]
        public void TournamentCommandHandler_HandleCommand_SignUpForTournamentCommand_CommandHandled()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(TournamentTestData.GetCreatedTournamentAggregate);

            Mock<ITournamentApplicationService> tournamentApplicationService = new Mock<ITournamentApplicationService>();

            TournamentCommandHandler handler = new TournamentCommandHandler(golfClubRepository.Object,
                                                                            tournamentRepository.Object,
                                                                            tournamentApplicationService.Object);

            SignUpForTournamentCommand command = TournamentTestData.GetSignUpForTournamentCommand();

            Should.NotThrow(async () => { await handler.Handle(command, CancellationToken.None); });
        }

        #endregion
    }
}