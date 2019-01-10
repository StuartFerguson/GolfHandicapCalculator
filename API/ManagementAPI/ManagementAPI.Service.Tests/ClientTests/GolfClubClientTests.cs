using ManagementAPI.Service.Client;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using System.Net;
using ManagementAPI.Service.Tests.GolfClub;
using Newtonsoft.Json;
using Shouldly;

namespace ManagementAPI.Service.Tests.ClientTests
{
    public class GolfClubClientTests
    {
        [Fact]
        public async Task GolfClubClient_CreateGolfClub_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.CreateGolfClubResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var response = await client.CreateGolfClub(passwordToken, GolfClubTestData.CreateGolfClubRequest,
                CancellationToken.None);

            response.ShouldNotBeNull();
            response.GolfClubId.ShouldBe(GolfClubTestData.AggregateId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_CreateGolfClub_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.CreateGolfClub(passwordToken, GolfClubTestData.CreateGolfClubRequest,
                    CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetGolfClubList_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetGolfClubListResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var response = await client.GetGolfClubList(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Count.ShouldBe(GolfClubTestData.GetGolfClubListResponse.Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_GetGolfClubList_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);
            
            var exception = Should.Throw(async () =>
            {
                await client.GetGolfClubList(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetSingleGolfClub_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetGolfClubListResponse.First()))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var response = await client.GetSingleGolfClub(passwordToken, CancellationToken.None);

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
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_GetSingleGolfClub_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.GetSingleGolfClub(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_AddMeasuredCourseToGolfClub_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NoContent,
                Content = new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            await client.AddMeasuredCourseToGolfClub(passwordToken, GolfClubTestData.AddMeasuredCourseToClubRequest, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_AddMeasuredCourseToGolfClub_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);
            
            var exception = Should.Throw(async () =>
            {
                await client.AddMeasuredCourseToGolfClub(passwordToken, GolfClubTestData.AddMeasuredCourseToClubRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task GolfClubClient_GetPendingMembershipRequests_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(GolfClubTestData.GetClubMembershipRequestResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var response = await client.GetPendingMembershipRequests(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Count.ShouldBe(GolfClubTestData.GetClubMembershipRequestResponse.Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task GolfClubClient_GetPendingMembershipRequests_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            GolfClubClient client = new GolfClubClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.GetPendingMembershipRequests(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }
    }
}
