namespace ManagementAPI.Player
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using DomainEvents;
    using Shared.EventSourcing;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;

    public class PlayerAggregate : Aggregate
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAggregate"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerAggregate()
        {
            this.HandicapAdjustments = new List<HandicapAdjustment>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAggregate"/> class.
        /// </summary>D:\Projects\StuartFerguson\GolfHandicapCalculatorAPI\API\ManagementAPI\ManagementAPI.Service\Dockerfile
        /// <param name="aggregateId">The aggregate identifier.</param>
        private PlayerAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.HandicapAdjustments = new List<HandicapAdjustment>();
        }

        #endregion

        private List<HandicapAdjustment> HandicapAdjustments;

        #region Properties

        /// <summary>
        /// Gets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime DateOfBirth { get; private set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; private set; }

        /// <summary>
        /// Gets the exact handicap.
        /// </summary>
        /// <value>
        /// The exact handicap.
        /// </value>
        public Decimal ExactHandicap { get; private set; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public String FirstName { get; private set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public String FullName { get; private set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public String Gender { get; private set; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been registered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been registered; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenRegistered { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has security user been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has security user been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasSecurityUserBeenCreated { get; private set; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public String LastName { get; private set; }

        /// <summary>
        /// Gets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public String MiddleName { get; private set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; private set; }

        /// <summary>
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        public Guid SecurityUserId { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static PlayerAggregate Create(Guid aggregateId)
        {
            return new PlayerAggregate(aggregateId);
        }

        /// <summary>
        /// Adjusts the handicap.
        /// </summary>
        /// <param name="handicapAdjustmentDataTransferObject">The handicap adjustment data transfer object.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <param name="scoreDate">The score date.</param>
        public void AdjustHandicap(HandicapAdjustmentDataTransferObject handicapAdjustmentDataTransferObject,
                                   Guid tournamentId,
                                   Guid golfClubId,
                                   Guid measuredCourseId,
                                   DateTime scoreDate)
        {
            Guard.ThrowIfNull(handicapAdjustmentDataTransferObject, typeof(ArgumentNullException), "Handicap Adjustment details must be provided to adjust a players handicap");
            Guard.ThrowIfInvalidGuid(tournamentId, typeof(ArgumentNullException), "Tournament Id must be provided to adjust a players handicap");
            Guard.ThrowIfInvalidGuid(golfClubId, typeof(ArgumentNullException), "Golf Club Id must be provided to adjust a players handicap");
            Guard.ThrowIfInvalidGuid(measuredCourseId, typeof(ArgumentNullException), "Measured Course Id must be provided to adjust a players handicap");
            Guard.ThrowIfInvalidDate(scoreDate, typeof(ArgumentNullException), "Score Date must be provided to adjust a players handicap");

            this.CheckIfPlayerHasBeenRegistered();

            this.CheckNotDuplicateHandicapAdjustment(handicapAdjustmentDataTransferObject, tournamentId, golfClubId, measuredCourseId, scoreDate);

            HandicapAdjustedEvent handicapAdjustedEvent = HandicapAdjustedEvent.Create(this.AggregateId,
                                                                                       handicapAdjustmentDataTransferObject.NumberOfStrokesBelowCss,
                                                                                       handicapAdjustmentDataTransferObject.AdjustmentValuePerStroke,
                                                                                       handicapAdjustmentDataTransferObject.TotalAdjustment,
                                                                                       tournamentId,
                                                                                       golfClubId,
                                                                                       measuredCourseId,
                                                                                       scoreDate);

            this.ApplyAndPend(handicapAdjustedEvent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handicapAdjustmentDataTransferObject"></param>
        /// <param name="tournamentId"></param>
        /// <param name="golfClubId"></param>
        /// <param name="measuredCourseId"></param>
        /// <param name="scoreDate"></param>
        private void CheckNotDuplicateHandicapAdjustment(HandicapAdjustmentDataTransferObject handicapAdjustmentDataTransferObject, Guid tournamentId,
                                                         Guid golfClubId,
                                                         Guid measuredCourseId,
                                                         DateTime scoreDate)
        {
            if (this.HandicapAdjustments.Any(h => h.GolfClubId == golfClubId &&
                                                  h.AdjustmentValuePerStroke == handicapAdjustmentDataTransferObject.AdjustmentValuePerStroke &&
                                                  h.MeasuredCourseId == measuredCourseId &&
                                                  h.NumberOfStrokesBelowCss == handicapAdjustmentDataTransferObject.NumberOfStrokesBelowCss &&
                                                  h.TournamentId == tournamentId &&
                                                  h.ScoreDate == scoreDate &&
                                                  h.TotalAdjustment == handicapAdjustmentDataTransferObject.TotalAdjustment))
            {
                throw new InvalidOperationException("Handicap adjustment has already been applied to this player");
            }
        }

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
        public void Register(String firstName,
                             String middleName,
                             String lastName,
                             String gender,
                             DateTime dateOfBirth,
                             Decimal exactHandicap,
                             String emailAddress)
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

            // Create the Player Registered domain event
            PlayerRegisteredEvent playerRegisteredEvent =
                PlayerRegisteredEvent.Create(this.AggregateId, firstName, middleName, lastName, gender, dateOfBirth, exactHandicap, emailAddress);

            // Apply and pend
            this.ApplyAndPend(playerRegisteredEvent);

            // Create the Opening Handicap Added domain event
            OpeningExactHandicapAddedEvent openingExactHandicapAddedEvent = OpeningExactHandicapAddedEvent.Create(this.AggregateId, exactHandicap);

            // Apply and pend
            this.ApplyAndPend(openingExactHandicapAddedEvent);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Calculates the handicap category.
        /// </summary>
        /// <param name="playingHandicap">The playing handicap.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private Int32 CalculateHandicapCategory(Int32 playingHandicap)
        {
            Int32 category = 0;

            switch(playingHandicap)
            {
                case var h when (h <= 5):
                    category = 1;
                    break;
                case var h when (h >= 6 && h <= 12):
                    category = 2;
                    break;
                case var h when (h >= 13 && h <= 21):
                    category = 3;
                    break;
                case var h when (h >= 22 && h <= 28):
                    category = 4;
                    break;
                case var h when (h >= 29):
                    category = 5;
                    break;
            }

            return category;
        }

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

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="openingExactHandicapAddedEvent">The opening handicap added event.</param>
        private void PlayEvent(OpeningExactHandicapAddedEvent openingExactHandicapAddedEvent)
        {
            this.ExactHandicap = openingExactHandicapAddedEvent.ExactHandicap;
            this.PlayingHandicap = this.CalculatePlayingHandicap(this.ExactHandicap);
            this.HandicapCategory = this.CalculateHandicapCategory(this.PlayingHandicap);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="playerRegisteredEvent">The player registered event.</param>
        private void PlayEvent(PlayerRegisteredEvent playerRegisteredEvent)
        {
            this.FirstName = playerRegisteredEvent.FirstName;
            this.MiddleName = playerRegisteredEvent.MiddleName;
            this.LastName = playerRegisteredEvent.LastName;
            this.FullName =
                $"{playerRegisteredEvent.FirstName}{(String.IsNullOrEmpty(playerRegisteredEvent.MiddleName) ? " " : " " + playerRegisteredEvent.MiddleName + " ")}{playerRegisteredEvent.LastName}";
            this.Gender = playerRegisteredEvent.Gender;
            this.DateOfBirth = playerRegisteredEvent.DateOfBirth;
            this.EmailAddress = playerRegisteredEvent.EmailAddress;
            this.HasBeenRegistered = true;
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="handicapAdjustedEvent">The handicap adjusted event.</param>
        private void PlayEvent(HandicapAdjustedEvent handicapAdjustedEvent)
        {
            HandicapAdjustment handicapAdjustment = HandicapAdjustment.Create(handicapAdjustedEvent.NumberOfStrokesBelowCss,
                                                                              handicapAdjustedEvent.AdjustmentValuePerStroke,
                                                                              handicapAdjustedEvent.TotalAdjustment,
                                                                              handicapAdjustedEvent.TournamentId,
                                                                              handicapAdjustedEvent.GolfClubId,
                                                                              handicapAdjustedEvent.MeasuredCourseId,
                                                                              handicapAdjustedEvent.ScoreDate);

            this.HandicapAdjustments.Add(handicapAdjustment);
            this.ExactHandicap += handicapAdjustedEvent.TotalAdjustment;
            this.PlayingHandicap = this.CalculatePlayingHandicap(this.ExactHandicap);
            this.HandicapCategory = this.CalculateHandicapCategory(this.PlayingHandicap);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="securityUserCreatedEvent">The security user created event.</param>
        private void PlayEvent(SecurityUserCreatedEvent securityUserCreatedEvent)
        {
            this.SecurityUserId = securityUserCreatedEvent.SecurityUserId;
            this.HasSecurityUserBeenCreated = true;
        }
        
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
    }
}