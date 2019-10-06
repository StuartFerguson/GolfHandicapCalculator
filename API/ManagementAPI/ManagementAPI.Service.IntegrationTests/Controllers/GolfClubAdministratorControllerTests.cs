namespace ManagementAPI.Service.IntegrationTests.Controllers
{
    using System;
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

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Startup" />
    [Collection("TestCollection")]
    public class GolfClubAdministratorControllerTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        #region Fields

        /// <summary>
        /// The web application factory
        /// </summary>
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAdministratorControllerTests"/> class.
        /// </summary>
        /// <param name="webApplicationFactory">The web application factory.</param>
        public GolfClubAdministratorControllerTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Golfs the club administrator controller delete golf club administrator list method not allowed returned.
        /// </summary>
        [Fact]
        public async Task GolfClubAdministratorController_DELETE_GolfClubAdministrator_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = "api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.DeleteAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Golfs the club administrator controller get golf club administrator not implemented returned.
        /// </summary>
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

        /// <summary>
        /// Golfs the club administrator controller get golf club administrator list method not allowed returned.
        /// </summary>
        [Fact]
        public async Task GolfClubAdministratorController_GET_GolfClubAdministratorList_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = "api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");

            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Golfs the club administrator controller patch golf club administrator list method not allowed returned.
        /// </summary>
        [Fact]
        public async Task GolfClubAdministratorController_PATCH_GolfClubAdministratorList_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = "api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = new StringContent(string.Empty);

            // 2. Act
            HttpResponseMessage response = await client.PatchAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        /// <summary>
        /// Golfs the club administrator controller post golf club administrator golf club administrator is returned.
        /// </summary>
        [Fact]
        public async Task GolfClubAdministratorController_POST_GolfClubAdministrator_GolfClubAdministratorIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            RegisterClubAdministratorRequest registerClubAdministratorRequest = TestData.RegisterClubAdministratorRequest;
            String uri = "api/golfclubadministrators/";
            StringContent content = Helpers.CreateStringContent(registerClubAdministratorRequest);
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.PostAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            RegisterClubAdministratorResponse responseObject = JsonConvert.DeserializeObject<RegisterClubAdministratorResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubAdministratorId.ShouldBe(TestData.GolfClubAdministratorUserId);
        }

        /// <summary>
        /// Golfs the club administrator controller put golf club administrator list method not allowed returned.
        /// </summary>
        [Fact]
        public async Task GolfClubAdministratorController_PUT_GolfClubAdministrator_MethodNotAllowedReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = "api/golfclubadministrators/";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            StringContent content = new StringContent(string.Empty);

            // 2. Act
            HttpResponseMessage response = await client.PutAsync(uri, content, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.MethodNotAllowed);
        }

        #endregion
    }
}