namespace ManagementAPI.Service.DataTransferObjects.Requests
{
    using System;
    using System.Collections.Generic;

    public class RecordPlayerTournamentScoreRequest
    {
        /// <summary>
        /// Gets or sets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; set; }

        /// <summary>
        /// Gets or sets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        public Dictionary<Int32,Int32> HoleScores { get; set; }
    }
}
