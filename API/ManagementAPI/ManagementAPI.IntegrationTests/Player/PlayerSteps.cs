using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using GolfClub;
    using Service.Client;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Shouldly;
    using ClubMembershipResponse = Service.DataTransferObjects.Responses.v2.ClubMembershipResponse;
    using RegisterPlayerResponse = Service.DataTransferObjects.Responses.v2.RegisterPlayerResponse;

    [Binding]
    [Scope(Tag = "player")]
    public class PlayerSteps
    {
        private readonly TestingContext TestingContext;
        
        public PlayerSteps(ScenarioContext scenarioContext, TestingContext testingContext)
        {
            this.TestingContext = testingContext;
        }

        [Given(@"I register the following details for a player")]
        public void GivenIRegisterTheFollowingDetailsForAPlayer(Table table)
        {
            TableRow tableRow = table.Rows.Single();

            RegisterPlayerRequest registerPlayerRequest = new RegisterPlayerRequest
                                                          {
                                                              DateOfBirth = DateTime.ParseExact(tableRow["DateOfBirth"], "yyyy-MM-dd", null),
                                                              FamilyName = tableRow["FamilyName"],
                                                              GivenName = tableRow["GivenName"],
                                                              MiddleName = tableRow["MiddleName"],
                                                              EmailAddress = tableRow["EmailAddress"],
                                                              ExactHandicap = Decimal.Parse(tableRow["ExactHandicap"]),
                                                              Gender = tableRow["Gender"]
                                                          };

            this.TestingContext.RegisterPlayerRequest = registerPlayerRequest;
            this.TestingContext.RegisterPlayerRequests.Add(tableRow["PlayerNumber"], registerPlayerRequest);
        }

        [Then(@"the player registration for player number (.*) should be successful")]
        public void ThenThePlayerRegistrationForPlayerNumberShouldBeSuccessful(String playerNumber)
        {
            Should.NotThrow(async () =>
                            {
                                RegisterPlayerResponse registerPlayerResponse = await this.TestingContext.DockerHelper.PlayerClient.RegisterPlayer(this.TestingContext.RegisterPlayerRequest,
                                                                                        CancellationToken.None).ConfigureAwait(false);

                                this.TestingContext.RegisterPlayerResponse = registerPlayerResponse;
                                this.TestingContext.RegisterPlayerResponses.Add(playerNumber, registerPlayerResponse);
                            });
        }
        
        [When(@"I request the player details for player number (.*)")]
        public async Task WhenIRequestThePlayerDetailsForPlayerNumber(String playerNumber)
        {
            RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);

            this.TestingContext.GetPlayerResponse = await this.TestingContext.DockerHelper.PlayerClient.GetPlayer(this.TestingContext.PlayerToken, registerPlayerResponse.PlayerId, CancellationToken.None).ConfigureAwait(false);
        }

        [Then(@"the player details will be returned")]
        public void ThenThePlayerDetailsWillBeReturned()
        {
            this.TestingContext.GetPlayerResponse.ShouldNotBeNull();
        }

        [When(@"I request a list of my memberships as player (.*)")]
        public async Task WhenIRequestAListOfMyMembershipsAsPlayer(String playerNumber)
        {
            RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);
            await Retry.For(async () =>
                            {
                                List<ClubMembershipResponse> getClubMembershipResponses = await this
                                                                                              .TestingContext.DockerHelper.PlayerClient
                                                                                              .GetPlayerMemberships(this.TestingContext.PlayerToken,
                                                                                                                    registerPlayerResponse.PlayerId,
                                                                                                                    CancellationToken.None).ConfigureAwait(false);
                                if (getClubMembershipResponses.Count == 0)
                                {
                                    throw new Exception();
                                }

                                this.TestingContext.GetGolfClubMembershipResponseList = getClubMembershipResponses;

                            }).ConfigureAwait(false);
        }


        [Then(@"a list of my memberships will be returned")]
        public void ThenAListOfMyMembershipsWillBeReturned()
        {
            this.TestingContext.GetGolfClubMembershipResponseList.ShouldNotBeNull();
            this.TestingContext.GetGolfClubMembershipResponseList.ShouldNotBeEmpty();
            this.TestingContext.GetGolfClubMembershipResponseList.Count.ShouldBe(1);
        }


    }
}
