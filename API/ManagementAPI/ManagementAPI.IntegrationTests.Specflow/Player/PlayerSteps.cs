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

namespace ManagementAPI.IntegrationTests.Specflow.Player
{
    [Binding]
    [Scope(Tag = "player")]
    public class PlayerSteps : GenericSteps
    {
        public PlayerSteps(ScenarioContext scenarioContext) : base(scenarioContext)
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

        [Given(@"I have my details to register")]
        public void GivenIHaveMyDetailsToRegister()
        {
            // Construct the request 
            this.ScenarioContext["RegisterPlayerRequest"] = IntegrationTestsTestData.RegisterPlayerRequest;
        }
        
        [When(@"I register my details on the system")]
        public async Task WhenIRegisterMyDetailsOnTheSystem()
        {
            var request = this.ScenarioContext.Get<RegisterPlayerRequest>("RegisterPlayerRequest");
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri($"http://127.0.0.1:{this.ManagementApiPort}");

                String requestSerialised = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                this.ScenarioContext["RegisterPlayerHttpResponse"] = await client.PostAsync("/api/Player", httpContent, CancellationToken.None).ConfigureAwait(false);
            }
        }
        
        [Then(@"my details are registered")]
        public void ThenMyDetailsAreRegistered()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("RegisterPlayerHttpResponse");
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Then(@"my player id will be returned in the response")]
        public async Task ThenMyPlayerIdWillBeReturnedInTheResponse()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("RegisterPlayerHttpResponse");

            var responseData = JsonConvert.DeserializeObject<RegisterPlayerResponse>(await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false));

            responseData.PlayerId.ShouldNotBe(Guid.Empty);
        }

        protected override void SetupSubscriptionServiceConfig()
        {
            
        }

        protected override void CleanupSubscriptionServiceConfig()
        {
            
        }
    }
}
