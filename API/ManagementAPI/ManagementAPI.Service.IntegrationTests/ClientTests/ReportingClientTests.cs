namespace ManagementAPI.Service.IntegrationTests.ClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client.v2;
    using Common;
    using DataTransferObjects.Responses.v2;
    using Shouldly;
    using Xunit;

    [Collection("TestCollection")]
    public class ReportingClientTests : IClassFixture<ManagmentApiWebFactory<Startup>>
    {
        private readonly ManagmentApiWebFactory<Startup> WebApplicationFactory;

        public ReportingClientTests(ManagmentApiWebFactory<Startup> webApplicationFactory)
        {
            this.WebApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async Task ReportingClient_GetMembersHandicapListReport_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetMembersHandicapListReportResponse getMembersHandicapListReport =
                await reportingClient.GetMembersHandicapListReport(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            getMembersHandicapListReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getMembersHandicapListReport.MembersHandicapListReportResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByAgeCategoryReport_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetNumberOfMembersByAgeCategoryReportResponse getNumberOfMembersByAgeCategoryReport =
                await reportingClient.GetNumberOfMembersByAgeCategoryReport(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            getNumberOfMembersByAgeCategoryReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getNumberOfMembersByAgeCategoryReport.MembersByAgeCategoryResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByHandicapCategoryReport_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetNumberOfMembersByHandicapCategoryReportResponse getNumberOfMembersByHandicapCategoryReport =
                await reportingClient.GetNumberOfMembersByHandicapCategoryReport(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            getNumberOfMembersByHandicapCategoryReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getNumberOfMembersByHandicapCategoryReport.MembersByHandicapCategoryResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_Day_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetNumberOfMembersByTimePeriodReportResponse getNumberOfMembersByTimePeriodReport =
                await reportingClient.GetNumberOfMembersByTimePeriodReport(token, TestData.PlayerId, TimePeriod.Day.ToString().ToLower(), CancellationToken.None);

            // 3. Assert
            getNumberOfMembersByTimePeriodReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getNumberOfMembersByTimePeriodReport.TimePeriod.ShouldBe(TimePeriod.Day);
            getNumberOfMembersByTimePeriodReport.MembersByTimePeriodResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_Month_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetNumberOfMembersByTimePeriodReportResponse getNumberOfMembersByTimePeriodReport =
                await reportingClient.GetNumberOfMembersByTimePeriodReport(token, TestData.PlayerId, TimePeriod.Month.ToString().ToLower(), CancellationToken.None);

            // 3. Assert
            getNumberOfMembersByTimePeriodReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getNumberOfMembersByTimePeriodReport.TimePeriod.ShouldBe(TimePeriod.Month);
            getNumberOfMembersByTimePeriodReport.MembersByTimePeriodResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_Year_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetNumberOfMembersByTimePeriodReportResponse getNumberOfMembersByTimePeriodReport =
                await reportingClient.GetNumberOfMembersByTimePeriodReport(token, TestData.PlayerId, TimePeriod.Year.ToString().ToLower(), CancellationToken.None);

            // 3. Assert
            getNumberOfMembersByTimePeriodReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getNumberOfMembersByTimePeriodReport.TimePeriod.ShouldBe(TimePeriod.Year);
            getNumberOfMembersByTimePeriodReport.MembersByTimePeriodResponse.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersReport_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddGolfClubAdministrator().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetNumberOfMembersReportResponse getNumberOfMembersReport =
                await reportingClient.GetNumberOfMembersReport(token, TestData.PlayerId, CancellationToken.None);

            // 3. Assert
            getNumberOfMembersReport.GolfClubId.ShouldBe(TestData.GolfClubId);
            getNumberOfMembersReport.NumberOfMembers.ShouldNotBe(0);
        }

        [Fact]
        public async Task ReportingClient_GetPlayerScores_ReportDataReturned()
        {
            // 1. Arrange
            HttpClient client = this.WebApplicationFactory.AddPlayer().CreateClient();
            Func<String, String> resolver = api => "http://localhost";
            IReportingClient reportingClient = new ReportingClient(resolver, client);

            String token =
                "eyJhbGciOiJSUzI1NiIsImtpZCI6ImVhZDQyNGJjNjI5MzU0NGM4MGFmZThhMDk2MzEyNjU2IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NzAyODk3MDksImV4cCI6MTU3MDI5MzMwOSwiaXNzIjoiaHR0cDovLzE5Mi4xNjguMS4xMzI6NTAwMSIsImF1ZCI6WyJodHRwOi8vMTkyLjE2OC4xLjEzMjo1MDAxL3Jlc291cmNlcyIsIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl0sImNsaWVudF9pZCI6ImdvbGZoYW5kaWNhcC50ZXN0ZGF0YWdlbmVyYXRvciIsInNjb3BlIjpbIm1hbmFnZW1lbnRhcGkiLCJzZWN1cmlydHlzZXJ2aWNlYXBpIl19.vLfs2bOMXshW93nw5TTOqd6NPGNYpcrhcom8yZoYc9WGSuYH48VqM5BdbodEukNNJmgbV9wUVgoj1uGztlFfHGFA_q6IQfd3xZln_LIxju6ZNZs8qUyRXDTGxu0dlfF8STLfBUq469SsY9eNi1hBYFyNxl963OfKqDSHAdeBg9yNlwnbky1Tnsxobu9W33fLcjH0KoutlwTFV51UFUEKCBk0w1zsjaDVZacETn74t56y0CvMS7ZSN2_yyunq4JvoUsh3xM5lQ-gl23eQyo6l4QE4wukCS7U_Zr2dg8-EF63VKiCH-ZD49M76TD9kIIge-XIgHqa2Xf3S-FpLxXfEqw";

            // 2. Act
            GetPlayerScoresResponse getPlayerScores =
                await reportingClient.GetPlayerScores(token, TestData.PlayerId,10, CancellationToken.None);

            // 3. Assert
            getPlayerScores.Scores.ShouldNotBeEmpty();
        }
    }
}