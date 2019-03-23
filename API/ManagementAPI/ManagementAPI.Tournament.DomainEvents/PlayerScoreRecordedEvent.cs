namespace ManagementAPI.Tournament.DomainEvents
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    [JsonObject]
    public class PlayerScoreRecordedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerScoreRecordedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerScoreRecordedEvent()
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
        private PlayerScoreRecordedEvent(Guid aggregateId,
                                         Guid eventId,
                                         Guid playerId,
                                         Int32 playingHandicap,
                                         Dictionary<Int32, Int32> holeScores) : base(aggregateId, eventId)
        {
            this.PlayerId = playerId;
            this.PlayingHandicap = playingHandicap;
            this.HoleScores = holeScores;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        [JsonProperty]
        public Dictionary<Int32, Int32> HoleScores { get; private set; }

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
        /// <returns></returns>
        public static PlayerScoreRecordedEvent Create(Guid aggregateId,
                                                      Guid playerId,
                                                      Int32 playingHandicap,
                                                      Dictionary<Int32, Int32> holeScores)
        {
            return new PlayerScoreRecordedEvent(aggregateId, Guid.NewGuid(), playerId, playingHandicap, holeScores);
        }

        #endregion
    }
}