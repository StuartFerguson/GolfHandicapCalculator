namespace ManagementAPI.BusinessLogic.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Database;
    using Database.Models;
    using GolfClub;
    using GolfClub.DomainEvents;
    using GolfClubMembership;
    using GolfClubMembership.DomainEvents;
    using IdentityModel;
    using Microsoft.EntityFrameworkCore;
    using Player;
    using Player.DomainEvents;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Services.ExternalServices;
    using Services.ExternalServices.DataTransferObjects;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;
    using Tournament.DomainEvents;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="IManagmentAPIManager" />
    public class ManagementAPIManager : IManagmentAPIManager
    {
        #region Fields

        /// <summary>
        /// The golf club membership repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubMembershipAggregate> GolfClubMembershipRepository;

        /// <summary>
        /// The club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The read model resolver
        /// </summary>
        private readonly Func<ManagementAPIReadModel> ReadModelResolver;

        /// <summary>
        /// The security service
        /// </summary>
        private readonly ISecurityService SecurityService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPIManager" /> class.
        /// </summary>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="readModelResolver">The read model resolver.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="securityService">The security service.</param>
        /// <param name="golfClubMembershipRepository">The golf club membership repository.</param>
        public ManagementAPIManager(IAggregateRepository<GolfClubAggregate> golfClubRepository,
                                    Func<ManagementAPIReadModel> readModelResolver,
                                    IAggregateRepository<PlayerAggregate> playerRepository,
                                    ISecurityService securityService,
                                    IAggregateRepository<GolfClubMembershipAggregate> golfClubMembershipRepository)
        {
            this.GolfClubRepository = golfClubRepository;
            this.ReadModelResolver = readModelResolver;
            this.PlayerRepository = playerRepository;
            this.SecurityService = securityService;
            this.GolfClubMembershipRepository = golfClubMembershipRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the club configuration.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club not found for Golf Club Id [{golfClubId}</exception>
        public async Task<GetGolfClubResponse> GetGolfClub(Guid golfClubId,
                                                           CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "A Golf Club Id must be provided to retrieve a golf club");

            GetGolfClubResponse result = null;

            // Find the club configuration by id
            GolfClubAggregate golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            // Check we have found the club configuration
            if (!golfClub.HasBeenCreated)
            {
                throw new NotFoundException($"Golf Club not found for Golf Club Id [{golfClubId}]");
            }

            // We have found the club configuration
            result = new GetGolfClubResponse
                     {
                         AddressLine1 = golfClub.AddressLine1,
                         EmailAddress = golfClub.EmailAddress,
                         PostalCode = golfClub.PostalCode,
                         Name = golfClub.Name,
                         Town = golfClub.Town,
                         Website = golfClub.Website,
                         Region = golfClub.Region,
                         TelephoneNumber = golfClub.TelephoneNumber,
                         AddressLine2 = golfClub.AddressLine2,
                         Id = golfClub.AggregateId
                     };

            return result;
        }

        /// <summary>
        /// Gets the club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubResponse>> GetGolfClubList(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> result = new List<GetGolfClubResponse>();

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<GolfClub> golfClubs = await context.GolfClub.ToListAsync(cancellationToken);

                foreach (GolfClub golfClub in golfClubs)
                {
                    result.Add(new GetGolfClubResponse
                               {
                                   AddressLine1 = golfClub.AddressLine1,
                                   EmailAddress = golfClub.EmailAddress,
                                   Name = golfClub.Name,
                                   Town = golfClub.Town,
                                   Website = golfClub.WebSite,
                                   Region = golfClub.Region,
                                   TelephoneNumber = golfClub.TelephoneNumber,
                                   PostalCode = golfClub.PostalCode,
                                   AddressLine2 = golfClub.AddressLine2,
                                   Id = golfClub.GolfClubId
                               });
                }

                // Order the result by name
                result = result.OrderBy(g => g.Name).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets the golf club members list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club not found with Id {golfClubId}</exception>
        public async Task<List<GetGolfClubMembershipDetailsResponse>> GetGolfClubMembersList(Guid golfClubId,
                                                                                             CancellationToken cancellationToken)
        {
            List<GetGolfClubMembershipDetailsResponse> result = new List<GetGolfClubMembershipDetailsResponse>();

            // Rehydrate the Golf Club
            GolfClubAggregate golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            if (!golfClub.HasBeenCreated)
            {
                throw new NotFoundException($"Golf Club not found with Id {golfClubId}");
            }

            // Rehydrate the Golf Club Membership
            GolfClubMembershipAggregate golfClubMembership = await this.GolfClubMembershipRepository.GetLatestVersion(golfClubId, cancellationToken);

            // Translate
            List<MembershipDataTransferObject> membershipList = golfClubMembership.GetMemberships();

            foreach (MembershipDataTransferObject membership in membershipList)
            {
                result.Add(new GetGolfClubMembershipDetailsResponse
                           {
                               GolfClubId = golfClub.AggregateId,
                               PlayerId = membership.PlayerId,
                               PlayerGender = membership.PlayerGender,
                               PlayerDateOfBirth = membership.PlayerDateOfBirth.ToString("dd/MM/yyyy"),
                               PlayerFullName = membership.PlayerFullName,
                               MembershipNumber = membership.MembershipNumber,
                               MembershipStatus = (MembershipStatus)membership.Status,
                               Name = golfClub.Name
                           });
            }

            // Return
            return result;
        }

        /// <summary>
        /// Gets the golf club users.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetGolfClubUserListResponse> GetGolfClubUsers(Guid golfClubId,
                                                                        CancellationToken cancellationToken)
        {
            GetGolfClubUserListResponse response = new GetGolfClubUserListResponse
                                                   {
                                                       Users = new List<GolfClubUserResponse>()
                                                   };

            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "Golf Club Id cannot be empty GUID");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<User> userList = await context.Users.Where(u => u.GolfClubId == golfClubId).ToListAsync();

                foreach (User user in userList)
                {
                    response.Users.Add(new GolfClubUserResponse
                                       {
                                           FamilyName = user.FamilyName,
                                           MiddleName = user.MiddleName,
                                           GivenName = user.GivenName,
                                           UserName = user.UserName,
                                           UserId = user.UserId,
                                           PhoneNumber = user.PhoneNumber,
                                           Email = user.Email,
                                           UserType = user.UserType,
                                           GolfClubId = user.GolfClubId
                                       });
                }
            }

            return response;
        }

        /// <summary>
        /// Gets the measured course list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club not found for Golf Club Id [{golfClubId}</exception>
        public async Task<GetMeasuredCourseListResponse> GetMeasuredCourseList(Guid golfClubId,
                                                                               CancellationToken cancellationToken)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "A Golf Club Id must be provided to retrieve a golf club");

            GetMeasuredCourseListResponse result = new GetMeasuredCourseListResponse
                                                   {
                                                       MeasuredCourses = new List<MeasuredCourseListResponse>(),
                                                       GolfClubId = golfClubId
                                                   };

            // Find the club configuration by id
            GolfClubAggregate golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            // Check we have found the club configuration
            if (!golfClub.HasBeenCreated)
            {
                throw new NotFoundException($"Golf Club not found for Golf Club Id [{golfClubId}]");
            }

            List<MeasuredCourseDataTransferObject> measuredCourses = golfClub.GetMeasuredCourses();

            foreach (MeasuredCourseDataTransferObject measuredCourseDataTransferObject in measuredCourses)
            {
                result.MeasuredCourses.Add(new MeasuredCourseListResponse
                                           {
                                               MeasuredCourseId = measuredCourseDataTransferObject.MeasuredCourseId,
                                               Name = measuredCourseDataTransferObject.Name,
                                               StandardScratchScore = measuredCourseDataTransferObject.StandardScratchScore,
                                               TeeColour = measuredCourseDataTransferObject.TeeColour
                                           });
            }

            return result;
        }

        /// <summary>
        /// Gets the player details.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Player not found with Id {playerId}</exception>
        public async Task<GetPlayerDetailsResponse> GetPlayerDetails(Guid playerId,
                                                                     CancellationToken cancellationToken)
        {
            GetPlayerDetailsResponse result = null;

            // Rehydrate the player
            PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(playerId, cancellationToken);

            if (!player.HasBeenRegistered)
            {
                throw new NotFoundException($"Player not found with Id {playerId}");
            }

            // Translate 
            result = new GetPlayerDetailsResponse
                     {
                         DateOfBirth = player.DateOfBirth,
                         FullName = player.FullName,
                         EmailAddress = player.EmailAddress,
                         Gender = player.Gender,
                         HasBeenRegistered = player.HasBeenRegistered,
                         FirstName = player.FirstName,
                         LastName = player.LastName,
                         MiddleName = player.MiddleName,
                         ExactHandicap = player.ExactHandicap,
                         HandicapCategory = player.HandicapCategory,
                         PlayingHandicap = player.PlayingHandicap
                     };

            return result;
        }

        /// <summary>
        /// Gets the players club memberships.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No club memberships found for Player Id {playerId}</exception>
        public async Task<List<ClubMembershipResponse>> GetPlayersClubMemberships(Guid playerId,
                                                                                  CancellationToken cancellationToken)
        {
            List<ClubMembershipResponse> result = new List<ClubMembershipResponse>();

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<PlayerClubMembership> debugList = await context.PlayerClubMembership.ToListAsync(cancellationToken);
                foreach (PlayerClubMembership playerClubMembership in debugList)
                {
                    Logger.LogDebug($"Player Id {playerClubMembership.PlayerId} Golf Club Id {playerClubMembership.GolfClubId}");
                }

                List<PlayerClubMembership> membershipList = await context.PlayerClubMembership.Where(p => p.PlayerId == playerId).ToListAsync(cancellationToken);

                if (membershipList.Count == 0)
                {
                    throw new NotFoundException($"No club memberships found for Player Id {playerId}");
                }

                foreach (PlayerClubMembership playerClubMembership in membershipList)
                {
                    result.Add(new ClubMembershipResponse
                               {
                                   MembershipNumber = playerClubMembership.MembershipNumber,
                                   MembershipId = playerClubMembership.MembershipId,
                                   GolfClubId = playerClubMembership.GolfClubId,
                                   RejectionReason = playerClubMembership.RejectionReason,
                                   Status = (MembershipStatus)playerClubMembership.Status,
                                   RejectedDateTime = playerClubMembership.RejectedDateTime,
                                   AcceptedDateTime = playerClubMembership.AcceptedDateTime,
                                   GolfClubName = playerClubMembership.GolfClubName
                               });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the player signed up tournaments.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<PlayerSignedUpTournamentsResponse> GetPlayerSignedUpTournaments(Guid playerId,
                                                                                          CancellationToken cancellationToken)
        {
            PlayerSignedUpTournamentsResponse result = new PlayerSignedUpTournamentsResponse();

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<PlayerTournamentSignUp> playerTournamentSignUps =
                    await context.PlayerTournamentSignUps.Where(p => p.ScoreEntered == false && p.PlayerId == playerId).ToListAsync(cancellationToken);

                foreach (PlayerTournamentSignUp playerTournamentSignUp in playerTournamentSignUps)
                {
                    result.PlayerSignedUpTournaments.Add(new PlayerSignedUpTournament
                                                         {
                                                             TournamentDate = playerTournamentSignUp.TournamentDate,
                                                             GolfClubId = playerTournamentSignUp.GolfClubId,
                                                             MeasuredCourseId = playerTournamentSignUp.MeasuredCourseId,
                                                             TournamentId = playerTournamentSignUp.TournamentId,
                                                             TournamentFormat = (TournamentFormat)playerTournamentSignUp.TournamentFormat,
                                                             PlayerId = playerTournamentSignUp.PlayerId,
                                                             MeasuredCourseName = playerTournamentSignUp.MeasuredCourseName,
                                                             GolfClubName = playerTournamentSignUp.GolfClubName,
                                                             MeasuredCourseTeeColour = playerTournamentSignUp.MeasuredCourseTeeColour,
                                                             TournamentName = playerTournamentSignUp.TournamentName
                                                         });
                }

                // Order the result by name
                result.PlayerSignedUpTournaments = result.PlayerSignedUpTournaments.OrderByDescending(g => g.TournamentDate).ToList();
            }

            return result;
        }

        /// <summary>
        /// Gets the tournament list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetTournamentListResponse> GetTournamentList(Guid golfClubId,
                                                                       CancellationToken cancellationToken)
        {
            GetTournamentListResponse result = new GetTournamentListResponse();
            result.Tournaments = new List<GetTournamentResponse>();
            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                List<Tournament> tournamentList = await context.Tournament.Where(t => t.GolfClubId == golfClubId).ToListAsync(cancellationToken);

                foreach (Tournament tournament in tournamentList)
                {
                    result.Tournaments.Add(new GetTournamentResponse
                                           {
                                               HasResultBeenProduced = tournament.HasResultBeenProduced,
                                               MeasuredCourseId = tournament.MeasuredCourseId,
                                               MeasuredCourseSSS = tournament.MeasuredCourseSSS,
                                               TournamentFormat = (TournamentFormat)tournament.Format,
                                               PlayerCategory = (PlayerCategory)tournament.PlayerCategory,
                                               MeasuredCourseName = tournament.MeasuredCourseName,
                                               MeasuredCourseTeeColour = tournament.MeasuredCourseTeeColour,
                                               TournamentDate = tournament.TournamentDate,
                                               TournamentId = tournament.TournamentId,
                                               TournamentName = tournament.Name,
                                               PlayersSignedUpCount = tournament.PlayersSignedUpCount,
                                               PlayersScoresRecordedCount = tournament.PlayersScoresRecordedCount,
                                               HasBeenCompleted = tournament.HasBeenCompleted,
                                               HasBeenCancelled = tournament.HasBeenCancelled
                                           });
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts the club information to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertGolfClubToReadModel(GolfClubCreatedEvent domainEvent,
                                                    CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.GolfClub.Where(c => c.GolfClubId == domainEvent.AggregateId).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    GolfClub golfClub = new GolfClub
                                        {
                                            AddressLine1 = domainEvent.AddressLine1,
                                            EmailAddress = domainEvent.EmailAddress,
                                            Name = domainEvent.Name,
                                            Town = domainEvent.Town,
                                            Region = domainEvent.Region,
                                            TelephoneNumber = domainEvent.TelephoneNumber,
                                            AddressLine2 = domainEvent.AddressLine2,
                                            GolfClubId = domainEvent.AggregateId,
                                            PostalCode = domainEvent.PostalCode,
                                            WebSite = domainEvent.Website
                                        };

                    context.GolfClub.Add(golfClub);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the measured course to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NotFoundException">Golf Club with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task InsertMeasuredCourseToReadModel(MeasuredCourseAddedEvent domainEvent,
                                                          CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Boolean isDuplicate = await context.MeasuredCourses.Where(m => m.GolfClubId == domainEvent.AggregateId &&
                                                                               m.MeasuredCourseId == domainEvent.MeasuredCourseId).AnyAsync(cancellationToken);

                GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                if (golfClub == null)
                {
                    throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                }

                if (!isDuplicate)
                {
                    MeasuredCourse measuredCourse = new MeasuredCourse
                                                    {
                                                        GolfClubId = domainEvent.AggregateId,
                                                        MeasuredCourseId = domainEvent.MeasuredCourseId,
                                                        Name = domainEvent.Name,
                                                        TeeColour = domainEvent.TeeColour,
                                                        SSS = domainEvent.StandardScratchScore
                                                    };

                    await context.AddAsync(measuredCourse, cancellationToken);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player handicap record to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NotFoundException">
        /// Golf Club with Id {domainEvent.AggregateId} not found in read model
        /// or
        /// Player with Id {domainEvent.PlayerId} not found
        /// </exception>
        public async Task InsertPlayerHandicapRecordToReporting(ClubMembershipRequestAcceptedEvent domainEvent,
                                                                CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.PlayerHandicapListReporting.Where(p => p.PlayerId == domainEvent.PlayerId && p.GolfClubId == domainEvent.AggregateId)
                                                   .AnyAsync(cancellationToken);
                Logger.LogInformation($"Is duplicate is {isDuplicate}");
                if (!isDuplicate)
                {
                    GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                    if (golfClub == null)
                    {
                        throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                    }

                    // Get the player
                    PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(domainEvent.PlayerId, cancellationToken);

                    if (!player.HasBeenRegistered)
                    {
                        throw new NotFoundException($"Player with Id {domainEvent.PlayerId} not found");
                    }

                    PlayerHandicapListReporting playerHandicapListReporting = new PlayerHandicapListReporting
                                                                              {
                                                                                  GolfClubId = domainEvent.AggregateId,
                                                                                  PlayerId = domainEvent.PlayerId,
                                                                                  PlayerName = domainEvent.PlayerFullName,
                                                                                  HandicapCategory = player.HandicapCategory,
                                                                                  PlayingHandicap = player.PlayingHandicap,
                                                                                  ExactHandicap = player.ExactHandicap
                                                                              };

                    context.PlayerHandicapListReporting.Add(playerHandicapListReporting);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player membership to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club with Id {domainEvent.AggregateId}</exception>
        public async Task InsertPlayerMembershipToReadModel(ClubMembershipRequestAcceptedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.PlayerClubMembership.Where(p => p.PlayerId == domainEvent.PlayerId &&
                                                                                    p.GolfClubId == domainEvent.AggregateId &&
                                                                                    p.Status == (Int32)MembershipStatus.Accepted).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                    if (golfClub == null)
                    {
                        throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                    }

                    PlayerClubMembership playerClubMembership = new PlayerClubMembership
                                                                {
                                                                    PlayerId = domainEvent.PlayerId,
                                                                    MembershipNumber = domainEvent.MembershipNumber,
                                                                    GolfClubId = domainEvent.AggregateId,
                                                                    MembershipId = domainEvent.MembershipId,
                                                                    RejectionReason = null,
                                                                    Status = (Int32)MembershipStatus.Accepted,
                                                                    AcceptedDateTime = domainEvent.AcceptedDateAndTime,
                                                                    RejectedDateTime = null,
                                                                    GolfClubName = golfClub.Name
                                                                };

                    context.PlayerClubMembership.Add(playerClubMembership);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player membership to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club with Id {domainEvent.AggregateId}</exception>
        public async Task InsertPlayerMembershipToReadModel(ClubMembershipRequestRejectedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.PlayerClubMembership.Where(p => p.PlayerId == domainEvent.PlayerId &&
                                                                                    p.GolfClubId == domainEvent.AggregateId &&
                                                                                    p.Status == (Int32)MembershipStatus.Rejected).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                    if (golfClub == null)
                    {
                        throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                    }

                    PlayerClubMembership playerClubMembership = new PlayerClubMembership
                                                                {
                                                                    PlayerId = domainEvent.PlayerId,
                                                                    MembershipNumber = null,
                                                                    GolfClubId = domainEvent.AggregateId,
                                                                    MembershipId = domainEvent.MembershipId,
                                                                    RejectionReason = domainEvent.RejectionReason,
                                                                    Status = (Int32)MembershipStatus.Rejected,
                                                                    AcceptedDateTime = null,
                                                                    RejectedDateTime = domainEvent.RejectionDateAndTime,
                                                                    GolfClubName = golfClub.Name
                                                                };

                    context.PlayerClubMembership.Add(playerClubMembership);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player membership to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NotFoundException">
        /// Golf Club with Id {domainEvent.AggregateId} not found in read model
        /// or
        /// Player with Id {domainEvent.PlayerId} not found
        /// </exception>
        public async Task InsertPlayerMembershipToReporting(ClubMembershipRequestAcceptedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.GolfClubMembershipReporting.Where(p => p.PlayerId == domainEvent.PlayerId && p.GolfClubId == domainEvent.AggregateId)
                                                   .AnyAsync(cancellationToken);
                Logger.LogInformation($"Is duplicate is {isDuplicate}");
                if (!isDuplicate)
                {
                    GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.AggregateId, cancellationToken);

                    if (golfClub == null)
                    {
                        throw new NotFoundException($"Golf Club with Id {domainEvent.AggregateId} not found in read model");
                    }

                    // Get the player
                    PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(domainEvent.PlayerId, cancellationToken);

                    if (!player.HasBeenRegistered)
                    {
                        throw new NotFoundException($"Player with Id {domainEvent.PlayerId} not found");
                    }

                    GolfClubMembershipReporting golfClubMembershipReporting = new GolfClubMembershipReporting
                                                                              {
                                                                                  GolfClubId = domainEvent.AggregateId,
                                                                                  GolfClubName = golfClub.Name,
                                                                                  PlayerId = domainEvent.PlayerId,
                                                                                  PlayerName = domainEvent.PlayerFullName,
                                                                                  DateJoined = domainEvent.AcceptedDateAndTime,
                                                                                  DateOfBirth = domainEvent.PlayerDateOfBirth,
                                                                                  HandicapCategory = player.HandicapCategory,
                                                                                  PlayerGender = domainEvent.PlayerGender
                                                                              };

                    context.GolfClubMembershipReporting.Add(golfClubMembershipReporting);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the player score to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NotFoundException">Tournament with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task InsertPlayerScoreToReadModel(PlayerScorePublishedEvent domainEvent,
                                                       CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                PublishedPlayerScore publishedPlayerScore = new PublishedPlayerScore
                                                            {
                                                                TournamentId = domainEvent.AggregateId,
                                                                GolfClubId = domainEvent.GolfClubId,
                                                                PlayerId = domainEvent.PlayerId,
                                                                MeasuredCourseId = domainEvent.MeasuredCourseId,
                                                                GrossScore = domainEvent.GrossScore,
                                                                NetScore = domainEvent.NetScore,
                                                                TournamentDate = tournament.TournamentDate,
                                                                TournamentFormat = tournament.Format,
                                                                CSS = domainEvent.CSS,
                                                                GolfClubName = tournament.GolfClubName,
                                                                MeasuredCourseName = tournament.MeasuredCourseName,
                                                                MeasuredCourseTeeColour = tournament.MeasuredCourseTeeColour,
                                                                TournamentName = tournament.Name,
                                                                PlayingHandicap =  domainEvent.PlayingHandicap
                                                            };

                await context.PublishedPlayerScores.AddAsync(publishedPlayerScore, cancellationToken);

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Inserts the player sign up for tournament to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NotFoundException">Tournament with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task InsertPlayerSignUpForTournamentToReadModel(PlayerSignedUpEvent domainEvent,
                                                                     CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                PlayerTournamentSignUp playerTournamentSignUp = new PlayerTournamentSignUp
                                                                {
                                                                    GolfClubId = tournament.GolfClubId,
                                                                    TournamentDate = tournament.TournamentDate,
                                                                    MeasuredCourseId = tournament.MeasuredCourseId,
                                                                    TournamentId = domainEvent.AggregateId,
                                                                    PlayerId = domainEvent.PlayerId,
                                                                    MeasuredCourseName = tournament.MeasuredCourseName,
                                                                    GolfClubName = tournament.GolfClubName,
                                                                    MeasuredCourseTeeColour = tournament.MeasuredCourseTeeColour,
                                                                    TournamentName = tournament.Name,
                                                                    TournamentFormat = tournament.Format,
                                                                    ScoreEntered = false
                                                                };

                await context.PlayerTournamentSignUps.AddAsync(playerTournamentSignUp);

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Inserts the player tournament score to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task InsertPlayerTournamentScoreToReadModel(TournamentResultForPlayerScoreProducedEvent domainEvent,
                                                                 CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.TournamentResultForPlayerScore
                                                   .Where(t => t.TournamentId == domainEvent.AggregateId && t.PlayerId == domainEvent.PlayerId)
                                                   .AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    TournamentResultForPlayerScore tournamentResultForPlayerScore = new TournamentResultForPlayerScore
                                                                                    {
                                                                                        Division = domainEvent.Division,
                                                                                        DivisionPosition = domainEvent.DivisionPosition,
                                                                                        GrossScore = domainEvent.GrossScore,
                                                                                        Last3Holes = domainEvent.Last3Holes,
                                                                                        Last6Holes = domainEvent.Last6Holes,
                                                                                        Last9Holes = domainEvent.Last9Holes,
                                                                                        NetScore = domainEvent.NetScore,
                                                                                        PlayerId = domainEvent.PlayerId,
                                                                                        PlayingHandicap = domainEvent.PlayingHandicap,
                                                                                        TournamentId = domainEvent.AggregateId,
                                                                                        TournamentResultForPlayerId = Guid.NewGuid()
                                                                                    };

                    context.TournamentResultForPlayerScore.Add(tournamentResultForPlayerScore);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the tournament to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Golf Club with Id {domainEvent.GolfClubId}</exception>
        public async Task InsertTournamentToReadModel(TournamentCreatedEvent domainEvent,
                                                      CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Boolean isDuplicate = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).AnyAsync(cancellationToken);

                GolfClub golfClub = await context.GolfClub.SingleOrDefaultAsync(g => g.GolfClubId == domainEvent.GolfClubId, cancellationToken);

                if (golfClub == null)
                {
                    throw new NotFoundException($"Golf Club with Id {domainEvent.GolfClubId} not found in read model");
                }

                MeasuredCourse measuredCourse =
                    await context.MeasuredCourses.SingleOrDefaultAsync(m => m.MeasuredCourseId == domainEvent.MeasuredCourseId, cancellationToken);

                if (measuredCourse == null)
                {
                    throw new NotFoundException($"Measured Course with Id {domainEvent.MeasuredCourseId} not found in read model");
                }

                if (!isDuplicate)
                {
                    Tournament tournament = new Tournament
                                            {
                                                PlayerCategory = domainEvent.PlayerCategory,
                                                Name = domainEvent.Name,
                                                Format = domainEvent.Format,
                                                GolfClubId = domainEvent.GolfClubId,
                                                MeasuredCourseId = domainEvent.MeasuredCourseId,
                                                GolfClubName = golfClub.Name,
                                                TournamentDate = domainEvent.TournamentDate,
                                                MeasuredCourseSSS = domainEvent.MeasuredCourseSSS,
                                                TournamentId = domainEvent.AggregateId,
                                                MeasuredCourseName = measuredCourse.Name,
                                                MeasuredCourseTeeColour = measuredCourse.TeeColour
                                            };

                    context.Tournament.Add(tournament);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// Inserts the user record to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertUserRecordToReadModel(GolfClubAdministratorSecurityUserCreatedEvent domainEvent,
                                                      CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            await this.InsertUserRecordToReadModel(domainEvent.AggregateId, domainEvent.GolfClubAdministratorSecurityUserId, cancellationToken);
        }

        /// <summary>
        /// Inserts the user record to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task InsertUserRecordToReadModel(MatchSecretarySecurityUserCreatedEvent domainEvent,
                                                      CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            await this.InsertUserRecordToReadModel(domainEvent.AggregateId, domainEvent.MatchSecretarySecurityUserId, cancellationToken);
        }

        /// <summary>
        /// Registers the club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<Guid> RegisterClubAdministrator(RegisterClubAdministratorRequest request,
                                                    CancellationToken cancellationToken)
        {
            // Allocate a new club Id 
            Guid golfClubAggregateId = Guid.NewGuid();

            // Now create a club admin security user
            RegisterUserRequest registerUserRequest = new RegisterUserRequest
                                                      {
                                                          EmailAddress = request.EmailAddress,
                                                          Claims = new Dictionary<String, String>
                                                                   {
                                                                       {"GolfClubId", golfClubAggregateId.ToString()}
                                                                   },
                                                          Password = "123456",
                                                          PhoneNumber = request.TelephoneNumber,
                                                          FamilyName = request.FamilyName,
                                                          GivenName = request.GivenName,
                                                          MiddleName = request.MiddleName,
                                                          Roles = new List<String>
                                                                  {
                                                                      "Club Administrator"
                                                                  }
                                                      };

            // Create the user
            RegisterUserResponse result = await this.SecurityService.RegisterUser(registerUserRequest, cancellationToken);

            return result.UserId;
        }

        /// <summary>
        /// Updates the player handicap record to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="NotFoundException">Player not found with Id {domainEvent.AggregateId}</exception>
        public async Task UpdatePlayerHandicapRecordToReporting(HandicapAdjustedEvent domainEvent,
                                                                CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Find the record in the reporting table for the player
                List<PlayerHandicapListReporting> playerRecords =
                    await context.PlayerHandicapListReporting.Where(r => r.PlayerId == domainEvent.AggregateId).ToListAsync(cancellationToken);

                if (playerRecords.Count > 0)
                {
                    // Rehydrate the player to get the latest playing handicap
                    PlayerAggregate playerAggregate = await this.PlayerRepository.GetLatestVersion(domainEvent.AggregateId, cancellationToken);

                    if (playerAggregate.HasBeenRegistered == false)
                    {
                        throw new NotFoundException($"Player not found with Id {domainEvent.AggregateId}");
                    }

                    foreach (PlayerHandicapListReporting playerHandicapListReporting in playerRecords)
                    {
                        playerHandicapListReporting.ExactHandicap = playerAggregate.ExactHandicap;
                        playerHandicapListReporting.PlayingHandicap = playerAggregate.PlayingHandicap;
                        playerHandicapListReporting.HandicapCategory = playerAggregate.HandicapCategory;
                    }
                }

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the player membership to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task UpdatePlayerMembershipToReporting(HandicapAdjustedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Find the record in the reporting table for the player
                List<GolfClubMembershipReporting> playerRecords =
                    await context.GolfClubMembershipReporting.Where(r => r.PlayerId == domainEvent.AggregateId).ToListAsync(cancellationToken);

                if (playerRecords.Count > 0)
                {
                    // Rehydrate the player to get the latest playing handicap
                    PlayerAggregate playerAggregate = await this.PlayerRepository.GetLatestVersion(domainEvent.AggregateId, cancellationToken);

                    if (playerAggregate.HasBeenRegistered == false)
                    {
                        throw new NotFoundException($"Player not found with Id {domainEvent.AggregateId}");
                    }

                    foreach (GolfClubMembershipReporting golfClubMembershipReporting in playerRecords)
                    {
                        golfClubMembershipReporting.HandicapCategory = playerAggregate.HandicapCategory;
                    }
                }

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the tournament in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Tournament with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task UpdateTournamentInReadModel(PlayerSignedUpEvent domainEvent,
                                                      CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                tournament.PlayersSignedUpCount++;

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the tournament in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Tournament with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task UpdateTournamentInReadModel(PlayerScoreRecordedEvent domainEvent,
                                                      CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                tournament.PlayersScoresRecordedCount++;

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateTournamentStatusInReadModel(TournamentResultProducedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                tournament.HasResultBeenProduced = true;

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Tournament with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task UpdateTournamentStatusInReadModel(TournamentCompletedEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                tournament.HasBeenCompleted = true;

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Tournament with Id {domainEvent.AggregateId} not found in read model</exception>
        public async Task UpdateTournamentStatusInReadModel(TournamentCancelledEvent domainEvent,
                                                            CancellationToken cancellationToken)
        {
            Guard.ThrowIfNull(domainEvent, typeof(ArgumentNullException), "Domain event cannot be null");

            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the tournament has not already been added to the read model
                Tournament tournament = await context.Tournament.Where(t => t.TournamentId == domainEvent.AggregateId).SingleOrDefaultAsync(cancellationToken);

                if (tournament == null)
                {
                    throw new NotFoundException($"Tournament with Id {domainEvent.AggregateId} not found in read model");
                }

                tournament.HasBeenCancelled = true;

                await context.SaveChangesAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Inserts the user record to read model.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task InsertUserRecordToReadModel(Guid golfClubId,
                                                       Guid userId,
                                                       CancellationToken cancellationToken)
        {
            using(ManagementAPIReadModel context = this.ReadModelResolver())
            {
                // Check the club has not already been added to the read model
                Boolean isDuplicate = await context.Users.Where(c => c.GolfClubId == golfClubId && c.UserId == userId).AnyAsync(cancellationToken);

                if (!isDuplicate)
                {
                    // Get the user details from the security service
                    GetUserResponse securityUser = await this.SecurityService.GetUserById(userId, cancellationToken);

                    String givenNameClaim = securityUser.Claims.Where(c => c.Key == JwtClaimTypes.GivenName).Select(c => c.Value).SingleOrDefault();
                    String middleNameClaim = securityUser.Claims.Where(c => c.Key == JwtClaimTypes.MiddleName).Select(c => c.Value).SingleOrDefault();
                    String familyNameClaim = securityUser.Claims.Where(c => c.Key == JwtClaimTypes.FamilyName).Select(c => c.Value).SingleOrDefault();

                    User user = new User
                                {
                                    GivenName = string.IsNullOrEmpty(givenNameClaim) == false ? givenNameClaim : string.Empty,
                                    MiddleName = string.IsNullOrEmpty(middleNameClaim) == false ? middleNameClaim : string.Empty,
                                    FamilyName = string.IsNullOrEmpty(familyNameClaim) == false ? familyNameClaim : string.Empty,
                                    UserName = securityUser.UserName,
                                    UserId = securityUser.UserId,
                                    PhoneNumber = securityUser.PhoneNumber,
                                    Email = securityUser.Email,
                                    GolfClubId = golfClubId,
                                    UserType = securityUser.Roles.First()
                                };

                    await context.Users.AddAsync(user, cancellationToken);

                    await context.SaveChangesAsync(cancellationToken);
                }
            }
        }

        #endregion
    }
}