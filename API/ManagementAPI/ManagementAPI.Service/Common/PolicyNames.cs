using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common
{
    public class PolicyNames
    {
        /// <summary>
        /// The get club list policy
        /// </summary>
        public const String GetClubListPolicy = "GetClubListPolicy";

        /// <summary>
        /// The get single club policy
        /// </summary>
        public const String GetSingleClubPolicy = "GetSingleClubPolicy";

        /// <summary>
        /// The create club policy
        /// </summary>
        public const String CreateClubPolicy = "CreateClubPolicy";

        /// <summary>
        /// The add measured course to club policy
        /// </summary>
        public const String AddMeasuredCourseToClubPolicy = "AddMeasuredCourseToClubPolicy";

        /// <summary>
        /// The register player policy
        /// </summary>
        public const String RegisterPlayerPolicy = "RegisterPlayerPolicy";

        /// <summary>
        /// The request club membership for player policy
        /// </summary>
        public const String RequestClubMembershipForPlayerPolicy = "RequestClubMembershipForPlayerPolicy";

        /// <summary>
        /// The create tournament policy
        /// </summary>
        public const String CreateTournamentPolicy = "CreateTournamentPolicy";

        /// <summary>
        /// The record player score for tournament policy
        /// </summary>
        public const String RecordPlayerScoreForTournamentPolicy = "RecordPlayerScoreForTournamentPolicy";

        /// <summary>
        /// The complete tournament policy
        /// </summary>
        public const String CompleteTournamentPolicy = "CompleteTournamentPolicy";

        /// <summary>
        /// The cancel tournament policy
        /// </summary>
        public const String CancelTournamentPolicy = "CancelTournamentPolicy";

        /// <summary>
        /// The produce tournament result policy
        /// </summary>
        public const String ProduceTournamentResultPolicy = "ProduceTournamentResultPolicy";

        /// <summary>
        /// The get pending membership requests policy
        /// </summary>
        public const String GetPendingMembershipRequestsPolicy = "GetPendingMembershipRequestsPolicy";

        /// <summary>
        /// The approve player membership request policy
        /// </summary>
        public const String ApprovePlayerMembershipRequestPolicy = "ApprovePlayerMembershipRequestPolicy";
    }
}
