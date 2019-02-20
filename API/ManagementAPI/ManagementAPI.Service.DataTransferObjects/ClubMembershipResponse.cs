using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects
{
    public class ClubMembershipResponse
    {
        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        public Guid MembershipId { get; set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        public String MembershipNumber { get; set; }

        /// <summary>
        /// Gets the accepted date time.
        /// </summary>
        /// <value>
        /// The accepted date time.
        /// </value>
        public DateTime AcceptedDateTime { get; set; }

        /// <summary>
        /// Gets the membership status.
        /// </summary>
        /// <value>
        /// The membership status.
        /// </value>
        public MembershipStatus Status { get; set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        public String RejectionReason  { get; set; }

        /// <summary>
        /// Gets the rejected date time.
        /// </summary>
        /// <value>
        /// The rejected date time.
        /// </value>
        public DateTime RejectedDateTime { get; set; }
    }
}
