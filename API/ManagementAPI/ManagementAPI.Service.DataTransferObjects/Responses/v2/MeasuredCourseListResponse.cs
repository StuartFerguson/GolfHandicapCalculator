namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class MeasuredCourseListResponse
    {
        #region Properties

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
        /// Gets or sets the standard scratch score.
        /// </summary>
        /// <value>
        /// The standard scratch score.
        /// </value>
        public Int32 StandardScratchScore { get; set; }

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