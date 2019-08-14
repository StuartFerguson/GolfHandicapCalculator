using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.Tests.Tournament
{
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Services.ApplicationServices;
    using GolfClubMembership;
    using ManagementAPI.GolfClubMembership;
    using ManagementAPI.Player;
    using ManagementAPI.Tournament;
    using Moq;
    using Player;
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
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate());
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMembershipRequested);

            TournamentApplicationService tournamentApplicationService = new TournamentApplicationService(tournamentRepository.Object,
                                                                                                         playerRepository.Object,
                                                                                                         golfClubMembershipRepository.Object);

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
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMembershipRequested);

            TournamentApplicationService tournamentApplicationService = new TournamentApplicationService(tournamentRepository.Object,
                                                                                                         playerRepository.Object,
                                                                                                         golfClubMembershipRepository.Object);

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
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate());
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate);

            TournamentApplicationService tournamentApplicationService = new TournamentApplicationService(tournamentRepository.Object,
                                                                                                         playerRepository.Object,
                                                                                                         golfClubMembershipRepository.Object);

            Should.Throw<InvalidOperationException>(async () =>
                                                    {
                                                        await tournamentApplicationService.SignUpPlayerForTournament(TournamentTestData.AggregateId,
                                                                                                                     TournamentTestData.PlayerId,
                                                                                                                     CancellationToken.None);
                                                    });
        }
    }
}
