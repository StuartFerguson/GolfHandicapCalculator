namespace ManagementAPI.Service.DataTransferObjects.Responses
{
    using System;

    public class PlayerScoreResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public Int32 CSS { get; set; }

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
        /// Gets or sets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        public Int32 GrossScore { get; set; }

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
        /// Gets or sets the measured course tee colour.
        /// </summary>
        /// <value>
        /// The measured course tee colour.
        /// </value>
        public String MeasuredCourseTeeColour { get; set; }

        /// <summary>
        /// Gets or sets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        public Int32 NetScore { get; set; }

        /// <summary>
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        public DateTime TournamentDate { get; set; }

        /// <summary>
        /// Gets or sets the tournament format.
        /// </summary>
        /// <value>
        /// The tournament format.
        /// </value>
        public TournamentFormat TournamentFormat { get; set; }

        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tournament.
        /// </summary>
        /// <value>
        /// The name of the tournament.
        /// </value>
        public String TournamentName { get; set; }

        #endregion
    }
}