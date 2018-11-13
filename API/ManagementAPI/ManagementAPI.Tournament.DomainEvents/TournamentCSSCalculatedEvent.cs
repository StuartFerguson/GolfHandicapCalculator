using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Tournament.DomainEvents
{
    [JsonObject]
    public class TournamentCSSCalculatedEvent : DomainEvent
    {
        #region Constructors 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCSSCalculatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentCSSCalculatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCSSCalculatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="css">The CSS.</param>
        private TournamentCSSCalculatedEvent(Guid aggregateId, Guid eventId, Int32 adjustment, Int32 css) : base(aggregateId, eventId)
        {
            this.Adjustment = adjustment;
            this.CSS = css;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the adjustment.
        /// </summary>
        /// <value>
        /// The adjustment.
        /// </value>
        [JsonProperty]
        public Int32 Adjustment { get; private set; }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        [JsonProperty]
        public Int32 CSS { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="adjustment">The adjustment.</param>
        /// <param name="css">The CSS.</param>
        /// <returns></returns>
        public static TournamentCSSCalculatedEvent Create(Guid aggregateId, Int32 adjustment, Int32 css)
        {
            return new TournamentCSSCalculatedEvent(aggregateId, Guid.NewGuid(), adjustment, css);
        }

        #endregion
    }
}