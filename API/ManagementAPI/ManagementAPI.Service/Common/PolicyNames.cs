using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common
{
    public class PolicyNames
    {
        /// <summary>
        /// The get golf club list policy
        /// </summary>
        public const String GetGolfClubListPolicy = "GetGolfClubListPolicy";

        /// <summary>
        /// The get single golf club policy
        /// </summary>
        public const String GetSingleGolfClubPolicy = "GetSingleGolfClubPolicy";

        /// <summary>
        /// The create golf club policy
        /// </summary>
        public const String CreateGolfClubPolicy = "CreateGolfClubPolicy";

        /// <summary>
        /// The add measured course to golf club policy
        /// </summary>
        public const String AddMeasuredCourseToGolfClubPolicy = "AddMeasuredCourseToGolfClubPolicy";

        /// <summary>
        /// The request club membership policy
        /// </summary>
        public const String RequestClubMembershipPolicy = "RequestClubMembershipPolicy";

        /// <summary>
        /// The register player policy
        /// </summary>
        public const String RegisterPlayerPolicy = "RegisterPlayerPolicy";

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
    }
}
