using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Tournament.DomainEvents
{
    [JsonObject]
    public class MemberScoreRecordedEvent : DomainEvent
    {
        #region Constructors        

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberScoreRecordedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MemberScoreRecordedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberScoreRecordedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        private MemberScoreRecordedEvent(Guid aggregateId,Guid eventId, Guid memberId, Int32 playingHandicap, Dictionary<Int32, Int32> holeScores) : base(aggregateId, eventId)
        {
            this.MemberId = memberId;
            this.PlayingHandicap = playingHandicap;
            this.HoleScores = holeScores;
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

        [JsonProperty]
        public Int32 PlayingHandicap { get; private set; }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        [JsonProperty]
        public Dictionary<Int32, Int32> HoleScores { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="memberId">The member identifier.</param>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <param name="holeScores">The hole scores.</param>
        /// <returns></returns>
        public static MemberScoreRecordedEvent Create(Guid aggregateId, Guid memberId, Int32 playingHandicap,  Dictionary<Int32, Int32> holeScores)
        {
            return new MemberScoreRecordedEvent(aggregateId, Guid.NewGuid(), memberId, playingHandicap, holeScores);
        }

        #endregion
    }
}