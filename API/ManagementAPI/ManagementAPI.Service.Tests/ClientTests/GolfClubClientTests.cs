using ManagementAPI.Service.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using System.Net;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Tests.GolfClub;
using Newtonsoft.Json;
using Shouldly;

namespace ManagementAPI.Service.Tests.ClientTests
{
    using DataTransferObjects.Responses;
    using GolfClubMembership;

    public class GolfClubClientTests
    {
        [Fact]
        public async Task GolfClubClient_CreateGolfClub_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.CreateGolfClubResponse))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            CreateGolfClubResponse response = await client.CreateGolfClub(passwordToken, GolfClubTestData.CreateGolfClubRequest,
                CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_CreateGolfClub_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.CreateGolfClub(passwordToken, GolfClubTestData.CreateGolfClubRequest,
                    CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubList_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetGolfClubListResponse))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            List<GetGolfClubResponse> response = await client.GetGolfClubList(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Count.ShouldBe(GolfClubTestData.GetGolfClubListResponse.Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_GetGolfClubList_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);
            
            Exception exception = Should.Throw(async () =>
            {
                await client.GetGolfClubList(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubUserList_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetGolfClubUserListResponse))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            GetGolfClubUserListResponse response = await client.GetGolfClubUserList(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Users.ShouldNotBeNull();
            response.Users.ShouldNotBeEmpty();
            response.Users.Count.ShouldBe(GolfClubTestData.GetGolfClubUserListResponse.Users.Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task GolfClubClient_GetGolfClubUserList_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.GetGolfClubUserList(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubMembersList_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubMembershipTestData.GetGolfClubMembershipDetailsResponse()))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            List<GetGolfClubMembershipDetailsResponse> response = await client.GetGolfClubMembershipList(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Count.ShouldBe(GolfClubMembershipTestData.GetGolfClubMembershipDetailsResponse().Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_GetGolfClubMembersList_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);
            
            Exception exception = Should.Throw(async () =>
            {
                await client.GetGolfClubMembershipList(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetSingleGolfClub_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetGolfClubListResponse.First()))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            GetGolfClubResponse response = await client.GetSingleGolfClub(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.AddressLine1.ShouldBe(GolfClubTestData.AddressLine1);
            response.AddressLine2.ShouldBe(GolfClubTestData.AddressLine2);
            response.EmailAddress.ShouldBe(GolfClubTestData.EmailAddress);
            response.Id.ShouldBe(GolfClubTestData.AggregateId);
            response.Name.ShouldBe(GolfClubTestData.Name);
            response.PostalCode.ShouldBe(GolfClubTestData.PostalCode);
            response.Region.ShouldBe(GolfClubTestData.Region);
            response.TelephoneNumber.ShouldBe(GolfClubTestData.TelephoneNumber);
            response.Town.ShouldBe(GolfClubTestData.Town);
            response.Website.ShouldBe(GolfClubTestData.Website);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_GetSingleGolfClub_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.GetSingleGolfClub(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_AddMeasuredCourseToGolfClub_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            await client.AddMeasuredCourseToGolfClub(passwordToken, GolfClubTestData.AddMeasuredCourseToClubRequest, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_AddMeasuredCourseToGolfClub_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);
            
            Exception exception = Should.Throw(async () =>
            {
                await client.AddMeasuredCourseToGolfClub(passwordToken, GolfClubTestData.AddMeasuredCourseToClubRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetMeasuredCourses_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetMeasuredCourseListResponse))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            GetMeasuredCourseListResponse measuredCourses = await client.GetMeasuredCourses(passwordToken, CancellationToken.None);
            measuredCourses.GolfClubId.ShouldBe(GolfClubTestData.GetMeasuredCourseListResponse.GolfClubId);
            measuredCourses.MeasuredCourses.Count.ShouldBe(GolfClubTestData.GetMeasuredCourseListResponse.MeasuredCourses.Count);

            measuredCourses.MeasuredCourses.First().MeasuredCourseId.ShouldBe(GolfClubTestData.GetMeasuredCourseListResponse.MeasuredCourses.First().MeasuredCourseId);
            measuredCourses.MeasuredCourses.First().Name.ShouldBe(GolfClubTestData.GetMeasuredCourseListResponse.MeasuredCourses.First().Name);
            measuredCourses.MeasuredCourses.First().StandardScratchScore.ShouldBe(GolfClubTestData.GetMeasuredCourseListResponse.MeasuredCourses.First().StandardScratchScore);
            measuredCourses.MeasuredCourses.First().TeeColour.ShouldBe(GolfClubTestData.GetMeasuredCourseListResponse.MeasuredCourses.First().TeeColour);

        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task GolfClubClient_GetMeasuredCourses_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.GetMeasuredCourses(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_RequestClubMembership_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            await client.RequestClubMembership(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task GolfClubClient_RequestClubMembership_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.RequestClubMembership(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_CreateMatchSecretary_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            await client.CreateMatchSecretary(passwordToken, GolfClubTestData.CreateMatchSecretaryRequest, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task GolfClubClient_CreateMatchSecretary_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.CreateMatchSecretary(passwordToken, GolfClubTestData.CreateMatchSecretaryRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_RegisterGolfClubAdministrator_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            await client.RegisterGolfClubAdministrator(GolfClubTestData.RegisterClubAdministratorRequest, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task GolfClubClient_RegisterGolfClubAdministrator_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.RegisterGolfClubAdministrator(GolfClubTestData.RegisterClubAdministratorRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_AddTournamentDivision_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            await client.AddTournamentDivision(passwordToken, GolfClubTestData.AddTournamentDivisionToGolfClubRequest, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task GolfClubClient_AddTournamentDivision_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
            {
                await client.AddTournamentDivision(passwordToken, GolfClubTestData.AddTournamentDivisionToGolfClubRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }
    }
}
