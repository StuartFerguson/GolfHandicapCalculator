namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class AddTournamentDivisionToGolfClubResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets or sets the tournament division.
        /// </summary>
        /// <value>
        /// The tournament division.
        /// </value>
        public Int32 TournamentDivision { get; set; }

        #endregion
    }
}