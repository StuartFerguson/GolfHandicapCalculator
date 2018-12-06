using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Tournament.DomainEvents
{
    [JsonObject]
    public class TournamentCompletedEvent : DomainEvent
    {
        #region Constructors 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCompletedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentCompletedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCompletedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="completedDate">The completed date.</param>
        private TournamentCompletedEvent(Guid aggregateId,Guid eventId, DateTime completedDate) : base(aggregateId, eventId)
        {
            this.CompletedDate = completedDate;
        }
        #endregion

        #region Properties
        
        /// <summary>
        /// Gets the completed date.
        /// </summary>
        /// <value>
        /// The completed date.
        /// </value>
        [JsonProperty]
        public DateTime CompletedDate { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="completedDate">The completed date.</param>
        /// <returns></returns>
        public static TournamentCompletedEvent Create(Guid aggregateId, DateTime completedDate)
        {
            return new TournamentCompletedEvent(aggregateId, Guid.NewGuid(), completedDate);
        }

        #endregion
    }
}