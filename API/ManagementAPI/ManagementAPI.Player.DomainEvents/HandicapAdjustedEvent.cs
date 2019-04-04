namespace ManagementAPI.Player.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    [JsonObject]
    public class HandicapAdjustedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRegisteredEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapAdjustedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRegisteredEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="numberOfStrokesBelowCss">The number of strokes below CSS.</param>
        /// <param name="adjustmentValuePerStroke">The adjustment value per stroke.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="scoreDate">The score date.</param>
        private HandicapAdjustedEvent(Guid aggregateId,
                                      Guid eventId,
                                      Int32 numberOfStrokesBelowCss,
                                      Decimal adjustmentValuePerStroke,
                                      Decimal totalAdjustment,
                                      Guid tournamentId,
                                      Guid golfClubId,
                                      Guid measuredCourseId,
                                      DateTime scoreDate) : base(aggregateId, eventId)
        {
            this.NumberOfStrokesBelowCss = numberOfStrokesBelowCss;
            this.AdjustmentValuePerStroke = adjustmentValuePerStroke;
            this.TotalAdjustment = totalAdjustment;
            this.TournamentId = tournamentId;
            this.GolfClubId = golfClubId;
            this.MeasuredCourseId = measuredCourseId;
            this.ScoreDate = scoreDate;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the adjustment value per stroke.
        /// </summary>
        /// <value>
        /// The adjustment value per stroke.
        /// </value>
        [JsonProperty]
        public Decimal AdjustmentValuePerStroke { get; private set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        [JsonProperty]
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        [JsonProperty]
        public Guid MeasuredCourseId { get; private set; }

        /// <summary>
        /// Gets the number of strokes below CSS.
        /// </summary>
        /// <value>
        /// The number of strokes below CSS.
        /// </value>
        [JsonProperty]
        public Int32 NumberOfStrokesBelowCss { get; private set; }

        /// <summary>
        /// Gets the score date.
        /// </summary>
        /// <value>
        /// The score date.
        /// </value>
        [JsonProperty]
        public DateTime ScoreDate { get; private set; }

        /// <summary>
        /// Gets the total adjustment.
        /// </summary>
        /// <value>
        /// The total adjustment.
        /// </value>
        [JsonProperty]
        public Decimal TotalAdjustment { get; private set; }

        /// <summary>
        /// Gets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        [JsonProperty]
        public Guid TournamentId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="numberOfStrokesBelowCss">The number of strokes below CSS.</param>
        /// <param name="adjustmentValuePerStroke">The adjustment value per stroke.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="scoreDate">The score date.</param>
        /// <returns></returns>
        public static HandicapAdjustedEvent Create(Guid aggregateId,
                                                   Int32 numberOfStrokesBelowCss,
                                                   Decimal adjustmentValuePerStroke,
                                                   Decimal totalAdjustment,
                                                   Guid tournamentId,
                                                   Guid golfClubId,
                                                   Guid measuredCourseId,
                                                   DateTime scoreDate)
        {
            return new HandicapAdjustedEvent(aggregateId,
                                             Guid.NewGuid(),
                                             numberOfStrokesBelowCss,
                                             adjustmentValuePerStroke,
                                             totalAdjustment,
                                             tournamentId,
                                             golfClubId,
                                             measuredCourseId,
                                             scoreDate);
        }

        #endregion
    }
}