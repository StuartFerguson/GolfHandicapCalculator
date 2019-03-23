namespace ManagementAPI.Tournament
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class PlayerScoreRecord
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScoreRecord" /> class.
        /// </summary>
        /// <param name="playerId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        private PlayerScoreRecord(Guid playerId,
                                  Int32 playingHandicap,
                                  Dictionary<Int32, Int32> holeScores)
        {
            this.PlayerId = playerId;
            this.HoleScores = holeScores;
            this.PlayingHandicap = playingHandicap;

            if (this.PlayingHandicap <= 5)
            {
                this.HandicapCategory = 1;
            }
            else if (this.PlayingHandicap >= 6 && this.PlayingHandicap <= 12)
            {
                this.HandicapCategory = 2;
            }
            else if (this.PlayingHandicap >= 13 && this.PlayingHandicap <= 20)
            {
                this.HandicapCategory = 3;
            }
            else if (this.PlayingHandicap >= 21 && this.PlayingHandicap <= 12)
            {
                this.HandicapCategory = 4;
            }
            else
            {
                this.HandicapCategory = 5;
            }

            if (this.HoleScores.Values.Contains(0))
            {
                // this an NR so record gross and net scores as 0
                this.GrossScore = 0;
                this.NetScore = 0;
            }
            else
            {
                this.GrossScore = this.HoleScores.Values.Sum();
                this.NetScore = this.GrossScore - this.PlayingHandicap;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        internal Int32 GrossScore { get; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        internal Int32 HandicapCategory { get; }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        internal Dictionary<Int32, Int32> HoleScores { get; }

        /// <summary>
        /// Gets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        internal Int32 NetScore { get; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        internal Guid PlayerId { get; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        internal Int32 PlayingHandicap { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified member identifier.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <returns></returns>
        internal static PlayerScoreRecord Create(Guid playerId,
                                                 Int32 playingHandicap,
                                                 Dictionary<Int32, Int32> holeScores)
        {
            return new PlayerScoreRecord(playerId, playingHandicap, holeScores);
        }

        #endregion
    }
}