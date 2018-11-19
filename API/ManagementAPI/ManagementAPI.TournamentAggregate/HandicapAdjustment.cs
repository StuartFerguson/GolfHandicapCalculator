using System;
using System.Collections.Generic;

namespace ManagementAPI.TournamentAggregate
{
    internal class HandicapAdjustment
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapAdjustment" /> class.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        private HandicapAdjustment(Guid memberId, List<Decimal> adjustments, Decimal totalAdjustment)
        {
            this.MemberId = memberId;
            this.Adjustments = adjustments;
            this.TotalAdjustment = totalAdjustment;
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
        /// Gets the adjustments.
        /// </summary>
        /// <value>
        /// The adjustments.
        /// </value>
        internal List<Decimal> Adjustments { get; private set; }

        /// <summary>
        /// Gets the total adjustment.
        /// </summary>
        /// <value>
        /// The total adjustment.
        /// </value>
        internal Decimal TotalAdjustment { get; private set; }

        #endregion

        #region Internal Methods

        #region internal static HandicapAdjustment Create(Guid memberId, List<Decimal> adjustments, Decimal totalAdjustment)
        /// <summary>
        /// Creates the specified member identifier.
        /// </summary>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        /// <returns></returns>
        internal static HandicapAdjustment Create(Guid memberId, List<Decimal> adjustments, Decimal totalAdjustment)
        {
            return new HandicapAdjustment(memberId, adjustments, totalAdjustment);
        }
        #endregion

        #endregion
    }
}