namespace ManagementAPI.Service.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using GolfClub.DomainEvents;
    using GolfClubMembership.DomainEvents;
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
        /// Inserts the golf club to read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task InsertGolfClubToReadModel(GolfClubCreatedEvent domainEvent,
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
        /// Registers the club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task RegisterClubAdministrator(RegisterClubAdministratorRequest request,
                                       CancellationToken cancellationToken);

        /// <summary>
        /// Updates the tournament status in read model.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task UpdateTournamentStatusInReadModel(TournamentResultProducedEvent domainEvent,
                                               CancellationToken cancellationToken);

        #endregion
    }
}