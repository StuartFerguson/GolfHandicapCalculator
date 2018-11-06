using System;
using System.Collections.Generic;
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
        /// <param name="holeScores">The hole scores.</param>
        private MemberScoreRecord(Guid memberId, Dictionary<Int32, Int32> holeScores)
        {
            this.MemberId = memberId;
            this.HoleScores = holeScores;
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
        /// Creates the specified tee colour.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <returns></returns>
        internal static MemberScoreRecord Create(Guid memberId, Dictionary<Int32, Int32> holeScores)
        {
            return new MemberScoreRecord(memberId, holeScores);
        }
        #endregion

        #endregion
    }
}

