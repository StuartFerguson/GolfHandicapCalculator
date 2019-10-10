using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    public class CreateTournamentResponse
    {
        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; set; }
    }
}
