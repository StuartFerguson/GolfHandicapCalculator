using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Database;
using ManagementAPI.Database.Models;
using ManagementAPI.GolfClub;
using ManagementAPI.GolfClub.DomainEvents;
using ManagementAPI.Player;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Manager;
using ManagementAPI.Service.Services;
using ManagementAPI.Service.Services.DataTransferObjects;
using ManagementAPI.Service.Tests.GolfClub;
using ManagementAPI.Service.Tests.Player;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using Shared.EventStore;
using Shared.Exceptions;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.General
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
        public async Task ManagmentAPIManager_GetGolfClub_GolfClubReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object,
                securityService.Object);

            GetGolfClubResponse result = await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(GolfClubTestData.Name);
            result.AddressLine1.ShouldBe(GolfClubTestData.AddressLine1);
            result.AddressLine2.ShouldBe(GolfClubTestData.AddressLine2);
            result.Town.ShouldBe(GolfClubTestData.Town);
            result.Region.ShouldBe(GolfClubTestData.Region);
            result.PostalCode.ShouldBe(GolfClubTestData.PostalCode);
            result.TelephoneNumber.ShouldBe(GolfClubTestData.TelephoneNumber);
            result.Website.ShouldBe(GolfClubTestData.Website);
            result.EmailAddress.ShouldBe(GolfClubTestData.EmailAddress);
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_InvalidGolfClubId_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            Should.ThrowAsync<ArgumentNullException>(async () => 
            {
                await manager.GetGolfClub(Guid.Empty, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            Should.ThrowAsync<NotFoundException>(async () => 
            {
                await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate());

            ManagementAPIReadModel context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            Should.ThrowAsync<NotFoundException>(async () => 
            {
                await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None);
            });
        }
        #endregion

        #region Insert Club Information To Read Model Tests

        [Fact]
        public async Task ManagementAPIManager_InsertClubInformationToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            GolfClubCreatedEvent domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = GetContext(databaseName);
            verifyContext.GolfClub.Count().ShouldBe(1);
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            GolfClubCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = GetContext(databaseName);
            context.GolfClub.Add(new Database.Models.GolfClub
            {
                EmailAddress = GolfClubTestData.EmailAddress,
                Name = GolfClubTestData.Name,
                AddressLine1 = GolfClubTestData.AddressLine1,
                Town = GolfClubTestData.Town,
                Region = GolfClubTestData.Region,
                TelephoneNumber = GolfClubTestData.TelephoneNumber,
                PostalCode = GolfClubTestData.PostalCode,
                AddressLine2 = GolfClubTestData.AddressLine2,
                GolfClubId = GolfClubTestData.AggregateId,
                WebSite = GolfClubTestData.Website
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            GolfClubCreatedEvent domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            Should.NotThrow(async () =>
            {
                await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None);
            });
        }

        #endregion

        #region Get Club List Tests

        [Fact]
        public async Task ManagementAPIManager_GetClubList_ListOfClubsReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = GetContext(databaseName);
            context.GolfClub.Add(new Database.Models.GolfClub
            {
                EmailAddress = GolfClubTestData.EmailAddress,
                Name = GolfClubTestData.Name,
                AddressLine1 = GolfClubTestData.AddressLine1,
                Town = GolfClubTestData.Town,
                Region = GolfClubTestData.Region,
                TelephoneNumber = GolfClubTestData.TelephoneNumber,
                PostalCode = GolfClubTestData.PostalCode,
                AddressLine2 = GolfClubTestData.AddressLine2,
                GolfClubId = GolfClubTestData.AggregateId,
                WebSite = GolfClubTestData.Website
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            List<GetGolfClubResponse> result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetClubList_NoClubsFound_ListOfClubsReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object, securityService.Object);

            List<GetGolfClubResponse> result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldBeEmpty();
        }

        #endregion

        #region Register Club Administrator Tests

        [Fact]
        public void ManagmentAPIManager_RegisterClubAdministrator_ClubAdministratorRegistered()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();
            securityService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ReturnsAsync(new RegisterUserResponse
                {
                    UserId = Guid.NewGuid()
                });

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object,
                securityService.Object);

            RegisterClubAdministratorRequest request = GolfClubTestData.RegisterClubAdministratorRequest;

            Should.NotThrow(async () => { await manager.RegisterClubAdministrator(request, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_RegisterClubAdministrator_ErrorCreatingUser_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            Mock<IOAuth2SecurityService> securityService = new Mock<IOAuth2SecurityService>();
            securityService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None))
                .ThrowsAsync(new InvalidOperationException("Error"));

            ManagmentAPIManager manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object,
                securityService.Object);

            RegisterClubAdministratorRequest request = GolfClubTestData.RegisterClubAdministratorRequest;

            Should.Throw<Exception>(async () => { await manager.RegisterClubAdministrator(request, CancellationToken.None); });
        }

        #endregion
    }
}
