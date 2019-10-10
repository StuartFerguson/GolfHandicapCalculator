using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.HandicapCalculation
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using GolfClub;
    using Service.Client;
    using Service.DataTransferObjects.Responses;
    using Service.DataTransferObjects.Responses.v2;
    using Shouldly;
    using CreateTournamentResponse = Service.DataTransferObjects.Responses.v2.CreateTournamentResponse;
    using RegisterPlayerResponse = Service.DataTransferObjects.Responses.v2.RegisterPlayerResponse;

    [Binding]
    public class ProcessHandicapAdjustmentsSteps
    {
        private readonly TestingContext TestingContext;

        public ProcessHandicapAdjustmentsSteps(TestingContext testingContext)
        {
            this.TestingContext = testingContext;
        }

        [When(@"I start the handicap calculation process for tournament number (.*) for golf club (.*) measured course '(.*)'")]
        public async Task WhenIStartTheHandicapCalculationProcessForTournamentNumberForGolfClubMeasuredCourse(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateTournamentResponse createTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);
            String uri = $"/api/HandicapCalculation?tournamentId={createTournamentResponse.TournamentId}";
            this.TestingContext.DockerHelper.HttpClient.DefaultRequestHeaders.Remove("Authorization");
            this.TestingContext.DockerHelper.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.TestingContext.GolfClubAdministratorToken}");

            HttpResponseMessage response = await this.TestingContext.DockerHelper.HttpClient.PostAsync(uri, new StringContent(String.Empty, Encoding.UTF8, "application/json"), CancellationToken.None).ConfigureAwait(false);

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        }

        [When(@"the process completes successfully for tournament number (.*) for golf club (.*) measured course '(.*)'")]
        public async Task WhenTheProcessCompletesSuccessfullyForTournamentNumberForGolfClubMeasuredCourse(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateTournamentResponse createTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            await Retry.For(async () =>
                            {
                                String uri =
                                    $"/api/HandicapCalculation?tournamentId={createTournamentResponse.TournamentId}";

                                this.TestingContext.DockerHelper.HttpClient.DefaultRequestHeaders.Remove("Authorization");
                                this.TestingContext.DockerHelper.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.TestingContext.GolfClubAdministratorToken}");

                                HttpResponseMessage response = await this.TestingContext.DockerHelper.HttpClient.GetAsync(uri, CancellationToken.None).ConfigureAwait(false);

                                response.StatusCode.ShouldBe(HttpStatusCode.OK);

                                String responseData = await response.Content.ReadAsStringAsync();

                                responseData.Contains("Completed").ShouldBeTrue();
                            });
        }


        [Then(@"the new playing handicap for player number (.*) is adjusted to (.*)")]
        public async Task ThenTheNewPlayingHandicapForPlayerNumberIsAdjustedTo(String playerNumber, Int32 playingHandicap)
        {
            RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);

            GetPlayerResponse player = await this.TestingContext.DockerHelper.PlayerClient.GetPlayer(this.TestingContext.PlayerToken, 
                                                                                                            registerPlayerResponse.PlayerId, CancellationToken.None).ConfigureAwait(false);

            player.PlayingHandicap.ShouldBe(playingHandicap);
        }
        
        [Then(@"the new exact handicap for player number (.*) is adjusted to (.*)")]
        public async Task ThenTheNewExactHandicapForPlayerNumberIsAdjustedTo(String playerNumber, Decimal exactHandicap)
        {
            RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);

            GetPlayerResponse player = await this.TestingContext.DockerHelper.PlayerClient.GetPlayer(this.TestingContext.PlayerToken,
                                                                                                            registerPlayerResponse.PlayerId, CancellationToken.None).ConfigureAwait(false);

            player.ExactHandicap.ShouldBe(exactHandicap);
        }
    }
}
