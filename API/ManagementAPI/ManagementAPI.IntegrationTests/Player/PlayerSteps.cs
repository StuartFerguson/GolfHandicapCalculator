namespace ManagementAPI.IntegrationTests.Player
{
    using System;
    using System.Linq;
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
                                                                                   "golfhandicap.mobile",
                                                                                   "golfhandicap.mobile",
                                                                                   IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                   IntegrationTestsTestData.RegisterClubAdministratorRequest.Password)
                                                                         .ConfigureAwait(false);
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.PlayerTestingContext.PlayerToken = await this.GetToken(TokenType.Password,
                                                                        "golfhandicap.mobile",
                                                                        "golfhandicap.mobile",
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

            Retry.For(async () =>
                      {
                          var passwordToken = this.PlayerTestingContext.PlayerToken;
                          var golfClubList = await client.GetGolfClubList(passwordToken, CancellationToken.None);

                          if (golfClubList.All(g => g.Id != response.GolfClubId))
                          {
                              throw new Exception("Golf Club not found in read model");
                          }
                      });

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

        #endregion
    }
}