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
            var request = this.ScenarioContext.Get<RegisterPlayerRequest>("RegisterPlayerRequest");

            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                this.ScenarioContext["RegisterPlayerResponse"] = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Then(@"my details are registered successfully")]
        public void ThenMyDetailsAreRegisteredSuccessfully()
        {
            var response = this.ScenarioContext.Get<RegisterPlayerResponse>("RegisterPlayerResponse");

            response.ShouldNotBeNull();
            response.PlayerId.ShouldNotBe(Guid.Empty);
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
        
        [Given(@"I am logged in as a golf club administrator")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministrator()
        {
            this.ScenarioContext["ClubAdministratorToken"] = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                IntegrationTestsTestData.RegisterClubAdministratorRequest.Password).ConfigureAwait(false);
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
        
        [Given(@"a player has been registered")]
        public void GivenAPlayerHasBeenRegistered()
        {
            var request = IntegrationTestsTestData.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                var response = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
                this.ScenarioContext["PlayerId"] = response.PlayerId;
            });
        }
        
        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.ScenarioContext["PlayerToken"] = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress,
                "123456").ConfigureAwait(false);
        }
        
        [When(@"I request club membership the request is successful")]
        public void WhenIRequestClubMembershipTheRequestIsSuccessful()
        {
            Guid golfClubId = this.ScenarioContext.Get<Guid>("GolfClubId");
            
            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            Should.NotThrow(async () => { await client.RequestClubMembership(bearerToken, golfClubId, CancellationToken.None).ConfigureAwait(false); });
        }

        [Given(@"a player has requested membership of the club")]
        public void GivenAPlayerHasRequestedMembershipOfTheClub()
        {
            Guid golfClubId = this.ScenarioContext.Get<Guid>("GolfClubId");
            
            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            Should.NotThrow(async () => { await client.RequestClubMembership(bearerToken, golfClubId, CancellationToken.None).ConfigureAwait(false); });
        }
        
        [Given(@"I approve a club membership request the request is successful")]
        public void GivenIApproveAClubMembershipRequestTheRequestIsSuccessful()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            
            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () => { await client.ApproveClubMembershipRequest(bearerToken, playerId, CancellationToken.None).ConfigureAwait(false); });
        }

        [Given(@"I reject a club membership request the request is successful")]
        public void GivenIRejectAClubMembershipRequestTheRequestIsSuccessful()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            
            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            var request = IntegrationTestsTestData.RejectMembershipRequestRequest;

            Should.NotThrow(async () => { await client.RejectClubMembershipRequest(bearerToken, playerId, request, CancellationToken.None).ConfigureAwait(false); });
        }
       
        protected override void SetupSubscriptionServiceConfig()
        {
            // Nothing in here
        }
    }
}
