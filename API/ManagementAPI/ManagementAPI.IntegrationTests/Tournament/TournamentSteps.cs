using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Tournament
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using GolfClub;
    using Service.DataTransferObjects.Requests;
    using Service.DataTransferObjects.Responses;
    using Service.DataTransferObjects.Responses.v2;
    using Shouldly;
    using CreateGolfClubResponse = Service.DataTransferObjects.Responses.v2.CreateGolfClubResponse;
    using MeasuredCourseListResponse = Service.DataTransferObjects.Responses.v2.MeasuredCourseListResponse;

    [Binding]
    [Scope(Tag = "tournament")]
    public class TournamentSteps
    {
        private readonly TestingContext TestingContext;

        public TournamentSteps(TestingContext testingContext)
        {
            this.TestingContext = testingContext;
        }

        [Given(@"When I create a tournament with the following details")]
        public async Task GivenWhenICreateATournamentWithTheFollowingDetails(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(tableRow["GolfClubNumber"]);

                List<MeasuredCourseListResponse> measuredCourseList = await this.TestingContext.DockerHelper.GolfClubClient.GetMeasuredCourses(this.TestingContext.GolfClubAdministratorToken,
                                                                                                            createGolfClubResponse.GolfClubId,
                                                                                                            CancellationToken.None).ConfigureAwait(false);

                MeasuredCourseListResponse measuredCourse = measuredCourseList.Single(m => m.Name == tableRow["MeasuredCourseName"]);

                TournamentFormat tournamentFormat = Enum.Parse<TournamentFormat>(tableRow["TournamentFormat"], true);
                PlayerCategory playerCategory = Enum.Parse<PlayerCategory>(tableRow["PlayerCategory"], true);

                // Work out the tournament date
                DateTime tournamentDate = this.CalculateTournamentDate(tableRow);

                CreateTournamentRequest createTournamentRequest = new CreateTournamentRequest
                                                                  {
                                                                      Format = (Int32)tournamentFormat,
                                                                      Name = tableRow["TournamentName"],
                                                                      MeasuredCourseId = measuredCourse.MeasuredCourseId,
                                                                      MemberCategory = (Int32)playerCategory,
                                                                      TournamentDate = tournamentDate
                };

                this.TestingContext.CreateTournamentRequests.Add(new Tuple<String, String, String>(tableRow["GolfClubNumber"], tableRow["MeasuredCourseName"],
                                                                                                   tableRow["TournamentNumber"]), createTournamentRequest);
            }
            
        }

        /// <summary>
        /// Calculates the tournament date.
        /// </summary>
        /// <param name="tableRow">The table row.</param>
        /// <returns></returns>
        private DateTime CalculateTournamentDate(TableRow tableRow)
        {
            String monthString = tableRow["TournamentMonth"];
            String dayString = tableRow["TournamentDay"];

            Int32 month = DateTime.ParseExact(monthString, "MMMM", CultureInfo.CurrentCulture).Month;
            Int32 day = Int32.Parse(dayString);

            DateTime result= new DateTime(DateTime.Now.Year, month, day);

            return result;
        }

        [Then(@"tournament number (.*) for golf club (.*) measured course '(.*)' will be created")]
        public async Task ThenTournamentNumberForGolfClubMeasuredCourseWillBeCreated(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateTournamentRequest createTournamentRequest = this.TestingContext.GetCreateTournamentRequest(golfClubNumber, measuredCourseName, tournamentNumber);

            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            CreateTournamentResponse createTournamentResponse = await this.TestingContext.DockerHelper.TournamentClient
                .CreateTournament(this.TestingContext.GolfClubAdministratorToken, createGolfClubResponse.GolfClubId, createTournamentRequest, CancellationToken.None)
                .ConfigureAwait(false);

            createTournamentResponse.TournamentId.ShouldNotBe(Guid.Empty);

            this.TestingContext.CreateTournamentResponses.Add(new Tuple<String, String, String>(golfClubNumber, measuredCourseName,
                                                                                               tournamentNumber), createTournamentResponse);
        }

        [When(@"I get the tournament list for golf club (.*)")]
        public async Task WhenIGetTheTournamentListForGolfClub(String golfClubNumber)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            GetTournamentListResponse tournamentList = await this.TestingContext.DockerHelper.TournamentClient
                .GetTournamentList(this.TestingContext.GolfClubAdministratorToken, createGolfClubResponse.GolfClubId, CancellationToken.None).ConfigureAwait(false);

            this.TestingContext.GetTournamentListResponses.Add(golfClubNumber, tournamentList);
        }
        
        [Then(@"(.*) tournament record will be returned for golf club (.*)")]
        public async Task ThenTournamentRecordWillBeReturnedForGolfClub(Int32 numberOfTournaments, String golfClubNumber)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            GetTournamentListResponse getTournamentListResponse = this.TestingContext.GetTournamentListResponse(golfClubNumber);

            await Retry.For(async () =>
                            {
                                GetTournamentListResponse tournamentList = await this.TestingContext.DockerHelper.TournamentClient
                                                                                     .GetTournamentList(this.TestingContext.GolfClubAdministratorToken,
                                                                                                        createGolfClubResponse.GolfClubId,
                                                                                                        CancellationToken.None).ConfigureAwait(false);

                                tournamentList.Tournaments.Count.ShouldBe(numberOfTournaments);

                                this.TestingContext.GetTournamentListResponses.Remove(golfClubNumber);
                                this.TestingContext.GetTournamentListResponses.Add(golfClubNumber, tournamentList);
                            });
        }

        [When(@"I request to cancel the tournament number (.*) for golf club (.*) measured course '(.*)' the tournament is cancelled")]
        public void WhenIRequestToCancelTheTournamentNumberForGolfClubMeasuredCourseTheTournamentIsCancelled(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            CreateTournamentResponse createTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            CancelTournamentRequest cancelTournamentRequest = new CancelTournamentRequest
                                                              {
                                                                  CancellationReason = "Test Cancel"
                                                              };

            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.TournamentClient.CancelTournament(this.TestingContext.GolfClubAdministratorToken,
                                                                                                          createGolfClubResponse.GolfClubId,
                                                                                                          createTournamentResponse.TournamentId,
                                                                                                          cancelTournamentRequest,
                                                                                                          CancellationToken.None).ConfigureAwait(false);
                            });
        }
        
        [When(@"player number (.*) signs up to play in tournament number (.*) for golf club (.*) measured course '(.*)'")]
        public async Task WhenPlayerNumberSignsUpToPlayInTournamentNumberForGolfClubMeasuredCourse(String playerNumber, String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            RegisterPlayerResponse getRegisterPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);

            CreateTournamentResponse getCreateTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            await this.TestingContext.DockerHelper.PlayerClient
                .SignUpPlayerForTournament(this.TestingContext.PlayerToken,
                                           getRegisterPlayerResponse.PlayerId,
                                           getCreateTournamentResponse.TournamentId,
                                           CancellationToken.None).ConfigureAwait(false);
        }

        [Then(@"player number (.*) is recorded as signed up for tournament number (.*) for golf club (.*) measured course '(.*)'")]
        public async Task ThenPlayerNumberIsRecordedAsSignedUpForTournamentNumberForGolfClubMeasuredCourse(String playerNumber, String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            RegisterPlayerResponse getRegisterPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);

            CreateTournamentResponse getCreateTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            await Retry.For(async () =>
                            {
                                PlayerSignedUpTournamentsResponse response = await this
                                                                                   .TestingContext.DockerHelper.PlayerClient
                                                                                   .GetTournamentsSignedUpFor(this.TestingContext.PlayerToken,
                                                                                                              getRegisterPlayerResponse.PlayerId,
                                                                                                              CancellationToken.None).ConfigureAwait(false);

                                List<PlayerSignedUpTournament> tournamentSignUp = response.PlayerSignedUpTournaments.Where(s => s.TournamentId == getCreateTournamentResponse.TournamentId).ToList();
                                tournamentSignUp.ShouldNotBeNull();
                                tournamentSignUp.ShouldNotBeEmpty();
                                tournamentSignUp.Count.ShouldBe(1);
                            });
        }
        
        [When(@"a player records the following score for tournament number (.*) for golf club (.*) measured course '(.*)'")]
        public void WhenAPlayerRecordsTheFollowingScoreForTournamentNumberForGolfClubMeasuredCourse(String tournamentNumber, String golfClubNumber, String measuredCourseName, Table table)
        {
            CreateTournamentResponse getCreateTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            foreach (TableRow tableRow in table.Rows)
            {
                RegisterPlayerResponse getRegisterPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(tableRow["PlayerNumber"]);

                RecordPlayerTournamentScoreRequest recordPlayerTournamentScoreRequest = new RecordPlayerTournamentScoreRequest
                {
                    HoleScores = new Dictionary<Int32, Int32>()
                };

                for (Int32 i = 1; i <= 18; i++)
                {
                    recordPlayerTournamentScoreRequest.HoleScores.Add(i, Int32.Parse(tableRow[$"Hole{i}"]));
                }

                this.TestingContext.RecordPlayerTournamentScoreRequests.Add(new Tuple<String, String, String,String>(golfClubNumber, measuredCourseName,
                                                                                                              tournamentNumber, tableRow["PlayerNumber"]), recordPlayerTournamentScoreRequest);
            }
        }

        [Then(@"the scores recorded by the players are recorded against tournament number (.*) for golf club (.*) measured course '(.*)'")]
        public void ThenTheScoresRecordedByThePlayersAreRecordedAgainstTournamentNumberForGolfClubMeasuredCourse(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateTournamentResponse getCreateTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            List<KeyValuePair<Tuple<String, String, String, String>, RecordPlayerTournamentScoreRequest>> filtered = this.TestingContext.RecordPlayerTournamentScoreRequests
                .Where(x => x.Key.Item1 == golfClubNumber && x.Key.Item2 == measuredCourseName && x.Key.Item3 == tournamentNumber).ToList();

            foreach (KeyValuePair<Tuple<String, String, String, String>, RecordPlayerTournamentScoreRequest> keyValuePair in filtered)
            {
                RegisterPlayerResponse getRegisterPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(keyValuePair.Key.Item4);

                Should.NotThrow(async () =>
                                {
                                    await this.TestingContext.DockerHelper.PlayerClient
                                        .RecordPlayerScore(this.TestingContext.PlayerToken,
                                                           getRegisterPlayerResponse.PlayerId,
                                                           getCreateTournamentResponse.TournamentId,
                                                           keyValuePair.Value,
                                                           CancellationToken.None).ConfigureAwait(false);
                                });
            }
            
        }

        [When(@"I request to complete the tournament number (.*) for golf club (.*) measured course '(.*)' the tournament is completed")]
        public void WhenIRequestToCompleteTheTournamentNumberForGolfClubMeasuredCourseTheTournamentIsCompleted(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            CreateTournamentResponse createTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.TournamentClient.CompleteTournament(this.TestingContext.GolfClubAdministratorToken,
                                                                                                         createGolfClubResponse.GolfClubId,
                                                                                                         createTournamentResponse.TournamentId,
                                                                                                         CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [When(@"I request to produce a tournament result for tournament number (.*) for golf club (.*) measured course '(.*)' the results are produced")]
        public void WhenIRequestToProduceATournamentResultForTournamentNumberForGolfClubMeasuredCourseTheResultsAreProduced(String tournamentNumber, String golfClubNumber, String measuredCourseName)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            CreateTournamentResponse createTournamentResponse = this.TestingContext.GetCreateTournamentResponse(golfClubNumber, measuredCourseName, tournamentNumber);

            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.TournamentClient
                                      .ProduceTournamentResult(this.TestingContext.GolfClubAdministratorToken, createGolfClubResponse.GolfClubId, createTournamentResponse.TournamentId, CancellationToken.None)
                                      .ConfigureAwait(false);
                            });
        }

    }
}
