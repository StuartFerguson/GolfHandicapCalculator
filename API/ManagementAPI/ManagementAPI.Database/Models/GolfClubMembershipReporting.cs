using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.Database.Models
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class GolfClubMembershipReporting
    {
        /// <summary>
        /// Gets or sets the golf club identifier.
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
        /// Gets or sets the player identifier.
        /// </summary>
        /// <value>
        /// The player identifier.
        /// </value>
        public Guid PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public String PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the player gender.
        /// </summary>
        /// <value>
        /// The player gender.
        /// </value>
        public String PlayerGender { get; set; }

        /// <summary>
        /// Gets or sets the handicap category.
        /// </summary>
        /// <value>
        /// The handicap category.
        /// </value>
        public Int32 HandicapCategory { get; set; }

        /// <summary>
        /// Gets or sets the date of birth.
        /// </summary>
        /// <value>
        /// The date of birth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the date joined.
        /// </summary>
        /// <value>
        /// The date joined.
        /// </value>
        public DateTime DateJoined { get; set; }
    }
}
