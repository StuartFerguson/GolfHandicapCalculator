using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.Common
{
    public class PolicyNames
    {
        public const String GetClubListPolicy = "GetClubListPolicy";
        public const String GetSingleClubPolicy = "GetSingleClubPolicy";
        public const String CreateClubPolicy = "CreateClubPolicy";
        public const String AddMeasuredCourseToClubPolicy = "AddMeasuredCourseToClubPolicy";
        public const String RegisterPlayerPolicy = "RegisterPlayerPolicy";
        public const String RequestClubMembershipForPlayerPolicy = "RequestClubMembershipForPlayerPolicy";
        public const String CreateTournamentPolicy = "CreateTournamentPolicy";
        public const String RecordPlayerScoreForTournamentPolicy = "RecordPlayerScoreForTournamentPolicy";
        public const String CompleteTournamentPolicy = "CompleteTournamentPolicy";
        public const String CancelTournamentPolicy = "CancelTournamentPolicy";
        public const String ProduceTournamentResultPolicy = "ProduceTournamentResultPolicy";
    }

    public class RoleNames
    {
        public const String ClubAdministrator = "CLUB ADMINISTRATOR";
        public const String MatchSecretary = "MATCH SECRETARY";
        public const String Player = "PLAYER";
    }

    public class CustomClaims
    {
        public const String ClubId = "ClubId";
        public const String PlayerId = "PlayerId";
    }
}
