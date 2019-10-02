namespace ManagementAPI.Service.IntegrationTests.ClientAndControllerTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client.v2;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class GolfClubAdministratorClientAndControllerTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public GolfClubAdministratorClientAndControllerTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task GolfClubAdministratorController_POST_GolfClubAdministrator_GolfClubAdministratorIsReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubAdministratorClient golfdGolfClubAdministratorClient = new GolfClubAdministratorClient(resolver, client);

            RegisterClubAdministratorRequest registerClubAdministratorRequest = TestData.RegisterClubAdministratorRequest;

            // 2. Act
            RegisterClubAdministratorResponse registerClubAdministratorResponse = await golfdGolfClubAdministratorClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, CancellationToken.None);

            // 3. Assert
            registerClubAdministratorResponse.GolfClubAdministratorId.ShouldBe(TestData.GolfClubAdministratorUserId);
        }
    }
}
