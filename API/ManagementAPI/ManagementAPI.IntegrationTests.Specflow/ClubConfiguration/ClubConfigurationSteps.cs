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

namespace ManagementAPI.IntegrationTests.Specflow.ClubConfiguration
{
    [Binding]
    [Scope(Tag = "clubconfiguration")]
    public class ClubConfigurationSteps : GenericSteps
    {
        public ClubConfigurationSteps(ScenarioContext scenarioContext) : base(scenarioContext)
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
            this.ScenarioContext["CreateClubConfigurationRequest"] = IntegrationTestsTestData.CreateClubConfigurationRequest; 
        }
        
        [When(@"I call Create Club Configuration")]
        public async Task WhenICallCreateClubConfiguration()
        {
            var request = this.ScenarioContext.Get<CreateClubConfigurationRequest>("CreateClubConfigurationRequest");
            
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

            this.ScenarioContext["CreateClubConfigurationHttpResponse"] = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
        }
        
        [Then(@"the club configuration will be created")]
        public void ThenTheClubConfigurationWillBeCreated()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("CreateClubConfigurationHttpResponse");
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Then(@"I will get the new Club Configuration Id in the response")]
        public async Task ThenIWillGetTheNewClubConfigurationIdInTheResponse()
        {
            var responseData = await GetResponseObject<CreateClubConfigurationResponse>("CreateClubConfigurationHttpResponse")
                .ConfigureAwait(false);

            responseData.ClubConfigurationId.ShouldNotBe(Guid.Empty);
        }

        [Given(@"My Club configuration has been created")]
        public async Task GivenMyClubConfigurationHasBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;
            
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);

            var responseData = await GetResponseObject<CreateClubConfigurationResponse>(httpResponse).ConfigureAwait(false);

            responseData.ClubConfigurationId.ShouldNotBe(Guid.Empty);

            // Cache the create club config response
            this.ScenarioContext["CreateClubConfigurationResponse"] = responseData;
        }
        
        [When(@"I request the details of the club")]
        public async Task WhenIRequestTheDetailsOfTheClub()
        {
            var createClubConfigurationResponse =
                this.ScenarioContext.Get<CreateClubConfigurationResponse>("CreateClubConfigurationResponse");

            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration/{createClubConfigurationResponse.ClubConfigurationId}";

            this.ScenarioContext["GetClubConfigurationHttpResponse"] = await MakeHttpGet(requestUri, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"the club configuration will be returned")]
        public void ThenTheClubConfigurationWillBeReturned()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("GetClubConfigurationHttpResponse");
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Then(@"the club data returned is the correct club")]
        public async Task ThenTheClubDataReturnedIsTheCorrectClub()
        {
            var responseData = await GetResponseObject<GetClubConfigurationResponse>("GetClubConfigurationHttpResponse").ConfigureAwait(false);

            var originalRequest = IntegrationTestsTestData.CreateClubConfigurationRequest;

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
            var createClubConfigurationResponse =
                this.ScenarioContext.Get<CreateClubConfigurationResponse>("CreateClubConfigurationResponse");

            var request = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
            request.ClubAggregateId = createClubConfigurationResponse.ClubConfigurationId;

            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");
            
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

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
            if (this.ScenarioContext.ScenarioInfo.Title == "Get Club List")
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
                Guid subscriptonServiceGroup = Guid.NewGuid();
                Guid subscriptonGroupId = Guid.NewGuid();

                // Insert the Subscription Stream
                String streamName = "$et-ManagementAPI.ClubConfiguration.DomainEvents.ClubConfigurationCreatedEvent";
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
                    $"insert into SubscriptionGroups(Id, BufferSize, EndpointId, Name, StreamPosition, SubscriptionStreamId) select '{subscriptonGroupId}', 10, '{endpointId}', 'ClubCreated', null, '{subscriptionStreamId}'";
                groupInsert.ExecuteNonQuery();
                
                // Insert the subscription service
                MySqlCommand subscriptionServiceInsert = connection.CreateCommand();
                subscriptionServiceInsert.CommandText =
                    $"insert into SubscriptionServices(SubscriptionServiceId, Description) select '{this.SubscriberServiceId}', 'Test Service'";
                subscriptionServiceInsert.ExecuteNonQuery();
                
                MySqlCommand subscriptonServiceGroupInsert = connection.CreateCommand();
                subscriptonServiceGroupInsert.CommandText = $"insert into SubscriptionServiceGroups(SubscriptionServiceGroupId, SubscriptionGroupId, SubscriptionServiceId) select '{subscriptonServiceGroup}', '{subscriptonGroupId}', '{this.SubscriberServiceId}' ";
                subscriptonServiceGroupInsert.ExecuteNonQuery();

                connection.Close();
            }
        }

        [Given(@"a club has already been created")]
        public async Task GivenAClubHasAlreadyBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";
            
            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);

            var responseData = await GetResponseObject<CreateClubConfigurationResponse>(httpResponse).ConfigureAwait(false);

            this.ScenarioContext["ClubConfigurationId"] = responseData.ClubConfigurationId;

            Thread.Sleep(30000);
        }
        
        [When(@"I request the list of clubs")]
        public async Task WhenIRequestTheListOfClubs()
        {
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

            // Get the token
            var bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            this.ScenarioContext["GetClubListHttpResponse"] = await MakeHttpGet(requestUri, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"a list of clubs will be returned")]
        public async Task ThenAListOfClubsWillBeReturned()
        {            
            var clubConfigurationId = this.ScenarioContext.Get<Guid>("ClubConfigurationId");

            var responseData = await GetResponseObject<List<GetClubConfigurationResponse>>("GetClubListHttpResponse")
                .ConfigureAwait(false);

            responseData.Any(r => r.Id == clubConfigurationId).ShouldBeTrue();
        }

        [Given(@"I am logged in as a club administrator")]
        public async Task GivenIAmLoggedInAsAClubAdministrator()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.CreateClubConfigurationRequest.EmailAddress, "123456").ConfigureAwait(false);

            this.ScenarioContext["ClubAdministratorToken"] = tokenResponse;
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                "player@test.co.uk", "123456").ConfigureAwait(false);

            this.ScenarioContext["PlayerToken"] = tokenResponse;
        }

    }
}
