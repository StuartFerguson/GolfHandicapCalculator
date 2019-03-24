namespace ManagementAPI.Tournament.DomainEvents
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Newtonsoft.Json;
    using Shared.EventSourcing;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventSourcing.DomainEvent" />
    [JsonObject]
    public class TournamentResultForPlayerScoreProducedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPI.Tournament.DomainEvents.TournamentResultForPlayerScoreProducedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentResultForPlayerScoreProducedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentResultForPlayerScoreProducedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="division">The division.</param>
        /// <param name="divisionPosition">The division position.</param>
        /// <param name="grossScore">The gross score.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="netScore">The nett score.</param>
        /// <param name="last9Holes">The last9 holes.</param>
        /// <param name="last6Holes">The last6 holes.</param>
        /// <param name="last3Holes">The last3 holes.</param>
        private TournamentResultForPlayerScoreProducedEvent(Guid aggregateId,
                                                            Guid eventId,
                                                            Guid playerId,
                                                            Int32 division,
                                                            Int32 divisionPosition,
                                                            Int32 grossScore,
                                                            Int32 playingHandicap,
                                                            Int32 netScore,
                                                            Decimal last9Holes,
                                                            Decimal last6Holes,
                                                            Decimal last3Holes) : base(aggregateId, eventId)
        {
            this.PlayerId = playerId;
            this.Division = division;
            this.DivisionPosition = divisionPosition;
            this.GrossScore = grossScore;
            this.PlayingHandicap = playingHandicap;
            this.NetScore = netScore;
            this.Last9Holes = last9Holes;
            this.Last6Holes = last6Holes;
            this.Last3Holes = last3Holes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the division.
        /// </summary>
        /// <value>
        /// The division.
        /// </value>
        [JsonProperty]
        public Int32 Division { get; private set; }

        /// <summary>
        /// Gets the division position.
        /// </summary>
        /// <value>
        /// The division position.
        /// </value>
        [JsonProperty]
        public Int32 DivisionPosition { get; private set; }

        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        [JsonProperty]
        public Int32 GrossScore { get; private set; }

        /// <summary>
        /// Gets the last3 holes.
        /// </summary>
        /// <value>
        /// The last3 holes.
        /// </value>
        [JsonProperty]
        public Decimal Last3Holes { get; private set; }

        /// <summary>
        /// Gets the last6 holes.
        /// </summary>
        /// <value>
        /// The last6 holes.
        /// </value>
        [JsonProperty]
        public Decimal Last6Holes { get; private set; }

        /// <summary>
        /// Gets the last9 holes.
        /// </summary>
        /// <value>
        /// The last9 holes.
        /// </value>
        [JsonProperty]
        public Decimal Last9Holes { get; private set; }

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
        /// <param name="division">The division.</param>
        /// <param name="divisionPosition">The division position.</param>
        /// <param name="grossScore">The gross score.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="netScore">The nett score.</param>
        /// <param name="last9Holes">The last9 holes.</param>
        /// <param name="last6Holes">The last6 holes.</param>
        /// <param name="last3Holes">The last3 holes.</param>
        /// <returns></returns>
        public static TournamentResultForPlayerScoreProducedEvent Create(Guid aggregateId,
                                                                         Guid playerId,
                                                                         Int32 division,
                                                                         Int32 divisionPosition,
                                                                         Int32 grossScore,
                                                                         Int32 playingHandicap,
                                                                         Int32 netScore,
                                                                         Decimal last9Holes,
                                                                         Decimal last6Holes,
                                                                         Decimal last3Holes)
        {
            return new TournamentResultForPlayerScoreProducedEvent(aggregateId,
                                                                   Guid.NewGuid(),
                                                                   playerId,
                                                                   division,
                                                                   divisionPosition,
                                                                   grossScore,
                                                                   playingHandicap,
                                                                   netScore,
                                                                   last9Holes,
                                                                   last6Holes,
                                                                   last3Holes);
        }

        #endregion
    }
}