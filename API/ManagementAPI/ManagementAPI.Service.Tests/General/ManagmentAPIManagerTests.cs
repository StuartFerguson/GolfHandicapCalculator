namespace ManagementAPI.Service.Tests.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using BusinessLogic.Manager;
    using BusinessLogic.Services.ExternalServices;
    using BusinessLogic.Services.ExternalServices.DataTransferObjects;
    using Database;
    using Database.Models;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using GolfClub;
    using GolfClubMembership;
    using ManagementAPI.GolfClub;
    using ManagementAPI.GolfClub.DomainEvents;
    using ManagementAPI.GolfClubMembership;
    using ManagementAPI.GolfClubMembership.DomainEvents;
    using ManagementAPI.Player;
    using ManagementAPI.Player.DomainEvents;
    using ManagementAPI.Tournament.DomainEvents;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Logging.Abstractions;
    using Moq;
    using Player;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;
    using Shouldly;
    using Tournament;
    using Xunit;

    public class ManagmentAPIManagerTests
    {
        public ManagmentAPIManagerTests()
        {
            Logger.Initialise(NullLogger.Instance);      
        }

        #region Methods
        
        [Fact]
        public async Task ManagementAPIManager_GetClubList_ListOfClubsReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<GetGolfClubResponse> result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetClubList_NoClubsFound_ListOfClubsReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<GetGolfClubResponse> result = await manager.GetGolfClubList(CancellationToken.None);

            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubMembersList_ClubHasNoMembers_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate);

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetGolfClubMembersList(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubMembersList_ClubNotCreated_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate);

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregate);

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetGolfClubMembersList(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubMembersList_MembersListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();
            golfClubMembershipRepository.Setup(x => x.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None))
                                        .ReturnsAsync(GolfClubMembershipTestData.GetCreatedGolfClubMembershipAggregateWithMultipleMembershipRequested);

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<GetGolfClubMembershipDetailsResponse> result = await manager.GetGolfClubMembersList(GolfClubTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetPlayersClubMemberships_MembershipListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.PlayerClubMembership.Add(new PlayerClubMembership
                                             {
                                                 AcceptedDateTime = GolfClubMembershipTestData.AcceptedDateAndTime,
                                                 PlayerId = GolfClubMembershipTestData.PlayerId,
                                                 MembershipNumber = GolfClubMembershipTestData.MembershipNumber,
                                                 GolfClubId = GolfClubMembershipTestData.AggregateId,
                                                 MembershipId = GolfClubMembershipTestData.MembershipId,
                                                 RejectionReason = null,
                                                 Status = GolfClubMembershipTestData.AcceptedStatus,
                                                 RejectedDateTime = null,
                                                 GolfClubName = GolfClubMembershipTestData.GolfClubName
                                             });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            List<ClubMembershipResponse> membershipList = await manager.GetPlayersClubMemberships(PlayerTestData.AggregateId, CancellationToken.None);

            membershipList.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetPlayersClubMemberships_NotMemberOfAnyClubs_MembershipListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetPlayersClubMemberships(PlayerTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_DuplicateRecord_NoErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubCreatedEvent domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            Should.NotThrow(async () => { await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertClubInformationToReadModel_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertClubInformationToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubCreatedEvent domainEvent = GolfClubTestData.GetGolfClubCreatedEvent();

            await manager.InsertGolfClubToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.GolfClub.Count().ShouldBe(1);
        }
        
        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.ThrowAsync<NotFoundException>(async () => { await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_GetGolfClub_GolfClubNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.ThrowAsync<NotFoundException>(async () => { await manager.GetGolfClub(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagmentAPIManager_GetGolfClub_GolfClubReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetCreatedGolfClubAggregate());

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

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
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.ThrowAsync<ArgumentNullException>(async () => { await manager.GetGolfClub(Guid.Empty, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_RegisterClubAdministrator_ClubAdministratorRegistered()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None)).ReturnsAsync(new RegisterUserResponse
                                                                                                                             {
                                                                                                                                 UserId = Guid.NewGuid()
                                                                                                                             });
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            RegisterClubAdministratorRequest request = GolfClubTestData.RegisterClubAdministratorRequest;

            Should.NotThrow(async () => { await manager.RegisterClubAdministrator(request, CancellationToken.None); });
        }

        [Fact]
        public void ManagmentAPIManager_RegisterClubAdministrator_ErrorCreatingUser_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();

            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.RegisterUser(It.IsAny<RegisterUserRequest>(), CancellationToken.None)).ThrowsAsync(new InvalidOperationException("Error"));
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            RegisterClubAdministratorRequest request = GolfClubTestData.RegisterClubAdministratorRequest;

            Should.Throw<Exception>(async () => { await manager.RegisterClubAdministrator(request, CancellationToken.None); });
        }

        private ManagementAPIReadModel GetContext(String databaseName)
        {
            DbContextOptionsBuilder<ManagementAPIReadModel> builder = new DbContextOptionsBuilder<ManagementAPIReadModel>()
                                                                      .UseInMemoryDatabase(databaseName)
                                                                      .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            ManagementAPIReadModel context = new ManagementAPIReadModel(builder.Options);

            return context;
        }

        [Fact]
        public async Task ManagmentAPIManager_GetPlayer_PlayerReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetPlayerDetailsResponse result = await manager.GetPlayerDetails(PlayerTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.EmailAddress.ShouldBe(PlayerTestData.EmailAddress);
            result.DateOfBirth.ShouldBe(PlayerTestData.DateOfBirth);
            result.ExactHandicap.ShouldBe(PlayerTestData.ExactHandicap);
            result.FirstName.ShouldBe(PlayerTestData.FirstName);
            result.FullName.ShouldBe(PlayerTestData.FullName);
            result.Gender.ShouldBe(PlayerTestData.Gender);
            result.HandicapCategory.ShouldBe(PlayerTestData.HandicapCategory);
            result.HasBeenRegistered.ShouldBeTrue();
            result.MiddleName.ShouldBe(PlayerTestData.MiddleName);
            result.LastName.ShouldBe(PlayerTestData.LastName);
            result.PlayingHandicap.ShouldBe(PlayerTestData.PlayingHandicap);
        }

        [Fact]
        public async Task ManagmentAPIManager_GetPlayer_PlayerNotFound_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () =>
                         {
                             await manager.GetPlayerDetails(PlayerTestData.AggregateId, CancellationToken.None);
                         });
        }

        [Fact]
        public async Task ManagementAPIManager_GetMeasuredCourseList_ListOfCoursesReturned()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetGolfClubAggregateWithMeasuredCourse);
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetMeasuredCourseListResponse measuredCourseList = await manager.GetMeasuredCourseList(GolfClubTestData.AggregateId, CancellationToken.None);

            measuredCourseList.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            measuredCourseList.MeasuredCourses.Count.ShouldBe(1);
            
            MeasuredCourseListResponse measuredCourse = measuredCourseList.MeasuredCourses.FirstOrDefault();
            measuredCourse.ShouldNotBeNull();

            MeasuredCourseDataTransferObject measuredCourseToCompare = GolfClubTestData.GetMeasuredCourseToAdd();

            measuredCourse.MeasuredCourseId.ShouldBe(measuredCourseToCompare.MeasuredCourseId);
            measuredCourse.Name.ShouldBe(measuredCourseToCompare.Name);
            measuredCourse.StandardScratchScore.ShouldBe(measuredCourseToCompare.StandardScratchScore);
            measuredCourse.TeeColour.ShouldBe(measuredCourseToCompare.TeeColour);
        }

        [Fact]
        public void ManagementAPIManager_GetMeasuredCourseList_GolfClubNotCreated_ErrorThrown()
        {
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            clubRepository.Setup(c => c.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetEmptyGolfClubAggregate);
            ManagementAPIReadModel context = this.GetContext(Guid.NewGuid().ToString("N"));

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();

            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<NotFoundException>(async () => { await manager.GetMeasuredCourseList(GolfClubTestData.AggregateId, CancellationToken.None); });
        }

        [Theory]
        [InlineData("", "middleName","familyName" )]
        [InlineData(null, "middleName", "familyName")]
        [InlineData("givenName", "", "familyName")]
        [InlineData("givenName", null, "familyName")]
        [InlineData("givenName", "middleName", "")]
        [InlineData("givenName", "middleName", null)]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_GolfClubAdministrator_RecordInsertedSuccessfully(String givenName, String middleName, String familyName)
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetClubAdministratorUserResponse(givenName, middleName, familyName));
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubAdministratorSecurityUserCreatedEvent domainEvent = GolfClubTestData.GetGolfClubAdministratorSecurityUserCreatedEvent();

            await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Users.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_GolfClubAdministrator_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.Users.Add(new User
            {
                GolfClubId = GolfClubTestData.AggregateId,
                UserId = GolfClubTestData.GolfClubAdministratorSecurityUserId,
                FamilyName = GolfClubTestData.RegisterClubAdministratorRequest.FamilyName,
                MiddleName = GolfClubTestData.RegisterClubAdministratorRequest.MiddleName,
                GivenName = GolfClubTestData.RegisterClubAdministratorRequest.GivenName,
                UserName = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                PhoneNumber = GolfClubTestData.RegisterClubAdministratorRequest.TelephoneNumber,
                Email = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                UserType = "Club Administrator"
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetClubAdministratorUserResponse(GolfClubTestData.CreateMatchSecretaryRequest.GivenName,
                                                                                                                                                               GolfClubTestData.CreateMatchSecretaryRequest.MiddleName,
                                                                                                                                                               GolfClubTestData.CreateMatchSecretaryRequest.FamilyName));
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = GolfClubTestData.GetMatchSecretarySecurityUserCreatedEvent();

            Should.NotThrow(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertUserRecordToReadModel_GolfClubAdministrator_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GolfClubAdministratorSecurityUserCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Theory]
        [InlineData("", "middleName", "familyName")]
        [InlineData(null, "middleName", "familyName")]
        [InlineData("givenName", "", "familyName")]
        [InlineData("givenName", null, "familyName")]
        [InlineData("givenName", "middleName", "")]
        [InlineData("givenName", "middleName", null)]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_MatchSecretary_RecordInsertedSuccessfully(String givenName, String middleName, String familyName)
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetMatchSecretaryUserResponse(givenName, middleName, familyName));
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = GolfClubTestData.GetMatchSecretarySecurityUserCreatedEvent();

            await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Users.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertUserRecordToReadModel_MatchSecretary_DuplicateRecord_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.Users.Add(new User
            {
                GolfClubId = GolfClubTestData.AggregateId,
                UserId = GolfClubTestData.MatchSecretarySecurityUserId,
                FamilyName = GolfClubTestData.CreateMatchSecretaryRequest.FamilyName,
                MiddleName = GolfClubTestData.CreateMatchSecretaryRequest.MiddleName,
                GivenName = GolfClubTestData.CreateMatchSecretaryRequest.GivenName,
                UserName = GolfClubTestData.CreateMatchSecretaryRequest.EmailAddress,
                PhoneNumber = GolfClubTestData.CreateMatchSecretaryRequest.TelephoneNumber,
                Email = GolfClubTestData.CreateMatchSecretaryRequest.EmailAddress,
                UserType = "Match Secretary"
            });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            securityService.Setup(s => s.GetUserById(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(GolfClubTestData.GetMatchSecretaryUserResponse(GolfClubTestData.CreateMatchSecretaryRequest.GivenName,
                                                                                                                                                               GolfClubTestData.CreateMatchSecretaryRequest.MiddleName,
                                                                                                                                                               GolfClubTestData.CreateMatchSecretaryRequest.FamilyName));
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = GolfClubTestData.GetMatchSecretarySecurityUserCreatedEvent();

            Should.NotThrow(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public void ManagementAPIManager_InsertUserRecordToReadModel_MatchSecretary_NullDomainEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            MatchSecretarySecurityUserCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertUserRecordToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubUserList_ListOfClubsReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.Users.Add(new User
                              {
                                  GolfClubId = GolfClubTestData.AggregateId,
                                  UserId = GolfClubTestData.GolfClubAdministratorSecurityUserId,
                                  FamilyName = GolfClubTestData.RegisterClubAdministratorRequest.FamilyName,
                                  MiddleName = GolfClubTestData.RegisterClubAdministratorRequest.MiddleName,
                                  GivenName = GolfClubTestData.RegisterClubAdministratorRequest.GivenName,
                                  UserName = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                                  PhoneNumber = GolfClubTestData.RegisterClubAdministratorRequest.TelephoneNumber,
                                  Email = GolfClubTestData.RegisterClubAdministratorRequest.EmailAddress,
                                  UserType = "Club Administrator"
                              });
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetGolfClubUserListResponse result = await manager.GetGolfClubUsers(GolfClubTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Users.ShouldNotBeEmpty();
            result.Users.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubUserList_NoUsersFound_ListOfUsersReturnedIsEmpty()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetGolfClubUserListResponse result = await manager.GetGolfClubUsers(GolfClubTestData.AggregateId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Users.ShouldBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_GetGolfClubUserList_InvalidGolfClubId_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();

            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            Should.Throw<ArgumentNullException>(async () => { await manager.GetGolfClubUsers(Guid.Empty, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestAcceptedEvent_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.PlayerClubMembership.Count().ShouldBe(1);
        }
        
        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestAcceptedEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async ()=>
                                                 {
                                                     await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);
                                                 });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestAcceptedEvent_GolfClubNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestAcceptedEvent_DuplicateRecord_NoErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            context.PlayerClubMembership.Add(new PlayerClubMembership
                                             {
                                                 GolfClubId = GolfClubTestData.AggregateId,
                                                 PlayerId = GolfClubTestData.PlayerId,
                                                 AcceptedDateTime = GolfClubTestData.MembershipAcceptedDateTime,
                                                 GolfClubName = GolfClubTestData.Name,
                                                 MembershipId = GolfClubTestData.MembershipId,
                                                 MembershipNumber = GolfClubTestData.MembershipNumber,
                                                 Status = GolfClubTestData.StatusAccepted
                                             });
            
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.NotThrow(async () =>
                                                {
                                                    await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);
                                                });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestRejectedEvent_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestRejectedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestRejectedEvent();

            await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.PlayerClubMembership.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestRejectedEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestRejectedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
                                                {
                                                    await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);
                                                });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestRejectedEvent_GolfClubNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestRejectedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestRejectedEvent();

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);
                                            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReadModel_ClubMembershipRequestRejectedEvent_DuplicateRecord_NoErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            context.PlayerClubMembership.Add(new PlayerClubMembership
            {
                GolfClubId = GolfClubTestData.AggregateId,
                PlayerId = GolfClubTestData.PlayerId,
                GolfClubName = GolfClubTestData.Name,
                MembershipId = GolfClubTestData.MembershipId,
                MembershipNumber = GolfClubTestData.MembershipNumber,
                RejectedDateTime = GolfClubTestData.MembershipRejectionDateTime,
                RejectionReason = GolfClubTestData.MembershipRejectionReason,
                Status = GolfClubTestData.StatusRejected
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestRejectedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestRejectedEvent();

            Should.NotThrow(async () =>
            {
                await manager.InsertPlayerMembershipToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReporting_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.NotThrow(async () =>
            {
                await manager.InsertPlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReporting_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.InsertPlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReporting_GolfClubNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.InsertPlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerMembershipToReporting_PlayerNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);

            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.InsertPlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerHandicapRecordToReporting_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.NotThrow(async () =>
            {
                await manager.InsertPlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerHandicapRecordToReporting_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.InsertPlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerHandicapRecordToReporting_GolfClubNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.Throw<NotFoundException>(async () =>
                                            {
                                                await manager.InsertPlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
                                            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerHandicapRecordToReporting_PlayerNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);

            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            ClubMembershipRequestAcceptedEvent domainEvent = GolfClubTestData.GetClubMembershipRequestAcceptedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.InsertPlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertTournamentToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCreatedEvent domainEvent = TournamentTestData.GetTournamentCreatedEvent();

            await manager.InsertTournamentToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Tournament.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertTournamentToReadModel_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCreatedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.InsertTournamentToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertTournamentToReadModel_GolfClubNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCreatedEvent domainEvent = TournamentTestData.GetTournamentCreatedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.InsertTournamentToReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertTournamentToReadModel_DuplicateRecord_NoErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            context.Tournament.Add(new Tournament
                                   {
                                       Format = TournamentTestData.TournamentFormat,
                                       GolfClubId = TournamentTestData.GolfClubId,
                                       Name = TournamentTestData.Name,
                                       TournamentDate = TournamentTestData.TournamentDate,
                                       MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                                       MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                                       PlayersSignedUpCount = 0,
                                       GolfClubName = TournamentTestData.GolfClubName,
                                       HasResultBeenProduced = false,
                                       MeasuredCourseName = String.Empty,
                                       MeasuredCourseTeeColour = String.Empty,
                                       PlayerCategory = TournamentTestData.PlayerCategory,
                                       PlayersScoresRecordedCount = 0,
                                       TournamentId = TournamentTestData.AggregateId

                                   });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCreatedEvent domainEvent = TournamentTestData.GetTournamentCreatedEvent();

            Should.NotThrow(async () => { await manager.InsertTournamentToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerTournamentScoreToReadModel_RecordInsertedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentResultForPlayerScoreProducedEvent domainEvent = TournamentTestData.GetTournamentResultForPlayerScoreProducedEvent();

            await manager.InsertPlayerTournamentScoreToReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.TournamentResultForPlayerScore.Count().ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerTournamentScoreToReadModel_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentResultForPlayerScoreProducedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () => { await manager.InsertPlayerTournamentScoreToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_InsertPlayerTournamentScoreToReadModel_DuplicateRecord_NoErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.TournamentResultForPlayerScore.Add(new TournamentResultForPlayerScore
                                                       {
                PlayerId = TournamentTestData.PlayerId,
                Tournament = new Tournament
                             {
                    Format = TournamentTestData.TournamentFormat,
                    GolfClubId = TournamentTestData.GolfClubId,
                    Name = TournamentTestData.Name,
                    TournamentDate = TournamentTestData.TournamentDate,
                    MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                    MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                    PlayersSignedUpCount = 0,
                    GolfClubName = TournamentTestData.GolfClubName,
                    HasResultBeenProduced = false,
                    MeasuredCourseName = String.Empty,
                    MeasuredCourseTeeColour = String.Empty,
                    PlayerCategory = TournamentTestData.PlayerCategory,
                    PlayersScoresRecordedCount = 0,
                    TournamentId = TournamentTestData.AggregateId
                },
                Division = TournamentTestData.Division,
                DivisionPosition = TournamentTestData.DivisionPosition,
                GrossScore = TournamentTestData.GrossScore,
                NetScore = TournamentTestData.NetScore,
                PlayingHandicap = TournamentTestData.PlayingHandicap,
                TournamentId = TournamentTestData.AggregateId,
                Last3Holes = TournamentTestData.Last3HolesScore,
                Last6Holes = TournamentTestData.Last6HolesScore,
                Last9Holes = TournamentTestData.Last9HolesScore,
                TournamentResultForPlayerId = Guid.NewGuid()

                                                       });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentResultForPlayerScoreProducedEvent domainEvent = TournamentTestData.GetTournamentResultForPlayerScoreProducedEvent();

            Should.NotThrow(async () => { await manager.InsertPlayerTournamentScoreToReadModel(domainEvent, CancellationToken.None); });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentInReadModel_PlayerSignedUpEvent_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.Tournament.Add(new Tournament
                                   {
                Format = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                Name = TournamentTestData.Name,
                TournamentDate = TournamentTestData.TournamentDate,
                MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                PlayersSignedUpCount = 0,
                GolfClubName = TournamentTestData.GolfClubName,
                HasResultBeenProduced = false,
                MeasuredCourseName = String.Empty,
                MeasuredCourseTeeColour = String.Empty,
                PlayerCategory = TournamentTestData.PlayerCategory,
                PlayersScoresRecordedCount = 0,
                TournamentId = TournamentTestData.AggregateId
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            PlayerSignedUpEvent domainEvent = TournamentTestData.GetPlayerSignedUpEvent();

            await manager.UpdateTournamentInReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Tournament.Count().ShouldBe(1);
            verifyContext.Tournament.First().PlayersSignedUpCount.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentInReadModel_PlayerSignedUpEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            PlayerSignedUpEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdateTournamentInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentInReadModel_PlayerSignedUpEvent_TournamentNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            PlayerSignedUpEvent domainEvent = TournamentTestData.GetPlayerSignedUpEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.UpdateTournamentInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentInReadModel_PlayerScoreRecordedEvent_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.Tournament.Add(new Tournament
            {
                Format = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                Name = TournamentTestData.Name,
                TournamentDate = TournamentTestData.TournamentDate,
                MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                PlayersSignedUpCount = 0,
                GolfClubName = TournamentTestData.GolfClubName,
                HasResultBeenProduced = false,
                MeasuredCourseName = String.Empty,
                MeasuredCourseTeeColour = String.Empty,
                PlayerCategory = TournamentTestData.PlayerCategory,
                PlayersScoresRecordedCount = 0,
                TournamentId = TournamentTestData.AggregateId
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            PlayerScoreRecordedEvent domainEvent = TournamentTestData.GetPlayerScoreRecordedEvent();

            await manager.UpdateTournamentInReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Tournament.Count().ShouldBe(1);
            verifyContext.Tournament.First().PlayersScoresRecordedCount.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentInReadModel_PlayerScoreRecordedEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            PlayerScoreRecordedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdateTournamentInReadModel(domainEvent, CancellationToken.None);
            });
        }
        
        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentInReadModel_PlayerScoreRecordedEvent_TournamentNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            PlayerScoreRecordedEvent domainEvent = TournamentTestData.GetPlayerScoreRecordedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.UpdateTournamentInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentCompletedEvent_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.Tournament.Add(new Tournament
            {
                Format = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                Name = TournamentTestData.Name,
                TournamentDate = TournamentTestData.TournamentDate,
                MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                PlayersSignedUpCount = 0,
                GolfClubName = TournamentTestData.GolfClubName,
                HasResultBeenProduced = false,
                MeasuredCourseName = String.Empty,
                MeasuredCourseTeeColour = String.Empty,
                PlayerCategory = TournamentTestData.PlayerCategory,
                PlayersScoresRecordedCount = 0,
                TournamentId = TournamentTestData.AggregateId
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCompletedEvent domainEvent = TournamentTestData.GetTournamentCompletedEvent();

            await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Tournament.Count().ShouldBe(1);
            verifyContext.Tournament.First().HasBeenCompleted.ShouldBeTrue();
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentCompletedEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCompletedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentCompletedEvent_TournamentNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCompletedEvent domainEvent = TournamentTestData.GetTournamentCompletedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentCancelledEvent_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.Tournament.Add(new Tournament
            {
                Format = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                Name = TournamentTestData.Name,
                TournamentDate = TournamentTestData.TournamentDate,
                MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                PlayersSignedUpCount = 0,
                GolfClubName = TournamentTestData.GolfClubName,
                HasResultBeenProduced = false,
                MeasuredCourseName = String.Empty,
                MeasuredCourseTeeColour = String.Empty,
                PlayerCategory = TournamentTestData.PlayerCategory,
                PlayersScoresRecordedCount = 0,
                TournamentId = TournamentTestData.AggregateId
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCancelledEvent domainEvent = TournamentTestData.GetTournamentCancelledEvent();

            await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Tournament.Count().ShouldBe(1);
            verifyContext.Tournament.First().HasBeenCancelled.ShouldBeTrue();
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentCancelledEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCancelledEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentCancelledEvent_TournamentNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentCancelledEvent domainEvent = TournamentTestData.GetTournamentCancelledEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);
            });
        }


        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentResultProducedEvent_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.Tournament.Add(new Tournament
            {
                Format = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                Name = TournamentTestData.Name,
                TournamentDate = TournamentTestData.TournamentDate,
                MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                PlayersSignedUpCount = 0,
                GolfClubName = TournamentTestData.GolfClubName,
                HasResultBeenProduced = false,
                MeasuredCourseName = String.Empty,
                MeasuredCourseTeeColour = String.Empty,
                PlayerCategory = TournamentTestData.PlayerCategory,
                PlayersScoresRecordedCount = 0,
                TournamentId = TournamentTestData.AggregateId
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentResultProducedEvent domainEvent = TournamentTestData.GetTournamentResultProducedEvent();

            await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);

            ManagementAPIReadModel verifyContext = this.GetContext(databaseName);
            verifyContext.Tournament.Count().ShouldBe(1);
            verifyContext.Tournament.First().HasResultBeenProduced.ShouldBeTrue();
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentResultProducedEvent_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentResultProducedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdateTournamentStatusInReadModel_TournamentResultProducedEvent_TournamentNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClub.Add(new GolfClub
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
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            TournamentResultProducedEvent domainEvent = TournamentTestData.GetTournamentResultProducedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.UpdateTournamentStatusInReadModel(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_GetTournamentList_ListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.Tournament.Add(new Tournament
            {
                Format = TournamentTestData.TournamentFormat,
                GolfClubId = TournamentTestData.GolfClubId,
                Name = TournamentTestData.Name,
                TournamentDate = TournamentTestData.TournamentDate,
                MeasuredCourseSSS = TournamentTestData.MeasuredCourseSSS,
                MeasuredCourseId = TournamentTestData.MeasuredCourseId,
                PlayersSignedUpCount = 0,
                GolfClubName = TournamentTestData.GolfClubName,
                HasResultBeenProduced = false,
                MeasuredCourseName = String.Empty,
                MeasuredCourseTeeColour = String.Empty,
                PlayerCategory = TournamentTestData.PlayerCategory,
                PlayersScoresRecordedCount = 0,
                TournamentId = TournamentTestData.AggregateId
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetTournamentListResponse result = await manager.GetTournamentList(TournamentTestData.GolfClubId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Tournaments.ShouldNotBeNull();
            result.Tournaments.Count.ShouldBe(1);
        }

        [Fact]
        public async Task ManagementAPIManager_GetTournamentList_NoTournaments_EmptyListReturned()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            GetTournamentListResponse result = await manager.GetTournamentList(TournamentTestData.GolfClubId, CancellationToken.None);

            result.ShouldNotBeNull();
            result.Tournaments.ShouldNotBeNull();
            result.Tournaments.ShouldBeEmpty();
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerMembershipToReporting_SingleRecord_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClubMembershipReporting.Add(new GolfClubMembershipReporting
                                                    {
                                                        DateOfBirth = PlayerTestData.DateOfBirth,
                                                        GolfClubId = PlayerTestData.GolfClubId,
                                                        PlayerId = PlayerTestData.AggregateId,
                                                        HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                                                        DateJoined = PlayerTestData.MembershipAcceptedDateTime,
                                                        GolfClubName = PlayerTestData.GolfClubName,
                                                        PlayerGender = PlayerTestData.Gender,
                                                        PlayerName = PlayerTestData.FullName
                                                    });

            context.SaveChanges();
            
            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();
            
            Should.NotThrow(async () =>
            {
                await manager.UpdatePlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerMembershipToReporting_MultipleRecords_RecordsUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.GolfClubMembershipReporting.Add(new GolfClubMembershipReporting
            {
                DateOfBirth = PlayerTestData.DateOfBirth,
                GolfClubId = PlayerTestData.GolfClubId,
                PlayerId = PlayerTestData.AggregateId,
                HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                DateJoined = PlayerTestData.MembershipAcceptedDateTime,
                GolfClubName = PlayerTestData.GolfClubName,
                PlayerGender = PlayerTestData.Gender,
                PlayerName = PlayerTestData.FullName
            });

            context.GolfClubMembershipReporting.Add(new GolfClubMembershipReporting
                                                    {
                                                        DateOfBirth = PlayerTestData.DateOfBirth,
                                                        GolfClubId = PlayerTestData.GolfClubId2,
                                                        PlayerId = PlayerTestData.AggregateId,
                                                        HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                                                        DateJoined = PlayerTestData.MembershipAcceptedDateTime,
                                                        GolfClubName = PlayerTestData.GolfClubName2,
                                                        PlayerGender = PlayerTestData.Gender,
                                                        PlayerName = PlayerTestData.FullName
                                                    });


            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.NotThrow(async () =>
            {
                await manager.UpdatePlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }


        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerMembershipToReporting_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdatePlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerMembershipToReporting_PlayerReportingRecordNotFound_ErrorNotThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.NotThrow(async () =>
            {
                await manager.UpdatePlayerMembershipToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerMembershipToReporting_PlayerAggregateNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.GolfClubMembershipReporting.Add(new GolfClubMembershipReporting
                                                    {
                                                        DateOfBirth = PlayerTestData.DateOfBirth,
                                                        GolfClubId = PlayerTestData.GolfClubId,
                                                        PlayerId = PlayerTestData.AggregateId,
                                                        HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                                                        DateJoined = PlayerTestData.MembershipAcceptedDateTime,
                                                        GolfClubName = PlayerTestData.GolfClubName,
                                                        PlayerGender = PlayerTestData.Gender,
                                                        PlayerName = PlayerTestData.FullName
                                                    });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.Throw<NotFoundException>(async () =>
                                                {
                                                    await manager.UpdatePlayerMembershipToReporting(domainEvent, CancellationToken.None);
                                                });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerHandicapRecordToReporting_SingleRecord_RecordUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.PlayerHandicapListReporting.Add(new PlayerHandicapListReporting
            {
                GolfClubId = PlayerTestData.GolfClubId,
                PlayerId = PlayerTestData.AggregateId,
                HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                PlayerName = PlayerTestData.FullName,
                PlayingHandicap = 10,
                ExactHandicap = 10.0m
            });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.NotThrow(async () =>
            {
                await manager.UpdatePlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerHandicapRecordToReporting_MultipleRecords_RecordsUpdatedSuccessfully()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.PlayerHandicapListReporting.Add(new PlayerHandicapListReporting
                                                    {
                                                        GolfClubId = PlayerTestData.GolfClubId,
                                                        PlayerId = PlayerTestData.AggregateId,
                                                        HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                                                        PlayerName = PlayerTestData.FullName,
                                                        PlayingHandicap = 10,
                                                        ExactHandicap = 10.0m
                                                    });

            context.PlayerHandicapListReporting.Add(new PlayerHandicapListReporting
                                                    {
                                                        GolfClubId = PlayerTestData.GolfClubId2,
                                                        PlayerId = PlayerTestData.AggregateId,
                                                        HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                                                        PlayerName = PlayerTestData.FullName,
                                                        PlayingHandicap = 10,
                                                        ExactHandicap = 10.0m
                                                    });


            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.NotThrow(async () =>
            {
                await manager.UpdatePlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerHandicapRecordToReporting_NullEvent_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);
            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetRegisteredPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = null;

            Should.Throw<ArgumentNullException>(async () =>
            {
                await manager.UpdatePlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerHandicapRecordToReporting_PlayerHandicapReportingRecordNotFound_ErrorNotThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.NotThrow(async () =>
            {
                await manager.UpdatePlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        [Fact]
        public async Task ManagementAPIManager_UpdatePlayerHandicapRecordToReporting_PlayerAggregateNotFound_ErrorThrown()
        {
            String databaseName = Guid.NewGuid().ToString("N");
            Mock<IAggregateRepository<GolfClubAggregate>> clubRepository = new Mock<IAggregateRepository<GolfClubAggregate>>();
            ManagementAPIReadModel context = this.GetContext(databaseName);

            context.PlayerHandicapListReporting.Add(new PlayerHandicapListReporting
                                                    {
                                                        GolfClubId = PlayerTestData.GolfClubId,
                                                        PlayerId = PlayerTestData.AggregateId,
                                                        HandicapCategory = PlayerTestData.HandicapCategoryCat2,
                                                        PlayerName = PlayerTestData.FullName,
                                                        PlayingHandicap = 10,
                                                        ExactHandicap = 10.0m
                                                    });

            context.SaveChanges();

            Func<ManagementAPIReadModel> contextResolver = () => { return context; };

            Mock<IAggregateRepository<PlayerAggregate>> playerRepository = new Mock<IAggregateRepository<PlayerAggregate>>();
            playerRepository.Setup(p => p.GetLatestVersion(It.IsAny<Guid>(), CancellationToken.None)).ReturnsAsync(PlayerTestData.GetEmptyPlayerAggregate);
            Mock<ISecurityService> securityService = new Mock<ISecurityService>();
            Mock<IAggregateRepository<GolfClubMembershipAggregate>> golfClubMembershipRepository = new Mock<IAggregateRepository<GolfClubMembershipAggregate>>();

            ManagementAPIManager manager = new ManagementAPIManager(clubRepository.Object,
                                                                    contextResolver,
                                                                    playerRepository.Object,
                                                                    securityService.Object,
                                                                    golfClubMembershipRepository.Object);

            HandicapAdjustedEvent domainEvent = PlayerTestData.GetHandicapAdjustedEvent();

            Should.Throw<NotFoundException>(async () =>
            {
                await manager.UpdatePlayerHandicapRecordToReporting(domainEvent, CancellationToken.None);
            });
        }

        #endregion
    }
}