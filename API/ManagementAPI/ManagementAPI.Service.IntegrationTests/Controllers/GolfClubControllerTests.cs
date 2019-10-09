namespace ManagementAPI.Service.IntegrationTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using DataTransferObjects.Responses.v2;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using Xunit;
    using CreateGolfClubResponse = DataTransferObjects.Responses.v2.CreateGolfClubResponse;
    using GetGolfClubResponse = DataTransferObjects.Responses.v2.GetGolfClubResponse;

    /// <summary>
    /// 
    /// </summary>
    [Collection("TestCollection")]
    public class GolfClubControllerTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public GolfClubControllerTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task GolfClubController_POST_CreateGolfClub_GolfClubIdIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            CreateGolfClubRequest createGolfClubRequest = TestData.CreateGolfClubRequest;
            String uri = "api/golfclubs/";
            StringContent content = Helpers.CreateStringContent(createGolfClubRequest);
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            CreateGolfClubResponse responseObject = JsonConvert.DeserializeObject<CreateGolfClubResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
        }

        [Fact]
        public async Task GolfClubController_GET_GetGolfClub_OnlyGolfClub_GolfClubIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            String uri = $"api/golfclubs/{TestData.GolfClubId.ToString()}";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetGolfClubResponse responseObject = JsonConvert.DeserializeObject<GetGolfClubResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.GolfClubId);
            responseObject.GolfClubMembershipDetailsResponseList.ShouldBeNull();
            responseObject.MeasuredCourses.ShouldBeNull();
            responseObject.Users.ShouldBeNull();
        }

        [Fact]
        public async Task GolfClubController_GET_GetGolfClub_IncludeMembers_GolfClubIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            CreateGolfClubRequest createGolfClubRequest = TestData.CreateGolfClubRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId.ToString()}?includeMembers=true";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetGolfClubResponse responseObject = JsonConvert.DeserializeObject<GetGolfClubResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.GolfClubId);
            responseObject.GolfClubMembershipDetailsResponseList.ShouldNotBeNull();
            responseObject.MeasuredCourses.ShouldBeNull();
            responseObject.Users.ShouldBeNull();
        }

        [Fact]
        public async Task GolfClubController_GET_GetGolfClub_IncludeMeasuredCourses_GolfClubIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            CreateGolfClubRequest createGolfClubRequest = TestData.CreateGolfClubRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId.ToString()}?includeMeasuredCourses=true";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetGolfClubResponse responseObject = JsonConvert.DeserializeObject<GetGolfClubResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.GolfClubId);
            responseObject.GolfClubMembershipDetailsResponseList.ShouldBeNull();
            responseObject.MeasuredCourses.ShouldNotBeNull();
            responseObject.Users.ShouldBeNull();
        }

        [Fact]
        public async Task GolfClubController_GET_GetGolfClub_IncludeUsers_GolfClubIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            CreateGolfClubRequest createGolfClubRequest = TestData.CreateGolfClubRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId.ToString()}?includeUsers=true";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetGolfClubResponse responseObject = JsonConvert.DeserializeObject<GetGolfClubResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.GolfClubId);
            responseObject.GolfClubMembershipDetailsResponseList.ShouldBeNull();
            responseObject.MeasuredCourses.ShouldBeNull();
            responseObject.Users.ShouldNotBeNull();
        }

        [Fact]
        public async Task GolfClubController_GET_GetGolfClubList_GolfClubIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            CreateGolfClubRequest createGolfClubRequest = TestData.CreateGolfClubRequest;
            String uri = $"api/golfclubs";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            List<GetGolfClubResponse> responseObject = JsonConvert.DeserializeObject<List<GetGolfClubResponse>>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GolfClubController_POST_AddMeasuredCourseToGolfClub_MeasuredCourseIdIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest = TestData.AddMeasuredCourseToClubRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId}/measuredcourses";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = Helpers.CreateStringContent(addMeasuredCourseToClubRequest);

            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            AddMeasuredCourseToClubResponse responseObject = JsonConvert.DeserializeObject<AddMeasuredCourseToClubResponse>(responseAsJson);
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.MeasuredCourseId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task GolfClubController_POST_CreateMatchSecretary_MatchSecretaryIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            CreateMatchSecretaryRequest createMatchSecretaryRequest = TestData.CreateMatchSecretaryRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId}/users";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = Helpers.CreateStringContent(createMatchSecretaryRequest);

            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            CreateMatchSecretaryResponse responseObject = JsonConvert.DeserializeObject<CreateMatchSecretaryResponse>(responseAsJson);
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.UserName.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task GolfClubController_POST_AddTournamentDivision_MatchSecretaryIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();

            AddTournamentDivisionToGolfClubRequest addTournamentDivisionToGolfClubRequest = TestData.AddTournamentDivisionToGolfClubRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId}/tournamentdivisions";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = Helpers.CreateStringContent(addTournamentDivisionToGolfClubRequest);

            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            AddTournamentDivisionToGolfClubResponse responseObject = JsonConvert.DeserializeObject<AddTournamentDivisionToGolfClubResponse>(responseAsJson);
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.TournamentDivision.ShouldBe(addTournamentDivisionToGolfClubRequest.Division);
        }

        [Fact]
        public async Task GolfClubController_POST_RequestClubMembership_MembershipIdReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();

            CreateMatchSecretaryRequest createMatchSecretaryRequest = TestData.CreateMatchSecretaryRequest;
            String uri = $"api/golfclubs/{TestData.GolfClubId}/players/{TestData.PlayerId}";

            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = new StringContent(String.Empty);

            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            RequestClubMembershipResponse responseObject = JsonConvert.DeserializeObject<RequestClubMembershipResponse>(responseAsJson);
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.PlayerId.ShouldBe(TestData.PlayerId);
            responseObject.MembershipId.ShouldBe(Guid.Empty);
        }
    }
}