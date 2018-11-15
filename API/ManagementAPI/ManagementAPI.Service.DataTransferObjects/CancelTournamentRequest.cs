using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.DataTransferObjects
{
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
