namespace ManagementAPI.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 
    /// </summary>
    public class Tournament
    {
        #region Properties

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public Int32 Format { get; set; }

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
        /// Gets or sets a value indicating whether this instance has result been produced.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has result been produced; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasResultBeenProduced { get; set; }

        /// <summary>
        /// Gets or sets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; set; }

        /// <summary>
        /// Gets or sets the name of the measured course.
        /// </summary>
        /// <value>
        /// The name of the measured course.
        /// </value>
        public String MeasuredCourseName { get; set; }

        /// <summary>
        /// Gets or sets the measured course SSS.
        /// </summary>
        /// <value>
        /// The measured course SSS.
        /// </value>
        public Int32 MeasuredCourseSSS { get; set; }

        /// <summary>
        /// Gets or sets the measured course tee colour.
        /// </summary>
        /// <value>
        /// The measured course tee colour.
        /// </value>
        public String MeasuredCourseTeeColour { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the player category.
        /// </summary>
        /// <value>
        /// The player category.
        /// </value>
        public Int32 PlayerCategory { get; set; }

        /// <summary>
        /// Gets or sets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        public DateTime TournamentDate { get; set; }

        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        [Key]
        public Guid TournamentId { get; set; }

        #endregion
    }
}