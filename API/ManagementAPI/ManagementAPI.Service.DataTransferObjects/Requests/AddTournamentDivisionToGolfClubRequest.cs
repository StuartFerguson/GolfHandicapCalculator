namespace ManagementAPI.Service.DataTransferObjects.Requests
{
    using System;

    public class AddTournamentDivisionToGolfClubRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        /// <value>
        /// The division.
        /// </value>
        public Int32 Division { get; set; }

        /// <summary>
        /// Gets or sets the end handicap.
        /// </summary>
        /// <value>
        /// The end handicap.
        /// </value>
        public Int32 EndHandicap { get; set; }

        /// <summary>
        /// Gets or sets the start handicap.
        /// </summary>
        /// <value>
        /// The start handicap.
        /// </value>
        public Int32 StartHandicap { get; set; }

        #endregion
    }
}