namespace ManagementAPI.IntegrationTests.Player
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using GolfClub;
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

        [Given(@"I have my details to register")]
        public void GivenIHaveMyDetailsToRegister()
        {
            // Construct the request 
            this.PlayerTestingContext.RegisterPlayerRequest = IntegrationTestsTestData.RegisterPlayerRequest;
        }

        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await this.RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);

            // Setup the base address resolver
            this.BaseAddressResolver = api => $"http://127.0.0.1:{this.ManagementApiPort}";

            this.HttpClient = new HttpClient();
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

        protected override void SetupSubscriptionServiceConfig()
        {
            // Nothing in here
        }

        #endregion
    }
}