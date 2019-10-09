namespace ManagementAPI.Service.IntegrationTests.ClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client.v2;
    using Common;
    using DataTransferObjects.Responses.v2;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class PlayerClientTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public PlayerClientTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task PlayerClient_RequestClubMembership_MembershipRequested()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IPlayerClient playerClient = new PlayerClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            RequestClubMembershipResponse requestClubMembershipResponse =
                await playerClient.RequestClubMembership(token, TestData.PlayerId, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            requestClubMembershipResponse.PlayerId.ShouldBe(TestData.PlayerId);
            requestClubMembershipResponse.GolfClubId.ShouldBe(TestData.GolfClubId);
            requestClubMembershipResponse.MembershipId.ShouldBe(Guid.Empty);
        }

        [Fact]
        public async Task PlayerClient_RegisterPlayer_PlayerCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IPlayerClient playerClient = new PlayerClient(resolver, client);
            
            // 2. Act
            RegisterPlayerResponse registerPlayerResponse =
                await playerClient.RegisterPlayer(TestData.RegisterPlayerRequest, CancellationToken.None);

            // 3. Assert
            registerPlayerResponse.PlayerId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task PlayerClient_GetPlayer_PlayerCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IPlayerClient playerClient = new PlayerClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetPlayerResponse getPlayerResponse =
                await playerClient.GetPlayer(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            getPlayerResponse.Id.ShouldBe(TestData.PlayerId);
        }

        [Fact]
        public async Task PlayerClient_GetPlayerMemberships_PlayerCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IPlayerClient playerClient = new PlayerClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<ClubMembershipResponse> clubMembershipResponseList =
                await playerClient.GetPlayerMemberships(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            clubMembershipResponseList.ShouldNotBeNull();
            clubMembershipResponseList.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task PlayerClient_GetTournamentsSignedUpFor_PlayerCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IPlayerClient playerClient = new PlayerClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<SignedUpTournamentResponse> signedUpTournamentResponseList =
                await playerClient.GetTournamentsSignedUpFor(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            signedUpTournamentResponseList.ShouldNotBeNull();
            signedUpTournamentResponseList.ShouldNotBeEmpty();
        }
    }
}