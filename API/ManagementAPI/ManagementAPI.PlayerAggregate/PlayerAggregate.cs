using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ManagementAPI.Player.DomainEvents;
using Shared.EventSourcing;
using Shared.EventStore;
using Shared.General;

namespace ManagementAPI.Player
{
    public class PlayerAggregate : Aggregate
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAggregate"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerAggregate()
        {
            // Nothing here
            this.Memberships = new List<ClubMembership>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAggregate"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private PlayerAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.Memberships = new List<ClubMembership>();
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public String FirstName { get; private set; }

        /// <summary>
        /// Gets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public String MiddleName { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public String LastName { get; private set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public String Gender { get; private set; }

        /// <summary>
        /// Gets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets the exact handicap.
        /// </summary>
        /// <value>
        /// The exact handicap.
        /// </value>
        public Decimal ExactHandicap { get; private set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; private set; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been registered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been registered; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenRegistered { get; private set; }

        /// <summary>
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        public Guid SecurityUserId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has security user been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has security user been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasSecurityUserBeenCreated { get; private set; }

        #endregion

        #region Fields

        /// <summary>
        /// The memberships
        /// </summary>
        private List<ClubMembership> Memberships;

        #endregion

        #region Public Methods

        #region public static PlayerAggregate Create(Guid aggregateId)        
        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static PlayerAggregate Create(Guid aggregateId)
        {
            return new PlayerAggregate(aggregateId);
        }
        #endregion

        #region public void Register(String firstName, String middleName, String lastName, String gender, DateTime dateOfBirth, Decimal exactHandicap, String emailAddress)
        /// <summary>
        /// Registers the specified first name.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="gender">The gender.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <param name="emailAddress">The email address.</param>
        public void Register(String firstName, String middleName, String lastName, String gender, DateTime dateOfBirth, Decimal exactHandicap, String emailAddress)
        {
            // Validate the registration details
            Guard.ThrowIfNullOrEmpty(firstName, typeof(ArgumentNullException), "A first name is required to register a player");
            Guard.ThrowIfNullOrEmpty(lastName, typeof(ArgumentNullException), "A last name is required to register a player");
            Guard.ThrowIfNullOrEmpty(gender, typeof(ArgumentNullException), "A gender is required to register a player");
            Guard.ThrowIfNullOrEmpty(emailAddress, typeof(ArgumentNullException), "An email address is required to register a player");

            this.ValidateGender(gender);
            this.ValidateDateOfBirth(dateOfBirth);
            this.ValidateHandicap(exactHandicap);

            this.CheckIfPlayerAlreadyRegistered();

            // Create the domain event
            PlayerRegisteredEvent playerRegisteredEvent = PlayerRegisteredEvent.Create(this.AggregateId, firstName,middleName,lastName, gender,
                dateOfBirth, exactHandicap, emailAddress);

            // Apply and pend
            this.ApplyAndPend(playerRegisteredEvent);
        }
        #endregion

        #region public void CreateSecurityUser(Guid secuityUserId)        
        /// <summary>
        /// Creates the security user.
        /// </summary>
        /// <param name="securityUserId">The security user identifier.</param>
        public void CreateSecurityUser(Guid securityUserId)
        {
            Guard.ThrowIfInvalidGuid(securityUserId, typeof(ArgumentNullException), "A security user id is required to create a players security user");

            this.CheckIfPlayerHasBeenRegistered();
            this.CheckIfSecurityUserAlreadyCreated();

            // Create the domain event
            SecurityUserCreatedEvent securityUserCreatedEvent = SecurityUserCreatedEvent.Create(this.AggregateId, securityUserId);

            // Apply and pend
            this.ApplyAndPend(securityUserCreatedEvent);
        }
        #endregion

        #region public void AddAcceptedMembership(Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        /// <summary>
        /// Adds the accepted membership.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        public void AddAcceptedMembership(Guid golfClubId, Guid membershipId, String membershipNumber, DateTime acceptedDateTime)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "A golf club Id must be provided to add an accepted membership to a player.");
            Guard.ThrowIfInvalidGuid(membershipId, typeof(ArgumentNullException), "A membership Id must be provided to add an accepted membership to a player.");
            Guard.ThrowIfNullOrEmpty(membershipNumber, typeof(ArgumentNullException), "A membership number must be provided to add an accepted membership to a player.");

            // Check if player is registered
            this.CheckIfPlayerHasBeenRegistered();

            // Ensure not a duplicate membership
            this.EnsureNotADuplicateMembershipAcceptance(membershipId);

            // Create the domain event
            AcceptedMembershipAddedEvent acceptedMembershipAddedEvent = AcceptedMembershipAddedEvent.Create(this.AggregateId, golfClubId, membershipId, membershipNumber, acceptedDateTime);

            // Apply and Pend
            this.ApplyAndPend(acceptedMembershipAddedEvent);
        }

        #endregion

        #region public void AddRejectedMembership(Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime)
        /// <summary>
        /// Adds the rejected membership.
        /// </summary>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectedDateTime">The rejected date time.</param>
        public void AddRejectedMembership(Guid golfClubId, Guid membershipId, String rejectionReason, DateTime rejectedDateTime)
        {
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "A golf club Id must be provided to add an rejected membership to a player.");
            Guard.ThrowIfInvalidGuid(membershipId, typeof(ArgumentNullException), "A membership Id must be provided to add an rejected membership to a player.");
            Guard.ThrowIfNullOrEmpty(rejectionReason, typeof(ArgumentNullException), "A membership number must be provided to add an rejected membership to a player.");

            // Check if player is registered
            this.CheckIfPlayerHasBeenRegistered();

            // Ensure not a duplicate membership
            this.EnsureNotADuplicateMembershipAcceptance(membershipId);

            // Create the domain event
            RejectedMembershipAddedEvent rejectedMembershipAddedEvent = RejectedMembershipAddedEvent.Create(this.AggregateId, golfClubId, membershipId, rejectionReason, rejectedDateTime);

            // Apply and Pend
            this.ApplyAndPend(rejectedMembershipAddedEvent);
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

        #region Private Methods (Play Events)

        #region private void PlayEvent(PlayerRegisteredEvent playerRegisteredEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="playerRegisteredEvent">The player registered event.</param>
        private void PlayEvent(PlayerRegisteredEvent playerRegisteredEvent)
        {
            this.FirstName = playerRegisteredEvent.FirstName;
            this.MiddleName = playerRegisteredEvent.MiddleName;
            this.LastName = playerRegisteredEvent.LastName;
            this.Gender = playerRegisteredEvent.Gender;
            this.DateOfBirth = playerRegisteredEvent.DateOfBirth;
            this.ExactHandicap = playerRegisteredEvent.ExactHandicap;
            this.EmailAddress = playerRegisteredEvent.EmailAddress;
            this.HasBeenRegistered = true;
            this.PlayingHandicap = this.CalculatePlayingHandicap(this.ExactHandicap);
            this.HandicapCategory = this.CalculateHandicapCategory(this.PlayingHandicap);
        }
        #endregion

        #region private void PlayEvent(SecurityUserCreatedEvent securityUserCreatedEvent)        
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="securityUserCreatedEvent">The security user created event.</param>
        private void PlayEvent(SecurityUserCreatedEvent securityUserCreatedEvent)
        {
            this.SecurityUserId = securityUserCreatedEvent.SecurityUserId;
            this.HasSecurityUserBeenCreated = true;
        }
        #endregion

        #region private void PlayEvent(AcceptedMembershipAddedEvent acceptedMembershipAddedEvent)
        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="acceptedMembershipAddedEvent">The accepted membership added event.</param>
        private void PlayEvent(AcceptedMembershipAddedEvent acceptedMembershipAddedEvent)
        {
            ClubMembership membership = ClubMembership.Create();

            membership.Approve(acceptedMembershipAddedEvent.GolfClubId, acceptedMembershipAddedEvent.MembershipId,
                acceptedMembershipAddedEvent.MembershipNumber,acceptedMembershipAddedEvent.AcceptedDateTime);
            
            this.Memberships.Add(membership);
        }
        #endregion

        #endregion

        #region Private Methods (Other)

        #region private Int32 CalculateHandicapCategory(Int32 playingHandicap)        
        /// <summary>
        /// Calculates the handicap category.
        /// </summary>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Int32 CalculateHandicapCategory(Int32 playingHandicap)
        {
            Int32 category = 0;

            switch (playingHandicap)
            {
                case var h when (h < 5):
                    category = 1;
                    break;
                case var h when (h >= 6 && h <=12):
                    category = 2;
                    break;
                case var h when (h >= 13 && h <=21):
                    category = 3;
                    break;
                case var h when (h >= 22 && h <=28):
                    category = 4;
                    break;
                case var h when (h >= 29):
                    category = 5;
                    break;
            }

            return category;
        }
        #endregion

        #region private Int32 CalculatePlayingHandicap(Decimal exactHandicap)        
        /// <summary>
        /// Calculates the playing handicap.
        /// </summary>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Int32 CalculatePlayingHandicap(Decimal exactHandicap)
        {
            return Convert.ToInt32(Math.Round(exactHandicap, MidpointRounding.AwayFromZero));
        }
        #endregion

        #region private void CheckIfPlayerAlreadyRegistered()        
        /// <summary>
        /// Checks if player already registered.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">This operation is invalid for a player that has already been registered</exception>
        private void CheckIfPlayerAlreadyRegistered()
        {
            if (this.HasBeenRegistered)
            {
                throw new InvalidOperationException("This operation is invalid for a player that has already been registered");
            }
        }
        #endregion

        #region private void ValidateHandicap(Decimal exactHandicap)        
        /// <summary>
        /// Validates the handicap.
        /// </summary>
        /// <param name="exactHandicap">The exact handicap.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">exactHandicap - Maximum exact handicap is 36</exception>
        private void ValidateHandicap(Decimal exactHandicap)
        {
            if (exactHandicap > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(exactHandicap), "Maximum exact handicap is 36");
            }
        }
        #endregion

        #region private void ValidateDateOfBirth(DateTime dateOfBirth)        
        /// <summary>
        /// Validates the date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth), "A players date of birth cannot be in the future");
            }
        }
        #endregion

        #region private void ValidateGender(String gender)        
        /// <summary>
        /// Validates the gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">gender - Gender must be either Male (M) or Female (F)</exception>
        private void ValidateGender(String gender)
        {
            if (gender != "M" && gender != "F")
            {
                throw new ArgumentOutOfRangeException(nameof(gender), "Gender must be either Male (M) or Female (F)");
            }
        }
        #endregion

        #region private void CheckIfPlayerHasBeenRegistered()                
        /// <summary>
        /// Checks if player has been registered.
        /// </summary>
        private void CheckIfPlayerHasBeenRegistered()
        {
            if (!this.HasBeenRegistered)
            {
                throw new InvalidOperationException("This operation is invalid for a player that has not been registered");
            }
        }
        #endregion

        #region private void CheckIfSecurityUserAlreadyCreated()        
        /// <summary>
        /// Checks if security user already created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation is invalid for a player that already has a created security user</exception>
        private void CheckIfSecurityUserAlreadyCreated()
        {
            if (this.HasSecurityUserBeenCreated)
            {
                throw new InvalidOperationException("This operation is invalid for a player that already has a created security user");
            }
        }
        #endregion

        #region private void EnsureNotADuplicateMembershipAcceptance(Guid membershipId)        
        /// <summary>
        /// Ensures the not a duplicate membership acceptance.
        /// </summary>
        /// <param name="membershipId">The membership identifier.</param>
        /// <exception cref="NotImplementedException"></exception>
        private void EnsureNotADuplicateMembershipAcceptance(Guid membershipId)
        {
            Boolean isDuplicateMembership = this.Memberships.Any(m => m.MembershipId == membershipId);

            if (isDuplicateMembership)
            {
                throw new InvalidOperationException($"A accepted membership with Id [{membershipId}] has already been added to Player Id [{this.AggregateId}]");
            }
        }
        #endregion

        #endregion
    }
}
