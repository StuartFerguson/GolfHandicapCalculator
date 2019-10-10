namespace ManagementAPI.Service.DataTransferObjects.Responses.v2
{
    using System;

    public class TournamentResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance has been cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCancelled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has been completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCompleted { get; set; }

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
        /// Gets or sets the player category.
        /// </summary>
        /// <value>
        /// The player category.
        /// </value>
        public PlayerCategory PlayerCategory { get; set; }

        /// <summary>
        /// Gets or sets the players scores recorded count.
        /// </summary>
        /// <value>
        /// The players scores recorded count.
        /// </value>
        public Int32 PlayersScoresRecordedCount { get; set; }

        /// <summary>
        /// Gets or sets the players signed up count.
        /// </summary>
        /// <value>
        /// The players signed up count.
        /// </value>
        public Int32 PlayersSignedUpCount { get; set; }

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