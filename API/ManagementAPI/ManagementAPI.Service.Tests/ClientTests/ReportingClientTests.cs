namespace ManagementAPI.Service.Tests.ClientTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using DataTransferObjects.Responses;
    using GolfClub;
    using Moq;
    using Newtonsoft.Json;
    using Shouldly;
    using Xunit;

    public class ReportingClientTests
    {
        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task ReportingClient_GetNumberOfMembersReport_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
                                                                                              Type exceptionType,
                                                                                              Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = statusCode,
                                                                                                  Content = new StringContent(string.Empty)
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.GetNumberOfMembersReport(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None); },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersReport_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.OK,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(GolfClubTestData
                                                                                                                                                        .GetNumberOfMembersReportResponse))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            GetNumberOfMembersReportResponse response = await client.GetNumberOfMembersReport(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            response.NumberOfMembers.ShouldBe(GolfClubTestData.GetNumberOfMembersReportResponse.NumberOfMembers);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task ReportingClient_GetNumberOfMembersByHandicapCategoryReport_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
                                                                                              Type exceptionType,
                                                                                              Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
            {
                CallBase = true
            };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(string.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.GetNumberOfMembersByHandicapCategoryReport(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None); },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByHandicapCategoryReport_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
            {
                CallBase = true
            };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(GolfClubTestData
                                                                                                                                                        .GetNumberOfMembersByHandicapCategoryReportResponse))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            GetNumberOfMembersByHandicapCategoryReportResponse response = await client.GetNumberOfMembersByHandicapCategoryReport(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
            response.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 1).NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByHandicapCategoryReportResponse.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 1).NumberOfMembers);
            response.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 2).NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByHandicapCategoryReportResponse.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 2).NumberOfMembers);
            response.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 3).NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByHandicapCategoryReportResponse.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 3).NumberOfMembers);
            response.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 4).NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByHandicapCategoryReportResponse.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 4).NumberOfMembers);
            response.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 5).NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByHandicapCategoryReportResponse.MembersByHandicapCategoryResponse.Single(x => x.HandicapCategory == 5).NumberOfMembers);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
                                                                                              Type exceptionType,
                                                                                              Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
            {
                CallBase = true
            };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(string.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.GetNumberOfMembersByTimePeriodReport(passwordToken, GolfClubTestData.AggregateId, GolfClubTestData.TimePeriod, CancellationToken.None); },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_ByDay_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.OK,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(GolfClubTestData
                                                                                                                                                        .GetNumberOfMembersByTimePeriodReportResponseByDay))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            GetNumberOfMembersByTimePeriodReportResponse response =
                await client.GetNumberOfMembersByTimePeriodReport(passwordToken, GolfClubTestData.AggregateId, GolfClubTestData.TimePeriodDay, CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);

            response.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-16").NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByTimePeriodReportResponseByDay.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-16")
                                              .NumberOfMembers);
            response.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-17").NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByTimePeriodReportResponseByDay.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08-17")
                                              .NumberOfMembers);

        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_ByMonth_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
            {
                CallBase = true
            };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(GolfClubTestData
                                                                                                                                                        .GetNumberOfMembersByTimePeriodReportResponseByMonth))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            GetNumberOfMembersByTimePeriodReportResponse response =
                await client.GetNumberOfMembersByTimePeriodReport(passwordToken, GolfClubTestData.AggregateId, GolfClubTestData.TimePeriodMonth, CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);

            response.MembersByTimePeriodResponse.Single(x => x.Period == "2019-07").NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByTimePeriodReportResponseByMonth.MembersByTimePeriodResponse.Single(x => x.Period == "2019-07")
                                              .NumberOfMembers);
            response.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08").NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByTimePeriodReportResponseByMonth.MembersByTimePeriodResponse.Single(x => x.Period == "2019-08")
                                              .NumberOfMembers);

        }

        [Fact]
        public async Task ReportingClient_GetNumberOfMembersByTimePeriodReport_ByYear_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
            {
                CallBase = true
            };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(GolfClubTestData
                                                                                                                                                        .GetNumberOfMembersByTimePeriodReportResponseByYear))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ReportingClient client = new ReportingClient(resolver, httpClient);

            GetNumberOfMembersByTimePeriodReportResponse response =
                await client.GetNumberOfMembersByTimePeriodReport(passwordToken, GolfClubTestData.AggregateId, GolfClubTestData.TimePeriodYear, CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);

            response.MembersByTimePeriodResponse.Single(x => x.Period == "2018").NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByTimePeriodReportResponseByYear.MembersByTimePeriodResponse.Single(x => x.Period == "2018")
                                              .NumberOfMembers);
            response.MembersByTimePeriodResponse.Single(x => x.Period == "2019").NumberOfMembers
                    .ShouldBe(GolfClubTestData.GetNumberOfMembersByTimePeriodReportResponseByYear.MembersByTimePeriodResponse.Single(x => x.Period == "2019")
                                              .NumberOfMembers);

        }

    }
    }