namespace ManagementAPI.IntegrationTests.GolfClub
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using Ductus.FluentDocker.Services.Extensions;
    using MySql.Data.MySqlClient;
    using Service.Client;
    using Service.DataTransferObjects;
    using Shouldly;
    using TechTalk.SpecFlow;

    [Binding]
    [Scope(Tag = "golfclub")]
    public class GolfClubSteps : GenericSteps
    {
        #region Fields

        private Func<String, String> BaseAddressResolver;

        private readonly GolfClubTestingContext GolfClubTestingContext;

        private HttpClient HttpClient;

        #endregion

        #region Constructors

        public GolfClubSteps(ScenarioContext scenarioContext,
                             GolfClubTestingContext golfClubTestingContext) : base(scenarioContext)
        {
            this.GolfClubTestingContext = golfClubTestingContext;
        }

        #endregion

        #region Methods

        [AfterScenario]
        public void AfterScenario()
        {
            this.StopSystem();
        }

        [Given(@"a player has been registered")]
        public void GivenAPlayerHasBeenRegistered()
        {
            RegisterPlayerRequest request = IntegrationTestsTestData.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () => { await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false); });
        }

        [Given(@"I am logged in as a golf club administrator")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministrator()
        {
            this.GolfClubTestingContext.ClubAdministratorToken = await this
                                                                       .GetToken(TokenType.Password,
                                                                                 "golfhandicap.mobile",
                                                                                 "golfhandicap.mobile",
                                                                                 IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                 IntegrationTestsTestData.RegisterClubAdministratorRequest.Password)
                                                                       .ConfigureAwait(false);
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.GolfClubTestingContext.PlayerToken = await this.GetToken(TokenType.Password,
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
                                this.GolfClubTestingContext.RegisterPlayerResponse = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [Given(@"I have registered as a golf club administrator")]
        public void GivenIHaveRegisteredAsAGolfClubAdministrator()
        {
            RegisterClubAdministratorRequest request = IntegrationTestsTestData.RegisterClubAdministratorRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () => { await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false); });
        }

        [Given(@"I have the details of the new club")]
        public void GivenIHaveTheDetailsOfTheNewClub()
        {
            this.GolfClubTestingContext.CreateGolfClubRequest = IntegrationTestsTestData.CreateGolfClubRequest;
        }

        [Given(@"my golf club has been created")]
        public async Task GivenMyGolfClubHasBeenCreated()
        {
            CreateGolfClubRequest request = IntegrationTestsTestData.CreateGolfClubRequest;

            String bearerToken = this.GolfClubTestingContext.ClubAdministratorToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.GolfClubTestingContext.GolfClubId = response.GolfClubId;
        }

        [Given(@"The club I want to register for is already created")]
        public async Task GivenTheClubIWantToRegisterForIsAlreadyCreated()
        {
            CreateGolfClubRequest request = IntegrationTestsTestData.CreateGolfClubRequest;

            String bearerToken = this.GolfClubTestingContext.ClubAdministratorToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.GolfClubTestingContext.GolfClubId = response.GolfClubId;
        }

        [Given(@"the following players have registered")]
        public async Task GivenTheFollowingPlayersHaveRegistered(Table table)
        {
            this.GolfClubTestingContext.RegisteredPlayers = new Dictionary<Int32, RegisteredPlayer>();

            foreach (TableRow tableRow in table.Rows)
            {
                RegisterPlayerRequest request = new RegisterPlayerRequest
                                                {
                                                    DateOfBirth = DateTime.ParseExact(tableRow["DateOfBirth"], "dd/MM/yyyy", null),
                                                    EmailAddress = tableRow["EmailAddress"],
                                                    Gender = tableRow["Gender"],
                                                    FirstName = tableRow["FirstName"],
                                                    LastName = tableRow["LastName"],
                                                    ExactHandicap = decimal.Parse(tableRow["ExactHandicap"])
                                                };

                IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

                RegisterPlayerResponse response = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);

                this.GolfClubTestingContext.RegisteredPlayers.Add(int.Parse(tableRow["PlayerId"]),
                                                                  new RegisteredPlayer
                                                                  {
                                                                      Request = request,
                                                                      Response = response
                                                                  });
            }
        }

        [Given(@"The following players have requested membership")]
        public async Task GivenTheFollowingPlayersHaveRequestedMembership(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                RegisteredPlayer registeredPlayer = this.GolfClubTestingContext.RegisteredPlayers[int.Parse(tableRow["PlayerId"])];

                this.GolfClubTestingContext.PlayerToken = await this
                                                                .GetToken(TokenType.Password,
                                                                          "golfhandicap.mobile",
                                                                          "golfhandicap.mobile",
                                                                          registeredPlayer.Request.EmailAddress,
                                                                          "123456").ConfigureAwait(false);

                IGolfClubClient golfClubClient = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

                await golfClubClient.RequestClubMembership(this.GolfClubTestingContext.PlayerToken, this.GolfClubTestingContext.GolfClubId, CancellationToken.None)
                                    .ConfigureAwait(false);
            }
        }

        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await this.RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);

            // Setup the base address resolver
            this.BaseAddressResolver = api => $"http://127.0.0.1:{this.ManagementApiPort}";

            this.HttpClient = new HttpClient();
        }

        /// <summary>
        /// Thens a list of golf clubs will be returned.
        /// </summary>
        [Then(@"a list of golf clubs will be returned")]
        public void ThenAListOfGolfClubsWillBeReturned()
        {
            List<GetGolfClubResponse> response = this.GolfClubTestingContext.GetGolfClubListResponse;

            response.ShouldNotBeEmpty();
            response.Count.ShouldBe(1);
        }

        [Then(@"a list of '(.*)' members should be returned")]
        public void ThenAListOfMembersShouldBeReturned(Int32 memberCount)
        {
            this.GolfClubTestingContext.GolfClubMembersList.Count.ShouldBe(memberCount);
        }

        [Then(@"my registration should be successful")]
        public void ThenMyRegistrationShouldBeSuccessful()
        {
            RegisterClubAdministratorRequest request = this.GolfClubTestingContext.RegisterClubAdministratorRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () => { await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false); });
        }

        [Then(@"the golf club configuration will be created successfully")]
        public void ThenTheGolfClubConfigurationWillBeCreatedSuccessfully()
        {
            CreateGolfClubResponse response = this.GolfClubTestingContext.CreateGolfClubResponse;

            response.ShouldNotBe(null);
            response.GolfClubId.ShouldNotBe(Guid.Empty);
        }

        [Then(@"the golf club data will be returned")]
        public void ThenTheGolfClubDataWillBeReturned()
        {
            GetGolfClubResponse response = this.GolfClubTestingContext.GetGolfClubResponse;

            response.ShouldNotBe(null);
            response.Name.ShouldBe(IntegrationTestsTestData.CreateGolfClubRequest.Name);
        }

        [Then(@"the measured course is added to the club")]
        public void ThenTheMeasuredCourseIsAddedToTheClub()
        {
            AddMeasuredCourseToClubRequest request = this.GolfClubTestingContext.AddMeasuredCourseToClubRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.GolfClubTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () => { await client.AddMeasuredCourseToGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false); });
        }

        [When(@"I add a measured course to the club")]
        public void WhenIAddAMeasuredCourseToTheClub()
        {
            this.GolfClubTestingContext.AddMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
        }

        [When(@"I call Create Golf Club")]
        public async Task WhenICallCreateGolfClub()
        {
            CreateGolfClubRequest request = this.GolfClubTestingContext.CreateGolfClubRequest;

            String bearerToken = this.GolfClubTestingContext.ClubAdministratorToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            this.GolfClubTestingContext.CreateGolfClubResponse = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
        }

        [When(@"I register my details as a golf club administrator")]
        public void WhenIRegisterMyDetailsAsAGolfClubAdministrator()
        {
            this.GolfClubTestingContext.RegisterClubAdministratorRequest = IntegrationTestsTestData.RegisterClubAdministratorRequest;
        }

        [When(@"I request a list of members")]
        public async Task WhenIRequestAListOfMembers()
        {
            IGolfClubClient golfClubClient = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            this.GolfClubTestingContext.GolfClubMembersList =
                await golfClubClient.GetGolfClubMembershipList(this.GolfClubTestingContext.ClubAdministratorToken, CancellationToken.None);
        }

        [When(@"I request club membership my request is accepted")]
        public void WhenIRequestClubMembershipMyRequestIsAccepted()
        {
            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.GolfClubTestingContext.PlayerToken;
            Guid golfClubId = this.GolfClubTestingContext.GolfClubId;

            Should.NotThrow(async () => { await client.RequestClubMembership(bearerToken, golfClubId, CancellationToken.None).ConfigureAwait(false); });
        }

        [When(@"I request the details of the golf club")]
        public async Task WhenIRequestTheDetailsOfTheGolfClub()
        {
            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.GolfClubTestingContext.ClubAdministratorToken;

            this.GolfClubTestingContext.GetGolfClubResponse = await client.GetSingleGolfClub(bearerToken, CancellationToken.None).ConfigureAwait(false);
        }

        [When(@"I request the list of golf clubs")]
        public async Task WhenIRequestTheListOfGolfClubs()
        {
            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.GolfClubTestingContext.PlayerToken;

            await Retry.For(async () =>
                            {
                                this.GolfClubTestingContext.GetGolfClubListResponse =
                                    await client.GetGolfClubList(bearerToken, CancellationToken.None).ConfigureAwait(false);

                                if (this.GolfClubTestingContext.GetGolfClubListResponse.Count == 0)
                                {
                                    throw new Exception("Empty Club List");
                                }
                            });
        }
        
        #endregion
    }
}