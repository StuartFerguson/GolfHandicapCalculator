using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    public class GetNumberOfMembersByAgeCategoryReportResponse
    {
        #region Constructors

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

    public class MembersByAgeCategoryResponse
    {
        /// <summary>
        /// Gets or sets the age category.
        /// </summary>
        /// <value>
        /// The age category.
        /// </value>
        public String AgeCategory { get; set; }

        /// <summary>
        /// Gets or sets the number of members.
        /// </summary>
        /// <value>
        /// The number of members.
        /// </value>
        public Int32 NumberOfMembers { get; set; }
    }
}
