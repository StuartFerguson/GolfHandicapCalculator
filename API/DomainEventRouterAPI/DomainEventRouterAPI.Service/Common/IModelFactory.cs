namespace DomainEventRouterAPI.Service.Common
{
    using ManagementAPI.GolfClubMembership.DomainEvents;
    using ManagementAPI.Service.DataTransferObjects.Responses;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        #region Methods

        /// <summary>
        /// Creates the specified golf club.
        /// </summary>
        /// <param name="golfClub">The golf club.</param>
        /// <param name="player">The player.</param>
        /// <param name="domainEvent">The domain event.</param>
        /// <returns></returns>
        GolfClubMembershipModel Create(GetGolfClubResponse golfClub,
                                       GetPlayerDetailsResponse player,
                                       ClubMembershipRequestAcceptedEvent domainEvent);

        #endregion
    }
}