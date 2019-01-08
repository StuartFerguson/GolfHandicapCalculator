using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Database;
using ManagementAPI.Database.Models;
using ManagementAPI.GolfClub;
using ManagementAPI.GolfClub.DomainEvents;
using ManagementAPI.Player;
using ManagementAPI.Player.DomainEvents;
using ManagementAPI.Service.Manager;
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

            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object);

            var result = await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None);

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
            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver, playerRepository.Object);

            Should.ThrowAsync<ArgumentNullException>(async () => 
            {
                await manager.GetGolfClub(Guid.Empty, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

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

            var context = GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

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
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            

            var domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None);

            var verifyContext = GetContext(databaseName);
            verifyContext.GolfClub.Count().ShouldBe(1);
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

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
            var context = GetContext(databaseName);
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

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

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
            
            var context = GetContext(databaseName);
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

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetClubList_NoClubsFound_ListOfClubsReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldBeEmpty();
        }

        #endregion

        #region Insert Club Membership Request To Read Model Tests

        [Fact]
        public async Task ManagementAPIManager_InsertClubMembershipRequestToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);

            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate());

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var domainEvent = PlayerTestData.GetClubMembershipRequestedEvent();

            await manager.InsertClubMembershipRequestToReadModel(domainEvent, CancellationToken.None);

            var verifyContext = GetContext(databaseName);
            verifyContext.ClubMembershipRequest.Count().ShouldBe(1);
        }

        [Fact]
        public void ManagementAPIManager_InsertClubMembershipRequestToReadModel_ClubNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate);

            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var domainEvent = PlayerTestData.GetClubMembershipRequestedEvent();

            Should.Throw<InvalidOperationException>(async () =>
            {
                await manager.InsertClubMembershipRequestToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubMembershipRequestToReadModel_PlayerNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);

            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var domainEvent = PlayerTestData.GetClubMembershipRequestedEvent();

            Should.Throw<InvalidOperationException>(async () =>
            {
                await manager.InsertClubMembershipRequestToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubMembershipRequestToReadModel_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            ClubMembershipRequestedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.InsertClubMembershipRequestToReadModel(domainEvent, CancellationToken.None);
            });
        }
        
        [Fact]
        public void ManagementAPIManager_InsertClubMembershipRequestToReadModel_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate);

            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var domainEvent = PlayerTestData.GetClubMembershipRequestedEvent();

            context.ClubMembershipRequest.Add(new ClubMembershipRequest
            {
                ClubId = domainEvent.ClubId,
                MembershipRequestedDateAndTime = domainEvent.MembershipRequestedDateAndTime,
                MembershipRequestId = domainEvent.EventId,
                PlayerId = domainEvent.AggregateId
            });
            context.SaveChanges();

            Should.Throw<InvalidOperationException>(async () =>
            {
                await manager.InsertClubMembershipRequestToReadModel(domainEvent, CancellationToken.None);
            });
        }
        
        #endregion

        #region Get Pending Membership Requests Tests

        [Fact]
        public async Task ManagementAPIManager_GetPendingMembershipRequests_ListOfPendingMembershipRequestsReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);
            context.ClubMembershipRequest.Add(new ClubMembershipRequest
            {
                ClubId = PlayerTestData.ClubId,
                MembershipRequestedDateAndTime = PlayerTestData.MembershipRequestedDateAndTime,
                PlayerId = PlayerTestData.AggregateId,
                MembershipRequestId = Guid.NewGuid(),
                HandicapCategory = 1,
                Age = PlayerTestData.Age,
                Gender = PlayerTestData.Gender,
                Status = 0,
                FirstName = PlayerTestData.FirstName,
                MiddleName = PlayerTestData.MiddleName,
                PlayingHandicap = PlayerTestData.PlayingHandicapCat1,
                LastName = PlayerTestData.LastName,
                ExactHandicap = PlayerTestData.ExactHandicapCat1
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var result = await manager.GetPendingMembershipRequests(PlayerTestData.ClubId, CancellationToken.None);

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetPendingMembershipRequests_NoPendingRequestsFound_ListOfPendingMembershipRequestsReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var result = await manager.GetPendingMembershipRequests(PlayerTestData.ClubId, CancellationToken.None);

            result.ShouldBeEmpty();
        }

        #endregion

        #region Remove Club Membership Request To Read Model Tests

        [Fact]
        public async Task ManagementAPIManager_RemoveClubMembershipRequestFromReadModel_Accept_RecordRemovedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            context.ClubMembershipRequest.Add(new ClubMembershipRequest
            {
                ClubId = PlayerTestData.ClubId,
                MembershipRequestedDateAndTime = PlayerTestData.MembershipRequestedDateAndTime,
                PlayerId = PlayerTestData.AggregateId,
                MembershipRequestId = Guid.NewGuid(),
                HandicapCategory = 1,
                Age = PlayerTestData.Age,
                Gender = PlayerTestData.Gender,
                Status = 0,
                FirstName = PlayerTestData.FirstName,
                MiddleName = PlayerTestData.MiddleName,
                PlayingHandicap = PlayerTestData.PlayingHandicapCat1,
                LastName = PlayerTestData.LastName,
                ExactHandicap = PlayerTestData.ExactHandicapCat1
            });
            context.SaveChanges();


            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var domainEvent = PlayerTestData.GetClubMembershipApprovedEvent();

            await manager.RemoveClubMembershipRequestFromReadModel(domainEvent, CancellationToken.None);

            var verifyContext = GetContext(databaseName);
            verifyContext.ClubMembershipRequest.Count().ShouldBe(0);
        }

        [Fact]
        public async Task ManagementAPIManager_RemoveClubMembershipRequestFromReadModel_Reject_RecordRemovedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            context.ClubMembershipRequest.Add(new ClubMembershipRequest
            {
                ClubId = PlayerTestData.ClubId,
                MembershipRequestedDateAndTime = PlayerTestData.MembershipRequestedDateAndTime,
                PlayerId = PlayerTestData.AggregateId,
                MembershipRequestId = Guid.NewGuid(),
                HandicapCategory = 1,
                Age = PlayerTestData.Age,
                Gender = PlayerTestData.Gender,
                Status = 0,
                FirstName = PlayerTestData.FirstName,
                MiddleName = PlayerTestData.MiddleName,
                PlayingHandicap = PlayerTestData.PlayingHandicapCat1,
                LastName = PlayerTestData.LastName,
                ExactHandicap = PlayerTestData.ExactHandicapCat1
            });
            context.SaveChanges();


            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            var domainEvent = PlayerTestData.GetClubMembershipRejectedEvent();

            await manager.RemoveClubMembershipRequestFromReadModel(domainEvent, CancellationToken.None);

            var verifyContext = GetContext(databaseName);
            verifyContext.ClubMembershipRequest.Count().ShouldBe(0);
        }

        [Fact]
        public async Task ManagementAPIManager_RemoveClubMembershipRequestFromReadModel_Accept_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            ClubMembershipApprovedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.RemoveClubMembershipRequestFromReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_RemoveClubMembershipRequestFromReadModel_Reject_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            ClubMembershipRejectedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.RemoveClubMembershipRequestFromReadModel(domainEvent, CancellationToken.None);
            });
        }
        
        [Fact]
        public async Task ManagementAPIManager_RemoveClubMembershipRequestFromReadModel_Accept_RequestNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            ClubMembershipApprovedEvent domainEvent = PlayerTestData.GetClubMembershipApprovedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.RemoveClubMembershipRequestFromReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_RemoveClubMembershipRequestFromReadModel_Reject_RequestNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            var context = GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            var manager = new ManagmentAPIManager(clubRepository.Object, contextResolver,playerRepository.Object);

            ClubMembershipRejectedEvent domainEvent = PlayerTestData.GetClubMembershipRejectedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.RemoveClubMembershipRequestFromReadModel(domainEvent, CancellationToken.None);
            });
        }

        #endregion
    }
}
