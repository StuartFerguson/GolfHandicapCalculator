namespace ManagementAPI.IntegrationTests.GolfClub
{
    using System;
    using System.Collections.Generic;
    using Service.DataTransferObjects;

    public class GolfClubTestingContext
    {
        public RegisterClubAdministratorRequest RegisterClubAdministratorRequest;

        public CreateGolfClubRequest CreateGolfClubRequest;

        public String ClubAdministratorToken;

        public CreateGolfClubResponse CreateGolfClubResponse;

        public Guid GolfClubId;

        public String PlayerToken;
        
        public GetGolfClubResponse GetGolfClubResponse;

        public List<GetGolfClubResponse> GetGolfClubListResponse;

        public AddMeasuredCourseToClubRequest AddMeasuredCourseToClubRequest;

        public RegisterPlayerResponse RegisterPlayerResponse;

        public Dictionary<Int32, RegisteredPlayer> RegisteredPlayers;

        public List<GolfClubMembershipDetails> GolfClubMembersList;
    }

    public class RegisteredPlayer
    {
        public RegisterPlayerRequest Request { get; set; }

        public RegisterPlayerResponse Response { get; set; }
    }

    public class PlayerTestingContext
    {
        public RegisterPlayerRequest RegisterPlayerRequest;

        public RegisterPlayerResponse RegisterPlayerResponse;

        public String ClubAdministratorToken;

        public String PlayerToken;

        public Guid GolfClubId;

        public List<ClubMembershipResponse> ClubMembershipResponses;
    }

    public class TournamentTestingContext
    {
        public String ClubAdministratorToken;

        public String PlayerToken;

        public Guid GolfClubId;

        public CreateTournamentRequest CreateTournamentRequest;

        public CreateTournamentResponse CreateTournamentResponse;

        public RecordMemberTournamentScoreRequest RecordMemberTournamentScoreRequest;
    }
}