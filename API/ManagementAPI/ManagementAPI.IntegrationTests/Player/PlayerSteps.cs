namespace ManagementAPI.IntegrationTests.Player
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using Ductus.FluentDocker.Services.Extensions;
    using GolfClub;
    using MySql.Data.MySqlClient;
    using Service.Client;
    using Service.DataTransferObjects;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "player")]
    public class PlayerSteps : GenericSteps
    {
        #region Fields

        private Func<String, String> BaseAddressResolver;

        private HttpClient HttpClient;

        private readonly PlayerTestingContext PlayerTestingContext;

        #endregion

        #region Constructors

        public PlayerSteps(ScenarioContext scenarioContext,
                           PlayerTestingContext playerTestingContext) : base(scenarioContext)
        {
            this.PlayerTestingContext = playerTestingContext;
        }

        #endregion

        #region Methods

        [AfterScenario]
        public void AfterScenario()
        {
            this.StopSystem();
        }

        [Given(@"I am logged in as a golf club administrator")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministrator()
        {
            this.PlayerTestingContext.ClubAdministratorToken = await this.GetToken(TokenType.Password,
                                                                                   "integrationTestClient",
                                                                                   "integrationTestClient",
                                                                                   IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                   IntegrationTestsTestData.RegisterClubAdministratorRequest.Password)
                                                                         .ConfigureAwait(false);
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.PlayerTestingContext.PlayerToken = await this.GetToken(TokenType.Password,
                                                                        "integrationTestClient",
                                                                        "integrationTestClient",
                                                                        IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress,
                                                                        "123456").ConfigureAwait(false);
        }

        [Given(@"I am registered as a player")]
        public void GivenIAmRegisteredAsAPlayer()
        {
            RegisterPlayerRequest request = IntegrationTestsTestData.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () =>
                            {
                                this.PlayerTestingContext.RegisterPlayerResponse = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [Given(@"I have my details to register")]
        public void GivenIHaveMyDetailsToRegister()
        {
            // Construct the request 
            this.PlayerTestingContext.RegisterPlayerRequest = IntegrationTestsTestData.RegisterPlayerRequest;
        }

        [Given(@"I have registered as a golf club administrator")]
        public void GivenIHaveRegisteredAsAGolfClubAdministrator()
        {
            RegisterClubAdministratorRequest request = IntegrationTestsTestData.RegisterClubAdministratorRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () => { await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false); });
        }

        [Given(@"The club I want to register for is already created")]
        public async Task GivenTheClubIWantToRegisterForIsAlreadyCreated()
        {
            CreateGolfClubRequest request = IntegrationTestsTestData.CreateGolfClubRequest;

            String bearerToken = this.PlayerTestingContext.ClubAdministratorToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.PlayerTestingContext.GolfClubId = response.GolfClubId;
        }

        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await this.RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);

            // Setup the base address resolver
            this.BaseAddressResolver = api => $"http://127.0.0.1:{this.ManagementApiPort}";

            this.HttpClient = new HttpClient();
        }

        [Then(@"a list of my memberships will be retunred")]
        public void ThenAListOfMyMembershipsWillBeRetunred()
        {
            this.PlayerTestingContext.ClubMembershipResponses.ShouldNotBeEmpty();
        }

        [Then(@"a my details will be returned")]
        public void ThenAMyDetailsWillBeReturned()
        {
            this.PlayerTestingContext.GetPlayerDetailsResponse.ShouldNotBeNull();
        }

        [Then(@"my details are registered successfully")]
        public void ThenMyDetailsAreRegisteredSuccessfully()
        {
            RegisterPlayerResponse response = this.PlayerTestingContext.RegisterPlayerResponse;

            response.ShouldNotBeNull();
            response.PlayerId.ShouldNotBe(Guid.Empty);
        }

        [When(@"I register my details on the system")]
        public void WhenIRegisterMyDetailsOnTheSystem()
        {
            RegisterPlayerRequest request = this.PlayerTestingContext.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () =>
                            {
                                this.PlayerTestingContext.RegisterPlayerResponse = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [When(@"I request a list of my memberships")]
        public async Task WhenIRequestAListOfMyMemberships()
        {
            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.PlayerTestingContext.PlayerToken;

            await Retry.For(async () => { this.PlayerTestingContext.ClubMembershipResponses = await client.GetPlayerMemberships(bearerToken, CancellationToken.None); });
        }

        [When(@"I request club membership my request is accepted")]
        public void WhenIRequestClubMembershipMyRequestIsAccepted()
        {
            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.PlayerTestingContext.PlayerToken;
            Guid golfClubId = this.PlayerTestingContext.GolfClubId;

            Should.NotThrow(async () => { await client.RequestClubMembership(bearerToken, golfClubId, CancellationToken.None).ConfigureAwait(false); });
        }

        [When(@"I request my player details")]
        public async Task WhenIRequestMyPlayerDetails()
        {
            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.PlayerTestingContext.PlayerToken;

            await Retry.For(async () => { this.PlayerTestingContext.GetPlayerDetailsResponse = await client.GetPlayer(bearerToken, CancellationToken.None); });
        }

        protected override void SetupSubscriptionServiceConfig()
        {
            String streamName = string.Empty;
            String subscriptionGroup = string.Empty;

            // Set the stream name
            switch(this.ScenarioContext.ScenarioInfo.Title)
            {
                case "Get Player Memberships":
                    streamName = "$ce-GolfClubMembershipAggregate";
                    subscriptionGroup = "GolfClubMembershipAggregate";
                    break;
            }

            if (!string.IsNullOrEmpty(streamName))
            {
                IPEndPoint mysqlEndpoint = Setup.DatabaseServerContainer.ToHostExposedEndpoint("3306/tcp");

                String server = "127.0.0.1";
                String database = "SubscriptionServiceConfiguration";
                String user = "root";
                String password = "Pa55word";
                String port = mysqlEndpoint.Port.ToString();
                String sslM = "none";

                String connectionString = $"server={server};port={port};user id={user}; password={password}; database={database}; SslMode={sslM}";

                MySqlConnection connection = new MySqlConnection(connectionString);

                connection.Open();

                Guid subscriptionStreamId = Guid.NewGuid();
                Guid endpointId = Guid.NewGuid();
                Guid subscriptionServiceGroup = Guid.NewGuid();
                Guid subscriptionGroupId = Guid.NewGuid();

                // Insert the Subscription Stream
                String endpointUrl = $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent/GolfClubMembership";

                MySqlCommand streamInsert = connection.CreateCommand();
                streamInsert.CommandText = $"insert into SubscriptionStream(Id, StreamName, SubscriptionType) select '{subscriptionStreamId}', '{streamName}', 0";
                streamInsert.ExecuteNonQuery();

                MySqlCommand endpointInsert = connection.CreateCommand();
                endpointInsert.CommandText = $"insert into EndPoints(EndpointId, name, url) select '{endpointId}', 'Golf Club Read Model', '{endpointUrl}'";
                endpointInsert.ExecuteNonQuery();

                MySqlCommand groupInsert = connection.CreateCommand();
                groupInsert.CommandText =
                    $"insert into SubscriptionGroups(Id, BufferSize, EndpointId, Name, StreamPosition, SubscriptionStreamId) select '{subscriptionGroupId}', 10, '{endpointId}', '{subscriptionGroup}', null, '{subscriptionStreamId}'";
                groupInsert.ExecuteNonQuery();

                // Insert the subscription service
                MySqlCommand subscriptionServiceInsert = connection.CreateCommand();
                subscriptionServiceInsert.CommandText =
                    $"insert into SubscriptionServices(SubscriptionServiceId, Description) select '{this.SubscriberServiceId}', 'Test Service'";
                subscriptionServiceInsert.ExecuteNonQuery();

                MySqlCommand subscriptonServiceGroupInsert = connection.CreateCommand();
                subscriptonServiceGroupInsert.CommandText =
                    $"insert into SubscriptionServiceGroups(SubscriptionServiceGroupId, SubscriptionGroupId, SubscriptionServiceId) select '{subscriptionServiceGroup}', '{subscriptionGroupId}', '{this.SubscriberServiceId}' ";
                subscriptonServiceGroupInsert.ExecuteNonQuery();

                connection.Close();
            }
        }

        #endregion
    }
}