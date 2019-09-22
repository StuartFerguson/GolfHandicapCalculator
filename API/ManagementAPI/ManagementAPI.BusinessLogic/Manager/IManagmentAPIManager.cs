namespace ManagementAPI.BusinessLogic.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClub.DomainEvents;
    using GolfClubMembership.DomainEvents;
    using Player.DomainEvents;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Tournament.DomainEvents;

    public interface IManagmentAPIManager
    {
        #region Methods

        /// <summary>
        /// Gets the golf club.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubResponse> GetGolfClub(Guid golfClubId,
                                              CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubResponse>> GetGolfClubList(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club members list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<GetGolfClubMembershipDetailsResponse>> GetGolfClubMembersList(Guid golfClubId,
                                                                                CancellationToken cancellationToken);

        /// <summary>
        /// Gets the golf club users.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetGolfClubUserListResponse> GetGolfClubUsers(Guid golfClubId,
                                                           CancellationToken cancellationToken);

        /// <summary>
        /// Gets the measured course list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetMeasuredCourseListResponse> GetMeasuredCourseList(Guid golfClubId,
                                                                  CancellationToken cancellationToken);

        /// <summary>
        /// Gets the player details.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetPlayerDetailsResponse> GetPlayerDetails(Guid playerId,
                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Gets the players club memberships.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<List<ClubMembershipResponse>> GetPlayersClubMemberships(Guid playerId,
                                                                     CancellationToken cancellationToken);

        /// <summary>
        /// Gets the player signed up tournaments.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<PlayerSignedUpTournamentsResponse> GetPlayerSignedUpTournaments(Guid playerId,
                                                                             CancellationToken cancellationToken);

        /// <summary>
        /// Gets the tournament list.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<GetTournamentListResponse> GetTournamentList(Guid golfClubId,
                                                          CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the golf club to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertGolfClubToReadModel(GolfClubCreatedEvent domainEvent,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the measured course to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertMeasuredCourseToReadModel(MeasuredCourseAddedEvent domainEvent,
                                             CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player handicap record to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerHandicapRecordToReporting(ClubMembershipRequestAcceptedEvent domainEvent,
                                                   CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player membership to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerMembershipToReadModel(ClubMembershipRequestAcceptedEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player membership to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerMembershipToReadModel(ClubMembershipRequestRejectedEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player membership to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerMembershipToReporting(ClubMembershipRequestAcceptedEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player score to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerScoreToReadModel(PlayerScorePublishedEvent domainEvent,
                                          CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player sign up for tournament to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerSignUpForTournamentToReadModel(PlayerSignedUpEvent domainEvent,
                                                        CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the player tournament score to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertPlayerTournamentScoreToReadModel(TournamentResultForPlayerScoreProducedEvent domainEvent,
                                                    CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the tournament to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertTournamentToReadModel(TournamentCreatedEvent domainEvent,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the user record to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertUserRecordToReadModel(GolfClubAdministratorSecurityUserCreatedEvent domainEvent,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Inserts the user record to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertUserRecordToReadModel(MatchSecretarySecurityUserCreatedEvent domainEvent,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Registers the club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterClubAdministrator(RegisterClubAdministratorRequest request,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Updates the player handicap record to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdatePlayerHandicapRecordToReporting(HandicapAdjustedEvent domainEvent,
                                                   CancellationToken cancellationToken);

        /// <summary>
        /// Updates the player membership to reporting.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdatePlayerMembershipToReporting(HandicapAdjustedEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Updates the tournament in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTournamentInReadModel(PlayerSignedUpEvent domainEvent,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Updates the tournament in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTournamentInReadModel(PlayerScoreRecordedEvent domainEvent,
                                         CancellationToken cancellationToken);

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTournamentStatusInReadModel(TournamentResultProducedEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTournamentStatusInReadModel(TournamentCancelledEvent domainEvent,
                                               CancellationToken cancellationToken);

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTournamentStatusInReadModel(TournamentCompletedEvent domainEvent,
                                               CancellationToken cancellationToken);

        #endregion
    }
}