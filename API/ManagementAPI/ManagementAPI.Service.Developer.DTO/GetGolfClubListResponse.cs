using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Service.Developer.DataTransferObjects
{
    public class GetGolfClubListResponse
    {
        /// <summary>
        /// Gets or sets the club configuration identifier.
        /// </summary>
        /// <value>
        /// The club configuration identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region { get; set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; set; }

        /// <summary>
        /// Gets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; set; }

        /// <summary>
        /// Gets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public String Website { get; set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }
    }

    public class GetGolfClubResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetGolfClubResponse"/> class.
        /// </summary>
        public GetGolfClubResponse()
        {
            this.GolfClubMemberships = new List<GolfClubMembershipResponse>();
        }

        /// <summary>
        /// Gets or sets the aggregate identifier.
        /// </summary>
        /// <value>
        /// The aggregate identifier.
        /// </value>
        public Guid AggregateId { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets the address line1.
        /// </summary>
        /// <value>
        /// The address line1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets the address line2.
        /// </summary>
        /// <value>
        /// The address line2.
        /// </value>
        public String AddressLine2 { get; set; }

        /// <summary>
        /// Gets the town.
        /// </summary>
        /// <value>
        /// The town.
        /// </value>
        public String Town { get; set; }

        /// <summary>
        /// Gets the region.
        /// </summary>
        /// <value>
        /// The region.
        /// </value>
        public String Region { get; set; }

        /// <summary>
        /// Gets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public String PostalCode { get; set; }

        /// <summary>
        /// Gets the telephone number.
        /// </summary>
        /// <value>
        /// The telephone number.
        /// </value>
        public String TelephoneNumber { get; set; }

        /// <summary>
        /// Gets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public String Website { get; set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; set; }

        /// <summary>
        /// Gets or sets the golf club memberships.
        /// </summary>
        /// <value>
        /// The golf club memberships.
        /// </value>
        public List<GolfClubMembershipResponse> GolfClubMemberships { get; set; }
    }

    public class GolfClubMembershipResponse
    {
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
    }

    public class GetPlayerResponse
    {
        public GetPlayerResponse()
        {
            this.ClubMemberships = new List<PlayerClubMembership>();
        }
        #region Properties

        public List<PlayerClubMembership> ClubMemberships { get; set; }

        /// <summary>
        /// Gets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public String EmailAddress { get; set; }

        /// <summary>
        /// Gets the exact handicap.
        /// </summary>
        /// <value>
        /// The exact handicap.
        /// </value>
        public Decimal ExactHandicap { get; set; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public String FirstName { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public String FullName { get; set; }

        /// <summary>
        /// Gets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public String Gender { get; set; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been registered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been registered; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenRegistered { get; set; }
        
        /// <summary>
        /// Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public String LastName { get; set; }

        /// <summary>
        /// Gets the name of the middle.
        /// </summary>
        /// <value>
        /// The name of the middle.
        /// </value>
        public String MiddleName { get; set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; set; }

        /// <summary>
        /// Gets the security user identifier.
        /// </summary>
        /// <value>
        /// The security user identifier.
        /// </value>
        public Guid SecurityUserId { get; set; }

        #endregion
    }

    public class PlayerClubMembership
    {
        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets or sets the name of the golf club.
        /// </summary>
        /// <value>
        /// The name of the golf club.
        /// </value>
        public String GolfClubName { get; set; }

        /// <summary>
        /// Gets the membership identifier.
        /// </summary>
        /// <value>
        /// The membership identifier.
        /// </value>
        public Guid MembershipId { get; set; }

        /// <summary>
        /// Gets the membership number.
        /// </summary>
        /// <value>
        /// The membership number.
        /// </value>
        public String MembershipNumber { get; set; }

        /// <summary>
        /// Gets the accepted date time.
        /// </summary>
        /// <value>
        /// The accepted date time.
        /// </value>
        public DateTime? AcceptedDateTime { get; set; }

        /// <summary>
        /// Gets the membership status.
        /// </summary>
        /// <value>
        /// The membership status.
        /// </value>
        public MembershipStatus Status { get; set; }

        /// <summary>
        /// Gets the rejection reason.
        /// </summary>
        /// <value>
        /// The rejection reason.
        /// </value>
        public String RejectionReason { get; set; }

        /// <summary>
        /// Gets the rejected date time.
        /// </summary>
        /// <value>
        /// The rejected date time.
        /// </value>
        public DateTime? RejectedDateTime { get; set; }
    }

    public enum MembershipStatus
    {
        /// <summary>
        /// The accepted
        /// </summary>
        Accepted,

        /// <summary>
        /// The rejected
        /// </summary>
        Rejected
    }

    public class GetTournamentResponse
    {
        public GetTournamentResponse()
        {
            this.Scores = new List<MemberScoreRecord>();
        }

        public List<MemberScoreRecord> Scores { get; set; }

        /// <summary>
        /// Gets the adjustment.
        /// </summary>
        /// <value>
        /// The adjustment.
        /// </value>
        public Int32 Adjustment { get; set; }

        /// <summary>
        /// Gets the cancelled date time.
        /// </summary>
        /// <value>
        /// The cancelled date time.
        /// </value>
        public DateTime CancelledDateTime { get; set; }

        /// <summary>
        /// Gets the cancelled reason.
        /// </summary>
        /// <value>
        /// The cancelled reason.
        /// </value>
        public String CancelledReason { get; set; }

        /// <summary>
        /// Gets the completed date time.
        /// </summary>
        /// <value>
        /// The completed date time.
        /// </value>
        public DateTime CompletedDateTime { get; set; }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <value>
        /// The CSS.
        /// </value>
        public Int32 CSS { get; set; }

        /// <summary>
        /// Gets a value indicating whether [CSS has been calculated].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [CSS has been calculated]; otherwise, <c>false</c>.
        /// </value>
        public Boolean CSSHasBeenCalculated { get; set; }

        /// <summary>
        /// Gets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public TournamentFormat Format { get; set; }

        /// <summary>
        /// Gets the golf club identifier.
        /// </summary>
        /// <value>
        /// The golf club identifier.
        /// </value>
        public Guid GolfClubId { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been cancelled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been cancelled; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCancelled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been completed; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCompleted { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has been created.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has been created; otherwise, <c>false</c>.
        /// </value>
        public Boolean HasBeenCreated { get; set; }

        /// <summary>
        /// Gets the measured course identifier.
        /// </summary>
        /// <value>
        /// The measured course identifier.
        /// </value>
        public Guid MeasuredCourseId { get; set; }

        /// <summary>
        /// Gets the measured course SSS.
        /// </summary>
        /// <value>
        /// The measured course SSS.
        /// </value>
        public Int32 MeasuredCourseSSS { get; set; }

        /// <summary>
        /// Gets the member category.
        /// </summary>
        /// <value>
        /// The member category.
        /// </value>
        public MemberCategory MemberCategory { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public String Name { get; set; }

        /// <summary>
        /// Gets the tournament date.
        /// </summary>
        /// <value>
        /// The tournament date.
        /// </value>
        public DateTime TournamentDate { get; set; }
    }

    public enum MemberCategory
    {
        Gents = 1,
        GentsSenior,
        Female,
        FemaleSenion,
        Junior
    }

    public enum TournamentFormat
    {
        Strokeplay = 1
    }

    public class MemberScoreRecord
    {
        public MemberScoreRecord()
        {
            this.HoleScores = new Dictionary<Int32, Int32>();
        }

        /// <summary>
        /// Gets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public Guid MemberId { get; set; }

        /// <summary>
        /// Gets the playing handicap.
        /// </summary>
        /// <value>
        /// The playing handicap.
        /// </value>
        public Int32 PlayingHandicap { get; set; }

        /// <summary>
        /// Gets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; set; }

        /// <summary>
        /// Gets the gross score.
        /// </summary>
        /// <value>
        /// The gross score.
        /// </value>
        public Int32 GrossScore { get; set; }

        /// <summary>
        /// Gets the net score.
        /// </summary>
        /// <value>
        /// The net score.
        /// </value>
        public Int32 NetScore { get; set; }

        /// <summary>
        /// Gets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        public Dictionary<Int32, Int32> HoleScores { get; set; }
    }
}
