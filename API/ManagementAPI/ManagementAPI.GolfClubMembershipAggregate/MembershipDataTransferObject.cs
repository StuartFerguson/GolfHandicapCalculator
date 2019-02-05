using System;

namespace ManagementAPI.GolfClubMembership
{
    public class MembershipDataTransferObject
    {
        #region Properties

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        public Guid MembershipId { get; set; }

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets the full name of the player.
        /// </summary>
        /// <value>
        /// The full name of the player.
        /// </value>
        public String PlayerFullName { get; set; }

        /// <summary>
        /// Gets the player date of birth.
        /// </summary>
        /// <value>
        /// The player date of birth.
        /// </value>
        public DateTime PlayerDateOfBirth { get; set; }

        /// <summary>
        /// Gets the player gender.
        /// </summary>
        /// <value>
        /// The player gender.
        /// </value>
        public String PlayerGender { get; set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        public String MembershipNumber { get; set; }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Int32 Status { get; set; } // NotSet/Requested/Accepted/Rejected

        /// <summary>
        /// Gets the requested date and time.
        /// </summary>
        /// <value>
        /// The requested date and time.
        /// </value>
        public DateTime RequestedDateAndTime { get; set; }

        /// <summary>
        /// Gets the accepted date and time.
        /// </summary>
        /// <value>
        /// The accepted date and time.
        /// </value>
        public DateTime AcceptedDateAndTime { get; set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        public String RejectionReason { get; set; }

        /// <summary>
        /// Gets the rejected date and time.
        /// </summary>
        /// <value>
        /// The rejected date and time.
        /// </value>
        public DateTime RejectedDateAndTime { get; set; }

        #endregion
    }
}