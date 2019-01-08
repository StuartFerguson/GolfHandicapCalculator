using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Tournament.DomainEvents
{
    [JsonObject]
    public class TournamentCreatedEvent : DomainEvent
    {
        #region Constructors 
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public TournamentCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="tournamentDate">The tournament date.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="measuredCourseSSS">The measured course SSS.</param>
        /// <param name="name">The name.</param>
        /// <param name="memberCategory">The member category.</param>
        /// <param name="format">The format.</param>
        private TournamentCreatedEvent(Guid aggregateId,Guid eventId, DateTime tournamentDate, Guid golfClubId, Guid measuredCourseId,
            Int32 measuredCourseSSS, String name, Int32 memberCategory, Int32 format) : base(aggregateId, eventId)
        {
            this.TournamentDate = tournamentDate;
            this.GolfClubId = golfClubId;
            this.MeasuredCourseId = measuredCourseId;
            this.MeasuredCourseSSS = measuredCourseSSS;
            this.Name = name;
            this.MemberCategory = memberCategory;
            this.Format = format;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        [JsonProperty]
        public DateTime TournamentDate { get; private set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        [JsonProperty]
        public Guid GolfClubId { get; private set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        [JsonProperty]
        public Guid MeasuredCourseId { get; private set; }

        /// <summary>
        /// Gets the measured course SSS.
        /// </summary>
        /// <value>
        /// The measured course SSS.
        /// </value>
        [JsonProperty]
        public Int32 MeasuredCourseSSS { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [JsonProperty]
        public String Name { get; private set; }

        /// <summary>
        /// Gets the member category.
        /// </summary>
        /// <value>
        /// The member category.
        /// </value>
        [JsonProperty]
        public Int32 MemberCategory { get; private set; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [JsonProperty]
        public Int32 Format { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="tournamentDate">The tournament date.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="measuredCourseSSS">The measured course SSS.</param>
        /// <param name="name">The name.</param>
        /// <param name="memberCategory">The member category.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static TournamentCreatedEvent Create(Guid aggregateId, DateTime tournamentDate, Guid golfClubId, Guid measuredCourseId,
            Int32 measuredCourseSSS, String name, Int32 memberCategory, Int32 format)
        {
            return new TournamentCreatedEvent(aggregateId, Guid.NewGuid(), tournamentDate, golfClubId,measuredCourseId, measuredCourseSSS, name,memberCategory, format);
        }

        #endregion
    }
}
