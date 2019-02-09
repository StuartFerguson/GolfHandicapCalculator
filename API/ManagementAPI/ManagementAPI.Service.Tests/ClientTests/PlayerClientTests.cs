using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.Client;
using ManagementAPI.Service.DataTransferObjects;
using ManagementAPI.Service.Tests.Player;
using Moq;
using Newtonsoft.Json;
using Shouldly;
using Xunit;

namespace ManagementAPI.Service.Tests.ClientTests
{
    public class PlayerClientTests
    {
        [Fact]
        public async Task PlayerClient_RegisterPlayer_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> { CallBase = true };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content =new StringContent(JsonConvert.SerializeObject(PlayerTestData.RegisterPlayerResponse))
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";

            PlayerClient client = new PlayerClient(resolver, httpClient);

            RegisterPlayerResponse response = await client.RegisterPlayer(PlayerTestData.RegisterPlayerRequest, CancellationToken.None);

            response.ShouldNotBeNull();
            response.PlayerId.ShouldBe(PlayerTestData.AggregateId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(InvalidDataException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task PlayerClient_RegisterPlayer_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
            Type exceptionType, Type innerExceptionType)
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler> {CallBase = true};
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(String.Empty)
            });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = (api) => "http://baseaddress";

            PlayerClient client = new PlayerClient(resolver, httpClient);

            Exception exception =
                Should.Throw(
                    async () =>
                    {
                        await client.RegisterPlayer(PlayerTestData.RegisterPlayerRequest, CancellationToken.None);
                    }, exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }
    }
}
