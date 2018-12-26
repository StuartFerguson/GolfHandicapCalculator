using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ManagementAPI.Player
{
    internal class ClubMembership
    {
        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClubMembership"/> class.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipRequestedDateAndTime">The membership requested date and time.</param>
        private ClubMembership(Guid clubId, DateTime membershipRequestedDateAndTime)
        {
            this.ClubId = clubId;
            this.MembershipRequestedDateAndTimeId = membershipRequestedDateAndTime;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets the club identifier.
        /// </summary>
        /// <value>
        /// The club identifier.
        /// </value>
        internal Guid ClubId { get; private set; }

        /// <summary>
        /// Gets the membership requested date and time identifier.
        /// </summary>
        /// <value>
        /// The membership requested date and time identifier.
        /// </value>
        internal DateTime MembershipRequestedDateAndTimeId { get; private set; }

        #endregion

        #region Internal Methods

        #region internal static ClubMembership Create(Guid clubId, DateTime membershipRequestedDateAndTime)        
        /// <summary>
        /// Creates the specified club identifier.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipRequestedDateAndTime">The membership requested date and time.</param>
        /// <returns></returns>
        internal static ClubMembership Create(Guid clubId, DateTime membershipRequestedDateAndTime)
        {
            return new ClubMembership(clubId, membershipRequestedDateAndTime);
        }
        #endregion

        #endregion
    }
}
