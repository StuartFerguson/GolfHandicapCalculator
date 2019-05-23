namespace ManagementAPI.GolfClubMembership
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.EventStore.Aggregate" />
    public class GolfClubMembershipAggregate : Aggregate
    {
        #region Fields

        /// <summary>
        /// The membership list
        /// </summary>
        private readonly List<Membership> MembershipList;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubMembershipAggregate" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public GolfClubMembershipAggregate()
        {
            // Nothing here
            this.MembershipList = new List<Membership>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubMembershipAggregate" /> class.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        private GolfClubMembershipAggregate(Guid aggregateId)
        {
            Guard.ThrowIfInvalidGuid(aggregateId, "Aggregate Id cannot be an Empty Guid");

            this.AggregateId = aggregateId;
            this.MembershipList = new List<Membership>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the specified aggregate identifier.
        /// </summary>
        /// <param name="aggregateId">The aggregate identifier.</param>
        /// <returns></returns>
        public static GolfClubMembershipAggregate Create(Guid aggregateId)
        {
            return new GolfClubMembershipAggregate(aggregateId);
        }

        /// <summary>
        /// Gets the membership.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Membership with Player Id {playerId} Date of Birth {dateOfBirth.Date:dd/MM/yyy} and Gender {gender}</exception>
        public MembershipDataTransferObject GetMembership(Guid playerId,
                                                          DateTime dateOfBirth,
                                                          String gender)
        {
            Membership membership = this.MembershipList.SingleOrDefault(m => m.PlayerId == playerId && m.PlayerDateOfBirth == dateOfBirth &&
                                                                             string.Compare(m.PlayerGender, gender, StringComparison.InvariantCultureIgnoreCase) == 0);

            if (membership == null)
            {
                throw new NotFoundException($"Membership with Player Id {playerId} Date of Birth {dateOfBirth.Date:dd/MM/yyy} and Gender {gender} not found.");
            }

            MembershipDataTransferObject result = this.CreateFrom(membership);

            return result;
        }

        /// <summary>
        /// Gets the memberships.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotFoundException">No Memberships found for Golf CLub Id {this.AggregateId}</exception>
        public List<MembershipDataTransferObject> GetMemberships()
        {
            List<MembershipDataTransferObject> result = new List<MembershipDataTransferObject>();

            if (this.MembershipList.Count == 0)
            {
                throw new NotFoundException($"No Memberships found for Golf CLub Id {this.AggregateId}.");
            }

            foreach (Membership membership in this.MembershipList)
            {
                result.Add(this.CreateFrom(membership));
            }

            return result;
        }

        /// <summary>
        /// Requests the membership.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="requestDateAndTime">The request date and time.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Players gender can only be 'M' (Male) or 'F' (Female)</exception>
        public void RequestMembership(Guid playerId,
                                      String playerFullName,
                                      DateTime playerDateOfBirth,
                                      String playerGender,
                                      DateTime requestDateAndTime)
        {
            Guid membershipId = Guid.NewGuid();

            Guard.ThrowIfInvalidGuid(playerId, typeof(ArgumentNullException), "A membership request must have valid player id");
            Guard.ThrowIfNullOrEmpty(playerFullName, typeof(ArgumentNullException), "A membership request must have player full name supplied");
            Guard.ThrowIfInvalidDate(playerDateOfBirth, typeof(ArgumentNullException), "A membership request must have player name supplied");
            Guard.ThrowIfNullOrEmpty(playerGender, typeof(ArgumentNullException), "A membership request must have player gender supplied");

            if (playerGender != "M" && playerGender != "F")
            {
                throw new ArgumentOutOfRangeException(playerGender, "Players gender can only be 'M' (Male) or 'F' (Female)");
            }

            this.ValidateAgainstDuplicateMembershipRequest(playerId, playerDateOfBirth, playerGender);

            try
            {
                this.ValidatePlayerDateOfBirth(membershipId, playerId, playerDateOfBirth);

                this.ValidateMembershipCount(playerId);

                // Get the membership number
                String membershipNumber = this.GenerateMembershipNumber();

                ClubMembershipRequestAcceptedEvent clubMembershipRequestAcceptedEvent =
                    ClubMembershipRequestAcceptedEvent.Create(this.AggregateId,
                                                              membershipId,
                                                              playerId,
                                                              playerFullName,
                                                              playerDateOfBirth,
                                                              playerGender,
                                                              requestDateAndTime,
                                                              membershipNumber);

                this.ApplyAndPend(clubMembershipRequestAcceptedEvent);
            }
            catch(InvalidOperationException e)
            {
                ClubMembershipRequestRejectedEvent clubMembershipRequestRejectedEvent =
                    ClubMembershipRequestRejectedEvent.Create(this.AggregateId,
                                                              membershipId,
                                                              playerId,
                                                              playerFullName,
                                                              playerDateOfBirth,
                                                              playerGender,
                                                              e.Message,
                                                              requestDateAndTime);

                this.ApplyAndPend(clubMembershipRequestRejectedEvent);
            }
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        protected override void PlayEvent(DomainEvent domainEvent)
        {
            this.PlayEvent((dynamic)domainEvent);
        }

        /// <summary>
        /// Calculates the age.
        /// </summary>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <returns></returns>
        private AgeResult CalculateAge(DateTime dateOfBirth)
        {
            AgeResult result = new AgeResult();

            // get current date.
            DateTime adtCurrentDate = DateTime.Now;

            // find the literal difference
            result.Days = adtCurrentDate.Day - dateOfBirth.Day;
            result.Months = adtCurrentDate.Month - dateOfBirth.Month;
            result.Years = adtCurrentDate.Year - dateOfBirth.Year;

            if (result.Days < 0)
            {
                result.Days += DateTime.DaysInMonth(adtCurrentDate.Year, adtCurrentDate.Month);
                result.Months--;
            }

            if (result.Months < 0)
            {
                result.Months += 12;
                result.Years--;
            }

            return result;
        }

        /// <summary>
        /// Creates from.
        /// </summary>
        /// <param name="membership">The membership.</param>
        /// <returns></returns>
        private MembershipDataTransferObject CreateFrom(Membership membership)
        {
            return new MembershipDataTransferObject
                   {
                       PlayerId = membership.PlayerId,
                       MembershipId = membership.MembershipId,
                       PlayerGender = membership.PlayerGender,
                       PlayerFullName = membership.PlayerFullName,
                       PlayerDateOfBirth = membership.PlayerDateOfBirth,
                       RejectionReason = membership.RejectionReason,
                       AcceptedDateAndTime = membership.AcceptedDateAndTime,
                       RejectedDateAndTime = membership.RejectedDateAndTime,
                       Status = membership.Status,
                       MembershipNumber = membership.MembershipNumber
                   };
        }

        /// <summary>
        /// Generates the membership number.
        /// </summary>
        /// <returns></returns>
        private String GenerateMembershipNumber()
        {
            // Get the count of current list (add 1 as this list is base 0)
            Int32 memberCount = this.MembershipList.Count() + 1;

            // Increment count by 1 for the new member
            memberCount++;

            return $"{memberCount:000000}";
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(ClubMembershipRequestAcceptedEvent domainEvent)
        {
            Membership membership = Membership.Create(domainEvent.MembershipId,
                                                      domainEvent.PlayerId,
                                                      domainEvent.PlayerFullName,
                                                      domainEvent.PlayerGender,
                                                      domainEvent.PlayerDateOfBirth);
            membership.Accepted(domainEvent.MembershipNumber, domainEvent.AcceptedDateAndTime);
            this.MembershipList.Add(membership);
        }

        /// <summary>
        /// Plays the event.
        /// </summary>
        /// <param name="domainEvent">The domain event.</param>
        private void PlayEvent(ClubMembershipRequestRejectedEvent domainEvent)
        {
            Membership membership = Membership.Create(domainEvent.MembershipId,
                                                      domainEvent.PlayerId,
                                                      domainEvent.PlayerFullName,
                                                      domainEvent.PlayerGender,
                                                      domainEvent.PlayerDateOfBirth);
            membership.Rejected(domainEvent.RejectionReason, domainEvent.RejectionDateAndTime);
            this.MembershipList.Add(membership);
        }

        /// <summary>
        /// Validates the against duplicate membership request.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <param name="gender">The gender.</param>
        /// <exception cref="System.InvalidOperationException">Membership with Player Id {playerId} Date of Birth {dateOfBirth.Date:dd/MM/yyy} and Gender {gender}</exception>
        /// <exception cref="InvalidOperationException">Membership with Id {membership}</exception>
        private void ValidateAgainstDuplicateMembershipRequest(Guid playerId,
                                                               DateTime dateOfBirth,
                                                               String gender)
        {
            Boolean membershipExists = this.MembershipList.Any(m => m.PlayerId == playerId && m.PlayerDateOfBirth == dateOfBirth &&
                                                                    string.Compare(m.PlayerGender, gender, StringComparison.InvariantCultureIgnoreCase) == 0);

            if (membershipExists)
            {
                throw new
                    InvalidOperationException($"Membership with Player Id {playerId} Date of Birth {dateOfBirth.Date:dd/MM/yyy} and Gender {gender} already requested");
            }
        }

        /// <summary>
        /// Validates the membership count.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <exception cref="System.InvalidOperationException">No more space at club for Player Id {playerId}</exception>
        /// <exception cref="InvalidOperationException">No more space at club for Player Id {playerId}</exception>
        private void ValidateMembershipCount(Guid playerId)
        {
            Int32 acceptedMemberCount = this.MembershipList.Count(m => m.Status == 1);

            if (acceptedMemberCount == GolfClubMembershipAggregate.MaximumNumberOfMembers)
            {
                throw new InvalidOperationException($"No more space at club for Player Id {playerId}");
            }
        }

        /// <summary>
        /// Validates the player date of birth.
        /// </summary>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="dateOfBirth">The date of birth.</param>
        /// <exception cref="System.InvalidOperationException">
        /// Player Id {playerId}
        /// or
        /// Player Id {playerId}
        /// </exception>
        /// <exception cref="InvalidOperationException">Player Id {playerId}
        /// or
        /// Player Id {playerId}</exception>
        private void ValidatePlayerDateOfBirth(Guid membershipId,
                                               Guid playerId,
                                               DateTime dateOfBirth)
        {
            // Get the players age 
            AgeResult ageResult = this.CalculateAge(dateOfBirth);

            if (ageResult.Years < 13)
            {
                throw new InvalidOperationException($"Player Id {playerId} is too young.");
            }

            if (ageResult.Years >= 90)
            {
                throw new InvalidOperationException($"Player Id {playerId} is too old.");
            }
        }

        #endregion

        #region Others

        /// <summary>
        /// The maximum number of members
        /// </summary>
        // TODO: This will be set at the club level
        private const Int32 MaximumNumberOfMembers = 500;

        #endregion
    }
}