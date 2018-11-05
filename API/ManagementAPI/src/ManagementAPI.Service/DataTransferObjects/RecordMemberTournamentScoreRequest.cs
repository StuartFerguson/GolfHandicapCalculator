using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.DataTransferObjects
{
    public class RecordMemberTournamentScoreRequest
    {
        /// <summary>
        /// Gets or sets the tournament identifier.
        /// </summary>
        /// <value>
        /// The tournament identifier.
        /// </value>
        public Guid TournamentId { get; set; }

        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public Guid MemberId { get; set; }

        /// <summary>
        /// Gets or sets the hole scores.
        /// </summary>
        /// <value>
        /// The hole scores.
        /// </value>
        public Dictionary<Int32,Int32> HoleScores { get; set; }
    }
}
