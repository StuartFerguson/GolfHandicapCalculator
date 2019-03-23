namespace ManagementAPI.Service.Developer.DataTransferObjects
{
    using System;
    using System.Collections.Generic;

    public class GetTournamentResponse
    {
        #region Constructors

        public GetTournamentResponse()
        {
            this.Scores = new List<MemberScoreRecord>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the adjustment.
        /// </summary>
        /// <value>
        /// The adjustment.
        /// </value>
        public Int32 Adjustment { get; set; }

        /// <summary>
        /// Gets the cancelled date time.
        /// </summary>
        /// <value>
        /// The cancelled date time.
        /// </value>
        public DateTime CancelledDateTime { get; set; }

        /// <summary>
        /// Gets the cancelled reason.
        /// </summary>
        /// <value>
        /// The cancelled reason.
        /// </value>
        public String CancelledReason { get; set; }

        /// <summary>
        /// Gets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        public DateTime CompletedDateTime { get; set; }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public Int32 CSS { get; set; }

        /// <summary>
        /// Gets a value indicating whether [CSS has been calculated].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CSS has been calculated]; otherwise, <c>false</c>.
        /// </value>
        public Boolean CSSHasBeenCalculated { get; set; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public TournamentFormat Format { get; set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCancelled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCompleted { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; set; }

        /// <summary>
        /// Gets the measured course SSS.
        /// </summary>
        /// <value>
        /// The measured course SSS.
        /// </value>
        public Int32 MeasuredCourseSSS { get; set; }

        /// <summary>
        /// Gets the member category.
        /// </summary>
        /// <value>
        /// The member category.
        /// </value>
        public PlayerCategory MemberCategory { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        public List<MemberScoreRecord> Scores { get; set; }

        /// <summary>
        /// Gets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        public DateTime TournamentDate { get; set; }

        #endregion
    }
}