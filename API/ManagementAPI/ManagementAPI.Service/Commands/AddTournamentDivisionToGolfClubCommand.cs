namespace ManagementAPI.Service.Commands
{
    using System;
    using DataTransferObjects;
    using Shared.CommandHandling;

    public class AddTournamentDivisionToGolfClubCommand : Command<String>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTournamentDivisionToGolfClubCommand"/> class.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="addTournamentDivisionToGolfClubRequest">The add tournament division to golf club request.</param>
        /// <param name="commandId">The command identifier.</param>
        private AddTournamentDivisionToGolfClubCommand(Guid golfClubId,
                                                       AddTournamentDivisionToGolfClubRequest addTournamentDivisionToGolfClubRequest,
                                                       Guid commandId) : base(commandId)
        {
            this.GolfClubId = golfClubId;
            this.AddTournamentDivisionToGolfClubRequest = addTournamentDivisionToGolfClubRequest;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the add tournament division to golf club request.
        /// </summary>
        /// <value>
        /// The add tournament division to golf club request.
        /// </value>
        public AddTournamentDivisionToGolfClubRequest AddTournamentDivisionToGolfClubRequest { get; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified golf club identifier.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="addTournamentDivisionToGolfClubRequest">The add tournament division to golf club request.</param>
        /// <returns></returns>
        public static AddTournamentDivisionToGolfClubCommand Create(Guid golfClubId,
                                                                    AddTournamentDivisionToGolfClubRequest addTournamentDivisionToGolfClubRequest)
        {
            return new AddTournamentDivisionToGolfClubCommand(golfClubId, addTournamentDivisionToGolfClubRequest, Guid.NewGuid());
        }

        #endregion
    }
}