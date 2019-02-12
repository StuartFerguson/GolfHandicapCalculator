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
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        internal Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        internal Guid MembershipId { get; private set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        internal String MembershipNumber { get; private set; }

        /// <summary>
        /// Gets the accepted date time.
        /// </summary>
        /// <value>
        /// The accepted date time.
        /// </value>
        internal DateTime AcceptedDateTime { get; private set; }

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

        #region internal static ClubMembership Create()
        /// <summary>
        /// Creates the specified club identifier.
        /// </summary>
        /// <returns></returns>
        internal static ClubMembership Create()
        {
            return new ClubMembership();
        }
        #endregion

        #region internal void Approve(Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        /// <summary>
        /// Approves the specified membership request approved date and time.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        internal void Approve(Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        {
            this.GolfClubId = golfClubId;
            this.MembershipId = membershipId;
            this.MembershipNumber = membershipNumber;
            this.AcceptedDateTime = acceptedDateTime;
            this.Status = MembershipStatus.Approved;
        }
        #endregion

        //#region internal void Reject(DateTime membershipRejectedDateAndTime, String rejectionReason)        
        ///// <summary>
        ///// Rejects the specified membership rejected date and time.
        ///// </summary>
        ///// <param name="membershipRejectedDateAndTime">The membership rejected date and time.</param>
        ///// <param name="rejectionReason">The rejection reason.</param>
        //internal void Reject(DateTime membershipRejectedDateAndTime, String rejectionReason)
        //{
        //    this.MembershipRejectedDateAndTime = membershipRejectedDateAndTime;
        //    this.RejectionReason = rejectionReason;
        //    this.Status = MembershipStatus.Rejected;
        //}
        //#endregion

        #endregion
    }
}
