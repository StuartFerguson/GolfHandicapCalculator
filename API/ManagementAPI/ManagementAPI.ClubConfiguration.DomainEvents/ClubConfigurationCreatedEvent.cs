using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Shared.EventSourcing;

namespace ManagementAPI.ClubConfiguration.DomainEvents
{
    [JsonObject]
    public class ClubConfigurationCreatedEvent : DomainEvent
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationCreatedEvent" /> class.
        /// </summary>
        public ClubConfigurationCreatedEvent()
        {
            //We need this for serialisation, so just embrace the DDD crime
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationCreatedEvent" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="telephoneNumber">The telephone number.</param>
        /// <param name="website">The website.</param>
        /// <param name="emailAddress">The email address.</param>
        private ClubConfigurationCreatedEvent(Guid aggregateId, Guid eventId, String name, String addressLine1, 
                                              String addressLine2, String town, String region, String postalCode, 
                                              String telephoneNumber, String website, String emailAddress) : base(aggregateId, eventId)
        {
            this.Name = name;
            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.Town = town;
            this.Region = region;
            this.PostalCode = postalCode;
            this.TelephoneNumber = telephoneNumber;
            this.Website = website;
            this.EmailAddress = emailAddress;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; private set; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; private set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; private set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region  { get; private set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; private set; }

        /// <summary>
        /// Gets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; private set; }

        /// <summary>
        /// Gets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public String Website { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="telephoneNumber">The telephone number.</param>
        /// <param name="website">The website.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public static ClubConfigurationCreatedEvent Create(Guid aggregateId,String name, String addressLine1, String addressLine2, 
                                                           String town, String region, String postalCode, String telephoneNumber, 
                                                           String website, String emailAddress)
        {
            return new ClubConfigurationCreatedEvent(aggregateId, Guid.NewGuid(), name, addressLine1, addressLine2, town, region,
                                                     postalCode, telephoneNumber, website, emailAddress);
        }

        #endregion



    }
}
