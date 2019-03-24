namespace ManagementAPI.Tournament
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
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
        /// Gets a value indicating whether this instance is published.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is published; otherwise, <c>false</c>.
        /// </value>
        internal Boolean IsPublished { get; private set; }

        /// <summary>
        /// Gets the last3 holes score.
        /// </summary>
        /// <value>
        /// The last3 holes score.
        /// </value>
        internal Decimal Last3HolesScore { get; private set; }

        /// <summary>
        /// Gets the last6 holes score.
        /// </summary>
        /// <value>
        /// The last6 holes score.
        /// </value>
        internal Decimal Last6HolesScore { get; private set; }

        /// <summary>
        /// Gets the last9 holes score.
        /// </summary>
        /// <value>
        /// The last9 holes score.
        /// </value>
        internal Decimal Last9HolesScore { get; private set; }

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

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        internal Int32 Position { get; private set; }

        /// <summary>
        /// Gets the tournament division.
        /// </summary>
        /// <value>
        /// The tournament division.
        /// </value>
        internal Int32 TournamentDivision { get; private set; }

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

        /// <summary>
        /// Publishes this instance.
        /// </summary>
        internal void Publish()
        {
            this.IsPublished = true;
        }

        /// <summary>
        /// Sets the count back scores.
        /// </summary>
        /// <param name="last9HolesScore">The last9 holes score.</param>
        /// <param name="last6HolesScore">The last6 holes score.</param>
        /// <param name="last3HolesScore">The last3 holes score.</param>
        internal void SetCountBackScores(Decimal last9HolesScore,
                                         Decimal last6HolesScore,
                                         Decimal last3HolesScore)
        {
            this.Last9HolesScore = last9HolesScore;
            this.Last6HolesScore = last6HolesScore;
            this.Last3HolesScore = last6HolesScore;
        }

        /// <summary>
        /// Sets the result details.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="tournamentDivision">The tournament division.</param>
        internal void SetResultDetails(Int32 position,
                                       Int32 tournamentDivision)
        {
            this.TournamentDivision = tournamentDivision;
            this.Position = position;
        }

        #endregion
    }
}