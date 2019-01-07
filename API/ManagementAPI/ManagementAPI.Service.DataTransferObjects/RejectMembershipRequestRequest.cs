using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects
{
    public class RejectMembershipRequestRequest
    {
        /// <summary>
        /// Gets or sets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        public String RejectionReason { get; set; }
    }
}
