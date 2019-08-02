namespace ManagementAPI.Service.Tests.ClientTests
{
    using System;
    using System.Collections.Generic;
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
    using Tournament;
    using Xunit;

    public class TournamentClientTests
    {
        #region Methods

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task TournamentClient_CancelTournament_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
                                               {
                                                   await client.CancelTournament(passwordToken,
                                                                                 GolfClubTestData.AggregateId,
                                                                                 TournamentTestData.AggregateId,
                                                                                 TournamentTestData.CancelTournamentRequest,
                                                                                 CancellationToken.None);
                                               },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_CancelTournament_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.NoContent,
                                                                                                  Content = new StringContent(string.Empty)
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.CancelTournament(passwordToken,
                                          GolfClubTestData.AggregateId,
                                          TournamentTestData.AggregateId,
                                          TournamentTestData.CancelTournamentRequest,
                                          CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task TournamentClient_CompleteTournament_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
                                               {
                                                   await client.CompleteTournament(passwordToken,
                                                                                   GolfClubTestData.AggregateId,
                                                                                   TournamentTestData.AggregateId,
                                                                                   CancellationToken.None);
                                               },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_CompleteTournament_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.NoContent,
                                                                                                  Content = new StringContent(string.Empty)
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.CompleteTournament(passwordToken, GolfClubTestData.AggregateId, TournamentTestData.AggregateId, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task TournamentClient_CreateTournament_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
                                               {
                                                   await client.CreateTournament(passwordToken,
                                                                                 GolfClubTestData.AggregateId,
                                                                                 TournamentTestData.CreateTournamentRequest,
                                                                                 CancellationToken.None);
                                               },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_CreateTournament_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.OK,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(TournamentTestData
                                                                                                                                                        .CreateTournamentResponse))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            CreateTournamentResponse response =
                await client.CreateTournament(passwordToken, GolfClubTestData.AggregateId, TournamentTestData.CreateTournamentRequest, CancellationToken.None);

            response.ShouldNotBeNull();
            response.TournamentId.ShouldBe(TournamentTestData.AggregateId);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task TournamentClient_GetTournamentList_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            Exception exception = Should.Throw(async () => { await client.GetTournamentList(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None); },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_GetTournamentList_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.NoContent,
                                                                                                  Content =
                                                                                                      new StringContent(JsonConvert.SerializeObject(TournamentTestData
                                                                                                                                                        .GetTournamentListResponse))
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.GetTournamentList(passwordToken, GolfClubTestData.AggregateId, CancellationToken.None);
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest, typeof(Exception), typeof(InvalidOperationException))]
        [InlineData(HttpStatusCode.Unauthorized, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.Forbidden, typeof(Exception), typeof(UnauthorizedAccessException))]
        [InlineData(HttpStatusCode.NotFound, typeof(Exception), typeof(KeyNotFoundException))]
        [InlineData(HttpStatusCode.InternalServerError, typeof(Exception), typeof(Exception))]
        [InlineData(HttpStatusCode.BadGateway, typeof(Exception), typeof(Exception))]
        public async Task TournamentClient_ProduceTournamentResult_FailedHttpCall_ErrorThrown(HttpStatusCode statusCode,
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
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            Exception exception = Should.Throw(async () =>
                                               {
                                                   await client.ProduceTournamentResult(passwordToken,
                                                                                        GolfClubTestData.AggregateId,
                                                                                        TournamentTestData.AggregateId,
                                                                                        CancellationToken.None);
                                               },
                                               exceptionType);

            exception.InnerException.ShouldBeOfType(innerExceptionType);
        }

        [Fact]
        public async Task TournamentClient_ProduceTournamentResult_SuccessfulResponse()
        {
            Mock<FakeHttpMessageHandler> fakeHttpMessageHandler = new Mock<FakeHttpMessageHandler>
                                                                  {
                                                                      CallBase = true
                                                                  };
            fakeHttpMessageHandler.Setup(f => f.Send(It.IsAny<HttpRequestMessage>())).Returns(new HttpResponseMessage
                                                                                              {
                                                                                                  StatusCode = HttpStatusCode.NoContent,
                                                                                                  Content = new StringContent(string.Empty)
                                                                                              });

            HttpClient httpClient = new HttpClient(fakeHttpMessageHandler.Object);
            Func<String, String> resolver = api => "http://baseaddress";
            String passwordToken = "passwordToken";

            TournamentClient client = new TournamentClient(resolver, httpClient);

            await client.ProduceTournamentResult(passwordToken, GolfClubTestData.AggregateId, TournamentTestData.AggregateId, CancellationToken.None);
        }

        #endregion
    }
}