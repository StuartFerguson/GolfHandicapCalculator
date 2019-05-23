namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;

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