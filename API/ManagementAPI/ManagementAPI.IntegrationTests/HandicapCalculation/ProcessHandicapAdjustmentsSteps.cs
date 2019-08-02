using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.HandicapCalculation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using GolfClub;
    using Service.Client;
    using Service.DataTransferObjects;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Shouldly;

    [Binding]
    public class ProcessHandicapAdjustmentsSteps : GenericSteps
    {
        private Func<String, String> BaseAddressResolver;
        private HttpClient HttpClient = null;

        private readonly HandicapCalculationTestingContext HandicapCalculationTestingContext;

        public ProcessHandicapAdjustmentsSteps(ScenarioContext scenarioContext, HandicapCalculationTestingContext handicapCalculationTestingContext) : base(scenarioContext)
        {
            this.HandicapCalculationTestingContext = handicapCalculationTestingContext;
        }

        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await this.RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);

            // Setup the base address resolver
            this.BaseAddressResolver = (api) => $"http://127.0.0.1:{this.ManagementApiPort}";

            this.HttpClient = new HttpClient();
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            this.StopSystem();
        }

        [Given(@"I have registered as a golf club administrator")]
        public void GivenIHaveRegisteredAsAGolfClubAdministrator()
        {
            RegisterClubAdministratorRequest request = IntegrationTestsTestData.RegisterClubAdministratorRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () =>
                            {
                                await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false);
                            });
        }
        
        [Given(@"I am logged in as a golf club administrator")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministrator()
        {
            this.HandicapCalculationTestingContext.ClubAdministratorToken = await GetToken(TokenType.Password, "golfhandicap.mobile", "golfhandicap.mobile",
                                                                                  IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                                                                                  IntegrationTestsTestData.RegisterClubAdministratorRequest.Password).ConfigureAwait(false);
        }
        
        [Given(@"my golf club has been created")]
        public async Task GivenMyGolfClubHasBeenCreated()
        {
            CreateGolfClubRequest request = IntegrationTestsTestData.CreateGolfClubRequest;

            String bearerToken = this.HandicapCalculationTestingContext.ClubAdministratorToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.HandicapCalculationTestingContext.GolfClubId = response.GolfClubId;
        }
        
        [Given(@"a measured course is added to the club")]
        public void GivenAMeasuredCourseIsAddedToTheClub()
        {
            AddMeasuredCourseToClubRequest request = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.HandicapCalculationTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
                            {
                                await client.AddMeasuredCourseToGolfClub(bearerToken, this.HandicapCalculationTestingContext.GolfClubId, request, CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [Given(@"a player has been registered with an exact handicap of (.*)")]
        public void GivenAPlayerHasBeenRegisteredWithAnExactHandicapOf(Decimal exactHandicap)
        {
            RegisterPlayerRequest request = IntegrationTestsTestData.RegisterPlayerRequest;
            request.ExactHandicap = exactHandicap;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () =>
                            {
                                this.HandicapCalculationTestingContext.RegisterPlayerResponse = await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
                            });
        }
        
        [Given(@"I have created a tournament")]
        public void GivenIHaveCreatedATournament()
        {
            CreateTournamentRequest request = IntegrationTestsTestData.CreateTournamentRequest;

            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.HandicapCalculationTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
                            {
                                this.HandicapCalculationTestingContext.CreateTournamentResponse = await client.CreateTournament(bearerToken, this.HandicapCalculationTestingContext.GolfClubId, request, CancellationToken.None).ConfigureAwait(false);
                            });
        }
        
        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.HandicapCalculationTestingContext.PlayerToken = await GetToken(TokenType.Password, "golfhandicap.mobile", "golfhandicap.mobile",
                                                                       IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress,
                                                                       "123456").ConfigureAwait(false);
        }
        
        [Given(@"I am requested membership of the golf club")]
        public async Task GivenIAmRequestedMembershipOfTheGolfClub()
        {
            String bearerToken = this.HandicapCalculationTestingContext.PlayerToken;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            await client.RequestClubMembership(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, this.HandicapCalculationTestingContext.GolfClubId, CancellationToken.None).ConfigureAwait(false);
        }
        
        [Given(@"my membership has been accepted")]
        public async Task GivenMyMembershipHasBeenAccepted()
        {
            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.HandicapCalculationTestingContext.PlayerToken;

            await Retry.For(async () =>
                            {
                                List<ClubMembershipResponse> response = await client.GetPlayerMemberships(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, CancellationToken.None).ConfigureAwait(false);

                                if (response.All(r => r.GolfClubId != this.HandicapCalculationTestingContext.GolfClubId))
                                {
                                    throw new Exception("Not a member of Golf Club");
                                }
                            });
        }
        
        [Given(@"I have signed in to play the tournament")]
        public async Task GivenIHaveSignedInToPlayTheTournament()
        {
            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.HandicapCalculationTestingContext.CreateTournamentResponse;

            String bearerToken = this.HandicapCalculationTestingContext.PlayerToken;

            await client.SignUpPlayerForTournament(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, createTournamentResponse.TournamentId, CancellationToken.None).ConfigureAwait(false);
        }
        
        [Given(@"my score of (.*) shots below the CSS has been recorded")]
        public async Task GivenMyScoreOfShotsBelowTheCSSHasBeenRecorded(Int32 strokesBelowCSS)
        {
            IPlayerClient playerClient = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.HandicapCalculationTestingContext.PlayerToken;

            GetPlayerDetailsResponse player = await playerClient.GetPlayer(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, CancellationToken.None);

            RecordPlayerTournamentScoreRequest request =
                IntegrationTestsTestData.GetScoreToRecord(player.PlayingHandicap, strokesBelowCSS);

            CreateTournamentResponse createTournamentResponse = this.HandicapCalculationTestingContext.CreateTournamentResponse;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow(async () =>
                            {
                                await client.RecordPlayerScore(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, createTournamentResponse.TournamentId, request, CancellationToken.None).ConfigureAwait(false);
                            });
        }
        
        [When(@"I request to complete the tournament the tournament is completed")]
        public void WhenIRequestToCompleteTheTournamentTheTournamentIsCompleted()
        {
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.HandicapCalculationTestingContext.CreateTournamentResponse;

            String bearerToken = this.HandicapCalculationTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
                            {
                                await client
                                      .CompleteTournament(bearerToken, this.HandicapCalculationTestingContext.GolfClubId, createTournamentResponse.TournamentId, CancellationToken.None)
                                      .ConfigureAwait(false);
                            });
        }
        
        [When(@"I start the handicap calculation process")]
        public async Task WhenIStartTheHandicapCalculationProcess()
        {
            String uri = $"{this.BaseAddressResolver(String.Empty)}/api/HandicapCalculation?tournamentId={this.HandicapCalculationTestingContext.CreateTournamentResponse.TournamentId}";

            HttpResponseMessage response = await this.HttpClient.PostAsync(uri, new StringContent(String.Empty, Encoding.UTF8, "application/json"), CancellationToken.None).ConfigureAwait(false);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
        
        [Then(@"the process completes successfully")]
        public async Task ThenTheProcessCompletesSuccessfully()
        {
            await Retry.For(async () =>
                            {
                                String uri =
                                    $"{this.BaseAddressResolver(String.Empty)}/api/HandicapCalculation?tournamentId={this.HandicapCalculationTestingContext.CreateTournamentResponse.TournamentId}";

                                HttpResponseMessage response = await this.HttpClient.GetAsync(uri, CancellationToken.None).ConfigureAwait(false);

                                response.StatusCode.ShouldBe(HttpStatusCode.OK);

                                String responseData = await response.Content.ReadAsStringAsync();

                                responseData.Contains("Completed").ShouldBeTrue();
                            });
        }
        
        [Then(@"my new playing handicap is adjusted to (.*)")]
        public async Task ThenMyNewPlayingHandicapIsAdjustedTo(Int32 playingHandicap)
        {
            IPlayerClient playerClient = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.HandicapCalculationTestingContext.PlayerToken;

            GetPlayerDetailsResponse player = await playerClient.GetPlayer(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, CancellationToken.None).ConfigureAwait(false);

            player.PlayingHandicap.ShouldBe(playingHandicap);
        }
        
        [Then(@"my new exact handicap is adjusted to (.*)")]
        public async Task ThenMyNewExactHandicapIsAdjustedTo(Decimal exactHandicap)
        {
            IPlayerClient playerClient = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.HandicapCalculationTestingContext.PlayerToken;

            GetPlayerDetailsResponse player = await playerClient.GetPlayer(bearerToken, this.HandicapCalculationTestingContext.RegisterPlayerResponse.PlayerId, CancellationToken.None).ConfigureAwait(false);

            player.ExactHandicap.ShouldBe(exactHandicap);
        }
    }
}
