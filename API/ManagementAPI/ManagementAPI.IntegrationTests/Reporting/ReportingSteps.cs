using System;
using TechTalk.SpecFlow;

namespace ManagementAPI.IntegrationTests.Reporting
{
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

    [Binding]
    [Scope(Tag = "reporting")]
    public class ReportingSteps
    {
        private readonly TestingContext TestingContext;

        public ReportingSteps(TestingContext testingContext) 
        {
            this.TestingContext = testingContext;
        }
        
        [When(@"I request a number of members report for club number (.*)")]
        public async Task WhenIRequestANumberOfMembersReportForClubNumber(String golfClubNumber)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GetNumberOfMembersReportResponse = await this.TestingContext.DockerHelper.ReportingClient.GetNumberOfMembersReport(this.TestingContext.GolfClubAdministratorToken,
                                                                golfClubResponse.GolfClubId,
                                                                CancellationToken.None).ConfigureAwait(false);
        }
        
        [Then(@"I am returned the number of members report data successfully")]
        public void ThenIAmReturnedTheNumberOfMembersReportDataSuccessfully()
        {
            this.TestingContext.GetNumberOfMembersReportResponse.ShouldNotBeNull();
        }


        [Then(@"the number of members count for club number (.*) is (.*)")]
        public async Task ThenTheNumberOfMembersCountForClubNumberIs(String golfClubNumber, Int32 membersCount)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            await Retry.For(async () =>
                            {
                                this.TestingContext.GetNumberOfMembersReportResponse = await this
                                                                                             .TestingContext.DockerHelper.ReportingClient
                                                                                             .GetNumberOfMembersReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                       golfClubResponse.GolfClubId,
                                                                                                                       CancellationToken.None).ConfigureAwait(false);

                                this.TestingContext.GetNumberOfMembersReportResponse.NumberOfMembers.ShouldBe(membersCount);
                            });
        }

        [When(@"I request a number of members by handicap category report for club number (.*)")]
        public async Task WhenIRequestANumberOfMembersByHandicapCategoryReportForClubNumber(String golfClubNumber)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GetNumberOfMembersByHandicapCategoryReportResponse = await this.TestingContext.DockerHelper.ReportingClient.GetNumberOfMembersByHandicapCategoryReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                                                   golfClubResponse.GolfClubId,
                                                                                                                                                   CancellationToken.None).ConfigureAwait(false);
        }

        [Then(@"I am returned the number of members by handicap category report data successfully")]
        public void ThenIAmReturnedTheNumberOfMembersByHandicapCategoryReportDataSuccessfully()
        {
            this.TestingContext.GetNumberOfMembersByHandicapCategoryReportResponse.ShouldNotBeNull();
        }

        [Then(@"the number of members by handicap category count for club number (.*) handicap category (.*) is (.*)")]
        public async Task ThenTheNumberOfMembersByHandicapCategoryCountForClubNumberHandicapCategoryIs(String golfClubNumber, Int32 handicapCategory, Int32 membersCount)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            await Retry.For(async () =>
                            {
                                this.TestingContext.GetNumberOfMembersByHandicapCategoryReportResponse = await this.TestingContext.DockerHelper.ReportingClient.GetNumberOfMembersByHandicapCategoryReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                                                                                                           golfClubResponse.GolfClubId,
                                                                                                                                                                                                           CancellationToken.None).ConfigureAwait(false);

                                MembersByHandicapCategoryResponse membersByHandicapCategoryResponse =
                                    this.TestingContext.GetNumberOfMembersByHandicapCategoryReportResponse.MembersByHandicapCategoryResponse.SingleOrDefault(h => h.HandicapCategory ==
                                                                                                                                                         handicapCategory);

                                membersByHandicapCategoryResponse.ShouldNotBeNull();
                                membersByHandicapCategoryResponse.NumberOfMembers.ShouldBe(membersCount);
                            });
        }

        [When(@"I request a number of members by time period '(.*)' report for club number (.*)")]
        public async Task WhenIRequestANumberOfMembersByTimePeriodReportForClubNumber(String timePeriod, String golfClubNumber)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GetNumberOfMembersByTimePeriodReportResponse = await this
                                                                                     .TestingContext.DockerHelper.ReportingClient
                                                                                     .GetNumberOfMembersByTimePeriodReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                           golfClubResponse.GolfClubId,
                                                                                                                           timePeriod,
                                                                                                                           CancellationToken.None).ConfigureAwait(false);
        }
        
        [Then(@"I am returned the number of members by time period report data successfully")]
        public void ThenIAmReturnedTheNumberOfMembersByTimePeriodReportDataSuccessfully()
        {
            this.TestingContext.GetNumberOfMembersByTimePeriodReportResponse.ShouldNotBeNull();
        }

        [Then(@"the number of members for the period '(.*)' in the number of members by time period '(.*)' report for club number (.*) is (.*)")]
        public async Task ThenTheNumberOfMembersForThePeriodInTheNumberOfMembersByTimePeriodReportForClubNumberIs(String periodString, String timePeriod, String golfClubNumber, Int32 membersCount)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            await Retry.For(async () =>
                            {
                                this.TestingContext.GetNumberOfMembersByTimePeriodReportResponse = await this
                                                                                                         .TestingContext.DockerHelper.ReportingClient
                                                                                                         .GetNumberOfMembersByTimePeriodReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                                               golfClubResponse.GolfClubId,
                                                                                                                                               timePeriod,
                                                                                                                                               CancellationToken.None).ConfigureAwait(false);
                                String periodValue = this.GetPeriodFromPeriodString(periodString);
                                MembersByTimePeriodResponse membersByTimePeriodResponse =
                                    this.TestingContext.GetNumberOfMembersByTimePeriodReportResponse.MembersByTimePeriodResponse
                                        .SingleOrDefault(h => h.Period == periodValue);

                                membersByTimePeriodResponse.ShouldNotBeNull();
                                membersByTimePeriodResponse.NumberOfMembers.ShouldBe(membersCount);
                            });
        }

        [When(@"I request a number of members by age category report for club number (.*)")]
        public async Task WhenIRequestANumberOfMembersByAgeCategoryReportForClubNumber(String golfClubNumber)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            this.TestingContext.GetNumberOfMembersByAgeCategoryReportResponse = await this
                                                                                     .TestingContext.DockerHelper.ReportingClient
                                                                                     .GetNumberOfMembersByAgeCategoryReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                           golfClubResponse.GolfClubId,
                                                                                                                           CancellationToken.None).ConfigureAwait(false);
        }

        [Then(@"I am returned the number of members by age category report data successfully")]
        public void ThenIAmReturnedTheNumberOfMembersByAgeCategoryReportDataSuccessfully()
        {
            this.TestingContext.GetNumberOfMembersByAgeCategoryReportResponse.ShouldNotBeNull();
        }

        [Then(@"the number of members by age category count for club number (.*) age category '(.*)' is (.*)")]
        public async Task ThenTheNumberOfMembersByAgeCategoryCountForClubNumberAgeCategoryIs(String golfClubNumber, String ageCategory, Int32 membersCount)
        {
            CreateGolfClubResponse golfClubResponse = this.TestingContext.GetCreateGolfClubResponse(golfClubNumber);

            await Retry.For(async () =>
                            {
                                this.TestingContext.GetNumberOfMembersByAgeCategoryReportResponse = await this
                                                                                                         .TestingContext.DockerHelper.ReportingClient
                                                                                                         .GetNumberOfMembersByAgeCategoryReport(this.TestingContext.GolfClubAdministratorToken,
                                                                                                                                               golfClubResponse.GolfClubId,
                                                                                                                                               CancellationToken.None).ConfigureAwait(false);
                                MembersByAgeCategoryResponse membersByAgeCategoryResponse =
                                    this.TestingContext.GetNumberOfMembersByAgeCategoryReportResponse.MembersByAgeCategoryResponse
                                        .SingleOrDefault(h => h.AgeCategory == ageCategory);

                                membersByAgeCategoryResponse.ShouldNotBeNull();
                                membersByAgeCategoryResponse.NumberOfMembers.ShouldBe(membersCount);
                            });
        }

        private String GetPeriodFromPeriodString(String periodString)
        {
            if (periodString == "Today")
            {
                return DateTime.Now.ToString("yyyy-MM-dd");
            }

            if (periodString == "This Month")
            {
                return DateTime.Now.ToString("yyyy-MM");
            }

            if (periodString == "This Year")
            {
                return DateTime.Now.ToString("yyyy");
            }

            throw new Exception($"Period String {periodString} not supported");
        }
    }
}
