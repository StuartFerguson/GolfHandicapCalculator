namespace ManagementAPI.Service.IntegrationTests.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses.v2;
    using Newtonsoft.Json;
    using Shouldly;
    using Xunit;
    using RegisterPlayerResponse = DataTransferObjects.Responses.RegisterPlayerResponse;

    [Collection("TestCollection")]
    public class PlayerControllerTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public PlayerControllerTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task PlayerController_POST_CreatePlayer_PlayerIdIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            RegisterPlayerRequest registerPlayerRequest = TestData.RegisterPlayerRequest;
            String uri = "api/players/";
            StringContent content = Helpers.CreateStringContent(registerPlayerRequest);
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            RegisterPlayerResponse responseObject = JsonConvert.DeserializeObject<RegisterPlayerResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.PlayerId.ShouldNotBe(Guid.Empty);
        }

        [Fact]
        public async Task PlayerController_GET_GetPlayer_PlayerIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();

            String uri = $"api/players/{TestData.PlayerId}";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri,  CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetPlayerResponse responseObject = JsonConvert.DeserializeObject<GetPlayerResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.PlayerId);
        }

        [Fact]
        public async Task PlayerController_GET_GetPlayer_WithMemberships_PlayerIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();

            String uri = $"api/players/{TestData.PlayerId}?includeMemberships=true";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetPlayerResponse responseObject = JsonConvert.DeserializeObject<GetPlayerResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.PlayerId);
            responseObject.ClubMemberships.ShouldNotBeNull();
            responseObject.ClubMemberships.ShouldNotBeEmpty();
            responseObject.SignedUpTournaments.ShouldBeNull();
        }

        [Fact]
        public async Task PlayerController_GET_GetPlayer_WithTournamentSignups_PlayerIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();

            String uri = $"api/players/{TestData.PlayerId}?includeTournamentSignups=true";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetPlayerResponse responseObject = JsonConvert.DeserializeObject<GetPlayerResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Id.ShouldBe(TestData.PlayerId);
            responseObject.ClubMemberships.ShouldBeNull();
            responseObject.SignedUpTournaments.ShouldNotBeNull();
            responseObject.SignedUpTournaments.ShouldNotBeEmpty();
        }
    }
}