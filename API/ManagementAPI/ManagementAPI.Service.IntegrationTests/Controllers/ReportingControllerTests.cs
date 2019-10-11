using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ManagementAPI.Service.IntegrationTests.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using Common;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses.v2;
    using Newtonsoft.Json;
    using Shouldly;

    [Collection("TestCollection")]
    public class ReportingControllerTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public ReportingControllerTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task ReportingController_GET_GetMemberHandicapList_MemberHandicapListReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/membershandicaplist";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetMembersHandicapListReportResponse responseObject = JsonConvert.DeserializeObject<GetMembersHandicapListReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.MembersHandicapListReportResponse.ShouldNotBeNull();
            responseObject.MembersHandicapListReportResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingController_GET_GetNumberOfMembersByAgeCategoryReport_NumberOfMembersByAgeCategoryReportReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/numberofmembersbyagecategory";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetNumberOfMembersByAgeCategoryReportResponse responseObject = JsonConvert.DeserializeObject<GetNumberOfMembersByAgeCategoryReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.MembersByAgeCategoryResponse.ShouldNotBeNull();
            responseObject.MembersByAgeCategoryResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingController_GET_GetNumberOfMembersByHandicapCategoryReport_NumberOfMembersByHandicapCategoryReportReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/numberofmembersbyhandicapcategory";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetNumberOfMembersByHandicapCategoryReportResponse responseObject = JsonConvert.DeserializeObject<GetNumberOfMembersByHandicapCategoryReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.MembersByHandicapCategoryResponse.ShouldNotBeNull();
            responseObject.MembersByHandicapCategoryResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingController_GET_GetNumberOfMembersByTimePeriodReport_Day_NumberOfMembersByTimePeriodReportReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/numberofmembersbytimeperiod/day";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetNumberOfMembersByTimePeriodReportResponse responseObject = JsonConvert.DeserializeObject<GetNumberOfMembersByTimePeriodReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.TimePeriod.ShouldBe(TimePeriod.Day);
            responseObject.MembersByTimePeriodResponse.ShouldNotBeNull();
            responseObject.MembersByTimePeriodResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingController_GET_GetNumberOfMembersByTimePeriodReport_Month_NumberOfMembersByTimePeriodReportReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/numberofmembersbytimeperiod/month";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetNumberOfMembersByTimePeriodReportResponse responseObject = JsonConvert.DeserializeObject<GetNumberOfMembersByTimePeriodReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.TimePeriod.ShouldBe(TimePeriod.Month);
            responseObject.MembersByTimePeriodResponse.ShouldNotBeNull();
            responseObject.MembersByTimePeriodResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingController_GET_GetNumberOfMembersByTimePeriodReport_Year_NumberOfMembersByTimePeriodReportReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/numberofmembersbytimeperiod/year";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetNumberOfMembersByTimePeriodReportResponse responseObject = JsonConvert.DeserializeObject<GetNumberOfMembersByTimePeriodReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.TimePeriod.ShouldBe(TimePeriod.Year);
            responseObject.MembersByTimePeriodResponse.ShouldNotBeNull();
            responseObject.MembersByTimePeriodResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingController_GET_GetNumberOfMembersReportResponse_NumberOfMembersReportResponseReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/golfclubs/{TestData.GolfClubId}/numberofmembers";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetNumberOfMembersReportResponse responseObject = JsonConvert.DeserializeObject<GetNumberOfMembersReportResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.GolfClubId.ShouldBe(TestData.GolfClubId);
            responseObject.NumberOfMembers.ShouldBe(100);
        }

        [Fact]
        public async Task ReportingController_GET_GetPlayerScores_PlayerScoresResponseReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.CreateClient();

            String uri = $"api/reports/players/{TestData.PlayerId}/scores";
            client.DefaultRequestHeaders.Add("api-version", "2.0");
            // 2. Act
            HttpResponseMessage response = await client.GetAsync(uri, CancellationToken.None);

            // 3. Assert
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            String responseAsJson = await response.Content.ReadAsStringAsync();

            responseAsJson.ShouldNotBeNullOrEmpty();

            GetPlayerScoresResponse responseObject = JsonConvert.DeserializeObject<GetPlayerScoresResponse>(responseAsJson);
            responseObject.ShouldNotBeNull();
            responseObject.Scores.ShouldNotBeNull();
            responseObject.Scores.ShouldNotBeEmpty();
        }
    }
}