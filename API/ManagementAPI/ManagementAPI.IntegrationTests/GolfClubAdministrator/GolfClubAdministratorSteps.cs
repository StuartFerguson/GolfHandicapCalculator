using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.GolfClubAdministrator
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using GolfClub;
    using Service.DataTransferObjects.Requests;
    using Shouldly;

    [Binding]
    [Scope(Tag = "golfclubadministrator")]
    public class GolfClubAdministratorSteps
    {
        private readonly TestingContext TestingContext;

        public GolfClubAdministratorSteps(TestingContext testingContext)
        {
            this.TestingContext = testingContext;
        }

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
                await this.TestingContext.DockerHelper.GolfClubAdministratorClient.RegisterGolfClubAdministrator(this.TestingContext.RegisterClubAdministratorRequest,
                                                                        CancellationToken.None).ConfigureAwait(false);
            });

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
                await this.TestingContext.DockerHelper.GolfClubAdministratorClient.RegisterGolfClubAdministrator(this.TestingContext.RegisterClubAdministratorRequest,
                                                                        CancellationToken.None).ConfigureAwait(false);
            });
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

                await this.TestingContext.DockerHelper.GolfClubAdministratorClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, CancellationToken.None).ConfigureAwait(false);

                this.TestingContext.GolfClubAdministratorRequests.Add(tableRow["GolfClubNumber"], registerClubAdministratorRequest);
            }
        }
    }
}
