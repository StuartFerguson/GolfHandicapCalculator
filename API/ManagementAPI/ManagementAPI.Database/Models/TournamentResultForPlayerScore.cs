namespace ManagementAPI.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TournamentResultForPlayerScore
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
        /// Gets or sets the division position.
        /// </summary>
        /// <value>
        /// The division position.
        /// </value>
        public Int32 DivisionPosition { get; set; }

        /// <summary>
        /// Gets or sets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        public Int32 GrossScore { get; set; }

        /// <summary>
        /// Gets or sets the last3 holes.
        /// </summary>
        /// <value>
        /// The last3 holes.
        /// </value>
        public Decimal Last3Holes { get; set; }

        /// <summary>
        /// Gets or sets the last6 holes.
        /// </summary>
        /// <value>
        /// The last6 holes.
        /// </value>
        public Decimal Last6Holes { get; set; }

        /// <summary>
        /// Gets or sets the last9 holes.
        /// </summary>
        /// <value>
        /// The last9 holes.
        /// </value>
        public Decimal Last9Holes { get; set; }

        /// <summary>
        /// Gets or sets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        public Int32 NetScore { get; set; }

        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; set; }

        /// <summary>
        /// Gets or sets the tournament.
        /// </summary>
        /// <value>
        /// The tournament.
        /// </value>
        [ForeignKey("TournamentId")]
        public Tournament Tournament { get; set; }

        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the tournament result for player identifier.
        /// </summary>
        /// <value>
        /// The tournament result for player identifier.
        /// </value>
        [Key]
        public Guid TournamentResultForPlayerId { get; set; }

        #endregion
    }
}