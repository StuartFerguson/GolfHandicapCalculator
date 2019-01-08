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
            // Nothing in here
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

            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/Player";

            this.ScenarioContext["RegisterPlayerHttpResponse"] = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
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
            var responseData = await GetResponseObject<RegisterPlayerResponse>("RegisterPlayerHttpResponse").ConfigureAwait(false);

            responseData.PlayerId.ShouldNotBe(Guid.Empty);
        }

        protected override void SetupSubscriptionServiceConfig()
        {
            // Nothing in here
        }
        
        [Given(@"I am registered as a player")]
        public async Task GivenIAmRegisteredAsAPlayer()
        {
            var request = IntegrationTestsTestData.RegisterPlayerRequest;
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/Player";

            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
            var responseData = await GetResponseObject<RegisterPlayerResponse>(httpResponse).ConfigureAwait(false);

            this.ScenarioContext["PlayerId"] = responseData.PlayerId;
        }
        
        [Given(@"The club I want to register for is already created")]
        public async Task GivenTheClubIWantToRegisterForIsAlreadyCreated()
        {
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;
            String requestUri = $"http://127.0.0.1:{this.ManagementApiPort}/api/ClubConfiguration";

            var httpResponse = await MakeHttpPost(requestUri, request).ConfigureAwait(false);
            var responseData = await GetResponseObject<CreateGolfClubResponse>(httpResponse).ConfigureAwait(false);

            this.ScenarioContext["ClubId"] = responseData.ClubConfigurationId;
        }
        
        [When(@"I request club membership")]
        public async Task WhenIRequestClubMembership()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            Guid clubId = this.ScenarioContext.Get<Guid>("ClubId");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Player/{playerId}/ClubMembershipRequest/{clubId}";

            String bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            Object resquestObject = null;
            this.ScenarioContext["ClubMembershipRequestHttpResponse"] = await MakeHttpPut(requestUri, resquestObject,bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"my request is successful")]
        public void ThenMyRequestIsSuccessful()
        {
            HttpResponseMessage httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("ClubMembershipRequestHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Given(@"I am logged in as a player")]
        public async Task GivenIAmLoggedInAsAPlayer()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                IntegrationTestsTestData.RegisterPlayerRequest.EmailAddress, "123456").ConfigureAwait(false);

            this.ScenarioContext["PlayerToken"] = tokenResponse;
        }

        [Given(@"I have requested a club membership")]
        public async Task GivenIHaveRequestedAClubMembership()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            Guid clubId = this.ScenarioContext.Get<Guid>("ClubId");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Player/{playerId}/ClubMembershipRequest/{clubId}";

            String bearerToken = this.ScenarioContext.Get<String>("PlayerToken");

            Object resquestObject = null;
            await MakeHttpPut(requestUri, resquestObject,bearerToken).ConfigureAwait(false);
        }
        
        [Given(@"I am logged in as a club administrator")]
        public async Task GivenIAmLoggedInAsAClubAdministrator()
        {
            var tokenResponse = await GetToken(TokenType.Password, "integrationTestClient", "integrationTestClient",
                "clubadministrator@test.co.uk", "123456").ConfigureAwait(false);

            this.ScenarioContext["ClubAdministratorToken"] = tokenResponse;
        }
        
        [Given(@"I approve a club membership request")]
        public async Task GivenIApproveAClubMembershipRequest()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            Guid clubId = this.ScenarioContext.Get<Guid>("ClubId");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Player/{playerId}/ClubMembershipRequest/{clubId}/Approve";

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            Object resquestObject = null;
            this.ScenarioContext["ClubMembershipRequestApprovalHttpResponse"] = await MakeHttpPut(requestUri, resquestObject,bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"my approval request is successful")]
        public void ThenMyApprovalRequestIsSuccessful()
        {
            HttpResponseMessage httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("ClubMembershipRequestApprovalHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Given(@"I reject a club membership request")]
        public async Task GivenIRejectAClubMembershipRequest()
        {
            Guid playerId = this.ScenarioContext.Get<Guid>("PlayerId");
            Guid clubId = this.ScenarioContext.Get<Guid>("ClubId");

            String requestUri =
                $"http://127.0.0.1:{this.ManagementApiPort}/api/Player/{playerId}/ClubMembershipRequest/{clubId}/Approve";

            String bearerToken = this.ScenarioContext.Get<String>("ClubAdministratorToken");

            RejectMembershipRequestRequest requestObject = new RejectMembershipRequestRequest
            {
                RejectionReason = "Rejected"
            };

            this.ScenarioContext["ClubMembershipRequestRejectionHttpResponse"] = await MakeHttpPut(requestUri, requestObject,bearerToken).ConfigureAwait(false);
        }
        
        [Then(@"my rejection request is successful")]
        public void ThenMyRejectionRequestIsSuccessful()
        {
            HttpResponseMessage httpResponse = this.ScenarioContext.Get<HttpResponseMessage>("ClubMembershipRequestRejectionHttpResponse");

            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK);
        }

    }
}
