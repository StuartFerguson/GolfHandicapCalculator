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
        /// Initializes a new instance of the <see cref="ClubMembership" /> class.
        /// </summary>
        private ClubMembership()
        {

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
        /// Gets the membership requested date and time.
        /// </summary>
        /// <value>
        /// The membership requested date and time.
        /// </value>
        internal DateTime MembershipRequestedDateAndTime { get; private set; }

        /// <summary>
        /// Gets the membership approved date and time.
        /// </summary>
        /// <value>
        /// The membership approved date and time.
        /// </value>
        internal DateTime MembershipApprovedDateAndTime { get; private set; }

        /// <summary>
        /// Gets the membership rejected date and time.
        /// </summary>
        /// <value>
        /// The membership rejected date and time.
        /// </value>
        internal DateTime MembershipRejectedDateAndTime { get; private set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        internal String RejectionReason { get; private set; }

        /// <summary>
        /// Gets the membership status.
        /// </summary>
        /// <value>
        /// The membership status.
        /// </value>
        internal MembershipStatus Status { get; private set; }

        #endregion

        #region Internal Enums     
        
        /// <summary>
        /// Membership Status Enum
        /// </summary>
        internal enum MembershipStatus
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2
        }
        #endregion

        #region Internal Methods

        #region internal static ClubMembership Create(Guid clubId, DateTime membershipRequestedDateAndTime)        
        /// <summary>
        /// Creates the specified club identifier.
        /// </summary>
        /// <returns></returns>
        internal static ClubMembership Create()
        {
            return new ClubMembership();
        }
        #endregion

        #region internal void Request(Guid clubId, DateTime membershipRequestedDateAndTime)        
        /// <summary>
        /// Requests the specified club identifier.
        /// </summary>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="membershipRequestedDateAndTime">The membership requested date and time.</param>
        internal void Request(Guid clubId, DateTime membershipRequestedDateAndTime)
        {
            this.ClubId = clubId;
            this.MembershipRequestedDateAndTime = membershipRequestedDateAndTime;
            this.Status = MembershipStatus.Pending; 
        }
        #endregion

        #region internal void Approve(DateTime membershipRequestApprovedDateAndTime)        
        /// <summary>
        /// Approves the specified membership request approved date and time.
        /// </summary>
        /// <param name="membershipRequestApprovedDateAndTime">The membership request approved date and time.</param>
        internal void Approve(DateTime membershipRequestApprovedDateAndTime)
        {
            this.MembershipApprovedDateAndTime = membershipRequestApprovedDateAndTime;
            this.Status = MembershipStatus.Approved;
        }
        #endregion

        #region internal void Reject(DateTime membershipRejectedDateAndTime, String rejectionReason)        
        /// <summary>
        /// Rejects the specified membership rejected date and time.
        /// </summary>
        /// <param name="membershipRejectedDateAndTime">The membership rejected date and time.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        internal void Reject(DateTime membershipRejectedDateAndTime, String rejectionReason)
        {
            this.MembershipRejectedDateAndTime = membershipRejectedDateAndTime;
            this.RejectionReason = rejectionReason;
            this.Status = MembershipStatus.Rejected;
        }
        #endregion

        #endregion
    }
}
