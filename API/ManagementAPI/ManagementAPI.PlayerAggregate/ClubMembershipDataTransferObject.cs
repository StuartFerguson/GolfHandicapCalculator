using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Player
{
    public class ClubMembershipDataTransferObject
    {
        /// <summary>
        /// Gets or sets the club identifier.
        /// </summary>
        /// <value>
        /// The club identifier.
        /// </value>
        public Guid ClubId { get; set; }

        /// <summary>
        /// Gets or sets the membership requested date and time.
        /// </summary>
        /// <value>
        /// The membership requested date and time.
        /// </value>
        public DateTime MembershipRequestedDateAndTime { get; set; }

        /// <summary>
        /// Gets or sets the membership approved date and time.
        /// </summary>
        /// <value>
        /// The membership approved date and time.
        /// </value>
        public DateTime MembershipApprovedDateAndTime { get; set; }

        /// <summary>
        /// Gets or sets the membership rejected date and time.
        /// </summary>
        /// <value>
        /// The membership rejected date and time.
        /// </value>
        public DateTime MembershipRejectedDateAndTime { get; set; }

        /// <summary>
        /// Gets or sets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        public String RejectionReason { get; set; }

        /// <summary>
        /// Gets or sets the membership status.
        /// </summary>
        /// <value>
        /// The membership status.
        /// </value>
        public MembershipStatus MembershipStatus { get; set; }
    }
}
