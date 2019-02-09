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

namespace ManagementAPI.IntegrationTests.Tournament
{
    [Binding]
    [Scope(Tag = "tournament")]
    public class TournamentSteps: GenericSteps
    {
        private Func<String, String> baseAddressResolver;
        private HttpClient httpClient = null;

        public TournamentSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {
            // Nothing here
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
        
        [Given(@"I have registered as a golf club administrator")]
        public void GivenIHaveRegisteredAsAGolfClubAdministrator()
        {
            RegisterClubAdministratorRequest request =  IntegrationTestsTestData.RegisterClubAdministratorRequest;

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
            CreateGolfClubRequest request = IntegrationTestsTestData.CreateGolfClubRequest;

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.ScenarioContext["GolfClubId"] = response.GolfClubId;

            if (this.ScenarioContext.ScenarioInfo.Title == "Get Golf Club List")
            {
                Thread.Sleep(30000);
            }
        }
        
        [Given(@"a measured course is added to the club")]
        public void GivenAMeasuredCourseIsAddedToTheClub()
        {
            AddMeasuredCourseToClubRequest request = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;

            IGolfClubClient client = new GolfClubClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                await client.AddMeasuredCourseToGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
            });  
        }
        
        [Given(@"I have the details of the new tournament")]
        public void GivenIHaveTheDetailsOfTheNewTournament()
        {
            this.ScenarioContext["CreateTournamentRequest"] = IntegrationTestsTestData.CreateTournamentRequest;
        }
        
        [When(@"I call Create Tournament")]
        public void WhenICallCreateTournament()
        {
            CreateTournamentRequest request = this.ScenarioContext.Get<CreateTournamentRequest>("CreateTournamentRequest");

            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                this.ScenarioContext["CreateTournamentResponse"] = await client.CreateTournament(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
            });  
        }
        
        [Then(@"the tournament will be created")]
        public void ThenTheTournamentWillBeCreated()
        {
            CreateTournamentResponse response = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            response.ShouldNotBeNull();
            response.TournamentId.ShouldNotBe(Guid.Empty);
        }

        [Given(@"I have created a tournament")]
        public void GivenIHaveCreatedATournament()
        {
            CreateTournamentRequest request = IntegrationTestsTestData.CreateTournamentRequest;

            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                this.ScenarioContext["CreateTournamentResponse"] = await client.CreateTournament(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
            });  
        }
        
        [Given(@"a player has been registered")]
        public void GivenAPlayerHasBeenRegistered()
        {
            RegisterPlayerRequest request = IntegrationTestsTestData.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.baseAddressResolver, this.httpClient);

            Should.NotThrow( async () =>
            {
                await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.ScenarioContext["PlayerToken"] = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress,
                "123456").ConfigureAwait(false);
        }
        
        [When(@"a player records their score")]
        public void WhenAPlayerRecordsTheirScore()
        {
            this.ScenarioContext["RecordMemberTournamentScoreRequest"] = IntegrationTestsTestData.RecordMemberTournamentScoreRequest;            
        }
        
        [Then(@"the score is recorded against the tournament")]
        public void ThenTheScoreIsRecordedAgainstTheTournament()
        {
            RecordMemberTournamentScoreRequest request = this.ScenarioContext.Get<RecordMemberTournamentScoreRequest>("RecordMemberTournamentScoreRequest");

            CreateTournamentResponse createTournamentResponse = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            Should.NotThrow(async () =>
            {
                await client.RecordPlayerScore(bearerToken, createTournamentResponse.TournamentId, request, CancellationToken.None).ConfigureAwait(false);
            }); 
        }
        
        [Given(@"some scores have been recorded")]
        public void GivenSomeScoresHaveBeenRecorded()
        {
            RecordMemberTournamentScoreRequest request = IntegrationTestsTestData.RecordMemberTournamentScoreRequest;

            CreateTournamentResponse createTournamentResponse = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            String bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            Should.NotThrow(async () =>
            {
                await client.RecordPlayerScore(bearerToken, createTournamentResponse.TournamentId, request, CancellationToken.None).ConfigureAwait(false);
            }); 
        }
        
        [When(@"I request to complete the tournament the tournament is completed")]
        public void WhenIRequestToCompleteTheTournamentTheTournamentIsCompleted()
        {
            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            CreateTournamentResponse createTournamentResponse = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                await client
                    .CompleteTournament(bearerToken, createTournamentResponse.TournamentId, CancellationToken.None)
                    .ConfigureAwait(false);
            });
        }

        [When(@"I request to cancel the tournament the tournament is cancelled")]
        public void WhenIRequestToCancelTheTournamentTheTournamentIsCancelled()
        {
            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            CreateTournamentResponse createTournamentResponse = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            CancelTournamentRequest cancelTournamentRequest = IntegrationTestsTestData.CancelTournamentRequest;

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                await client
                    .CancelTournament(bearerToken, createTournamentResponse.TournamentId, cancelTournamentRequest, CancellationToken.None)
                    .ConfigureAwait(false);
            });
        }

        [Given(@"I have completed the tournament")]
        public void GivenIHaveCompletedTheTournament()
        {
            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            CreateTournamentResponse createTournamentResponse = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                await client
                    .CompleteTournament(bearerToken, createTournamentResponse.TournamentId, CancellationToken.None)
                    .ConfigureAwait(false);
            });
        }
        
        [When(@"I request to produce a tournament result the results are produced")]
        public void WhenIRequestToProduceATournamentResultTheResultsAreProduced()
        {
            ITournamentClient client = new TournamentClient(this.baseAddressResolver, this.httpClient);

            CreateTournamentResponse createTournamentResponse = this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Should.NotThrow(async () =>
            {
                await client
                    .ProduceTournamentResult(bearerToken, createTournamentResponse.TournamentId, CancellationToken.None)
                    .ConfigureAwait(false);
            });
        }
        
        protected override void SetupSubscriptionServiceConfig()
        {
            // Nothing here
        }
    }
}
