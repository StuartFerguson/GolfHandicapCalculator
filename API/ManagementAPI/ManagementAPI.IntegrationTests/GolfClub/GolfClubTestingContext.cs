namespace ManagementAPI.IntegrationTests.GolfClub
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Service.DataTransferObjects;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;

    public class GolfClubTestingContext
    {
        public RegisterClubAdministratorRequest RegisterClubAdministratorRequest;

        public CreateMatchSecretaryRequest CreateMatchSecretaryRequest;

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

        public List<GetGolfClubMembershipDetailsResponse> GolfClubMembersList;

        public HttpResponseMessage LastHttpResponseMessage;

        public GetMeasuredCourseListResponse GetMeasuredCourseListResponse;

        public GetGolfClubUserListResponse GetGolfClubUserListResponse;
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

        public GetPlayerDetailsResponse GetPlayerDetailsResponse;
    }

    public class TournamentTestingContext
    {
        public String ClubAdministratorToken;

        public String PlayerToken;

        public Guid GolfClubId;

        public CreateTournamentRequest CreateTournamentRequest;

        public CreateTournamentResponse CreateTournamentResponse;

        public RecordPlayerTournamentScoreRequest RecordPlayerTournamentScoreRequest;

        public GetTournamentListResponse GetTournamentListResponse;

        public RegisterPlayerResponse RegisterPlayerResponse;
    }

    public class HandicapCalculationTestingContext
    {
        public String ClubAdministratorToken;

        public String PlayerToken;

        public Guid GolfClubId;

        public CreateTournamentRequest CreateTournamentRequest;

        public CreateTournamentResponse CreateTournamentResponse;

        public RecordPlayerTournamentScoreRequest RecordPlayerTournamentScoreRequest;

        public RegisterPlayerResponse RegisterPlayerResponse;
    }
}