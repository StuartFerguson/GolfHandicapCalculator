namespace ManagementAPI.Tournament.DataTransferObjects
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class PlayerScoreRecordDataTransferObject
    {
        #region Properties

        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        public Int32 GrossScore { get; set; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; set; }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        public Dictionary<Int32, Int32> HoleScores { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is published.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is published; otherwise, <c>false</c>.
        /// </value>
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the last3 holes score.
        /// </summary>
        /// <value>
        /// The last3 holes score.
        /// </value>
        public Decimal Last3HolesScore { get; set; }

        /// <summary>
        /// Gets or sets the last6 holes score.
        /// </summary>
        /// <value>
        /// The last6 holes score.
        /// </value>
        public Decimal Last6HolesScore { get; set; }

        /// <summary>
        /// Gets or sets the last9 holes score.
        /// </summary>
        /// <value>
        /// The last9 holes score.
        /// </value>
        public Decimal Last9HolesScore { get; set; }

        /// <summary>
        /// Gets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        public Int32 NetScore { get; set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Int32 Position { get; set; }

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