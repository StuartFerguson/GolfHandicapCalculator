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
    }
}
