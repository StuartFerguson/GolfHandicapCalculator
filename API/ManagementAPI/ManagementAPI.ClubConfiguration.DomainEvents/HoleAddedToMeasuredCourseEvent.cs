using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.GolfClub.DomainEvents
{
    [JsonObject]
    public class HoleAddedToMeasuredCourseEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HoleAddedToMeasuredCourseEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public HoleAddedToMeasuredCourseEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="HoleAddedToMeasuredCourseEvent" /> class from being created.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="holeNumber">The hole number.</param>
        /// <param name="lengthInYards">The length in yards.</param>
        /// <param name="lengthInMeters">The length in meters.</param>
        /// <param name="par">The par.</param>
        /// <param name="strokeIndex">Index of the stroke.</param>
        private HoleAddedToMeasuredCourseEvent(Guid aggregateId, Guid eventId, Guid measuredCourseId, Int32 holeNumber, Int32 lengthInYards,
            Int32 lengthInMeters, Int32 par, Int32 strokeIndex) : base(aggregateId, eventId)
        {
            this.MeasuredCourseId = measuredCourseId;
            this.HoleNumber = holeNumber;
            this.LengthInYards = lengthInYards;
            this.LengthInMeters = lengthInMeters;
            this.Par = par;
            this.StrokeIndex = strokeIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        [JsonProperty]
        public Guid MeasuredCourseId { get; private set; }

        /// <summary>
        /// Gets the hole number.
        /// </summary>
        /// <value>
        /// The hole number.
        /// </value>
        [JsonProperty]
        public Int32 HoleNumber { get; private set; }

        /// <summary>
        /// Gets or sets the length in yards.
        /// </summary>
        /// <value>
        /// The length in yards.
        /// </value>
        [JsonProperty]
        public Int32 LengthInYards { get; private set; }

        /// <summary>
        /// Gets or sets the length in meters.
        /// </summary>
        /// <value>
        /// The length in meters.
        /// </value>
        [JsonProperty]
        public Int32 LengthInMeters { get; private set; }

        /// <summary>
        /// Gets the par.
        /// </summary>
        /// <value>
        /// The par.
        /// </value>
        [JsonProperty]
        public Int32 Par { get; private set; }

        /// <summary>
        /// Gets the index of the stroke.
        /// </summary>
        /// <value>
        /// The index of the stroke.
        /// </value>
        [JsonProperty]
        public Int32 StrokeIndex { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="holeNumber">The hole number.</param>
        /// <param name="lengthInYards">The length in yards.</param>
        /// <param name="lengthInMeters">The length in meters.</param>
        /// <param name="par">The par.</param>
        /// <param name="strokeIndex">Index of the stroke.</param>
        /// <returns></returns>
        public static HoleAddedToMeasuredCourseEvent Create(Guid aggregateId, Guid measuredCourseId, Int32 holeNumber, Int32 lengthInYards,
            Int32 lengthInMeters, Int32 par, Int32 strokeIndex)
        {
            return new HoleAddedToMeasuredCourseEvent(aggregateId, Guid.NewGuid(), measuredCourseId, holeNumber,lengthInYards,lengthInMeters,par, strokeIndex);
        }

        #endregion
    }
}