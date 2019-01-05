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
        /// Gets or sets the membership status.
        /// </summary>
        /// <value>
        /// The membership status.
        /// </value>
        public MembershipStatus MembershipStatus { get; set; }
    }

    public enum MembershipStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
