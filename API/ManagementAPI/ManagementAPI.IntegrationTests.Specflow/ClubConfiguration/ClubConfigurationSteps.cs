using System;
using System.Collections.Generic;
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

        }
        
        [Given(@"The Golf Handicapping System Is Running")]
        public void GivenTheGolfHandicappingSystemIsRunning()
        {
            RunSystem(this.ScenarioContext.ScenarioInfo.Title);
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
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");

                String requestSerialised = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                this.ScenarioContext["CreateClubConfigurationHttpResponse"] = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None).ConfigureAwait(false);
            }
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
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("CreateClubConfigurationHttpResponse");

            var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

            responseData.ClubConfigurationId.ShouldNotBe(Guid.Empty);
        }

        [Given(@"My Club configuration has been created")]
        public async Task GivenMyClubConfigurationHasBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");

                String requestSerialised = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var httpResponse = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None).ConfigureAwait(false);

                httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

                var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

                responseData.ClubConfigurationId.ShouldNotBe(Guid.Empty);

                // Cache the create club config response
                this.ScenarioContext["CreateClubConfigurationResponse"] = responseData;
            }
        }
        
        [When(@"I request the details of the club")]
        public async Task WhenIRequestTheDetailsOfTheClub()
        {
            var createClubConfigurationResponse =
                this.ScenarioContext.Get<CreateClubConfigurationResponse>("CreateClubConfigurationResponse");

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");

                this.ScenarioContext["GetClubConfigurationHttpResponse"] = await client.GetAsync($"/api/ClubConfiguration/{createClubConfigurationResponse.ClubConfigurationId}", CancellationToken.None).ConfigureAwait(false);
            }
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
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("GetClubConfigurationHttpResponse");

            var responseData = JsonConvert.DeserializeObject<GetClubConfigurationResponse>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

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

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");


                var addMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
                addMeasuredCourseToClubRequest.ClubAggregateId = createClubConfigurationResponse.ClubConfigurationId;

                var requestSerialised = JsonConvert.SerializeObject(addMeasuredCourseToClubRequest);
                var httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                this.ScenarioContext["AddMeasuredCourseToClubHttpResponse"] = await client.PutAsync("/api/ClubConfiguration", httpContent, CancellationToken.None).ConfigureAwait(false);
            }
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

                // Insert the Subscription Stream
                String streamName = "$et-ManagementAPI.ClubConfiguration.DomainEvents.ClubConfigurationCreatedEvent";
                String endpointUrl = $"http://{this.ManagementAPIContainer.Name}:5000/api/DomainEvent";

                MySqlCommand streamInsert = connection.CreateCommand();
                streamInsert.CommandText =
                    $"insert into SubscriptionStream(Id, StreamName, SubscriptionType) select 'f47f87ce-fd4f-11e8-ac9e-00155d0d422e', '{streamName}', 0";
                streamInsert.ExecuteNonQuery();

                MySqlCommand endpointInsert = connection.CreateCommand();
                endpointInsert.CommandText =
                    $"insert into EndPoints(EndpointId, name, url) select 'a05ee7ce-fd4f-11e8-ac9e-00155d0d422e', 'ManagementAPI', '{endpointUrl}'";
                endpointInsert.ExecuteNonQuery();

                MySqlCommand groupInsert = connection.CreateCommand();
                groupInsert.CommandText =
                    "insert into SubscriptionGroups(Id, BufferSize, EndpointId, Name, StreamPosition, SubscriptionStreamId) select uuid(), 10, 'a05ee7ce-fd4f-11e8-ac9e-00155d0d422e', 'ClubCreated', null, 'f47f87ce-fd4f-11e8-ac9e-00155d0d422e'";
                groupInsert.ExecuteNonQuery();
                
                connection.Close();
            }
        }

        protected override void CleanupSubscriptionServiceConfig()
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

            MySqlCommand streamDelete = connection.CreateCommand();
            streamDelete.CommandText =
                $"delete from SubscriptionStream";
            streamDelete.ExecuteNonQuery();

            MySqlCommand endpointDelete = connection.CreateCommand();
            endpointDelete.CommandText =
                $"delete from EndPoints";
            endpointDelete.ExecuteNonQuery();

            MySqlCommand groupDelete = connection.CreateCommand();
            groupDelete.CommandText =
                "delete from SubscriptionGroups";
            groupDelete.ExecuteNonQuery();

            connection.Close();
        }

        [Given(@"a club has already been created")]
        public async Task GivenAClubHasAlreadyBeenCreated()
        {
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");

                String requestSerialised = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var httpResponse = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None).ConfigureAwait(false);

                httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

                var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

                responseData.ClubConfigurationId.ShouldNotBe(Guid.Empty);
            }

            Thread.Sleep(10000);
        }
        
        [When(@"I request the list of clubs")]
        public async Task WhenIRequestTheListOfClubs()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");
                
                this.ScenarioContext["GetClubListHttpResponse"] = await client.GetAsync("/api/ClubConfiguration", CancellationToken.None).ConfigureAwait(false);
            }
        }
        
        [Then(@"a list of clubs will be returned")]
        public async Task ThenAListOfClubsWillBeReturned()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("GetClubListHttpResponse");

            var responseData = JsonConvert.DeserializeObject<List<GetClubConfigurationResponse>>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

            responseData.Count.ShouldBe(1);
        }

    }
}
