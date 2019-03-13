namespace ManagementAPI.GolfClub.DomainEvents
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
    public class TournamentDivisionAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Values the tuple.
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        public TournamentDivisionAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentDivisionAddedEvent"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="division">The division.</param>
        /// <param name="startHandicap">The start handicap.</param>
        /// <param name="endHandicap">The end handicap.</param>
        private TournamentDivisionAddedEvent(Guid aggregateId,
                                             Guid eventId,
                                             Int32 division,
                                             Int32 startHandicap,
                                             Int32 endHandicap) : base(aggregateId, eventId)
        {
            this.Division = division;
            this.StartHandicap = startHandicap;
            this.EndHandicap = endHandicap;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the division number.
        /// </summary>
        /// <value>
        /// The division number.
        /// </value>
        public Int32 Division { get; private set; }

        /// <summary>
        /// Gets the end handicap.
        /// </summary>
        /// <value>
        /// The end handicap.
        /// </value>
        public Int32 EndHandicap { get; private set; }

        /// <summary>
        /// Gets the start handicap.
        /// </summary>
        /// <value>
        /// The start handicap.
        /// </value>
        public Int32 StartHandicap { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="divisionNumber">The division number.</param>
        /// <param name="startHandicap">The start handicap.</param>
        /// <param name="endHandicap">The end handicap.</param>
        /// <returns></returns>
        public static TournamentDivisionAddedEvent Create(Guid aggregateId,
                                                          Int32 divisionNumber,
                                                          Int32 startHandicap,
                                                          Int32 endHandicap)
        {
            return new TournamentDivisionAddedEvent(aggregateId, Guid.NewGuid(), divisionNumber, startHandicap, endHandicap);
        }

        #endregion
    }
}