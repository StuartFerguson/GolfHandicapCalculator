using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagementAPI.Service.IntegrationTests.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using Common;
    using DataTransferObjects.Requests;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class GolfClubAdministratorClientAndControllerTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;
        //private readonly HttpClient Client;

        public GolfClubAdministratorClientAndControllerTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task GolfClubAdministratorController_POST_GolfClubAdministrator_GolfClubAdministratorIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            RegisterClubAdministratorRequest registerClubAdministratorRequest = TestData.RegisterClubAdministratorRequest;
            String uri = $"api/golfclubadministrators/";
            StringContent content = Helpers.CreateStringContent(registerClubAdministratorRequest);
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);
            response.Headers.Location.OriginalString.ShouldBe($"api/golfclubadministrators/{TestData.GolfClubAdministratorUserId}");

            String responseAsJson = await response.Content.ReadAsStringAsync();

            JObject jObject = JObject.Parse(responseAsJson);
            JToken value = jObject.GetValue("golfClubAdministratorId");
            Guid golfClubAdministratorId = Guid.Parse(value.ToString());

            golfClubAdministratorId.ShouldBe(TestData.GolfClubAdministratorUserId);
        }

        [Fact]
        public async Task GolfClubAdministratorController_GET_GolfClubAdministrator_NotImplementedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/golfclubadministrators/{TestData.GolfClubAdministratorUserId}";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.NotImplemented);
        }

        [Fact]
        public async Task GolfClubAdministratorController_GET_GolfClubAdministratorList_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task GolfClubAdministratorController_PUT_GolfClubAdministratorList_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = new StringContent(String.Empty);

            // 2. Act
            HttpResponseMessage response = await client.PutAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task GolfClubAdministratorController_PATCH_GolfClubAdministratorList_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = new StringContent(String.Empty);

            // 2. Act
            HttpResponseMessage response = await client.PatchAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task GolfClubAdministratorController_DELETE_GolfClubAdministratorList_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.DeleteAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }
    }
}
