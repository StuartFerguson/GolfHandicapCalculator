namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class PlayerSignedUpTournamentsResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSignedUpTournamentsResponse"/> class.
        /// </summary>
        public PlayerSignedUpTournamentsResponse()
        {
            this.PlayerSignedUpTournaments = new List<PlayerSignedUpTournament>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the player signed up tournaments.
        /// </summary>
        /// <value>
        /// The player signed up tournaments.
        /// </value>
        public List<PlayerSignedUpTournament> PlayerSignedUpTournaments { get; set; }

        #endregion
    }
}