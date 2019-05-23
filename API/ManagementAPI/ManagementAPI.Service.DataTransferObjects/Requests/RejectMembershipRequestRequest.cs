namespace ManagementAPI.Service.DataTransferObjects.Requests
{
    using System;

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
