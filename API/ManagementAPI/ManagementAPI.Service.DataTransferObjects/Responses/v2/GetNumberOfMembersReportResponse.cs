namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class GetNumberOfMembersReportResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets or sets the number of members.
        /// </summary>
        /// <value>
        /// The number of members.
        /// </value>
        public Int32 NumberOfMembers { get; set; }

        #endregion
    }
}