using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.ClubConfiguration;
using ManagementAPI.ClubConfiguration.DomainEvents;
using ManagementAPI.Database;
using ManagementAPI.Database.Models;
using ManagementAPI.Service.Manager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Shared.EventStore;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests
{
    public class ManagmentAPIManagerTests
    {
        private ManagementAPIReadModel GetContext(String databaseName)
        {
            DbContextOptionsBuilder<ManagementAPIReadModel> builder = new DbContextOptionsBuilder<ManagementAPIReadModel>()
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            ManagementAPIReadModel context = new ManagementAPIReadModel(builder.Options);
            
            return context;
        }

        #region Get Club Configuration Tests
        [Fact]
        public async Task ManagmentAPIManager_GetClubConfiguration_ClubConfigurationReturned()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(ClubConfigurationTestData.GetCreatedClubConfigurationAggregate());

            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

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
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            Should.ThrowAsync<ArgumentNullException>(async () => 
            {
                await manager.GetClubConfiguration(Guid.Empty, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetClubConfiguration_ClubConfigurationNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            Should.ThrowAsync<NotFoundException>(async () => 
            {
                await manager.GetClubConfiguration(ClubConfigurationTestData.AggregateId, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetClubConfiguration_ClubConfigurationNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(ClubConfigurationTestData.GetEmptyClubConfigurationAggregate());

            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            Should.ThrowAsync<NotFoundException>(async () => 
            {
                await manager.GetClubConfiguration(ClubConfigurationTestData.AggregateId, CancellationToken.None);
            });
        }
        #endregion

        #region Insert Club Information To Read Model Tests

        [Fact]
        public async Task ManagementAPIManager_InsertClubInformationToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            var domainEvent = ClubConfigurationTestData.GetClubConfigurationCreatedEvent();

            await manager.InsertClubInformationToReadModel(domainEvent, CancellationToken.None);

            var verifyContext = GetContext(databaseName);
            verifyContext.ClubInformation.Count().ShouldBe(1);
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            ClubConfigurationCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.InsertClubInformationToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            var context = GetContext(databaseName);
            context.ClubInformation.Add(new ClubInformation
            {
                EmailAddress = ClubConfigurationTestData.EmailAddress,
                Name = ClubConfigurationTestData.Name,
                AddressLine1 = ClubConfigurationTestData.AddressLine1,
                Town = ClubConfigurationTestData.Town,
                Region = ClubConfigurationTestData.Region,
                TelephoneNumber = ClubConfigurationTestData.TelephoneNumber,
                PostalCode = ClubConfigurationTestData.PostalCode,
                AddressLine2 = ClubConfigurationTestData.AddressLine2,
                ClubConfigurationId = ClubConfigurationTestData.AggregateId,
                WebSite = ClubConfigurationTestData.Website
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            ClubConfigurationCreatedEvent domainEvent = ClubConfigurationTestData.GetClubConfigurationCreatedEvent();

            Should.NotThrow(async () =>
            {
                await manager.InsertClubInformationToReadModel(domainEvent, CancellationToken.None);
            });
        }

        #endregion

        #region Get Club List Tests

        [Fact]
        public async Task ManagementAPIManager_GetClubList_ListOfClubsReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            
            var context = GetContext(databaseName);
            context.ClubInformation.Add(new ClubInformation
            {
                EmailAddress = ClubConfigurationTestData.EmailAddress,
                Name = ClubConfigurationTestData.Name,
                AddressLine1 = ClubConfigurationTestData.AddressLine1,
                Town = ClubConfigurationTestData.Town,
                Region = ClubConfigurationTestData.Region,
                TelephoneNumber = ClubConfigurationTestData.TelephoneNumber,
                PostalCode = ClubConfigurationTestData.PostalCode,
                AddressLine2 = ClubConfigurationTestData.AddressLine2,
                ClubConfigurationId = ClubConfigurationTestData.AggregateId,
                WebSite = ClubConfigurationTestData.Website
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            var result = await manager.GetClubList(CancellationToken.None);

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetClubList_NoClubsFound_ListOfClubsReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<ClubConfigurationAggregate>> clubRepository = new Mock<IAggregateRepository<ClubConfigurationAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver);

            var result = await manager.GetClubList(CancellationToken.None);

            result.ShouldBeEmpty();
        }

        #endregion
    }
}
