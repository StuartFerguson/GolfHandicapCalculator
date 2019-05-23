namespace ManagementAPI.Service.DataTransferObjects.Requests
{
    using System;

    public class CancelTournamentRequest
    {
        /// <summary>
        /// Gets or sets the cancellation reason.
        /// </summary>
        /// <value>
        /// The cancellation reason.
        /// </value>
        public String CancellationReason { get; set; }
    }
}
