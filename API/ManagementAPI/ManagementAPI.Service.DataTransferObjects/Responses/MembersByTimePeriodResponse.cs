namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class MembersByTimePeriodResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the number of members.
        /// </summary>
        /// <value>
        /// The number of members.
        /// </value>
        public Int32 NumberOfMembers { get; set; }

        /// <summary>
        /// Gets or sets the period.
        /// </summary>
        /// <value>
        /// The period.
        /// </value>
        public String Period { get; set; }

        #endregion
    }
}