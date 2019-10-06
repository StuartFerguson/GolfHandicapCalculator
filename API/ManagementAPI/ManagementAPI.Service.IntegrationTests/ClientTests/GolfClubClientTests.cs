namespace ManagementAPI.Service.IntegrationTests.ClientAndControllerTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client.v2;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses.v2;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class GolfClubClientTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public GolfClubClientTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task GolfClubClient_CreateGolfClub_GolfClubIsCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            CreateGolfClubRequest createGolfClubRequest = TestData.CreateGolfClubRequest;

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            CreateGolfClubResponse createGolfClubResponse = await golfClubClient.CreateGolfClub(token, createGolfClubRequest, CancellationToken.None);

            // 3. Assert
            createGolfClubResponse.GolfClubId.ShouldBe(TestData.GolfClubId);
        }

        [Fact]
        public async Task GolfClubClient_AddMeasuredCourseToGolfClub_MeasuredCourseIsCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest = TestData.AddMeasuredCourseToClubRequest;

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            AddMeasuredCourseToClubResponse addMeasuredCourseToClubResponse = await golfClubClient.AddMeasuredCourseToGolfClub(token, TestData.GolfClubId, addMeasuredCourseToClubRequest, CancellationToken.None);

            // 3. Assert
            addMeasuredCourseToClubResponse.GolfClubId.ShouldBe(TestData.GolfClubId);
            addMeasuredCourseToClubResponse.MeasuredCourseId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task GolfClubClient_AddTournamentDivision_TournamentDivisionIsCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);
            AddTournamentDivisionToGolfClubRequest addTournamentDivisionToGolfClubRequest = TestData.AddTournamentDivisionToGolfClubRequest;

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            AddTournamentDivisionToGolfClubResponse addTournamentDivisionToGolfClubResponse = await golfClubClient.AddTournamentDivision(token, TestData.GolfClubId, addTournamentDivisionToGolfClubRequest, CancellationToken.None);

            // 3. Assert
            addTournamentDivisionToGolfClubResponse.GolfClubId.ShouldBe(TestData.GolfClubId);
            addTournamentDivisionToGolfClubResponse.TournamentDivision.ShouldBe(addTournamentDivisionToGolfClubRequest.Division);
        }

        [Fact]
        public async Task GolfClubClient_CreateMatchSecretary_MatchSecretaryIsCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);
            CreateMatchSecretaryRequest createMatchSecretaryRequest = TestData.CreateMatchSecretaryRequest;

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            CreateMatchSecretaryResponse createMatchSecretaryResponse = await golfClubClient.CreateMatchSecretary(token, TestData.GolfClubId, createMatchSecretaryRequest, CancellationToken.None);

            // 3. Assert
            createMatchSecretaryResponse.GolfClubId.ShouldBe(TestData.GolfClubId);
            createMatchSecretaryResponse.UserName.ShouldBe(createMatchSecretaryRequest.EmailAddress);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClub_GolfClubListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);
            
            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetGolfClubResponse getGolfClubResponse = await golfClubClient.GetGolfClub(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getGolfClubResponse.Id.ShouldBe(TestData.GolfClubId);
            getGolfClubResponse.Users.ShouldBeNull();
            getGolfClubResponse.MeasuredCourses.ShouldBeNull();
            getGolfClubResponse.GolfClubMembershipDetailsResponseList.ShouldBeNull();
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubMembershipList_GolfClubMembershipListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<GolfClubMembershipDetailsResponse> getClubMembershipDetailsResponses = await golfClubClient.GetGolfClubMembershipList(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getClubMembershipDetailsResponses.ShouldNotBeNull();
            getClubMembershipDetailsResponses.ShouldNotBeEmpty();
            getClubMembershipDetailsResponses.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GolfClubClient_GetMeasuredCourses_MeasuredCoursesListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<MeasuredCourseListResponse> getMeasuredCourses = await golfClubClient.GetMeasuredCourses(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getMeasuredCourses.ShouldNotBeNull();
            getMeasuredCourses.ShouldNotBeEmpty();
            getMeasuredCourses.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubUserList_GolfClubUserListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<GolfClubUserResponse> golfClubUserList = await golfClubClient.GetGolfClubUserList(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            golfClubUserList.ShouldNotBeNull();
            golfClubUserList.ShouldNotBeEmpty();
            golfClubUserList.Count.ShouldBe(1);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubList_GolfClubListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<GetGolfClubResponse> getGolfClubListResponse = await golfClubClient.GetGolfClubList(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getGolfClubListResponse.Count.ShouldBe(1);
            getGolfClubListResponse.First().Id.ShouldBe(TestData.GolfClubId);
            getGolfClubListResponse.First().Users.ShouldBeNull();
            getGolfClubListResponse.First().MeasuredCourses.ShouldBeNull();
            getGolfClubListResponse.First().GolfClubMembershipDetailsResponseList.ShouldBeNull();
        }

        [Fact(Skip = "Back end code not implemented yet")]
        public async Task GolfClubClient_GetGolfClubList_IncludeMembers_GolfClubListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<GetGolfClubResponse> getGolfClubListResponse = await golfClubClient.GetGolfClubList(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getGolfClubListResponse.Count.ShouldBe(1);
            getGolfClubListResponse.First().Id.ShouldBe(TestData.GolfClubId);
            getGolfClubListResponse.First().Users.ShouldBeNull();
            getGolfClubListResponse.First().MeasuredCourses.ShouldBeNull();
            getGolfClubListResponse.First().GolfClubMembershipDetailsResponseList.ShouldNotBeNull();
        }

        [Fact(Skip = "Back end code not implemented yet")]
        public async Task GolfClubClient_GetGolfClubList_IncludeMeasuredCourses_GolfClubListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<GetGolfClubResponse> getGolfClubListResponse = await golfClubClient.GetGolfClubList(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getGolfClubListResponse.Count.ShouldBe(1);
            getGolfClubListResponse.First().Id.ShouldBe(TestData.GolfClubId);
            getGolfClubListResponse.First().Users.ShouldBeNull();
            getGolfClubListResponse.First().MeasuredCourses.ShouldNotBeNull();
            getGolfClubListResponse.First().GolfClubMembershipDetailsResponseList.ShouldBeNull();
        }

        [Fact(Skip="Back end code not implemented yet")]
        public async Task GolfClubClient_GetGolfClubList_IncludeUsers_GolfClubListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubClient golfClubClient = new GolfClubClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            List<GetGolfClubResponse> getGolfClubListResponse = await golfClubClient.GetGolfClubList(token, TestData.GolfClubId, CancellationToken.None);

            // 3. Assert
            getGolfClubListResponse.Count.ShouldBe(1);
            getGolfClubListResponse.First().Id.ShouldBe(TestData.GolfClubId);
            getGolfClubListResponse.First().Users.ShouldNotBeNull();
            getGolfClubListResponse.First().MeasuredCourses.ShouldBeNull();
            getGolfClubListResponse.First().GolfClubMembershipDetailsResponseList.ShouldBeNull();
        }

    }
}