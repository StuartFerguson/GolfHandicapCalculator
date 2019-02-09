using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.IntegrationTests.Common;
using ManagementAPI.IntegrationTests.DataTransferObjects;
using ManagementAPI.Service.Client;
using ManagementAPI.Service.DataTransferObjects;
using Shouldly;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Player
{
    [Binding]
    [Scope(Tag = "player")]
    public class PlayerSteps : GenericSteps
    {
        private Func<String, String> baseAddressResolver;
        private HttpClient httpClient = null;

        public PlayerSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            // Nothing in here
        }

        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);

            // Setup the base address resolver
            baseAddressResolver = (api) => $"http://127.0.0.1:{this.ManagementApiPort}";

            httpClient = new HttpClient();
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
        public void WhenIRegisterMyDetailsOnTheSystem()
        {
            RegisterPlayerRequest request = this.ScenarioContext.Get<RegisterPlayerRequest>("RegisterPlayerRequest");

            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                this.ScenarioContext["RegisterPlayerResponse"] = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Then(@"my details are registered successfully")]
        public void ThenMyDetailsAreRegisteredSuccessfully()
        {
            RegisterPlayerResponse response = this.ScenarioContext.Get<RegisterPlayerResponse>("RegisterPlayerResponse");

            response.ShouldNotBeNull();
            response.PlayerId.ShouldNotBe(Guid.Empty);
        }
        
        protected override void SetupSubscriptionServiceConfig()
        {
            // Nothing in here
        }
    }
}
