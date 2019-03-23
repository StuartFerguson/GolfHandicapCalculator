namespace ManagementAPI.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using Developer.DataTransferObjects;
    using GolfClub;
    using GolfClubMembership;
    using Manager;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Player;
    using Shared.EventStore;
    using Swashbuckle.AspNetCore.Annotations;
    using Swashbuckle.AspNetCore.Filters;
    using Tournament;
    using Tournament.DataTransferObjects;
    using DeveloperGetGolfClubResponse = Developer.DataTransferObjects.GetGolfClubResponse;
    using GetGolfClubResponse = DataTransferObjects.GetGolfClubResponse;
    using MembershipStatus = Developer.DataTransferObjects.MembershipStatus;
    using PlayerCategory = Developer.DataTransferObjects.PlayerCategory;
    using TournamentFormat = Developer.DataTransferObjects.TournamentFormat;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    [Authorize(Policy = PolicyNames.DeveloperControllerPolicy)]
    public class DeveloperController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The golf club membership repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubMembershipAggregate> GolfClubMembershipRepository;

        /// <summary>
        /// The golf club repository
        /// </summary>
        private readonly IAggregateRepository<GolfClubAggregate> GolfClubRepository;

        /// <summary>
        /// The manager
        /// </summary>
        private readonly IManagmentAPIManager Manager;

        /// <summary>
        /// The player repository
        /// </summary>
        private readonly IAggregateRepository<PlayerAggregate> PlayerRepository;

        /// <summary>
        /// The tournament repository
        /// </summary>
        private readonly IAggregateRepository<TournamentAggregate> TournamentRepository;

        #endregion

        #region Constructors

        // GetTournament

        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperController"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="golfClubRepository">The golf club repository.</param>
        /// <param name="golfClubMembershipRepository">The golf club membership repository.</param>
        /// <param name="playerRepository">The player repository.</param>
        /// <param name="tournamentRepository">The tournament repository.</param>
        public DeveloperController(IManagmentAPIManager manager,
                                   IAggregateRepository<GolfClubAggregate> golfClubRepository,
                                   IAggregateRepository<GolfClubMembershipAggregate> golfClubMembershipRepository,
                                   IAggregateRepository<PlayerAggregate> playerRepository,
                                   IAggregateRepository<TournamentAggregate> tournamentRepository)
        {
            this.Manager = manager;
            this.GolfClubRepository = golfClubRepository;
            this.GolfClubMembershipRepository = golfClubMembershipRepository;
            this.PlayerRepository = playerRepository;
            this.TournamentRepository = tournamentRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="includeMemberships">if set to <c>true</c> [include memberships].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GolfClub/{golfClubId}/")]
        public async Task<IActionResult> GetGolfClub([FromRoute] Guid golfClubId,
                                                     [FromQuery] Boolean includeMemberships,
                                                     CancellationToken cancellationToken)
        {
            GolfClubAggregate golfClub = await this.GolfClubRepository.GetLatestVersion(golfClubId, cancellationToken);

            if (!golfClub.HasBeenCreated)
            {
                return this.NotFound($"Golf Club not found with Id {golfClubId}");
            }

            // Translate to Developer DTO
            DeveloperGetGolfClubResponse response = new DeveloperGetGolfClubResponse
                                                    {
                                                        AddressLine1 = golfClub.AddressLine1,
                                                        Name = golfClub.Name,
                                                        Town = golfClub.Town,
                                                        Website = golfClub.Website,
                                                        Region = golfClub.Region,
                                                        TelephoneNumber = golfClub.TelephoneNumber,
                                                        EmailAddress = golfClub.EmailAddress,
                                                        PostalCode = golfClub.PostalCode,
                                                        AggregateId = golfClub.AggregateId,
                                                        AddressLine2 = golfClub.AddressLine2,
                                                        HasBeenCreated = golfClub.HasBeenCreated
                                                    };

            if (includeMemberships)
            {
                GolfClubMembershipAggregate golfClubMemberships = await this.GolfClubMembershipRepository.GetLatestVersion(golfClubId, cancellationToken);

                List<MembershipDataTransferObject> memberships = golfClubMemberships.GetMemberships();

                memberships.ForEach(m =>
                                    {
                                        response.GolfClubMemberships.Add(new GolfClubMembershipResponse
                                                                         {
                                                                             AcceptedDateAndTime = m.AcceptedDateAndTime,
                                                                             MembershipId = m.MembershipId,
                                                                             MembershipNumber = m.MembershipNumber,
                                                                             PlayerDateOfBirth = m.PlayerDateOfBirth,
                                                                             PlayerFullName = m.PlayerFullName,
                                                                             PlayerGender = m.PlayerGender,
                                                                             PlayerId = m.PlayerId,
                                                                             RejectedDateAndTime = m.RejectedDateAndTime,
                                                                             RejectionReason = m.RejectionReason,
                                                                             RequestedDateAndTime = m.RequestedDateAndTime,
                                                                             Status = m.Status
                                                                         });
                                    });
            }

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GolfClub/GetGolfClubList")]
        public async Task<IActionResult> GetGolfClubList(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            if (golfClubList == null || golfClubList.Count == 0)
            {
                return this.NoContent();
            }

            // Translate to Developer DTO
            List<GetGolfClubListResponse> response = new List<GetGolfClubListResponse>();

            golfClubList.ForEach(x =>
                                 {
                                     response.Add(new GetGolfClubListResponse
                                                  {
                                                      AddressLine1 = x.AddressLine1,
                                                      AddressLine2 = x.AddressLine2,
                                                      EmailAddress = x.EmailAddress,
                                                      Id = x.Id,
                                                      Name = x.Name,
                                                      PostalCode = x.PostalCode,
                                                      Region = x.Region,
                                                      TelephoneNumber = x.TelephoneNumber,
                                                      Town = x.Town,
                                                      Website = x.Website
                                                  });
                                 });

            return this.Ok(response);
        }

        /// <summary>
        /// Gets the number of golf clubs.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GolfClub/GetNumberOfClubs")]
        public async Task<IActionResult> GetNumberOfGolfClubs(CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> golfClubList = await this.Manager.GetGolfClubList(cancellationToken);

            if (golfClubList == null || golfClubList.Count == 0)
            {
                return this.NoContent();
            }

            return this.Ok(golfClubList.Count);
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="includeMemberships">if set to <c>true</c> [include memberships].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Player/{playerId}/")]
        public async Task<IActionResult> GetPlayer([FromRoute] Guid playerId,
                                                   [FromQuery] Boolean includeMemberships,
                                                   CancellationToken cancellationToken)
        {
            PlayerAggregate player = await this.PlayerRepository.GetLatestVersion(playerId, cancellationToken);

            if (!player.HasBeenRegistered)
            {
                return this.NotFound($"Player not found with Id {playerId}");
            }

            GetPlayerResponse playerResponse = new GetPlayerResponse
                                               {
                                                   DateOfBirth = player.DateOfBirth,
                                                   EmailAddress = player.EmailAddress,
                                                   HasBeenRegistered = player.HasBeenRegistered,
                                                   ExactHandicap = player.ExactHandicap,
                                                   FirstName = player.FirstName,
                                                   FullName = player.FullName,
                                                   Gender = player.Gender,
                                                   HandicapCategory = player.HandicapCategory,
                                                   LastName = player.LastName,
                                                   MiddleName = player.MiddleName,
                                                   PlayingHandicap = player.PlayingHandicap,
                                                   SecurityUserId = player.SecurityUserId
                                               };

            if (includeMemberships)
            {
                List<ClubMembershipResponse> playerMemberships = await this.Manager.GetPlayersClubMemberships(playerId, cancellationToken);

                if (playerMemberships != null && playerMemberships.Count != 0)
                {
                    playerMemberships.ForEach(p =>
                                              {
                                                  playerResponse.ClubMemberships.Add(new PlayerClubMembership
                                                                                     {
                                                                                         MembershipId = p.MembershipId,
                                                                                         Status = p.Status.ConvertTo<MembershipStatus>(),
                                                                                         MembershipNumber = p.MembershipNumber,
                                                                                         RejectionReason = p.RejectionReason,
                                                                                         AcceptedDateTime = p.AcceptedDateTime,
                                                                                         GolfClubId = p.GolfClubId,
                                                                                         GolfClubName = p.GolfClubName,
                                                                                         RejectedDateTime = p.RejectedDateTime
                                                                                     });
                                              });
                }
            }

            return this.Ok(playerResponse);
        }

        /// <summary>
        /// Gets the tournament.
        /// </summary>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="includeScores">if set to <c>true</c> [include scores].</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("Tournament/{tournamentId}/")]
        public async Task<IActionResult> GetTournament([FromRoute] Guid tournamentId,
                                                       [FromQuery] Boolean includeScores,
                                                       CancellationToken cancellationToken)
        {
            TournamentAggregate tournament = await this.TournamentRepository.GetLatestVersion(tournamentId, cancellationToken);

            if (!tournament.HasBeenCreated)
            {
                return this.NotFound($"Tournament not found with Id {tournamentId}");
            }

            GetTournamentResponse response = new GetTournamentResponse
                                             {
                                                 HasBeenCreated = tournament.HasBeenCreated,
                                                 Name = tournament.Name,
                                                 GolfClubId = tournament.GolfClubId,
                                                 Adjustment = tournament.Adjustment,
                                                 CSS = tournament.CSS,
                                                 CSSHasBeenCalculated = tournament.CSSHasBeenCalculated,
                                                 CancelledDateTime = tournament.CancelledDateTime,
                                                 CancelledReason = tournament.CancelledReason,
                                                 CompletedDateTime = tournament.CompletedDateTime,
                                                 Format = tournament.Format.ConvertTo<TournamentFormat>(),
                                                 HasBeenCancelled = tournament.HasBeenCancelled,
                                                 HasBeenCompleted = tournament.HasBeenCompleted,
                                                 MeasuredCourseId = tournament.MeasuredCourseId,
                                                 MeasuredCourseSSS = tournament.MeasuredCourseSSS,
                                                 MemberCategory = tournament.PlayerCategory.ConvertTo<PlayerCategory>(),
                                                 TournamentDate = tournament.TournamentDate
                                             };

            if (includeScores)
            {
                List<PlayerScoreRecordDataTransferObject> scores = tournament.GetScores();

                if (scores.Count > 0)
                {
                    scores.ForEach(s =>
                                   {
                                       response.Scores.Add(new MemberScoreRecord
                                                           {
                                                               GrossScore = s.GrossScore,
                                                               PlayingHandicap = s.PlayingHandicap,
                                                               HandicapCategory = s.HandicapCategory,
                                                               HoleScores = s.HoleScores,
                                                               MemberId = s.PlayerId,
                                                               NetScore = s.NetScore
                                                           });
                                   });
                }
            }

            return this.Ok(response);
        }

        #endregion
    }
}