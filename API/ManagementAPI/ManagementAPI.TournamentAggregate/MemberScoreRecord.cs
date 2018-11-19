using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagementAPI.TournamentAggregate
{
    internal class MemberScoreRecord
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberScoreRecord" /> class.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        private MemberScoreRecord(Guid memberId, Int32 playingHandicap, Dictionary<Int32, Int32> holeScores)
        {
            this.MemberId = memberId;
            this.HoleScores = holeScores;
            this.PlayingHandicap = playingHandicap;

            if (PlayingHandicap <= 5)
            {
                this.HandicapCategory = 1;
            }
            else if (PlayingHandicap >= 6 && PlayingHandicap <= 12)
            {
                this.HandicapCategory = 2;
            }
            else if (PlayingHandicap >= 13 && PlayingHandicap <= 20)
            {
                this.HandicapCategory = 3;
            }
            else if (PlayingHandicap >= 21 && PlayingHandicap <= 12)
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

        #region Internal Properties

        /// <summary>
        /// Gets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        internal Guid MemberId { get; private set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        internal Int32 PlayingHandicap { get; private set; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        internal Int32 HandicapCategory { get; private set; }

        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        internal Int32 GrossScore { get; private set; }

        /// <summary>
        /// Gets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        internal Int32 NetScore { get; private set; }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        internal Dictionary<Int32, Int32> HoleScores { get; private set; }

        #endregion

        #region Internal Methods

        #region internal static MemberScoreRecord Create(Guid memberId, Dictionary<Int32, Int32> holeScores)        
        /// <summary>
        /// Creates the specified member identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <returns></returns>
        internal static MemberScoreRecord Create(Guid memberId, Int32 playingHandicap, Dictionary<Int32, Int32> holeScores)
        {
            return new MemberScoreRecord(memberId, playingHandicap, holeScores);
        }
        #endregion

        #endregion
    }
}

