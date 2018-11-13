using System;

namespace ManagementAPI.IntegrationTests.DataTransferObjects
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