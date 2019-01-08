using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ductus.FluentDocker.Services.Extensions;
using ManagementAPI.IntegrationTests.Specflow.Common;
using ManagementAPI.Service.DataTransferObjects;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Shouldly;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Specflow.GolfClub
{
    [Binding]
    [Scope(Tag = "golfclub")]
    public class GolfClubSteps : GenericSteps
    {
        public GolfClubSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            // Nothing in here
        }
        
        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);
        }
        
        [AfterScenario()]
        public void AfterScenario()
        {
            StopSystem();
        }

        [Given(@"I have the details of the new club")]
        public void GivenIHaveTheDetailsOfTheNewClub()
        {
            // Construct the request 
            this.ScenarioContext["CreateGolfClubRequest"] = IntegrationTestsTestData.CreateGolfClubRequest; 
        }
        
        [When(@"I call Create Golf Club")]
        public async Task WhenICallCreateGolfClub()
        {
            var request = this.ScenarioContext.Get<CreateGolfClubRequest>("CreateGolfClubRequest");
            
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub";

            this.ScenarioContext["CreateGolfClubHttpResponse"] = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
        }
        
        [Then(@"the golf club configuration will be created")]
        public void ThenTheGolfClubConfigurationWillBeCreated()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("CreateGolfClubHttpResponse");
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Then(@"I will get the new Golf Club Id in the response")]
        public async Task ThenIWillGetTheNewGolfClubIdInTheResponse()
        {
            var responseData = await GetResponseObject<CreateGolfClubResponse>("CreateGolfClubHttpResponse")
                .ConfigureAwait(false);

            responseData.GolfClubId.ShouldNotBe(Guid.Empty);
        }

        [Given(@"My Golf Club has been created")]
        public async Task GivenMyGolfClubHasBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateGolfClubRequest;
            
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub";

            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);

            var responseData = await GetResponseObject<CreateGolfClubResponse>(httpResponse).ConfigureAwait(false);

            responseData.GolfClubId.ShouldNotBe(Guid.Empty);

            // Cache the create club config response
            this.ScenarioContext["CreateGolfClubResponse"] = responseData;
        }
        
        [When(@"I request the details of the golf club")]
        public async Task WhenIRequestTheDetailsOfTheGolfClub()
        {
            var createGolfClubResponse =
                this.ScenarioContext.Get<CreateGolfClubResponse>("CreateGolfClubResponse");

            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub/{createGolfClubResponse.GolfClubId}";

            this.ScenarioContext["GetSingleGolfClubHttpResponse"] = await MakeHttpGet(requestUri, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"the golf club will be returned")]
        public void ThenTheGolfClubWillBeReturned()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("GetSingleGolfClubHttpResponse");
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Then(@"the club data returned is the correct club")]
        public async Task ThenTheClubDataReturnedIsTheCorrectClub()
        {
            var responseData = await GetResponseObject<GetGolfClubResponse>("GetSingleGolfClubHttpResponse").ConfigureAwait(false);

            var originalRequest = IntegrationTestsTestData.CreateGolfClubRequest;

            responseData.Name.ShouldBe(originalRequest.Name);
            responseData.AddressLine1.ShouldBe(originalRequest.AddressLine1);
            responseData.AddressLine2.ShouldBe(originalRequest.AddressLine2);
            responseData.Town.ShouldBe(originalRequest.Town);
            responseData.Region.ShouldBe(originalRequest.Region);
            responseData.PostalCode.ShouldBe(originalRequest.PostalCode);
            responseData.TelephoneNumber.ShouldBe(originalRequest.TelephoneNumber);
            responseData.Website.ShouldBe(originalRequest.Website);
            responseData.EmailAddress.ShouldBe(originalRequest.EmailAddress);
        }
        
        [When(@"I add a measured course to the club")]
        public async Task WhenIAddAMeasuredCourseToTheClub()
        {
            var createGolfClubResponse =
                this.ScenarioContext.Get<CreateGolfClubResponse>("CreateGolfClubResponse");

            var request = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
            
            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");
            
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub/{createGolfClubResponse.GolfClubId}";

            this.ScenarioContext["AddMeasuredCourseToClubHttpResponse"] = await MakeHttpPut(requestUri, request, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"the measured course is added to the club")]
        public void ThenTheMeasuredCourseIsAddedToTheClub()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("AddMeasuredCourseToClubHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        protected override void SetupSubscriptionServiceConfig()
        {
            String streamName = String.Empty;

            // Set the stream name
            switch (this.ScenarioContext.ScenarioInfo.Title)
            {
                case "Get Golf Club List":
                    streamName = "$et-ManagementAPI.GolfClub.DomainEvents.GolfClubCreatedEvent";
                    break;
                case "Get Pending Membership Request List":
                    streamName = "$et-ManagementAPI.Player.DomainEvents.ClubMembershipRequestedEvent";
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
                    $"insert into SubscriptionGroups(Id, BufferSize, EndpointId, Name, StreamPosition, SubscriptionStreamId) select '{subscriptionGroupId}', 10, '{endpointId}', 'ClubCreated', null, '{subscriptionStreamId}'";
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

        [Given(@"a golf club has already been created")]
        public async Task GivenAGolfClubHasAlreadyBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateGolfClubRequest;

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub";
            
            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);

            var responseData = await GetResponseObject<CreateGolfClubResponse>(httpResponse).ConfigureAwait(false);

            this.ScenarioContext["GolfClubId"] = responseData.GolfClubId;

            Thread.Sleep(30000);
        }
        
        [When(@"I request the list of golf clubs")]
        public async Task WhenIRequestTheListOfGolfClubs()
        {
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub";

            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            this.ScenarioContext["GetGolfClubListHttpResponse"] = await MakeHttpGet(requestUri, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"a list of golf clubs will be returned")]
        public async Task ThenAListOfGolfClubsWillBeReturned()
        {            
            var golfClubId = this.ScenarioContext.Get<Guid>("GolfClubId");

            var responseData = await GetResponseObject<List<GetGolfClubResponse>>("GetGolfClubListHttpResponse")
                .ConfigureAwait(false);

            responseData.Any(r => r.Id == golfClubId).ShouldBeTrue();
        }

        [Given(@"I am logged in as a club administrator")]
        public async Task GivenIAmLoggedInAsAClubAdministrator()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.CreateGolfClubRequest.EmailAddress, "123456").ConfigureAwait(false);

            this.ScenarioContext["ClubAdministratorToken"] = tokenResponse;
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                "player@test.co.uk", "123456").ConfigureAwait(false);

            this.ScenarioContext["PlayerToken"] = tokenResponse;
        }

        [Given(@"a player has already registed")]
        public async Task GivenAPlayerHasAlreadyRegisted()
        {
            var request = IntegrationTestsTestData.RegisterPlayerRequest;
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/Player";

            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
            var responseData = await GetResponseObject<RegisterPlayerResponse>(httpResponse).ConfigureAwait(false);

            this.ScenarioContext["PlayerId"] = responseData.PlayerId;
        }
        
        [Given(@"a player has requested membership of the club")]
        public async Task GivenAPlayerHasRequestedMembershipOfTheClub()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            Guid golfClubId = this.ScenarioContext.Get<Guid>("GolfClubId");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Player/{playerId}/ClubMembershipRequest/{golfClubId}";

            var bearerToken = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                "player@test.co.uk", "123456").ConfigureAwait(false);

            Object resquestObject = null;
            await MakeHttpPut(requestUri, resquestObject,bearerToken).ConfigureAwait(false);

            Thread.Sleep(30000);
        }
        
        [When(@"I request the list of pending membership requests")]
        public async Task WhenIRequestTheListOfPendingMembershipRequests()
        {
            Guid golfClubId = this.ScenarioContext.Get<Guid>("GolfClubId");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/GolfClub/{golfClubId}/PendingMembershipRequests";

            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            this.ScenarioContext["GetPendingMembershipRequestsHttpResponse"] = await MakeHttpGet(requestUri, bearerToken);
        }
        
        [Then(@"a list of pending membership requests will be returned")]
        public async Task ThenAListOfPendingMembershipRequestsWillBeReturned()
        {
            var golfClubId = this.ScenarioContext.Get<Guid>("GolfClubId");

            var responseData = await GetResponseObject<List<GetClubMembershipRequestResponse>>("GetPendingMembershipRequestsHttpResponse")
                .ConfigureAwait(false);

            responseData.Any(r => r.ClubId == golfClubId).ShouldBeTrue();
        }

    }
}
