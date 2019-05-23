namespace ManagementAPI.GolfClub
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using DomainEvents;
    using Shared.EventSourcing;
    using Shared.EventStore;
    using Shared.Exceptions;
    using Shared.General;

    public class GolfClubAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The match secretary security user identifier list
        /// </summary>
        private List<Guid> MatchSecretarySecurityUserIdList;

        /// <summary>
        /// The measured courses
        /// </summary>
        private List<MeasuredCourse> MeasuredCourses;

        /// <summary>
        /// The tournament divisions
        /// </summary>
        private List<TournamentDivision> TournamentDivisions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAggregate"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public GolfClubAggregate()
        {
            // Nothing here            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAggregate"/> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private GolfClubAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
        }

        #endregion

        #region Properties

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
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; private set; }

        /// <summary>
        /// Gets the golf club administrator security user identifier.
        /// </summary>
        /// <value>
        /// The golf club administrator security user identifier.
        /// </value>
        public Guid GolfClubAdministratorSecurityUserId { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has admin security user been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has admin security user been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasAdminSecurityUserBeenCreated { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; private set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; private set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region { get; private set; }

        /// <summary>
        /// Gets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; private set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; private set; }

        /// <summary>
        /// Gets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public String Website { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the measured course.
        /// </summary>
        /// <param name="measuredCourse">The measured course.</param>
        public void AddMeasuredCourse(MeasuredCourseDataTransferObject measuredCourse)
        {
            // Apply the business rules here
            // Check club has been created
            this.CheckHasGolfClubAlreadyBeenCreated();

            // Check for a duplicate measured course addition
            this.CheckNotDuplicateMeasuredCourse(measuredCourse);

            // Validate the measured course data
            this.ValidateMeasuredCourse(measuredCourse);

            // Now apply and pend the required events

            // First the measured course added event
            MeasuredCourseAddedEvent measuredCourseAddedEvent = MeasuredCourseAddedEvent.Create(this.AggregateId,
                                                                                                measuredCourse.MeasuredCourseId,
                                                                                                measuredCourse.Name,
                                                                                                measuredCourse.TeeColour,
                                                                                                measuredCourse.StandardScratchScore);
            this.ApplyAndPend(measuredCourseAddedEvent);

            // Now add an event for each hole
            foreach (HoleDataTransferObject holeDataTransferObject in measuredCourse.Holes)
            {
                HoleAddedToMeasuredCourseEvent holeAddedToMeasuredCourseEvent =
                    HoleAddedToMeasuredCourseEvent.Create(this.AggregateId,
                                                          measuredCourse.MeasuredCourseId,
                                                          holeDataTransferObject.HoleNumber,
                                                          holeDataTransferObject.LengthInYards,
                                                          holeDataTransferObject.LengthInMeters,
                                                          holeDataTransferObject.Par,
                                                          holeDataTransferObject.StrokeIndex);
                this.ApplyAndPend(holeAddedToMeasuredCourseEvent);
            }
        }

        /// <summary>
        /// Adds the tournament division.
        /// </summary>
        /// <param name="tournamentDivision">The tournament division.</param>
        public void AddTournamentDivision(TournamentDivisionDataTransferObject tournamentDivision)
        {
            Guard.ThrowIfNull(tournamentDivision, typeof(ArgumentNullException), "Tournament Division cannot be null");

            this.CheckHasGolfClubAlreadyBeenCreated();

            // Check for duplicate division
            this.ValidateForDuplicateDivision(tournamentDivision.Division);

            // check for clashing ranges
            this.ValidateDivisionRanges(tournamentDivision);

            this.ValidateTournamentDivision(tournamentDivision);

            // Raise a domain event
            TournamentDivisionAddedEvent tournamentDivisionAddedEvent =
                TournamentDivisionAddedEvent.Create(this.AggregateId, tournamentDivision.Division, tournamentDivision.StartHandicap, tournamentDivision.EndHandicap);

            // Apply and pend
            this.ApplyAndPend(tournamentDivisionAddedEvent);
        }

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static GolfClubAggregate Create(Guid aggregateId)
        {
            return new GolfClubAggregate(aggregateId);
        }

        /// <summary>
        /// Creates the golf club.
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
        public void CreateGolfClub(String name,
                                   String addressLine1,
                                   String addressLine2,
                                   String town,
                                   String region,
                                   String postalCode,
                                   String telephoneNumber,
                                   String website,
                                   String emailAddress)
        {
            // Now apply the business rules
            Guard.ThrowIfNullOrEmpty(name, typeof(ArgumentNullException), "A club must have a name to be created");
            Guard.ThrowIfNullOrEmpty(addressLine1, typeof(ArgumentNullException), "A club must have an Address line 1 to be created");
            Guard.ThrowIfNullOrEmpty(town, typeof(ArgumentNullException), "A club must have a town to be created");
            Guard.ThrowIfNullOrEmpty(region, typeof(ArgumentNullException), "A club must have a region to be created");
            Guard.ThrowIfNullOrEmpty(postalCode, typeof(ArgumentNullException), "A club must have a postal code to be created");
            Guard.ThrowIfNullOrEmpty(telephoneNumber, typeof(ArgumentNullException), "A club must have a telephone number to be created");

            this.CheckGolfClubHasNotAlreadyBeenCreated();

            // Now create the domain event
            GolfClubCreatedEvent golfClubCreatedEvent =
                GolfClubCreatedEvent.Create(this.AggregateId, name, addressLine1, addressLine2, town, region, postalCode, telephoneNumber, website, emailAddress);

            // Apply and Pend the event
            this.ApplyAndPend(golfClubCreatedEvent);
        }

        /// <summary>
        /// Creates the admin user.
        /// </summary>
        /// <param name="golfClubAdminSecurityUserId">The golf club admin security user identifier.</param>
        public void CreateGolfClubAdministratorSecurityUser(Guid golfClubAdminSecurityUserId)
        {
            Guard.ThrowIfInvalidGuid(golfClubAdminSecurityUserId,
                                     typeof(ArgumentNullException),
                                     "A golf club admin security user id is required to create a club admin security user");

            this.CheckHasGolfClubAlreadyBeenCreated();
            this.CheckHasClubAdminSecurityUserAlreadyBeenCreated();

            // Create the domain event
            GolfClubAdministratorSecurityUserCreatedEvent golfClubAdministratorSecurityUserCreatedEvent =
                GolfClubAdministratorSecurityUserCreatedEvent.Create(this.AggregateId, golfClubAdminSecurityUserId);

            // Apply and pend
            this.ApplyAndPend(golfClubAdministratorSecurityUserCreatedEvent);
        }

        /// <summary>
        /// Creates the match secretary security user.
        /// </summary>
        /// <param name="matchSecretarySecurityUserId">The match secretary security user identifier.</param>
        public void CreateMatchSecretarySecurityUser(Guid matchSecretarySecurityUserId)
        {
            Guard.ThrowIfInvalidGuid(matchSecretarySecurityUserId,
                                     typeof(ArgumentNullException),
                                     "A match secretary admin security user id is required to create a match secretary security user");

            this.CheckHasGolfClubAlreadyBeenCreated();
            this.CheckHasMatchSecretarySecurityUserAlreadyBeenCreated(matchSecretarySecurityUserId);

            // Create the domain event
            MatchSecretarySecurityUserCreatedEvent matchSecretarySecurityUserCreatedEvent =
                MatchSecretarySecurityUserCreatedEvent.Create(this.AggregateId, matchSecretarySecurityUserId);

            // Apply and pend
            this.ApplyAndPend(matchSecretarySecurityUserCreatedEvent);
        }

        /// <summary>
        /// Gets the measured course.
        /// </summary>
        /// <param name="measuredCourseId">The measured course identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No measured course found for Club {this.Name} with Measured Course Id {measuredCourseId}</exception>
        public MeasuredCourseDataTransferObject GetMeasuredCourse(Guid measuredCourseId)
        {
            Boolean measuredCourseFound = this.MeasuredCourses.Any(m => m.MeasuredCourseId == measuredCourseId);

            if (!measuredCourseFound)
            {
                throw new NotFoundException($"No measured course found for Club {this.Name} with Measured Course Id {measuredCourseId}");
            }

            MeasuredCourse measuredCourse = this.MeasuredCourses.Where(m => m.MeasuredCourseId == measuredCourseId).Single();

            MeasuredCourseDataTransferObject result = new MeasuredCourseDataTransferObject
                                                      {
                                                          Name = measuredCourse.Name,
                                                          MeasuredCourseId = measuredCourse.MeasuredCourseId,
                                                          StandardScratchScore = measuredCourse.StandardScratchScore,
                                                          TeeColour = measuredCourse.TeeColour,
                                                          Holes = new List<HoleDataTransferObject>()
                                                      };

            foreach (Hole measuredCourseHole in measuredCourse.Holes)
            {
                result.Holes.Add(new HoleDataTransferObject
                                 {
                                     HoleNumber = measuredCourseHole.HoleNumber,
                                     Par = measuredCourseHole.Par,
                                     LengthInYards = measuredCourseHole.LengthInYards,
                                     StrokeIndex = measuredCourseHole.StrokeIndex,
                                     LengthInMeters = measuredCourseHole.LengthInMeters
                                 });
            }

            return result;
        }

        /// <summary>
        /// Gets the measured courses.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No measured courses found for Club {this.Name}</exception>
        public List<MeasuredCourseDataTransferObject> GetMeasuredCourses()
        {
            if (this.MeasuredCourses.Any() == false)
            {
                throw new NotFoundException($"No measured courses found for Club {this.Name}");
            }

            List<MeasuredCourseDataTransferObject> measuredCourses = new List<MeasuredCourseDataTransferObject>();

            foreach (MeasuredCourse measuredCourse in this.MeasuredCourses)
            {
                MeasuredCourseDataTransferObject result = new MeasuredCourseDataTransferObject
                                                          {
                                                              Name = measuredCourse.Name,
                                                              MeasuredCourseId = measuredCourse.MeasuredCourseId,
                                                              StandardScratchScore = measuredCourse.StandardScratchScore,
                                                              TeeColour = measuredCourse.TeeColour,
                                                              Holes = new List<HoleDataTransferObject>()
                                                          };

                foreach (Hole measuredCourseHole in measuredCourse.Holes)
                {
                    result.Holes.Add(new HoleDataTransferObject
                                     {
                                         HoleNumber = measuredCourseHole.HoleNumber,
                                         Par = measuredCourseHole.Par,
                                         LengthInYards = measuredCourseHole.LengthInYards,
                                         StrokeIndex = measuredCourseHole.StrokeIndex,
                                         LengthInMeters = measuredCourseHole.LengthInMeters
                                     });
                }

                measuredCourses.Add(result);
            }

            return measuredCourses;
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
        /// Checks the golf club has not already been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a Golf Club that already been created</exception>
        private void CheckGolfClubHasNotAlreadyBeenCreated()
        {
            if (this.HasBeenCreated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a Golf Club that already been created");
            }
        }

        /// <summary>
        /// Checks the has club admin security user already been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a Club that already has had an admin security user created</exception>
        private void CheckHasClubAdminSecurityUserAlreadyBeenCreated()
        {
            if (this.HasAdminSecurityUserBeenCreated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a Club that already has had an admin security user created");
            }
        }

        /// <summary>
        /// Checks the has golf club already been created.
        /// </summary>
        /// <exception cref="InvalidOperationException">This operation cannot be performed on a Golf Club that has already been created</exception>
        private void CheckHasGolfClubAlreadyBeenCreated()
        {
            if (!this.HasBeenCreated)
            {
                throw new InvalidOperationException("This operation cannot be performed on a Golf Club that has not been created");
            }
        }

        /// <summary>
        /// Checks the has match secretary security user already been created.
        /// </summary>
        /// <param name="securityUserId">The security user identifier.</param>
        /// <exception cref="InvalidDataException">Match secretary user with Id {securityUserId}</exception>
        private void CheckHasMatchSecretarySecurityUserAlreadyBeenCreated(Guid securityUserId)
        {
            if (this.MatchSecretarySecurityUserIdList.Any(m => m == securityUserId))
            {
                throw new InvalidOperationException($"Match secretary user with Id {securityUserId} has already been added to this Golf Club");
            }
        }

        /// <summary>
        /// Checks the not duplicate measured course.
        /// </summary>
        /// <param name="measuredCourse">The measured course.</param>
        /// <exception cref="InvalidOperationException">Unable to add measured course as this has a duplicate Course Id</exception>
        private void CheckNotDuplicateMeasuredCourse(MeasuredCourseDataTransferObject measuredCourse)
        {
            if (this.MeasuredCourses.Any(m => m.MeasuredCourseId == measuredCourse.MeasuredCourseId))
            {
                throw new InvalidOperationException("Unable to add measured course as this has a duplicate Course Id");
            }
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(MatchSecretarySecurityUserCreatedEvent domainEvent)
        {
            this.MatchSecretarySecurityUserIdList.Add(domainEvent.MatchSecretarySecurityUserId);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(TournamentDivisionAddedEvent domainEvent)
        {
            TournamentDivision tournamentDivision = TournamentDivision.Create(domainEvent.Division, domainEvent.StartHandicap, domainEvent.EndHandicap);

            this.TournamentDivisions.Add(tournamentDivision);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(GolfClubCreatedEvent domainEvent)
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
            this.MeasuredCourses = new List<MeasuredCourse>();
            this.TournamentDivisions = new List<TournamentDivision>();
            this.MatchSecretarySecurityUserIdList = new List<Guid>();
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(MeasuredCourseAddedEvent domainEvent)
        {
            MeasuredCourse measuredCourse =
                MeasuredCourse.Create(domainEvent.MeasuredCourseId, domainEvent.Name, domainEvent.TeeColour, domainEvent.StandardScratchScore);
            this.MeasuredCourses.Add(measuredCourse);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(HoleAddedToMeasuredCourseEvent domainEvent)
        {
            // Find the measured course
            MeasuredCourse measuredCourse = this.MeasuredCourses.Single(m => m.MeasuredCourseId == domainEvent.MeasuredCourseId);
            measuredCourse.AddHole(Hole.Create(domainEvent.HoleNumber, domainEvent.LengthInYards, domainEvent.LengthInMeters, domainEvent.Par, domainEvent.StrokeIndex));
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(GolfClubAdministratorSecurityUserCreatedEvent domainEvent)
        {
            this.HasAdminSecurityUserBeenCreated = true;
            this.GolfClubAdministratorSecurityUserId = domainEvent.GolfClubAdministratorSecurityUserId;
        }

        /// <summary>
        /// Validates the division ranges.
        /// </summary>
        /// <param name="tournamentDivision">The tournament division.</param>
        /// <exception cref="InvalidDataException">
        /// Start Handicap Already in a division range.
        /// or
        /// Start Handicap Already in a division range.
        /// </exception>
        private void ValidateDivisionRanges(TournamentDivisionDataTransferObject tournamentDivision)
        {
            Boolean startHandicapInRange =
                this.TournamentDivisions.Any(t => tournamentDivision.StartHandicap >= t.StartHandicap && tournamentDivision.StartHandicap <= t.EndHandicap);
            Boolean endHandicapInRange =
                this.TournamentDivisions.Any(t => tournamentDivision.EndHandicap >= t.StartHandicap && tournamentDivision.EndHandicap <= t.EndHandicap);

            if (startHandicapInRange)
            {
                throw new InvalidDataException("Start Handicap Already in a division range.");
            }

            if (endHandicapInRange)
            {
                throw new InvalidDataException("End Handicap Already in a division range.");
            }
        }

        /// <summary>
        /// Validates for duplicate division.
        /// </summary>
        /// <param name="tournamentDivision">The tournament division.</param>
        /// <exception cref="InvalidOperationException">Division Number {tournamentDivision} already added to Golf Club Id {this.AggregateId}</exception>
        private void ValidateForDuplicateDivision(Int32 tournamentDivision)
        {
            Boolean duplicateDivision = this.TournamentDivisions.Any(t => t.Division == tournamentDivision);

            if (duplicateDivision)
            {
                throw new InvalidOperationException($"Division Number {tournamentDivision} already added to Golf Club Id {this.AggregateId}");
            }
        }

        /// <summary>
        /// Validates the measured course.
        /// </summary>
        /// <param name="measuredCourse">The measured course.</param>
        private void ValidateMeasuredCourse(MeasuredCourseDataTransferObject measuredCourse)
        {
            Guard.ThrowIfNullOrEmpty(measuredCourse.Name, typeof(ArgumentNullException), "A measured course must have a name");
            Guard.ThrowIfNullOrEmpty(measuredCourse.TeeColour, typeof(ArgumentNullException), "A measured course must have a tee colour");
            Guard.ThrowIfNegative(measuredCourse.StandardScratchScore,
                                  typeof(ArgumentNullException),
                                  "A measured course must have a non negative Standard Scratch Score");
            Guard.ThrowIfZero(measuredCourse.StandardScratchScore, typeof(ArgumentNullException), "A measured course must have a non zero Standard Scratch Score");

            // Only validate the holes if the have been populated
            if (measuredCourse.Holes.Count != 18)
            {
                throw new InvalidDataException("A measured course must have 18 holes");
            }

            // Check there are no missing hole numbers
            IEnumerable<Int32> holeNumberList = measuredCourse.Holes.Select(h => h.HoleNumber);
            List<Int32> missingHoleNumbers = Enumerable
                                             .Range(GolfClubAggregate.MinimumHoleNumber, GolfClubAggregate.MaximumHoleNumber - GolfClubAggregate.MinimumHoleNumber + 1)
                                             .Except(holeNumberList).ToList();

            if (missingHoleNumbers.Count > 0)
            {
                // there are missing hole numbers
                throw new InvalidDataException($"Hole numbers {string.Join(",", missingHoleNumbers)} are missing from the measured course");
            }

            // Check there are no missing stroke indexes
            IEnumerable<Int32> strokeIndexList = measuredCourse.Holes.Select(h => h.StrokeIndex);
            List<Int32> missingStrokeIndexes = Enumerable
                                               .Range(GolfClubAggregate.MinimumStrokeIndex,
                                                      GolfClubAggregate.MaximumStrokeIndex - GolfClubAggregate.MinimumStrokeIndex + 1).Except(strokeIndexList).ToList();

            if (missingStrokeIndexes.Count > 0)
            {
                // there are missing stroke indexes
                throw new InvalidDataException($"Hole with Stroke Indexes {string.Join(",", missingStrokeIndexes)} are missing from the measured course");
            }

            // Check all holes have a valid length in yards
            IEnumerable<Int32> holesWithInvalidLengthInYards = measuredCourse.Holes.Where(h => h.LengthInYards <= 0).Select(h => h.HoleNumber);

            if (holesWithInvalidLengthInYards.Any())
            {
                throw new
                    InvalidDataException($"All holes must have a length in yards, hole numbers {string.Join(",", holesWithInvalidLengthInYards)} are missing a valid length in yards");
            }

            // Check all holes have a valid par
            IEnumerable<Int32> holesWithInvalidPar = measuredCourse.Holes.Where(h => h.Par != 3 && h.Par != 4 && h.Par != 5).Select(h => h.HoleNumber);

            if (holesWithInvalidPar.Any())
            {
                throw new
                    InvalidDataException($"All holes must have a valid Par of 3,4 or 5, hole numbers {string.Join(",", holesWithInvalidPar)} are missing a valid Par");
            }
        }

        /// <summary>
        /// Validates the tournament division.
        /// </summary>
        /// <param name="tournamentDivision">The tournament division.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Division - Tournament Division must be between 1 and 5
        /// or
        /// StartHandicap - Tournament Division Start Handicap must be between -10 and 36
        /// or
        /// EndHandicap - Tournament Division End Handicap must be between -10 and 36
        /// or
        /// EndHandicap - Tournament Division End Handicap cannot be <= Start Handicap
        /// </exception>
        private void ValidateTournamentDivision(TournamentDivisionDataTransferObject tournamentDivision)
        {
            if (tournamentDivision.Division <= 0 || tournamentDivision.Division > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(tournamentDivision.Division), "Tournament Division must be between 1 and 5");
            }

            if (tournamentDivision.StartHandicap < -10 || tournamentDivision.StartHandicap > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(tournamentDivision.StartHandicap), "Tournament Division Start Handicap must be between -10 and 36");
            }

            if (tournamentDivision.EndHandicap < -10 || tournamentDivision.EndHandicap > 36)
            {
                throw new ArgumentOutOfRangeException(nameof(tournamentDivision.EndHandicap), "Tournament Division End Handicap must be between -10 and 36");
            }

            if (tournamentDivision.EndHandicap <= tournamentDivision.StartHandicap)
            {
                throw new ArgumentOutOfRangeException(nameof(tournamentDivision.EndHandicap), "Tournament Division End Handicap cannot be <= Start Handicap");
            }
        }

        #endregion

        #region Others

        /// <summary>
        /// The maximum hole number
        /// </summary>
        private const Int32 MaximumHoleNumber = 18;

        /// <summary>
        /// The maximum stroke index
        /// </summary>
        private const Int32 MaximumStrokeIndex = 18;

        /// <summary>
        /// The minimum hole number
        /// </summary>
        private const Int32 MinimumHoleNumber = 1;

        /// <summary>
        /// The minimum stroke index
        /// </summary>
        private const Int32 MinimumStrokeIndex = 1;

        #endregion
    }
}