using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ductus.FluentDocker.Services.Extensions;
using ManagementAPI.IntegrationTests.Common;
using ManagementAPI.IntegrationTests.DataTransferObjects;
using ManagementAPI.Service.Client;
using ManagementAPI.Service.DataTransferObjects;
using MySql.Data.MySqlClient;
using Shouldly;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.GolfClub
{
    [Binding]
    [Scope(Tag = "golfclub")]
    public class GolfClubSteps : GenericSteps
    {
        private Func<String, String> baseAddressResolver;
        private HttpClient httpClient = null;

        public GolfClubSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            // Nothing in here
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            StopSystem();
        }

        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);

            // Setup the base address resolver
            baseAddressResolver = (api) => $"http://127.0.0.1:{this.ManagementApiPort}";

            httpClient = new HttpClient();
        }
        
        [When(@"I register my details as a golf club administrator")]
        public void WhenIRegisterMyDetailsAsAGolfClubAdministrator()
        {
            this.ScenarioContext["RegisterGolfClubAdministratorRequest"] = IntegrationTestsTestData.RegisterClubAdministratorRequest;
        }
        
        [Then(@"my registration should be successful")]
        public void ThenMyRegistrationShouldBeSuccessful()
        {
            var request = this.ScenarioContext.Get<RegisterClubAdministratorRequest>("RegisterGolfClubAdministratorRequest");

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false);
            });
        }

        [Given(@"I have registered as a golf club administrator")]
        public void GivenIHaveRegisteredAsAGolfClubAdministrator()
        {
            var request =  IntegrationTestsTestData.RegisterClubAdministratorRequest;

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Given(@"I have the details of the new club")]
        public void GivenIHaveTheDetailsOfTheNewClub()
        {
            this.ScenarioContext["CreateGolfClubRequest"] = IntegrationTestsTestData.CreateGolfClubRequest;
        }
        
        [Given(@"I am logged in as a golf club administrator")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministrator()
        {
            this.ScenarioContext["ClubAdministratorToken"] = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                IntegrationTestsTestData.RegisterClubAdministratorRequest.Password).ConfigureAwait(false);
        }
        
        [When(@"I call Create Golf Club")]
        public async Task WhenICallCreateGolfClub()
        {
            var request = this.ScenarioContext.Get<CreateGolfClubRequest>("CreateGolfClubRequest");

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            this.ScenarioContext["CreateGolfClubResponse"] = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
        }
        
        [Then(@"the golf club configuration will be created successfully")]
        public void ThenTheGolfClubConfigurationWillBeCreatedSuccessfully()
        {
            var response = this.ScenarioContext.Get<CreateGolfClubResponse>("CreateGolfClubResponse");

            response.ShouldNotBe(null);
            response.GolfClubId.ShouldNotBe(Guid.Empty);
        }

        [Given(@"my golf club has been created")]
        public async Task GivenMyGolfClubHasBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateGolfClubRequest;

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            var response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.ScenarioContext["GolfClubId"] = response.GolfClubId;

            if (this.ScenarioContext.ScenarioInfo.Title == "Get Golf Club List")
            {
                Thread.Sleep(30000);
            }
        }
        
        [When(@"I request the details of the golf club")]
        public async Task WhenIRequestTheDetailsOfTheGolfClub()
        {
            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            this.ScenarioContext["GetSingleGolfClubResponse"] = await client.GetSingleGolfClub(bearerToken, CancellationToken.None).ConfigureAwait(false);
        }
        
        [Then(@"the golf club data will be returned")]
        public void ThenTheGolfClubDataWillBeReturned()
        {
            var response = this.ScenarioContext.Get<GetGolfClubResponse>("GetSingleGolfClubResponse");

            response.ShouldNotBe(null);
            response.Name.ShouldBe(IntegrationTestsTestData.CreateGolfClubRequest.Name);
        }
        
        [Given(@"a player has been registered")]
        public void GivenAPlayerHasBeenRegistered()
        {
            var request = IntegrationTestsTestData.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.ScenarioContext["PlayerToken"] = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress,
                "123456").ConfigureAwait(false);
        }
        
        [When(@"I request the list of golf clubs")]
        public async Task WhenIRequestTheListOfGolfClubs()
        {
            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            var bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            this.ScenarioContext["GetGolfClubListResponse"] = await client.GetGolfClubList(bearerToken, CancellationToken.None).ConfigureAwait(false);            
        }
        
        [Then(@"a list of golf clubs will be returned")]
        public void ThenAListOfGolfClubsWillBeReturned()
        {
            var response = this.ScenarioContext.Get<List<GetGolfClubResponse>>("GetGolfClubListResponse");

            response.ShouldNotBeEmpty();
            response.Count.ShouldBe(1);
        }
        
        [When(@"I add a measured course to the club")]
        public void WhenIAddAMeasuredCourseToTheClub()
        {
            this.ScenarioContext["AddMeasuredCourseToClubRequest"] = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;            
        }
        
        [Then(@"the measured course is added to the club")]
        public void ThenTheMeasuredCourseIsAddedToTheClub()
        {
            var request = this.ScenarioContext.Get<AddMeasuredCourseToClubRequest>("AddMeasuredCourseToClubRequest");

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                await client.AddMeasuredCourseToGolfClub(bearerToken, request, CancellationToken.None)
                        .ConfigureAwait(false);
            });            
        }
        
        protected override void SetupSubscriptionServiceConfig()
        {
            String streamName = String.Empty;
            String subscriptionGroup = String.Empty;

            // Set the stream name
            switch (this.ScenarioContext.ScenarioInfo.Title)
            {
                case "Get Golf Club List":
                    streamName = "$et-ManagementAPI.GolfClub.DomainEvents.GolfClubCreatedEvent";
                    subscriptionGroup = "ClubCreated";
                    break;
                case "Get Pending Membership Request List":
                    streamName = "$et-ManagementAPI.Player.DomainEvents.ClubMembershipRequestedEvent";
                    subscriptionGroup = "MembershipRequested";
                    break;
            }
                        
            if (!String.IsNullOrEmpty(streamName))
            {
                var mysqlEndpoint = Setup.DatabaseServerContainer.ToHostExposedEndpoint("3306/tcp");

                // Try opening a connection
                Int32 maxRetries = 10;
                Int32 counter = 1;

                String server = "127.0.0.1";
                String database = "SubscriptionServiceConfiguration";
                String user = "root";
                String password = "Pa55word";
                String port = mysqlEndpoint.Port.ToString();
                String sslM = "none";

                String connectionString =
                    $"server={server};port={port};user id={user}; password={password}; database={database}; SslMode={sslM}";

                MySqlConnection connection = new MySqlConnection(connectionString);

                connection.Open();

                Guid subscriptionStreamId = Guid.NewGuid();
                Guid endpointId = Guid.NewGuid();
                Guid subscriptionServiceGroup = Guid.NewGuid();
                Guid subscriptionGroupId = Guid.NewGuid();

                // Insert the Subscription Stream
                String endpointUrl = $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent";

                MySqlCommand streamInsert = connection.CreateCommand();
                streamInsert.CommandText =
                    $"insert into SubscriptionStream(Id, StreamName, SubscriptionType) select '{subscriptionStreamId}', '{streamName}', 0";
                streamInsert.ExecuteNonQuery();

                MySqlCommand endpointInsert = connection.CreateCommand();
                endpointInsert.CommandText =
                    $"insert into EndPoints(EndpointId, name, url) select '{endpointId}', 'ManagementAPI', '{endpointUrl}'";
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
                subscriptonServiceGroupInsert.CommandText = $"insert into SubscriptionServiceGroups(SubscriptionServiceGroupId, SubscriptionGroupId, SubscriptionServiceId) select '{subscriptionServiceGroup}', '{subscriptionGroupId}', '{this.SubscriberServiceId}' ";
                subscriptonServiceGroupInsert.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
