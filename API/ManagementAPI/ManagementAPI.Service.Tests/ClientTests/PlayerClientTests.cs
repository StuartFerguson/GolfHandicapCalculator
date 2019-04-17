namespace ManagementAPI.Service.Tests.ClientTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Client;
    using DataTransferObjects;
    using Moq;
    using Newtonsoft.Json;
    using Player;
    using Shouldly;
    using Xunit;

    public class PlayerClientTests
    {
        #region Methods

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task PlayerClient_GetPlayer_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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
            PlayerClient client = new PlayerClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.GetPlayer(passwordToken, CancellationToken.None); }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task PlayerClient_GetPlayer_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.OK,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(PlayerTestData
                                                                                                                                                        .GetPlayerDetailsResponse))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";
            PlayerClient client = new PlayerClient(resolver, httpClient);

            GetPlayerDetailsResponse response = await client.GetPlayer(passwordToken, CancellationToken.None);

            response.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task PlayerClient_GetPlayerMemberships_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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

            PlayerClient client = new PlayerClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.GetPlayerMemberships(passwordToken, CancellationToken.None); }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task PlayerClient_GetPlayerMemberships_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.OK,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(PlayerTestData
                                                                                                                                                        .ClubMembershipResponses))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "mypasswordtoken";
            PlayerClient client = new PlayerClient(resolver, httpClient);

            List<ClubMembershipResponse> response = await client.GetPlayerMemberships(passwordToken, CancellationToken.None);

            response.ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task PlayerClient_RegisterPlayer_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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

            PlayerClient client = new PlayerClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.RegisterPlayer(PlayerTestData.RegisterPlayerRequest, CancellationToken.None); }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task PlayerClient_RegisterPlayer_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.OK,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(PlayerTestData
                                                                                                                                                        .RegisterPlayerResponse))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";

            PlayerClient client = new PlayerClient(resolver, httpClient);

            RegisterPlayerResponse response = await client.RegisterPlayer(PlayerTestData.RegisterPlayerRequest, CancellationToken.None);

            response.ShouldNotBeNull();
            response.PlayerId.ShouldBe(PlayerTestData.AggregateId);
        }

        #endregion
    }
}