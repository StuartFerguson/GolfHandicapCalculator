namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class GetNumberOfMembersByAgeCategoryReportResponse
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GetNumberOfMembersByAgeCategoryReportResponse"/> class.
        /// </summary>
        public GetNumberOfMembersByAgeCategoryReportResponse()
        {
            this.MembersByAgeCategoryResponse = new List<MembersByAgeCategoryResponse>();
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
        public List<MembersByAgeCategoryResponse> MembersByAgeCategoryResponse { get; set; }

        #endregion
    }
}