using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Tournament.DomainEvents
{
    [JsonObject]
    public class TournamentCancelledEvent : DomainEvent
    {
        #region Constructors 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPI.Tournament.DomainEvents.TournamentCancelledEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentCancelledEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagementAPI.Tournament.DomainEvents.TournamentCancelledEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="cancelledDate">The cancelled date.</param>
        /// <param name="cancellationReason">The cancellation reason.</param>
        private TournamentCancelledEvent(Guid aggregateId,Guid eventId, DateTime cancelledDate,String cancellationReason) : base(aggregateId, eventId)
        {
            this.CancelledDate = cancelledDate;
            this.CancellationReason = cancellationReason;
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
        public DateTime CancelledDate { get; private set; }

        /// <summary>
        /// Gets the cancellation reason.
        /// </summary>
        /// <value>
        /// The cancellation reason.
        /// </value>
        [JsonProperty]
        public String CancellationReason { get; private set; }
        
        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="cancelledDate">The cancelled date.</param>
        /// <param name="cancellationReason">The cancellation reason.</param>
        /// <returns></returns>
        public static TournamentCancelledEvent Create(Guid aggregateId, DateTime cancelledDate, String cancellationReason)
        {
            return new TournamentCancelledEvent(aggregateId, Guid.NewGuid(), cancelledDate, cancellationReason);
        }

        #endregion
    }
}