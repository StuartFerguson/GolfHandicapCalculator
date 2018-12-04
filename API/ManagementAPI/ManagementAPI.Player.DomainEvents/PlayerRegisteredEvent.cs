using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.Player.DomainEvents
{
    [JsonObject]
    public class PlayerRegisteredEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRegisteredEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerRegisteredEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRegisteredEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="age">The age.</param>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <param name="emailAddress">The email address.</param>
        private PlayerRegisteredEvent(Guid aggregateId, Guid eventId, String firstName, String middleName, String lastName, String gender, Int32 age, Decimal exactHandicap, String emailAddress) : base(aggregateId, eventId)
        {
            this.FirstName = firstName;
            this.MiddleName = middleName;
            this.LastName = lastName;
            this.Gender = gender;
            this.Age = age;
            this.ExactHandicap = exactHandicap;
            this.EmailAddress = emailAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [JsonProperty]
        public String FirstName { get; private set; }

        /// <summary>
        /// Gets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        [JsonProperty]
        public String MiddleName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [JsonProperty]
        public String LastName { get; private set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [JsonProperty]
        public String Gender { get; private set; }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        [JsonProperty]
        public Int32 Age { get; private set; }

        /// <summary>
        /// Gets the exact handicap.
        /// </summary>
        /// <value>
        /// The exact handicap.
        /// </value>
        [JsonProperty]
        public Decimal ExactHandicap { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [JsonProperty]
        public String EmailAddress { get; private set; }

        #endregion

        #region Public Methods

        #region public static PlayerRegisteredEvent Create()
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="age">The age.</param>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public static PlayerRegisteredEvent Create(Guid aggregateId, String firstName, String middleName,String lastName, String gender, Int32 age, Decimal exactHandicap, String emailAddress)
        {
            return new PlayerRegisteredEvent(aggregateId, Guid.NewGuid(), firstName,middleName,lastName,gender,age, exactHandicap,emailAddress);
        }
        #endregion

        #endregion
    }

    [JsonObject]
    public class SecurityUserCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUserCreatedEvent" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public SecurityUserCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityUserCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        private SecurityUserCreatedEvent(Guid aggregateId, Guid eventId, Guid securityUserId) : base(aggregateId, eventId)
        {
            this.SecurityUserId = securityUserId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [JsonProperty]
        public Guid SecurityUserId { get; private set; }

        #endregion

        #region Public Methods

        #region public static SecurityUserCreatedEvent Create(Guid aggregateId, Guid securityUserId)
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <returns></returns>
        public static SecurityUserCreatedEvent Create(Guid aggregateId, Guid securityUserId)
        {
            return new SecurityUserCreatedEvent(aggregateId, Guid.NewGuid(), securityUserId);
        }
        #endregion

        #endregion
    }
}
