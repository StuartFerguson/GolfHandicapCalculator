namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;
    using System.Collections.Generic;

    public class GetNumberOfMembersByHandicapCategoryReportResponse
    {
        #region Constructors

        public GetNumberOfMembersByHandicapCategoryReportResponse()
        {
            this.MembersByHandicapCategoryResponse = new List<MembersByHandicapCategoryResponse>();
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
        /// Gets or sets the members by handicap category response.
        /// </summary>
        /// <value>
        /// The members by handicap category response.
        /// </value>
        public List<MembersByHandicapCategoryResponse> MembersByHandicapCategoryResponse { get; set; }

        #endregion
    }
}