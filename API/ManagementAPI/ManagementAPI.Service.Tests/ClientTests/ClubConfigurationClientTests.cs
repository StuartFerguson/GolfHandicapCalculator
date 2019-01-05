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
using ManagementAPI.Service.Tests.ClubConfiguration;
using Newtonsoft.Json;
using Shouldly;

namespace ManagementAPI.Service.Tests.ClientTests
{
    public class ClubConfigurationClientTests
    {
        [Fact]
        public async Task ClubConfigurationClient_CreateClubConfiguration_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(ClubConfigurationTestData.CreateClubConfigurationResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var response = await client.CreateClubConfiguration(ClubConfigurationTestData.CreateClubConfigurationRequest,
                CancellationToken.None);

            response.ShouldNotBeNull();
            response.ClubConfigurationId.ShouldBe(ClubConfigurationTestData.AggregateId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task ClubConfigurationClient_CreateClubConfiguration_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content =new StringContent(String.Empty)
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.CreateClubConfiguration(ClubConfigurationTestData.CreateClubConfigurationRequest,
                    CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ClubConfigurationClient_GetClubConfigurationList_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(ClubConfigurationTestData.GetClubConfigurationListResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var response = await client.GetClubConfigurationList(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Count.ShouldBe(ClubConfigurationTestData.GetClubConfigurationListResponse.Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task ClubConfigurationClient_GetClubConfigurationList_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
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

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);
            
            var exception = Should.Throw(async () =>
            {
                await client.GetClubConfigurationList(passwordToken, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ClubConfigurationClient_GetSingleClubConfiguration_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(ClubConfigurationTestData.GetClubConfigurationListResponse.First()))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var response = await client.GetSingleClubConfiguration(passwordToken, ClubConfigurationTestData.AggregateId, CancellationToken.None);

            response.ShouldNotBeNull();
            response.AddressLine1.ShouldBe(ClubConfigurationTestData.AddressLine1);
            response.AddressLine2.ShouldBe(ClubConfigurationTestData.AddressLine2);
            response.EmailAddress.ShouldBe(ClubConfigurationTestData.EmailAddress);
            response.Id.ShouldBe(ClubConfigurationTestData.AggregateId);
            response.Name.ShouldBe(ClubConfigurationTestData.Name);
            response.PostalCode.ShouldBe(ClubConfigurationTestData.PostalCode);
            response.Region.ShouldBe(ClubConfigurationTestData.Region);
            response.TelephoneNumber.ShouldBe(ClubConfigurationTestData.TelephoneNumber);
            response.Town.ShouldBe(ClubConfigurationTestData.Town);
            response.Website.ShouldBe(ClubConfigurationTestData.Website);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task ClubConfigurationClient_GetSingleClubConfiguration_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
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

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.GetSingleClubConfiguration(passwordToken, ClubConfigurationTestData.AggregateId, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ClubConfigurationClient_AddMeasuredCourseToClubConfiguration_SuccessfulResponse()
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

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            await client.AddMeasuredCourseToClubConfiguration(passwordToken, ClubConfigurationTestData.AggregateId, ClubConfigurationTestData.AddMeasuredCourseToClubRequest, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task ClubConfigurationClient_AddMeasuredCourseToClubConfiguration_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
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

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);
            
            var exception = Should.Throw(async () =>
            {
                await client.AddMeasuredCourseToClubConfiguration(passwordToken, ClubConfigurationTestData.AggregateId, ClubConfigurationTestData.AddMeasuredCourseToClubRequest, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task ClubConfigurationClient_GetPendingMembershipRequests_SuccessfulResponse()
        {
            var fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(ClubConfigurationTestData.GetClubMembershipRequestResponse))
            });

            var httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";
            String passwordToken = "mypasswordtoken";

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var response = await client.GetPendingMembershipRequests(passwordToken, ClubConfigurationTestData.AggregateId, CancellationToken.None);

            response.ShouldNotBeNull();
            response.Count.ShouldBe(ClubConfigurationTestData.GetClubMembershipRequestResponse.Count);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception),typeof(Exception))]
        public async Task ClubConfigurationClient_GetPendingMembershipRequests_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode, Type exceptionType, Type innerExceptionType)
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

            ClubConfigurationClient client = new ClubConfigurationClient(resolver, httpClient);

            var exception = Should.Throw(async () =>
            {
                await client.GetPendingMembershipRequests(passwordToken, ClubConfigurationTestData.AggregateId, CancellationToken.None);
            }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }
    }
}
