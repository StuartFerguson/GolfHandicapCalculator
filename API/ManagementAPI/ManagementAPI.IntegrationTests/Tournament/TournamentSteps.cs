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
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Ductus.FluentDocker.Services.Extensions;
    using GolfClub;
    using Microsoft.VisualStudio.TestPlatform.Common;
    using MySql.Data.MySqlClient;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;

    [Binding]
    [Scope(Tag = "tournament")]
    public class TournamentSteps: GenericSteps
    {
        private Func<String, String> BaseAddressResolver;
        private HttpClient HttpClient = null;

        private readonly TournamentTestingContext TournamentTestingContext;

        public TournamentSteps(ScenarioContext scenarioContext, TournamentTestingContext tournamentTestingContext) : base(scenarioContext)
        {
            this.TournamentTestingContext = tournamentTestingContext;
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
            RegisterClubAdministratorRequest request =  IntegrationTestsTestData.RegisterClubAdministratorRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow( async () =>
            {
                await client.RegisterGolfClubAdministrator(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Given(@"I am logged in as a golf club administrator")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministrator()
        {
            this.TournamentTestingContext.ClubAdministratorToken = await GetToken(TokenType.Password, "golfhandicap.mobile", "golfhandicap.mobile",
                IntegrationTestsTestData.RegisterClubAdministratorRequest.EmailAddress,
                IntegrationTestsTestData.RegisterClubAdministratorRequest.Password).ConfigureAwait(false);
        }
        
        [Given(@"my golf club has been created")]
        public async Task GivenMyGolfClubHasBeenCreated()
        {
            CreateGolfClubRequest request = IntegrationTestsTestData.CreateGolfClubRequest;

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);

            this.TournamentTestingContext.GolfClubId = response.GolfClubId;
        }
        
        [Given(@"a measured course is added to the club")]
        public void GivenAMeasuredCourseIsAddedToTheClub()
        {
            AddMeasuredCourseToClubRequest request = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
            {
                await client.AddMeasuredCourseToGolfClub(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
            });  
        }
        
        [Given(@"I have the details of the new tournament")]
        public void GivenIHaveTheDetailsOfTheNewTournament()
        {
            this.TournamentTestingContext.CreateTournamentRequest = IntegrationTestsTestData.CreateTournamentRequest;
        }
        
        [When(@"I call Create Tournament")]
        public void WhenICallCreateTournament()
        {
            CreateTournamentRequest request = this.TournamentTestingContext.CreateTournamentRequest;

            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
            {
                this.TournamentTestingContext.CreateTournamentResponse = await client.CreateTournament(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
            });  
        }
        
        [Then(@"the tournament will be created")]
        public void ThenTheTournamentWillBeCreated()
        {
            CreateTournamentResponse response = this.TournamentTestingContext.CreateTournamentResponse;

            response.ShouldNotBeNull();
            response.TournamentId.ShouldNotBe(Guid.Empty);
        }

        [Given(@"I have created a tournament")]
        public void GivenIHaveCreatedATournament()
        {
            CreateTournamentRequest request = IntegrationTestsTestData.CreateTournamentRequest;

            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
            {
                this.TournamentTestingContext.CreateTournamentResponse = await client.CreateTournament(bearerToken, request, CancellationToken.None).ConfigureAwait(false);
            });  
        }
        
        [Given(@"a player has been registered")]
        public void GivenAPlayerHasBeenRegistered()
        {
            RegisterPlayerRequest request = IntegrationTestsTestData.RegisterPlayerRequest;

            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            Should.NotThrow( async () =>
            {
                await client.RegisterPlayer(request, CancellationToken.None).ConfigureAwait(false);
            });
        }
        
        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            this.TournamentTestingContext.PlayerToken = await GetToken(TokenType.Password, "golfhandicap.mobile", "golfhandicap.mobile",
                IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress,
                "123456").ConfigureAwait(false);
        }
        
        [When(@"a player records their score")]
        public void WhenAPlayerRecordsTheirScore()
        {
            this.TournamentTestingContext.RecordPlayerTournamentScoreRequest = IntegrationTestsTestData.RecordPlayerTournamentScoreRequest;            
        }
        
        [Then(@"the score is recorded against the tournament")]
        public void ThenTheScoreIsRecordedAgainstTheTournament()
        {
            RecordPlayerTournamentScoreRequest request = this.TournamentTestingContext.RecordPlayerTournamentScoreRequest;

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.TournamentTestingContext.PlayerToken;

            Should.NotThrow(async () =>
            {
                await client.RecordPlayerScore(bearerToken, createTournamentResponse.TournamentId, request, CancellationToken.None).ConfigureAwait(false);
            }); 
        }
        
        [Given(@"I have signed in to play the tournament")]
        public async Task GivenIHaveSignedInToPlayTheTournament()
        {
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            String bearerToken = this.TournamentTestingContext.PlayerToken;

            await client.SignUpPlayerForTournament(bearerToken, createTournamentResponse.TournamentId, CancellationToken.None).ConfigureAwait(false);
        }

        [Given(@"I am requested membership of the golf club")]
        public async Task  GivenIAmRequestedMembershipOfTheGolfClub()
        {
            String bearerToken = this.TournamentTestingContext.PlayerToken;

            IGolfClubClient client = new GolfClubClient(this.BaseAddressResolver, this.HttpClient);

            await client.RequestClubMembership(bearerToken, this.TournamentTestingContext.GolfClubId, CancellationToken.None).ConfigureAwait(false);
        }
        
        [Given(@"my membership has been accepted")]
        public async Task GivenMyMembershipHasBeenAccepted()
        {
            IPlayerClient client = new PlayerClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.TournamentTestingContext.PlayerToken;
            
            await Retry.For(async () =>
                            {

                                List<ClubMembershipResponse> response = await client.GetPlayerMemberships(bearerToken, CancellationToken.None).ConfigureAwait(false);

                                if (response.All(r => r.GolfClubId != this.TournamentTestingContext.GolfClubId))
                                {
                                    throw new Exception("Not a member of Golf Club");
                                }
                            });    
        }
        
        [Given(@"some scores have been recorded")]
        public void GivenSomeScoresHaveBeenRecorded()
        {
            RecordPlayerTournamentScoreRequest request = IntegrationTestsTestData.RecordPlayerTournamentScoreRequest;

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            String bearerToken = this.TournamentTestingContext.PlayerToken;

            Should.NotThrow(async () =>
            {
                await client.RecordPlayerScore(bearerToken, createTournamentResponse.TournamentId, request, CancellationToken.None).ConfigureAwait(false);
            }); 
        }
        
        [When(@"I request to complete the tournament the tournament is completed")]
        public void WhenIRequestToCompleteTheTournamentTheTournamentIsCompleted()
        {
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

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
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            CancelTournamentRequest cancelTournamentRequest = IntegrationTestsTestData.CancelTournamentRequest;

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

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
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

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
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            String bearerToken = this.TournamentTestingContext.ClubAdministratorToken;

            Should.NotThrow(async () =>
            {
                await client
                    .ProduceTournamentResult(bearerToken, createTournamentResponse.TournamentId, CancellationToken.None)
                    .ConfigureAwait(false);
            });
        }
        
        [Given(@"I sign up to play the tournament")]
        public void GivenISignUpToPlayTheTournament()
        {
            ITournamentClient client = new TournamentClient(this.BaseAddressResolver, this.HttpClient);

            CreateTournamentResponse createTournamentResponse = this.TournamentTestingContext.CreateTournamentResponse;

            String bearerToken = this.TournamentTestingContext.PlayerToken;

            Should.NotThrow(async () =>
                            {
                                await client.SignUpPlayerForTournament(bearerToken, createTournamentResponse.TournamentId, CancellationToken.None).ConfigureAwait(false);
                            });
        }
        
        [Then(@"I am recorded as signed up")]
        public void ThenIAmRecordedAsSignedUp()
        {
            // Nothing to check here at the moment
        }


    }
}
