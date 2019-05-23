using System;

namespace ManagementAPI.GolfClubMembership
{
    internal class Membership
    {
        #region Properties

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        internal Guid MembershipId { get; private set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        internal Guid PlayerId { get; private set; }

        /// <summary>
        /// Gets the full name of the player.
        /// </summary>
        /// <value>
        /// The full name of the player.
        /// </value>
        internal String PlayerFullName { get; private set; }

        /// <summary>
        /// Gets the player date of birth.
        /// </summary>
        /// <value>
        /// The player date of birth.
        /// </value>
        internal DateTime PlayerDateOfBirth { get; private set; }

        /// <summary>
        /// Gets the player gender.
        /// </summary>
        /// <value>
        /// The player gender.
        /// </value>
        internal String PlayerGender { get; private set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        internal String MembershipNumber { get; private set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        internal Int32 Status { get; private set; } // NotSet/Requested/Accepted/Rejected
        
        /// <summary>
        /// Gets the accepted date and time.
        /// </summary>
        /// <value>
        /// The accepted date and time.
        /// </value>
        internal DateTime AcceptedDateAndTime { get; private set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        internal String RejectionReason { get; private set; }

        /// <summary>
        /// Gets the rejected date and time.
        /// </summary>
        /// <value>
        /// The rejected date and time.
        /// </value>
        internal DateTime RejectedDateAndTime { get; private set; }

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="Membership" /> class.
        /// </summary>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        internal Membership(Guid membershipId, Guid playerId, String playerFullName, String playerGender, DateTime playerDateOfBirth)
        {
            this.MembershipId = membershipId;
            this.PlayerId = playerId;
            this.PlayerFullName = playerFullName;
            this.PlayerGender = playerGender;
            this.PlayerDateOfBirth = playerDateOfBirth;
            this.Status = 0;
        }
        #endregion

        #region Internal Methods

        #region internal static Membership Create(Guid membershipId, Guid playerId, String playerFullName, String playerGender, DateTime playerDateOfBirth)        
        /// <summary>
        /// Creates the specified player identifier.
        /// </summary>
        /// <param name="membershipId">The membership identifier.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="playerFullName">Full name of the player.</param>
        /// <param name="playerGender">The player gender.</param>
        /// <param name="playerDateOfBirth">The player date of birth.</param>
        /// <returns></returns>
        internal static Membership Create(Guid membershipId, Guid playerId,String playerFullName, String playerGender, DateTime playerDateOfBirth)
        {
            return new Membership(membershipId, playerId,playerFullName, playerGender, playerDateOfBirth);
        }
        #endregion
        
        #region internal void Accepted(String membershipNumber, DateTime acceptedDateTime)        
        /// <summary>
        /// Accepteds the specified membership number.
        /// </summary>
        /// <param name="membershipNumber">The membership number.</param>
        /// <param name="acceptedDateTime">The accepted date time.</param>
        internal void Accepted(String membershipNumber, DateTime acceptedDateTime)
        {
            this.AcceptedDateAndTime = acceptedDateTime;
            this.Status = 1;
            this.MembershipNumber = membershipNumber;
        }
        #endregion

        #region internal void Rejected(String rejectionReason, DateTime rejectionDateTime)        
        /// <summary>
        /// Rejecteds the specified rejection reason.
        /// </summary>
        /// <param name="rejectionReason">The rejection reason.</param>
        /// <param name="rejectionDateTime">The rejection date time.</param>
        internal void Rejected(String rejectionReason, DateTime rejectionDateTime)
        {
            this.RejectedDateAndTime = rejectionDateTime;
            this.Status = 2;
            this.RejectionReason = rejectionReason;
        }
        #endregion

        #endregion
    }
}