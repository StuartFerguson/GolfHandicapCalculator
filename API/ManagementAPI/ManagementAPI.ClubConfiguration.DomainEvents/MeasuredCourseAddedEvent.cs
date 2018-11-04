using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.ClubConfiguration.DomainEvents
{
    [JsonObject]
    public class MeasuredCourseAddedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasuredCourseAddedEvent"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public MeasuredCourseAddedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MeasuredCourseAddedEvent" /> class from being created.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="teeColour">The tee colour.</param>
        /// <param name="standardScratchScore">The standard scratch score.</param>
        private MeasuredCourseAddedEvent(Guid aggregateId, Guid eventId, Guid measuredCourseId, String name, String teeColour, Int32 standardScratchScore) : base(aggregateId, eventId)
        {
            this.MeasuredCourseId = measuredCourseId;
            this.Name = name;
            this.TeeColour = teeColour;
            this.StandardScratchScore = standardScratchScore;
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
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty]
        public String Name { get; private set; }
        
        /// <summary>
        /// Gets the tee colour.
        /// </summary>
        /// <value>
        /// The tee colour.
        /// </value>
        [JsonProperty]
        public String TeeColour { get; private set; }

        /// <summary>
        /// Gets the standard scratch score.
        /// </summary>
        /// <value>
        /// The standard scratch score.
        /// </value>
        [JsonProperty]
        public Int32 StandardScratchScore { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="courseId">The course identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="teeColour">The tee colour.</param>
        /// <param name="standardScratchScore">The standard scratch score.</param>
        /// <returns></returns>
        public static MeasuredCourseAddedEvent Create(Guid aggregateId, Guid measuredCourseId, String name,
            String teeColour, Int32 standardScratchScore)
        {
            return new MeasuredCourseAddedEvent(aggregateId, Guid.NewGuid(), measuredCourseId, name,teeColour,standardScratchScore);
        }

        #endregion
    }
}
