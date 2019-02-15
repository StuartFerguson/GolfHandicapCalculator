namespace DataGenerator
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    internal class GolfClubDetails
    {
        #region Properties

        /// <summary>
        /// Gets or sets the admin email address.
        /// </summary>
        /// <value>
        /// The admin email address.
        /// </value>
        public String AdminEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the admin password.
        /// </summary>
        /// <value>
        /// The admin password.
        /// </value>
        public String AdminPassword { get; set; }

        /// <summary>
        /// Gets or sets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets or sets the name of the golf club.
        /// </summary>
        /// <value>
        /// The name of the golf club.
        /// </value>
        public String GolfClubName { get; set; }

        /// <summary>
        /// Gets or sets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; set; }

        #endregion
    }
}