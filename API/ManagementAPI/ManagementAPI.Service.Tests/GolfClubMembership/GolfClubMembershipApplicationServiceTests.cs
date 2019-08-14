using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.GolfClub;
using ManagementAPI.GolfClubMembership;
using ManagementAPI.Player;
using ManagementAPI.Service.Tests.GolfClub;
using ManagementAPI.Service.Tests.Player;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.GolfClubMembership
{
    using BusinessLogic.Services.ApplicationServices;

    public class GolfClubMembershipApplicationServiceTests
    {
        [Fact]
        public async Task GolfClubMembershipApplicationService_RequestClubMembership_MembershipRequestProcessed()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            golfClubRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());
            
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate());

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(),CancellationToken.None)).ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate());

            GolfClubMembershipApplicationService service = new GolfClubMembershipApplicationService(golfClubRepository.Object,
                playerRepository.Object,
                golfClubMembershipRepository.Object);

            await service.RequestClubMembership(GolfClubMembershipTestData.PlayerId, GolfClubMembershipTestData.AggregateId,
                CancellationToken.None);

            golfClubMembershipRepository.Verify(
                x => x.SaveChanges(It.IsAny<GolfClubMembershipAggregate>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task GolfClubMembershipApplicationService_RequestClubMembership_PlayerNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            golfClubRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());
            
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate());

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(),CancellationToken.None)).ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate());

            GolfClubMembershipApplicationService service = new GolfClubMembershipApplicationService(golfClubRepository.Object,
                playerRepository.Object,
                golfClubMembershipRepository.Object);

            Should.Throw<InvalidDataException>( async () => await service.RequestClubMembership(GolfClubMembershipTestData.PlayerId, GolfClubMembershipTestData.AggregateId,
                CancellationToken.None));

            golfClubMembershipRepository.Verify(
                x => x.SaveChanges(It.IsAny<GolfClubMembershipAggregate>(), CancellationToken.None), Times.Never);
        }

        [Fact]
        public void GolfClubMembershipApplicationService_RequestClubMembership_ClubNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> golfClubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            golfClubRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate());
            
            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate());

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(g => g.GetLatestVersion(It.IsAny<Guid>(),CancellationToken.None)).ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate());

            GolfClubMembershipApplicationService service = new GolfClubMembershipApplicationService(golfClubRepository.Object,
                playerRepository.Object,
                golfClubMembershipRepository.Object);

            Should.Throw<InvalidDataException>( async () => await service.RequestClubMembership(GolfClubMembershipTestData.PlayerId, GolfClubMembershipTestData.AggregateId,
                CancellationToken.None));

            golfClubMembershipRepository.Verify(
                x => x.SaveChanges(It.IsAny<GolfClubMembershipAggregate>(), CancellationToken.None), Times.Never);
        }
    }
}
