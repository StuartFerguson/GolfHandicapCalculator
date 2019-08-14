using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Reporting
{
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

    [Binding]
    [Scope(Tag = "reporting")]
    public class ReportingSteps
    {
        private readonly TestingContext TestingContext;

        public ReportingSteps(TestingContext testingContext) 
        {
            this.TestingContext = testingContext;
        }
        
        /*
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

                await this.GolfClubClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, CancellationToken.None).ConfigureAwait(false); ;

                this.ReportingTestingContext.GolfClubAdministratorRequests.Add(tableRow["GolfClubNumber"], registerClubAdministratorRequest);
            }
        }


        [Given(@"the following golf clubs exist")]
        public async Task GivenTheFollowingGolfClubsExist(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                RegisterClubAdministratorRequest registerClubAdministratorRequest = this.ReportingTestingContext.GetRegisterClubAdministratorRequest(tableRow["GolfClubNumber"]);
                
                this.ReportingTestingContext.GolfClubAdministratorToken = await this.GetToken(TokenType.Password,
                              "golfhandicap.mobile",
                              "golfhandicap.mobile",
                              registerClubAdministratorRequest.EmailAddress,
                              registerClubAdministratorRequest.Password).ConfigureAwait(false); ;

                CreateGolfClubRequest createGolfClubRequest = new CreateGolfClubRequest
                                                              {
                                                                  AddressLine1 = tableRow["AddressLine1"],
                                                                  EmailAddress = tableRow["EmailAddress"],
                                                                  TelephoneNumber = tableRow["TelephoneNumber"],
                                                                  Name = tableRow["Name"],
                                                                  AddressLine2 = tableRow["AddressLine2"],
                                                                  PostalCode = tableRow["PostalCode"],
                                                                  Region = tableRow["Region"],
                                                                  Town = tableRow["Town"],
                                                                  Website = tableRow["Website"]
                                                              };

                this.ReportingTestingContext.CreateGolfClubRequests.Add(tableRow["GolfClubNumber"], createGolfClubRequest);

                CreateGolfClubResponse createGolfClubResponse = await this
                                                   .GolfClubClient.CreateGolfClub(this.ReportingTestingContext.GolfClubAdministratorToken,
                                                                                  createGolfClubRequest,
                                                                                  CancellationToken.None).ConfigureAwait(false);
                
                this.ReportingTestingContext.CreateGolfClubResponses.Add(tableRow["GolfClubNumber"], createGolfClubResponse);
            }
        }
        
        [Given(@"the following players have registered")]
        public async Task GivenTheFollowingPlayersHaveRegistered(Table table)
        {
            foreach (TableRow tableRow in table.Rows)
            {
                RegisterPlayerRequest registerPlayerRequest = new RegisterPlayerRequest
                                                              {
                                                                  DateOfBirth = DateTime.ParseExact(tableRow["DateOfBirth"], "yyyy-MM-dd", null),
                                                                  FamilyName = tableRow["FamilyName"],
                                                                  GivenName = tableRow["GivenName"],
                                                                  MiddleName = tableRow["MiddleName"],
                                                                  EmailAddress = tableRow["EmailAddress"],
                                                                  ExactHandicap = Decimal.Parse(tableRow["FamilyName"]),
                                                                  Gender = tableRow["Gender"],
                                                              };

                this.ReportingTestingContext.RegisterPlayerRequests.Add(tableRow["PlayerNumber"], registerPlayerRequest);

                RegisterPlayerResponse registerPlayerResponse = await this.PlayerClient.RegisterPlayer(registerPlayerRequest, CancellationToken.None).ConfigureAwait(false);

                this.ReportingTestingContext.RegisterPlayerResponses.Add(tableRow["PlayerNumber"], registerPlayerResponse);
            }
        }
        */
        //[Given(@"the following players are club members of the following golf clubs")]
        //public async Task GivenTheFollowingPlayersAreClubMembersOfTheFollowingGolfClubs(Table table)
        //{
        //    foreach (TableRow tableRow in table.Rows)
        //    {
        //        RegisterPlayerRequest registerPlayerRequest = this.TestingContext.GetRegisterPlayerRequest(tableRow["PlayerNumber"]);

        //        RegisterPlayerResponse registerPlayerResponse = this.TestingContext.GetRegisterPlayerResponse(tableRow["PlayerNumber"]);

        //        CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(tableRow["GolfClubNumber"]);

        //        this.TestingContext.PlayerToken = await this.TestingContext.DockerHelper.GetToken(TokenType.Password,
        //                                                                   "golfhandicap.mobile",
        //                                                                   "golfhandicap.mobile",
        //                                                                   registerPlayerRequest.EmailAddress,
        //                                                                   "123456").ConfigureAwait(false);

        //        await this.TestingContext.DockerHelper.PlayerClient.RequestClubMembership(this.TestingContext.PlayerToken,
        //                                                      registerPlayerResponse.PlayerId,
        //                                                      golfClubResponse.GolfClubId,
        //                                                      CancellationToken.None).ConfigureAwait(false);
        //    }
        //}

        [When(@"I request a number of members report for club number (.*)")]
        public async Task WhenIRequestANumberOfMembersReportForClubNumber(String golfClubNumber)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GetNumberOfMembersReportResponse = await this.TestingContext.DockerHelper.ReportingClient.GetNumberOfMembersReport(this.TestingContext.GolfClubAdministratorToken,
                                                                golfClubResponse.GolfClubId,
                                                                CancellationToken.None).ConfigureAwait(false);
        }
        
        [Then(@"I am returned the report data successfully")]
        public void ThenIAmReturnedTheReportDataSuccessfully()
        {
            this.TestingContext.GetNumberOfMembersReportResponse.ShouldNotBeNull();
        }

        [Then(@"the number of members count for club number (.*) is (.*)")]
        public async Task ThenTheNumberOfMembersCountForClubNumberIs(String golfClubNumber, Int32 membersCount)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            await Retry.For(async () =>
                            {
                                this.TestingContext.GetNumberOfMembersReportResponse.NumberOfMembers.ShouldBe(membersCount);

                                this.TestingContext.GetNumberOfMembersReportResponse = await this
                                                                                             .TestingContext.DockerHelper.ReportingClient
                                                                                             .GetNumberOfMembersReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                       golfClubResponse.GolfClubId,
                                                                                                                       CancellationToken.None).ConfigureAwait(false);
                            });
        }
    }
}
