using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.Tests.Tournament
{
    using System.Threading;
    using System.Threading.Tasks;
    using ManagementAPI.Player;
    using ManagementAPI.Tournament;
    using Moq;
    using Player;
    using Services.DomainServices;
    using Shared.EventStore;
    using Shouldly;
    using Xunit;

    public class TournamentApplicationServiceTests
    {
        [Fact]
        public void TournamentApplicationService_SignUpPlayerForTournament_PlayerSignedUp()
        {
            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(TournamentTestData.GetCreatedTournamentAggregate);
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregateWithMembershipAdded);
            TournamentApplicationService tournamentApplicationService = new TournamentApplicationService(tournamentRepository.Object,
                                                                                                         playerRepository.Object);

            Should.NotThrow(async () =>
                             {
                                 await tournamentApplicationService.SignUpPlayerForTournament(TournamentTestData.AggregateId,
                                                                                              TournamentTestData.PlayerId,
                                                                                              CancellationToken.None);
                             });
        }

        [Fact]
        public void TournamentApplicationService_SignUpPlayerForTournament_UnregisteredPlayer_ErrorThrown()
        {
            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(TournamentTestData.GetCreatedTournamentAggregate);
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            TournamentApplicationService tournamentApplicationService = new TournamentApplicationService(tournamentRepository.Object,
                                                                                                         playerRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                            {
                                await tournamentApplicationService.SignUpPlayerForTournament(TournamentTestData.AggregateId,
                                                                                             TournamentTestData.PlayerId,
                                                                                             CancellationToken.None);
                            });
        }

        [Fact]
        public void TournamentApplicationService_SignUpPlayerForTournament_NotMemberOfClub_ErrorThrown()
        {
            Mock<IAggregateRepository<TournamentAggregate>> tournamentRepository = new Mock<IAggregateRepository<TournamentAggregate>>();
            tournamentRepository.Setup(t => t.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(TournamentTestData.GetCreatedTournamentAggregate);
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            TournamentApplicationService tournamentApplicationService = new TournamentApplicationService(tournamentRepository.Object,
                                                                                                         playerRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await tournamentApplicationService.SignUpPlayerForTournament(TournamentTestData.AggregateId,
                                                                                                                     TournamentTestData.PlayerId,
                                                                                                                     CancellationToken.None);
                                                    });
        }

        // Unregistered Player
        // Not a member of club
    }
}
