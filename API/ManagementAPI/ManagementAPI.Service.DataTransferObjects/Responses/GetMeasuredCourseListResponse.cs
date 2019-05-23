namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// </summary>
    public class GetMeasuredCourseListResponse
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
        /// Gets or sets the measured courses.
        /// </summary>
        /// <value>
        /// The measured courses.
        /// </value>
        public List<MeasuredCourseListResponse> MeasuredCourses { get; set; }

        #endregion
    }
}