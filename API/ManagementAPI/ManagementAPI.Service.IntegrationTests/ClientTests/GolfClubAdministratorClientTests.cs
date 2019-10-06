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
    using DataTransferObjects.Responses.v2;
    using Newtonsoft.Json.Linq;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class GolfClubAdministratorClientTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public GolfClubAdministratorClientTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task GolfClubAdministratorClient_RegisterGolfClubAdministrator_GolfClubAdministratorIsCreated()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IGolfClubAdministratorClient golfClubAdministratorClient = new GolfClubAdministratorClient(resolver, client);

            RegisterClubAdministratorRequest registerClubAdministratorRequest = TestData.RegisterClubAdministratorRequest;

            // 2. Act
            RegisterClubAdministratorResponse registerClubAdministratorResponse = await golfClubAdministratorClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, CancellationToken.None);

            // 3. Assert
            registerClubAdministratorResponse.GolfClubAdministratorId.ShouldBe(TestData.GolfClubAdministratorUserId);
        }
    }
}
