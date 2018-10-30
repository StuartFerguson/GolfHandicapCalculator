using System;
using System.Diagnostics.CodeAnalysis;
using ManagementAPI.ClubConfiguration.DomainEvents;
using Shared.EventSourcing;
using Shared.EventStore;
using Shared.General;

namespace ManagementAPI.ClubConfigurationAggregate
{
    public class ClubConfigurationAggregate : Aggregate
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationAggregate"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ClubConfigurationAggregate()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationAggregate"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private ClubConfigurationAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
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

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; private set; }

        #endregion

        #region Public Methods

        #region public static ClubConfigurationAggregate Create(Guid aggregateId)        
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static ClubConfigurationAggregate Create(Guid aggregateId)
        {
            return new ClubConfigurationAggregate(aggregateId);
        }
        #endregion

        #region public void CreateClubConfiguration()
        /// <summary>
        /// Creates the club configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="town">The town.</param>
        /// <param name="region">The region.</param>
        /// <param name="postalCode">The postal code.</param>
        /// <param name="telephoneNumber">The telephone number.</param>
        /// <param name="website">The website.</param>
        /// <param name="emailAddress">The email address.</param>
        public void CreateClubConfiguration(String name, String addressLine1, String addressLine2, String town, String region, 
                                            String postalCode, String telephoneNumber, String website, String emailAddress)
        {
            // Now apply the business rules
            Guard.ThrowIfNullOrEmpty(name, typeof(ArgumentNullException), "A club must have a name to be created");
            Guard.ThrowIfNullOrEmpty(addressLine1, typeof(ArgumentNullException), "A club must have an Address line 1 to be created");
            Guard.ThrowIfNullOrEmpty(town, typeof(ArgumentNullException), "A club must have a town to be created");
            Guard.ThrowIfNullOrEmpty(region, typeof(ArgumentNullException), "A club must have a region to be created");
            Guard.ThrowIfNullOrEmpty(postalCode, typeof(ArgumentNullException), "A club must have a postal code to be created");
            Guard.ThrowIfNullOrEmpty(telephoneNumber, typeof(ArgumentNullException), "A club must have a telephone number to be created");

            CheckHasClubConfigurationAlreadyBeenCreated();

            // Now create the domain event
            ClubConfigurationCreatedEvent clubConfigurationCreatedEvent = ClubConfigurationCreatedEvent.Create(
                this.AggregateId, name, addressLine1, addressLine2,
                town, region, postalCode, telephoneNumber, website, emailAddress);

            // Apply and Pend the event
            this.ApplyAndPend(clubConfigurationCreatedEvent);
        }
        #endregion

        #endregion

        #region Protected Methods

        #region protected override void PlayEvent(DomainEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic) domainEvent);
        }
        #endregion

        #endregion

        #region Private Methods (Play Event)

        #region private void PlayEvent(ClubConfigurationCreatedEvent domainEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(ClubConfigurationCreatedEvent domainEvent)
        {
            this.Name = domainEvent.Name;
            this.AddressLine1 = domainEvent.AddressLine1;
            this.AddressLine2 = domainEvent.AddressLine2;
            this.Town = domainEvent.Town;
            this.Region = domainEvent.Region;
            this.PostalCode = domainEvent.PostalCode;
            this.TelephoneNumber = domainEvent.TelephoneNumber;
            this.Website = domainEvent.Website;
            this.EmailAddress = domainEvent.EmailAddress;
            this.HasBeenCreated = true;
        }
        #endregion

        #endregion

        #region Private Methods

        #region private void CheckHasClubConfigurationAlreadyBeenCreated()
        /// <summary>
        /// Checks the has club configuration already been created.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This operation cannot be performed on a Club that already has configuration created</exception>
        private void CheckHasClubConfigurationAlreadyBeenCreated()
        {
            if (this.HasBeenCreated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a Club that already has configuration created");
            }
        }
        #endregion

        #endregion
    }
}
