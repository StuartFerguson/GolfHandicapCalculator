namespace DomainEventRouterAPI.Service.Common
{
    using ManagementAPI.GolfClubMembership.DomainEvents;
    using ManagementAPI.Service.DataTransferObjects.Responses;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DomainEventRouterAPI.Service.Common.IModelFactory" />
    public class ModelFactory : IModelFactory
    {
        #region Methods

        /// <summary>
        /// Creates the specified golf club.
        /// </summary>
        /// <param name="golfClub">The golf club.</param>
        /// <param name="player">The player.</param>
        /// <param name="domainEvent">The domain event.</param>
        /// <returns></returns>
        public GolfClubMembershipModel Create(GetGolfClubResponse golfClub,
                                              GetPlayerDetailsResponse player,
                                              ClubMembershipRequestAcceptedEvent domainEvent)
        {
            GolfClubMembershipModel model = new GolfClubMembershipModel();

            model.PlayerId = domainEvent.PlayerId;
            model.DateJoined = domainEvent.AcceptedDateAndTime;
            model.DateOfBirth = player.DateOfBirth;
            model.ExactHandicap = player.ExactHandicap;
            model.Gender = player.Gender;
            model.GolfClubId = golfClub.Id;
            model.GolfClubName = golfClub.Name;
            model.HandicapCategory = player.HandicapCategory;
            model.PlayerName = domainEvent.PlayerFullName;
            model.PlayingHandicap = player.PlayingHandicap;

            return model;
        }

        #endregion
    }
}