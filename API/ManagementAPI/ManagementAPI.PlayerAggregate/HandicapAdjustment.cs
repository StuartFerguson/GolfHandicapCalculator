namespace ManagementAPI.Player
{
    using System;

    internal class HandicapAdjustment
    {
        /// <summary>
        /// Gets or sets the number of strokes below CSS.
        /// </summary>
        /// <value>
        /// The number of strokes below CSS.
        /// </value>
        internal Int32 NumberOfStrokesBelowCss { get; private set; }

        /// <summary>
        /// Gets or sets the adjustment value per stroke.
        /// </summary>
        /// <value>
        /// The adjustment value per stroke.
        /// </value>
        internal Decimal AdjustmentValuePerStroke { get; private set; }

        /// <summary>
        /// Gets or sets the total adjustment.
        /// </summary>
        /// <value>
        /// The total adjustment.
        /// </value>
        internal Decimal TotalAdjustment { get; private set; }

        /// <summary>
        /// Gets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        internal Guid TournamentId { get; private set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        internal Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        internal Guid MeasuredCourseId { get; private set; }

        /// <summary>
        /// Gets the score date.
        /// </summary>
        /// <value>
        /// The score date.
        /// </value>
        internal DateTime ScoreDate { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapAdjustment"/> class.
        /// </summary>
        /// <param name="numberOfStrokesBelowCss">The number of strokes below CSS.</param>
        /// <param name="adjustmentValuePerStroke">The adjustment value per stroke.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="scoreDate">The score date.</param>
        private HandicapAdjustment(Int32 numberOfStrokesBelowCss,
                                   Decimal adjustmentValuePerStroke,
                                   Decimal totalAdjustment,
                                   Guid tournamentId,
                                   Guid golfClubId,
                                   Guid measuredCourseId,
                                   DateTime scoreDate)
        {
            this.NumberOfStrokesBelowCss = numberOfStrokesBelowCss;
            this.AdjustmentValuePerStroke = adjustmentValuePerStroke;
            this.TotalAdjustment = totalAdjustment;
            this.TournamentId = tournamentId;
            this.GolfClubId = golfClubId;
            this.MeasuredCourseId = measuredCourseId;
            this.ScoreDate = scoreDate;
        }

        /// <summary>
        /// Creates the specified number of strokes below CSS.
        /// </summary>
        /// <param name="numberOfStrokesBelowCss">The number of strokes below CSS.</param>
        /// <param name="adjustmentValuePerStroke">The adjustment value per stroke.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="scoreDate">The score date.</param>
        /// <returns></returns>
        internal static HandicapAdjustment Create(Int32 numberOfStrokesBelowCss,
                                                  Decimal adjustmentValuePerStroke,
                                                  Decimal totalAdjustment,
                                                  Guid tournamentId,
                                                  Guid golfClubId,
                                                  Guid measuredCourseId,
                                                  DateTime scoreDate)
        {
            return new HandicapAdjustment(numberOfStrokesBelowCss,adjustmentValuePerStroke,totalAdjustment, tournamentId, golfClubId, measuredCourseId, scoreDate);
        }
    }
}