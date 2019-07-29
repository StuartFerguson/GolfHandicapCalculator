namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System.Collections.Generic;

    public class GetTournamentListResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the tournaments.
        /// </summary>
        /// <value>
        /// The tournaments.
        /// </value>
        public List<GetTournamentResponse> Tournaments { get; set; }

        #endregion
    }
}