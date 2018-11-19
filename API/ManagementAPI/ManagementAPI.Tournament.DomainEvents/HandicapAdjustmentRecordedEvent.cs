using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Tournament.DomainEvents
{
    [JsonObject]
    public class HandicapAdjustmentRecordedEvent : DomainEvent
    {
        #region Constructors        

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapAdjustmentRecordedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HandicapAdjustmentRecordedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HandicapAdjustmentRecordedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="grossScore">The gross score.</param>
        /// <param name="netScore">The net score.</param>
        /// <param name="css">The CSS.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        private HandicapAdjustmentRecordedEvent(Guid aggregateId,Guid eventId, Guid memberId, Int32 grossScore, Int32 netScore, Int32 css, Int32 playingHandicap, List<Decimal> adjustments, Decimal totalAdjustment) : base(aggregateId, eventId)
        {
            this.MemberId = memberId;
            this.GrossScore = grossScore;
            this.NetScore = netScore;
            this.CSS = css;
            this.PlayingHandicap = playingHandicap;
            this.Adjustments = adjustments;
            this.TotalAdjustment = totalAdjustment;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        [JsonProperty]
        public Guid MemberId { get; private set; }
        
        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        [JsonProperty]
        public Int32 GrossScore { get; private set; }

        /// <summary>
        /// Gets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        [JsonProperty]
        public Int32 NetScore { get; private set; }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        [JsonProperty]
        public Int32 CSS { get; private set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        [JsonProperty]
        public Int32 PlayingHandicap { get; private set; }

        /// <summary>
        /// Gets the adjustments.
        /// </summary>
        /// <value>
        /// The adjustments.
        /// </value>
        [JsonProperty]
        public List<Decimal> Adjustments { get; private set; }

        /// <summary>
        /// Gets the total adjustment.
        /// </summary>
        /// <value>
        /// The total adjustment.
        /// </value>
        [JsonProperty]
        public Decimal TotalAdjustment { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="grossScore">The gross score.</param>
        /// <param name="netScore">The net score.</param>
        /// <param name="css">The CSS.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="adjustments">The adjustments.</param>
        /// <param name="totalAdjustment">The total adjustment.</param>
        /// <returns></returns>
        public static HandicapAdjustmentRecordedEvent Create(Guid aggregateId, Guid memberId, Int32 grossScore, Int32 netScore, Int32 css, Int32 playingHandicap, List<Decimal> adjustments, Decimal totalAdjustment)
        {
            return new HandicapAdjustmentRecordedEvent(aggregateId, Guid.NewGuid(), memberId, grossScore, netScore, css,playingHandicap,adjustments,totalAdjustment);
        }

        #endregion
    }
}