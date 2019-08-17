using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.GolfClub
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Common;
    using DataTransferObjects;
    using ManagementAPI.Service.DataTransferObjects.Requests;
    using Reporting;
    using Service.Client;
    using Service.DataTransferObjects.Responses;
    using Shouldly;

    [Binding]
    [Scope(Tag = "golfclub")]
    public class GolfClubSteps
    {
        private readonly TestingContext TestingContext;
        
        [When(@"I register the following details for a golf club administrator")]
        public void WhenIRegisterTheFollowingDetailsForAGolfClubAdministrator(Table table)
        {
            TableRow tableRow = table.Rows.Single();

            RegisterClubAdministratorRequest registerClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                {
                                                                                    ConfirmPassword = tableRow["ConfirmPassword"],
                                                                                    FamilyName = tableRow["FamilyName"],
                                                                                    GivenName = tableRow["GivenName"],
                                                                                    MiddleName = tableRow["MiddleName"],
                                                                                    EmailAddress = tableRow["EmailAddress"],
                                                                                    TelephoneNumber = tableRow["TelephoneNumber"],
                                                                                    Password = tableRow["Password"]
                                                                                };

            this.TestingContext.RegisterClubAdministratorRequest = registerClubAdministratorRequest;
        }

        [Then(@"the golf club administrator registration should be successful")]
        public void ThenTheGolfClubAdministratorRegistrationShouldBeSuccessful()
        {
            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.GolfClubClient.RegisterGolfClubAdministrator(this.TestingContext.RegisterClubAdministratorRequest,
                                                                                        CancellationToken.None).ConfigureAwait(false);
                                ;
                            });

        }

        public GolfClubSteps(TestingContext testingContext)
        {
            this.TestingContext = testingContext;
        }

        [Given(@"the following golf club administrator has been registered")]
        public void GivenTheFollowingGolfClubAdministratorHasBeenRegistered(Table table)
        {
            TableRow tableRow = table.Rows.Single();

            RegisterClubAdministratorRequest registerClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                {
                                                                                    ConfirmPassword = tableRow["ConfirmPassword"],
                                                                                    FamilyName = tableRow["FamilyName"],
                                                                                    GivenName = tableRow["GivenName"],
                                                                                    MiddleName = tableRow["MiddleName"],
                                                                                    EmailAddress = tableRow["EmailAddress"],
                                                                                    TelephoneNumber = tableRow["TelephoneNumber"],
                                                                                    Password = tableRow["Password"]
                                                                                };

            this.TestingContext.RegisterClubAdministratorRequest = registerClubAdministratorRequest;
            this.TestingContext.GolfClubAdministratorRequests.Add(tableRow["GolfClubNumber"], registerClubAdministratorRequest);

            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.GolfClubClient.RegisterGolfClubAdministrator(this.TestingContext.RegisterClubAdministratorRequest,
                                                                                        CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [Given(@"I am logged in as the administrator for golf club (.*)")]
        public async Task GivenIAmLoggedInAsTheAdministratorForGolfClub(Int32 golfClubNumber)
        {
            this.TestingContext.GolfClubAdministratorToken = await this.TestingContext.DockerHelper.GetToken(TokenType.Password,
                                                                                         "golfhandicap.mobile",
                                                                                         "golfhandicap.mobile",
                                                                                         this.TestingContext.RegisterClubAdministratorRequest.EmailAddress,
                                                                                         this.TestingContext.RegisterClubAdministratorRequest.Password)
                                                                               .ConfigureAwait(false);
        }

        [When(@"I create a golf club with the following details")]
        public async Task WhenICreateAGolfClubWithTheFollowingDetails(Table table)
        {
            TableRow tableRow = table.Rows.Single();

            CreateGolfClubRequest createGolfClubRequest = new CreateGolfClubRequest
                                                          {
                                                              EmailAddress = tableRow["EmailAddress"],
                                                              AddressLine1 = tableRow["AddressLine1"],
                                                              AddressLine2 = tableRow["AddressLine2"],
                                                              Name = tableRow["GolfClubName"],
                                                              PostalCode = tableRow["PostalCode"],
                                                              Region = tableRow["Region"],
                                                              TelephoneNumber = tableRow["TelephoneNumber"],
                                                              Town = tableRow["Town"],
                                                              Website = tableRow["WebSite"],
                                                          };
            this.TestingContext.CreateGolfClubRequest = createGolfClubRequest;

            this.TestingContext.CreateGolfClubResponse = await this.TestingContext.DockerHelper.GolfClubClient.CreateGolfClub(this.TestingContext.GolfClubAdministratorToken,
                                                                                                          createGolfClubRequest,
                                                                                                          CancellationToken.None).ConfigureAwait(false);

            this.TestingContext.CreateGolfClubResponses.Add(tableRow["GolfClubNumber"], this.TestingContext.CreateGolfClubResponse);
        }

        [Then(@"the golf club is created successfully")]
        public void ThenTheGolfClubIsCreatedSuccessfully()
        {
            this.TestingContext.CreateGolfClubResponse.ShouldNotBeNull();
            this.TestingContext.CreateGolfClubResponse.GolfClubId.ShouldNotBe(Guid.Empty);
        }

        [When(@"I request the details of the golf club (.*)")]
        public async Task WhenIRequestTheDetailsOfTheGolfClub(Int32 golfClubNumber)
        {
            this.TestingContext.GetGolfClubResponse = await this.TestingContext.DockerHelper.GolfClubClient.GetSingleGolfClub(this.TestingContext.GolfClubAdministratorToken,
                                                                                                          this.TestingContext.CreateGolfClubResponse.GolfClubId,
                                                                                                          CancellationToken.None).ConfigureAwait(false);
        }

        [Then(@"the golf club data will be returned")]
        public void ThenTheGolfClubDataWillBeReturned()
        {
            GetGolfClubResponse response = this.TestingContext.GetGolfClubResponse;

            response.ShouldNotBeNull();
            response.Name.ShouldBe("Test Golf Club 1");
        }

        [Given(@"the following golf club administrators have been registered")]
        public async Task GivenTheFollowingGolfClubAdministratorsHaveBeenRegistered(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                RegisterClubAdministratorRequest registerClubAdministratorRequest = new RegisterClubAdministratorRequest
                                                                                    {
                                                                                        ConfirmPassword = tableRow["ConfirmPassword"],
                                                                                        FamilyName = tableRow["FamilyName"],
                                                                                        GivenName = tableRow["GivenName"],
                                                                                        MiddleName = tableRow["MiddleName"],
                                                                                        EmailAddress = tableRow["EmailAddress"],
                                                                                        TelephoneNumber = tableRow["TelephoneNumber"],
                                                                                        Password = tableRow["Password"]
                                                                                    };

                await this.TestingContext.DockerHelper.GolfClubClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, CancellationToken.None).ConfigureAwait(false);

                this.TestingContext.GolfClubAdministratorRequests.Add(tableRow["GolfClubNumber"], registerClubAdministratorRequest);
            }
        }

        [Given(@"the following golf clubs exist")]
        public async Task GivenTheFollowingGolfClubsExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                RegisterClubAdministratorRequest registerClubAdministratorRequest =
                    this.TestingContext.GetRegisterClubAdministratorRequest(tableRow["GolfClubNumber"]);

                this.TestingContext.GolfClubAdministratorToken = await this.TestingContext.DockerHelper.GetToken(TokenType.Password,
                                                                                             "golfhandicap.mobile",
                                                                                             "golfhandicap.mobile",
                                                                                             registerClubAdministratorRequest.EmailAddress,
                                                                                             registerClubAdministratorRequest.Password).ConfigureAwait(false);
                ;

                CreateGolfClubRequest createGolfClubRequest = new CreateGolfClubRequest
                                                              {
                                                                  AddressLine1 = tableRow["AddressLine1"],
                                                                  EmailAddress = tableRow["EmailAddress"],
                                                                  TelephoneNumber = tableRow["TelephoneNumber"],
                                                                  Name = tableRow["GolfClubName"],
                                                                  AddressLine2 = tableRow["AddressLine2"],
                                                                  PostalCode = tableRow["PostalCode"],
                                                                  Region = tableRow["Region"],
                                                                  Town = tableRow["Town"],
                                                                  Website = tableRow["WebSite"]
                                                              };

                this.TestingContext.CreateGolfClubRequests.Add(tableRow["GolfClubNumber"], createGolfClubRequest);

                CreateGolfClubResponse createGolfClubResponse = await this.TestingContext.DockerHelper.GolfClubClient.CreateGolfClub(this.TestingContext.GolfClubAdministratorToken,
                                                                                                     createGolfClubRequest,
                                                                                                     CancellationToken.None).ConfigureAwait(false);

                this.TestingContext.CreateGolfClubResponses.Add(tableRow["GolfClubNumber"], createGolfClubResponse);
            }
        }
        
        [Given(@"the following players have registered")]
        public async Task GivenTheFollowingPlayersHaveRegistered(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                Int32 age = Int32.Parse(tableRow["Age"]);

                RegisterPlayerRequest registerPlayerRequest = new RegisterPlayerRequest
                                                              {
                                                                  DateOfBirth = DateTime.Now.AddYears(age * -1),
                                                                  FamilyName = tableRow["FamilyName"],
                                                                  GivenName = tableRow["GivenName"],
                                                                  MiddleName = tableRow["MiddleName"],
                                                                  EmailAddress = tableRow["EmailAddress"],
                                                                  ExactHandicap = Decimal.Parse(tableRow["ExactHandicap"]),
                                                                  Gender = tableRow["Gender"],
                                                              };

                this.TestingContext.RegisterPlayerRequests.Add(tableRow["PlayerNumber"], registerPlayerRequest);

                RegisterPlayerResponse registerPlayerResponse =
                    await this.TestingContext.DockerHelper.PlayerClient.RegisterPlayer(registerPlayerRequest, CancellationToken.None).ConfigureAwait(false);

                this.TestingContext.RegisterPlayerResponses.Add(tableRow["PlayerNumber"], registerPlayerResponse);
            }
        }

        [Given(@"I am logged in as player number (.*)")]
        public async Task GivenIAmLoggedInAsPlayerNumber(String playerNumber)
        {
            RegisterPlayerRequest registerPlayerRequest = this.TestingContext.GetRegisterPlayerRequest(playerNumber);

            this.TestingContext.PlayerToken = await this.TestingContext.DockerHelper.GetToken(TokenType.Password,
                                                                      "golfhandicap.mobile",
                                                                      "golfhandicap.mobile",
                                                                      registerPlayerRequest.EmailAddress,
                                                                      "123456").ConfigureAwait(false);
        }

        [When(@"I request the list of golf clubs")]
        public async Task WhenIRequestTheListOfGolfClubs()
        {
            await Retry.For(async () =>
                            {
                                List<GetGolfClubResponse> getGolfClubResponses = await this
                                                                                       .TestingContext.DockerHelper.PlayerClient
                                                                                       .GetGolfClubList(this.TestingContext.PlayerToken,
                                                                                                        this.TestingContext.PlayerId,
                                                                                                        CancellationToken.None).ConfigureAwait(false);

                                if (getGolfClubResponses.Count == 0)
                                {
                                    throw new Exception();
                                }

                                this.TestingContext.GetGolfClubResponseList = getGolfClubResponses;
                            }).ConfigureAwait(false);

        }

        [Then(@"a list of golf clubs will be returned")]
        public void ThenAListOfGolfClubsWillBeReturned()
        {
            this.TestingContext.GetGolfClubResponseList.ShouldNotBeNull();
            this.TestingContext.GetGolfClubResponseList.ShouldNotBeEmpty();
        }

        [Then(@"the list will contain (.*) golf clubs")]
        public void ThenTheListWillContainGolfClubs(Int32 golfClubCount)
        {
            this.TestingContext.GetGolfClubResponseList.Count.ShouldBe(golfClubCount);
        }

        [When(@"I add a measured course to the club with the following details")]
        public void WhenIAddAMeasuredCourseToTheClubWithTheFollowingDetails(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest = new AddMeasuredCourseToClubRequest
                                                                                {
                                                                                    Name = tableRow["Name"],
                                                                                    MeasuredCourseId = Guid.NewGuid(),
                                                                                    StandardScratchScore = Int32.Parse(tableRow["StandardScratchScore"]),
                                                                                    TeeColour = tableRow["TeeColour"],
                                                                                    Holes = new List<HoleDataTransferObjectRequest>()
                                                                                };

                this.TestingContext.AddMeasuredCourseToClubRequests.Add(new KeyValuePair<String, String>(tableRow["GolfClubNumber"],
                                                                                                                 tableRow["MeasuredCourseNumber"]),
                                                                                addMeasuredCourseToClubRequest);
            }
        }

        [When(@"with the following holes")]
        public void WhenWithTheFollowingHoles(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest =
                    this.TestingContext.GetAddMeasuredCourseToClubRequest(tableRow["GolfClubNumber"], tableRow["MeasuredCourseNumber"]);

                addMeasuredCourseToClubRequest.Holes.Add(new HoleDataTransferObjectRequest
                                                         {
                                                             HoleNumber = Int32.Parse(tableRow["HoleNumber"]),
                                                             LengthInMeters = 0,
                                                             LengthInYards = Int32.Parse(tableRow["LengthInYards"]),
                                                             Par = Int32.Parse(tableRow["Par"]),
                                                             StrokeIndex = Int32.Parse(tableRow["StrokeIndex"])
                                                         });
            }
        }

        [Then(@"the measured course is added to the club successfully")]
        public void ThenTheMeasuredCourseIsAddedToTheClubSuccessfully()
        {
            foreach (KeyValuePair<KeyValuePair<String, String>, AddMeasuredCourseToClubRequest> addMeasuredCourseToClubRequest in this
                                                                                                                                  .TestingContext
                                                                                                                                  .AddMeasuredCourseToClubRequests)
            {
                CreateGolfClubResponse createGolfClubResponse = this.TestingContext.CreateGolfClubResponses[addMeasuredCourseToClubRequest.Key.Key];

                Should.NotThrow(async () =>
                                {
                                    await this.TestingContext.DockerHelper.GolfClubClient.AddMeasuredCourseToGolfClub(this.TestingContext.GolfClubAdministratorToken,
                                                                                          createGolfClubResponse.GolfClubId,
                                                                                          addMeasuredCourseToClubRequest.Value,
                                                                                          CancellationToken.None).ConfigureAwait(false);
                                });
            }
        }

        [When(@"I ask for a list of measured courses against the golf club (.*)")]
        public async Task WhenIAskForAListOfMeasuredCoursesAgainstTheGolfClub(String golfClubNumber)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.CreateGolfClubResponses[golfClubNumber];

            await Retry.For(async () =>
                      {
                          GetMeasuredCourseListResponse response = await this.TestingContext.DockerHelper.GolfClubClient.GetMeasuredCourses(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                createGolfClubResponse.GolfClubId,
                                                                                                                CancellationToken.None).ConfigureAwait(false);
                          
                          this.TestingContext.MeasuredCourseList = response;

                      }).ConfigureAwait(false);
        }

        [Then(@"the list of ""(.*)"" measured courses should be returned")]
        public void ThenTheListOfMeasuredCoursesShouldBeReturned(Int32 numberOfMeasuredCourses)
        {
            this.TestingContext.MeasuredCourseList.ShouldNotBeNull();
            this.TestingContext.MeasuredCourseList.MeasuredCourses.ShouldNotBeNull();
            this.TestingContext.MeasuredCourseList.MeasuredCourses.ShouldNotBeEmpty();
            this.TestingContext.MeasuredCourseList.MeasuredCourses.Count.ShouldBe(numberOfMeasuredCourses);
        }

        [When(@"I add tournament division (.*) with a start handicap of (.*) and and end handicap of (.*)")]
        public void WhenIAddTournamentDivisionWithAStartHandicapOfAndAndEndHandicapOf(Int32 division,
                                                                                      Int32 startHandicap,
                                                                                      Int32 endHandicap)
        {
            this.TestingContext.AddTournamentDivisionToGolfClubRequest = new AddTournamentDivisionToGolfClubRequest
                                                                                 {
                                                                                     Division = division,
                                                                                     StartHandicap = startHandicap,
                                                                                     EndHandicap = endHandicap
                                                                                 };

        }

        [Then(@"the divsion is addded successfully to golf club (.*)")]
        public void ThenTheDivsionIsAdddedSuccessfullyToGolfClub(String golfClubNumber)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.GolfClubClient.AddTournamentDivision(this.TestingContext.GolfClubAdministratorToken,
                                                                                createGolfClubResponse.GolfClubId,
                                                                                this.TestingContext.AddTournamentDivisionToGolfClubRequest,
                                                                                CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [When(@"I register the following details for a match secretary")]
        public void WhenIRegisterTheFollowingDetailsForAMatchSecretary(Table table)
        {
            TableRow tableRow = table.Rows.Single();

            this.TestingContext.CreateGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(tableRow["GolfClubNumber"]);

            CreateMatchSecretaryRequest createMatchSecretaryRequest = new CreateMatchSecretaryRequest
                                                                      {
                                                                          ConfirmPassword = tableRow["ConfirmPassword"],
                                                                          FamilyName = tableRow["FamilyName"],
                                                                          GivenName = tableRow["GivenName"],
                                                                          MiddleName = tableRow["MiddleName"],
                                                                          EmailAddress = tableRow["EmailAddress"],
                                                                          TelephoneNumber = tableRow["TelephoneNumber"],
                                                                          Password = tableRow["Password"]
                                                                      };

            this.TestingContext.CreateMatchSecretaryRequest = createMatchSecretaryRequest;
        }

        [Then(@"the match secretary registration should be successful")]
        public void ThenTheMatchSecretaryRegistrationShouldBeSuccessful()
        {
            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.GolfClubClient.CreateMatchSecretary(this.TestingContext.GolfClubAdministratorToken,
                                                                               this.TestingContext.CreateGolfClubResponse.GolfClubId,
                                                                               this.TestingContext.CreateMatchSecretaryRequest,
                                                                               CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [When(@"I request membership of club number (.*) for player number (.*) the request is successful")]
        public void WhenIRequestMembershipOfClubNumberForPlayerNumberTheRequestIsSuccessful(String clubNumber,
                                                                                            String playerNumber)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(clubNumber);
            RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(playerNumber);

            Should.NotThrow(async () =>
                            {
                                await this.TestingContext.DockerHelper.PlayerClient.RequestClubMembership(this.TestingContext.PlayerToken,
                                                                              registerPlayerResponse.PlayerId,
                                                                              createGolfClubResponse.GolfClubId,
                                                                              CancellationToken.None).ConfigureAwait(false);
                            });
        }

        [Given(@"the following players are club members of the following golf clubs")]
        public async Task GivenTheFollowingPlayersAreClubMembersOfTheFollowingGolfClubs(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                RegisterPlayerRequest registerPlayerRequest = this.TestingContext.GetRegisterPlayerRequest(tableRow["PlayerNumber"]);

                RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(tableRow["PlayerNumber"]);

                CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(tableRow["GolfClubNumber"]);

                this.TestingContext.PlayerToken = await this.TestingContext.DockerHelper.GetToken(TokenType.Password,
                                                                          "golfhandicap.mobile",
                                                                          "golfhandicap.mobile",
                                                                          registerPlayerRequest.EmailAddress,
                                                                          "123456").ConfigureAwait(false);

                await this.TestingContext.DockerHelper.PlayerClient.RequestClubMembership(this.TestingContext.PlayerToken,
                                                              registerPlayerResponse.PlayerId,
                                                              golfClubResponse.GolfClubId,
                                                              CancellationToken.None).ConfigureAwait(false);
            }
        }

        [When(@"I request a list of members for golf club number (.*)")]
        public async Task WhenIRequestAListOfMembersForGolfClubNumber(String golfClubNumber)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GolfClubMembersList =
                await this.TestingContext.DockerHelper.GolfClubClient.GetGolfClubMembershipList(this.TestingContext.GolfClubAdministratorToken,
                                                                    golfClubResponse.GolfClubId,
                                                                    CancellationToken.None);
        }

        [When(@"the a list of (.*) members is returned")]
        public void WhenTheAListOfMembersIsReturned(Int32 numberOfMembers)
        {
            this.TestingContext.GolfClubMembersList.ShouldNotBeNull();
            this.TestingContext.GolfClubMembersList.ShouldNotBeEmpty();
            this.TestingContext.GolfClubMembersList.Count.ShouldBe(numberOfMembers);
        }

        [Given(@"I am logged in as a golf club administrator for golf club (.*)")]
        public async Task GivenIAmLoggedInAsAGolfClubAdministratorForGolfClub(String golfClubNumber)
        {
            RegisterClubAdministratorRequest registerClubAdministratorRequest = this.TestingContext.GetRegisterClubAdministratorRequest(golfClubNumber);

            this.TestingContext.GolfClubAdministratorToken = await this.TestingContext.DockerHelper.GetToken(TokenType.Password,
                                                                                     "golfhandicap.mobile",
                                                                                     "golfhandicap.mobile",
                                                                                     registerClubAdministratorRequest.EmailAddress,
                                                                                     registerClubAdministratorRequest.Password).ConfigureAwait(false);
        }

        [When(@"I request a list of users for the for golf club (.*)")]
        public async Task WhenIRequestAListOfUsersForTheForGolfClub(String golfClubNumber)
        {
            CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GetGolfClubUserListResponse = await this.TestingContext.DockerHelper.GolfClubClient.GetGolfClubUserList(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                createGolfClubResponse.GolfClubId,
                                                                                                                CancellationToken.None).ConfigureAwait(false);
        }

        [Then(@"(.*) users details are returned for golf club (.*)")]
        public async Task ThenUsersDetailsAreReturnedForGolfClub(Int32 numberOfUsers,String golfClubNumber)
        {
            if (this.TestingContext.GetGolfClubUserListResponse.Users.Count != numberOfUsers)
            {
                CreateGolfClubResponse createGolfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

                await Retry.For(async () =>
                                {


                                    this.TestingContext.GetGolfClubUserListResponse = await this.TestingContext.DockerHelper.GolfClubClient
                                                                                                    .GetGolfClubUserList(this.TestingContext
                                                                                                                             .GolfClubAdministratorToken,
                                                                                                                         createGolfClubResponse.GolfClubId,
                                                                                                                         CancellationToken.None).ConfigureAwait(false);

                                    if (this.TestingContext.GetGolfClubUserListResponse.Users.Count != numberOfUsers)
                                    {
                                        throw new Exception();
                                    }
                                });
            }

            this.TestingContext.GetGolfClubUserListResponse.Users.ShouldNotBeNull();
            this.TestingContext.GetGolfClubUserListResponse.Users.ShouldNotBeEmpty();
            this.TestingContext.GetGolfClubUserListResponse.Users.Count.ShouldBe(numberOfUsers);
        }

        [Then(@"the golf club administrators details are returned for golf club (.*)")]
        public void ThenTheGolfClubAdministratorsDetailsAreReturnedForGolfClub(String golfClubNumber)
        {
            RegisterClubAdministratorRequest registerClubAdministratorRequest = this.TestingContext.GetRegisterClubAdministratorRequest(golfClubNumber);

            GolfClubUserResponse user = this.TestingContext.GetGolfClubUserListResponse.Users
                                            .SingleOrDefault(u => u.Email == registerClubAdministratorRequest.EmailAddress);

            user.ShouldNotBeNull();
        }

        [Then(@"the match secretarys details are returned for golf club (.*)")]
        public void ThenTheMatchSecretarysDetailsAreReturnedForGolfClub(String golfClubNumber)
        {
            GolfClubUserResponse user = this.TestingContext.GetGolfClubUserListResponse.Users
                                            .SingleOrDefault(u => u.Email == this.TestingContext.CreateMatchSecretaryRequest.EmailAddress);

            user.ShouldNotBeNull();
        }


    }
}
