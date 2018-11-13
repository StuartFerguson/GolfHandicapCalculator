using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Manager;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class ManagmentAPIManagerTests
    {
        [Fact]
        public async Task ManagmentAPIManager_GetClubConfiguration_ClubConfigurationReturned()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(ClubConfigurationTestData.GetCreatedClubConfigurationAggregate());

            var manager = new ManagmentAPIManager(clubRepository.Object);

            var result = await manager.GetClubConfiguration(ClubConfigurationTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(ClubConfigurationTestData.Name);
            result.AddressLine1.ShouldBe(ClubConfigurationTestData.AddressLine1);
            result.AddressLine2.ShouldBe(ClubConfigurationTestData.AddressLine2);
            result.Town.ShouldBe(ClubConfigurationTestData.Town);
            result.Region.ShouldBe(ClubConfigurationTestData.Region);
            result.PostalCode.ShouldBe(ClubConfigurationTestData.PostalCode);
            result.TelephoneNumber.ShouldBe(ClubConfigurationTestData.TelephoneNumber);
            result.Website.ShouldBe(ClubConfigurationTestData.Website);
            result.EmailAddress.ShouldBe(ClubConfigurationTestData.EmailAddress);
        }

        [Fact]
        public void ManagmentAPIManager_GetClubConfiguration_InvalidClubId_ErrorThrown()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>();
            var manager = new ManagmentAPIManager(clubRepository.Object);

            Should.ThrowAsync<ArgumentNullException>(async () => 
            {
                await manager.GetClubConfiguration(Guid.Empty, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetClubConfiguration_ClubConfigurationNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>();
            var manager = new ManagmentAPIManager(clubRepository.Object);

            Should.ThrowAsync<NotFoundException>(async () => 
            {
                await manager.GetClubConfiguration(Guid.Empty, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetClubConfiguration_ClubConfigurationNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate.ClubConfigurationAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(ClubConfigurationTestData.GetEmptyClubConfigurationAggregate());

            var manager = new ManagmentAPIManager(clubRepository.Object);

            Should.ThrowAsync<NotFoundException>(async () => 
            {
                await manager.GetClubConfiguration(Guid.Empty, CancellationToken.None);
            });
        }
    }
}
