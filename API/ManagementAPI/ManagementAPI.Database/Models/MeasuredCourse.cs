namespace ManagementAPI.Database.Models
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class MeasuredCourse
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
        /// Gets or sets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the SSS.
        /// </summary>
        /// <value>
        /// The SSS.
        /// </value>
        public Int32 SSS { get; set; }

        /// <summary>
        /// Gets or sets the tee colour.
        /// </summary>
        /// <value>
        /// The tee colour.
        /// </value>
        public String TeeColour { get; set; }

        #endregion
    }
}