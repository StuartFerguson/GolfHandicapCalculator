using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.IntegrationTests.Specflow.Common;
using ManagementAPI.Service.DataTransferObjects;
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

                this.ScenarioContext["GetClubConfigurationHttpResponse"] = await client.GetAsync($"/api/ClubConfiguration?clubId={createClubConfigurationResponse.ClubConfigurationId}", CancellationToken.None).ConfigureAwait(false);
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
    }
}
