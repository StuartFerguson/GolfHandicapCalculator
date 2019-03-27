namespace ManagementAPI.Tournament.DomainEvents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventSourcing.DomainEvent" />
    [JsonObject]
    public class PlayerScorePublishedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScorePublishedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerScorePublishedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScoreRecordedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="playerId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="grossScore">The gross score.</param>
        /// <param name="netScore">The net score.</param>
        /// <param name="css">The CSS.</param>
        private PlayerScorePublishedEvent(Guid aggregateId,
                                          Guid eventId,
                                          Guid playerId,
                                          Int32 playingHandicap,
                                          Dictionary<Int32, Int32> holeScores,
                                          Guid golfClubId,
                                          Guid measuredCourseId,
                                          Int32 grossScore,
                                          Int32 netScore,
                                          Int32 css) : base(aggregateId, eventId)
        {
            this.PlayerId = playerId;
            this.PlayingHandicap = playingHandicap;
            this.HoleScores = holeScores;
            this.GolfClubId = golfClubId;
            this.MeasuredCourseId = measuredCourseId;
            this.GrossScore = grossScore;
            this.NetScore = netScore;
            this.CSS = css;
        }

        #endregion

        #region Properties

        [JsonProperty]
        public Int32 CSS { get; private set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        [JsonProperty]
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        [JsonProperty]
        public Int32 GrossScore { get; private set; }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        [JsonProperty]
        public Dictionary<Int32, Int32> HoleScores { get; private set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        [JsonProperty]
        public Guid MeasuredCourseId { get; private set; }

        /// <summary>
        /// Gets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        [JsonProperty]
        public Int32 NetScore { get; private set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        [JsonProperty]
        public Guid PlayerId { get; private set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        [JsonProperty]
        public Int32 PlayingHandicap { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="grossScore">The gross score.</param>
        /// <param name="netScore">The net score.</param>
        /// <param name="css">The CSS.</param>
        /// <returns></returns>
        public static PlayerScorePublishedEvent Create(Guid aggregateId,
                                                       Guid playerId,
                                                       Int32 playingHandicap,
                                                       Dictionary<Int32, Int32> holeScores,
                                                       Guid golfClubId,
                                                       Guid measuredCourseId,
                                                       Int32 grossScore,
                                                       Int32 netScore,
                                                       Int32 css)
        {
            return new PlayerScorePublishedEvent(aggregateId,
                                                 Guid.NewGuid(),
                                                 playerId,
                                                 playingHandicap,
                                                 holeScores,
                                                 golfClubId,
                                                 measuredCourseId,
                                                 grossScore,
                                                 netScore,
                                                 css);
        }

        #endregion
    }
}