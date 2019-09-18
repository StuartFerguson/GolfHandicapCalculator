namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class GetPlayerScoresResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPlayerScoresResponse"/> class.
        /// </summary>
        public GetPlayerScoresResponse()
        {
            this.Scores = new List<PlayerScoreResponse>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public List<PlayerScoreResponse> Scores { get; set; }

        #endregion
    }
}