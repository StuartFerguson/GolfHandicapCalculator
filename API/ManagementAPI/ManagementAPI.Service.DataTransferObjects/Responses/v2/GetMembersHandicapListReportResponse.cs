namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class GetMembersHandicapListReportResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMembersHandicapListReportResponse"/> class.
        /// </summary>
        public GetMembersHandicapListReportResponse()
        {
            this.MembersHandicapListReportResponse = new List<MembersHandicapListReportResponse>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets or sets the members handicap list report response.
        /// </summary>
        /// <value>
        /// The members handicap list report response.
        /// </value>
        public List<MembersHandicapListReportResponse> MembersHandicapListReportResponse { get; set; }

        #endregion
    }
}