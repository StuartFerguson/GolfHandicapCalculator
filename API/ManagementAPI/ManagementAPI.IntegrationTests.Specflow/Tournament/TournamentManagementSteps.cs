using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ductus.FluentDocker.Services.Extensions;
using ManagementAPI.IntegrationTests.Specflow.Common;
using ManagementAPI.Service.DataTransferObjects;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Shouldly;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Specflow.Tournament
{
    [Binding]
    [Scope(Tag = "tournamentmanagement")]
    public class TournamentManagementSteps: GenericSteps
    {
        public TournamentManagementSteps(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }
        
        [Given(@"The Golf Handicapping System Is Running")]
        public async Task GivenTheGolfHandicappingSystemIsRunning()
        {
            await RunSystem(this.ScenarioContext.ScenarioInfo.Title).ConfigureAwait(false);
        }

        [AfterScenario()]
        public void AfterScenario()
        {
            StopSystem();
        }
        
        [Given(@"My Club configuration has been already created")]
        public async Task GivenMyClubConfigurationHasBeenAlreadyCreated()
        {
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
            var responseData = await GetResponseObject<CreateClubConfigurationResponse>(httpResponse);

            this.ScenarioContext["ClubConfigurationId"] = responseData.ClubConfigurationId;            
        }
        
        [Given(@"the club has a measured course")]
        public async Task GivenTheClubHasAMeasuredCourse()
        {
            var clubConfigurationId = this.ScenarioContext.Get<Guid>("ClubConfigurationId");

            var addMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
            addMeasuredCourseToClubRequest.ClubAggregateId = clubConfigurationId;

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            var httpResponse = await MakeHttpPut(requestUri, addMeasuredCourseToClubRequest, bearerToken).ConfigureAwait(false);

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);            
        }
        
        [Given(@"I have the details of the new tournament")]
        public void GivenIHaveTheDetailsOfTheNewTournament()
        {
            var clubConfigurationId = this.ScenarioContext.Get<Guid>("ClubConfigurationId");
            var addMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;

            var createTournamentRequest = IntegrationTestsTestData.CreateTournamentRequest;
            createTournamentRequest.ClubConfigurationId = clubConfigurationId;
            createTournamentRequest.MeasuredCourseId = addMeasuredCourseToClubRequest.MeasuredCourseId;

            this.ScenarioContext["CreateTournamentRequest"] = createTournamentRequest;
        }
        
        [When(@"I call Create Tournament")]
        public async Task WhenICallCreateTournament()
        {
            var createTournamentRequest = this.ScenarioContext.Get<CreateTournamentRequest>("CreateTournamentRequest");

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament";

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            this.ScenarioContext["CreateTournamentHttpResponse"] = await MakeHttpPost(requestUri, createTournamentRequest, bearerToken).ConfigureAwait(false);            
        }
        
        [Then(@"the tournament will be created")]
        public void ThenTheTournamentWillBeCreated()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("CreateTournamentHttpResponse");
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }
        
        [Then(@"I will get the new Tournament Id in the response")]
        public async Task ThenIWillGetTheNewTournamentIdInTheResponse()
        {
            var responseData = await GetResponseObject<CreateTournamentResponse>("CreateTournamentHttpResponse");

            responseData.TournamentId.ShouldNotBe(Guid.Empty);
        }
        
        [Given(@"I have created a tournament")]
        public async Task GivenIHaveCreatedATournament()
        {
            var clubConfigurationId = this.ScenarioContext.Get<Guid>("ClubConfigurationId");
            var addMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;

            var createTournamentRequest = IntegrationTestsTestData.CreateTournamentRequest;
            createTournamentRequest.ClubConfigurationId = clubConfigurationId;
            createTournamentRequest.MeasuredCourseId = addMeasuredCourseToClubRequest.MeasuredCourseId;

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament";

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            var httpResponse = await MakeHttpPost(requestUri, createTournamentRequest, bearerToken).ConfigureAwait(false);

            var createTournamentResponseData = await GetResponseObject<CreateTournamentResponse>(httpResponse).ConfigureAwait(false);

            this.ScenarioContext["CreateTournamentResponse"] = createTournamentResponseData;
        }
        
        [When(@"a member records their score")]
        public async Task WhenAMemberRecordsTheirScore()
        {
            var createTournamentResponseData =
                this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            var recordMemberTournamentScoreRequest = IntegrationTestsTestData.RecordMemberTournamentScoreRequest;

            var bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament/{createTournamentResponseData.TournamentId}/RecordMemberScore";
            this.ScenarioContext["RecordMemberTournamentScoreHttpResponse"] = await MakeHttpPut(requestUri, recordMemberTournamentScoreRequest, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"the score is recorded against the tournament")]
        public void ThenTheScoreIsRecordedAgainstTheTournament()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("RecordMemberTournamentScoreHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [When(@"I request to cancel the tournament")]
        public async Task WhenIRequestToCancelTheTournament()
        {
            var createTournamentResponseData =
                this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            var cancelTournamentRequest = IntegrationTestsTestData.CancelTournamentRequest;

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament/{createTournamentResponseData.TournamentId}/Cancel";

            this.ScenarioContext["CancelTournamentHttpResponse"] = await MakeHttpPut(requestUri, cancelTournamentRequest, bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"the tournament is cancelled")]
        public void ThenTheTournamentIsCancelled()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("CancelTournamentHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Given(@"some scores have been recorded")]
        public async Task GivenSomeScoresHaveBeenRecorded()
        {
            for (Int32 i = 0; i < 5; i++)
            {
                var createTournamentResponseData =
                    this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

                var recordMemberTournamentScoreRequest = IntegrationTestsTestData.RecordMemberTournamentScoreRequest;
                recordMemberTournamentScoreRequest.MemberId = Guid.NewGuid();

                var bearerToken = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                    "player@test.co.uk", "123456").ConfigureAwait(false);

                String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament/{createTournamentResponseData.TournamentId}/RecordMemberScore";
                var httpResponse = await MakeHttpPut(requestUri, recordMemberTournamentScoreRequest, bearerToken).ConfigureAwait(false);

                httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
            }
        }
        
        [When(@"I request to complete the tournament")]
        public async Task WhenIRequestToCompleteTheTournament()
        {
            var createTournamentResponseData =
                this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament/{createTournamentResponseData.TournamentId}/Complete";

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Object requestObject = null;
            this.ScenarioContext["CompleteTournamentHttpResponse"] = await MakeHttpPut(requestUri, requestObject, bearerToken).ConfigureAwait(false);            
        }
        
        [Then(@"the tournament is completed")]
        public void ThenTheTournamentIsCompleted()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("CompleteTournamentHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [Given(@"the tournament is completed")]
        public async Task GivenTheTournamentIsCompleted()
        {
            var createTournamentResponseData =
                this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament/{createTournamentResponseData.TournamentId}/Complete";
            
            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Object requestObject = null;
            var httpResponse  = await MakeHttpPut(requestUri, requestObject, bearerToken).ConfigureAwait(false);  
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }
        
        [When(@"I request to produce a tournament result")]
        public async Task WhenIRequestToProduceATournamentResult()
        {
            var createTournamentResponseData =
                this.ScenarioContext.Get<CreateTournamentResponse>("CreateTournamentResponse");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Tournament/{createTournamentResponseData.TournamentId}/ProduceResult";

            var bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Object requestObject = null;
            this.ScenarioContext["ProduceResultHttpResponse"] = await MakeHttpPut(requestUri, requestObject,bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"the results are produced")]
        public void ThenTheResultsAreProduced()
        {
            var httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("ProduceResultHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        protected override void SetupSubscriptionServiceConfig()
        {            
            // Nothing in here
        }

        [Given(@"I am logged in as a club administrator")]
        public async Task GivenIAmLoggedInAsAClubAdministrator()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.CreateClubConfigurationRequest.EmailAddress, "123456").ConfigureAwait(false);

            this.ScenarioContext["ClubAdministratorToken"] = tokenResponse;
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                "player@test.co.uk", "123456").ConfigureAwait(false);

            this.ScenarioContext["PlayerToken"] = tokenResponse;
        }


    }
}
