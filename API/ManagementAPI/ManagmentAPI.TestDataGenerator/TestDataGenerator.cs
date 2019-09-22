namespace ManagmentAPI.TestDataGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ManagementAPI.Service.Client;
    using ManagementAPI.Service.DataTransferObjects;
    using ManagementAPI.Service.DataTransferObjects.Requests;
    using ManagementAPI.Service.DataTransferObjects.Responses;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagmentAPI.TestDataGenerator.ITestDataGenerator" />
    public class TestDataGenerator : ITestDataGenerator
    {
        #region Fields

        /// <summary>
        /// The access tokens
        /// </summary>
        private readonly Dictionary<String, TokenResponse> AccessTokens;

        /// <summary>
        /// The address resolver
        /// </summary>
        private readonly Func<String, String> AddressResolver;

        /// <summary>
        /// The golf club client
        /// </summary>
        private readonly IGolfClubClient GolfClubClient;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private readonly HttpClient HttpClient;

        /// <summary>
        /// The player client
        /// </summary>
        private readonly IPlayerClient PlayerClient;

        /// <summary>
        /// The tournament client
        /// </summary>
        private readonly ITournamentClient TournamentClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataGenerator"/> class.
        /// </summary>
        /// <param name="golfClubClient">The golf club client.</param>
        /// <param name="playerClient">The player client.</param>
        /// <param name="tournamentClient">The tournament client.</param>
        /// <param name="addressResolver">The address resolver.</param>
        public TestDataGenerator(IGolfClubClient golfClubClient,
                                 IPlayerClient playerClient,
                                 ITournamentClient tournamentClient,
                                 Func<String, String> addressResolver)
        {
            this.GolfClubClient = golfClubClient;
            this.PlayerClient = playerClient;
            this.TournamentClient = tournamentClient;
            this.HttpClient = new HttpClient();
            this.AddressResolver = addressResolver;
            this.AccessTokens = new Dictionary<String, TokenResponse>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the tournament division.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddTournamentDivision(String accessToken,
                                                Guid golfClubId,
                                                AddTournamentDivisionToGolfClubRequest request,
                                                CancellationToken cancellationToken)
        {
            await this.GolfClubClient.AddTournamentDivision(accessToken, golfClubId, request, cancellationToken);
        }

        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="addMeasuredCourseToClubRequest">The add measured course to club request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddMeasuredCourseToGolfClub(String accessToken,
                                                      Guid golfClubId,
                                                      AddMeasuredCourseToClubRequest addMeasuredCourseToClubRequest,
                                                      CancellationToken cancellationToken)
        {
            addMeasuredCourseToClubRequest.MeasuredCourseId = Guid.NewGuid();

            await this.GolfClubClient.AddMeasuredCourseToGolfClub(accessToken, golfClubId, addMeasuredCourseToClubRequest, cancellationToken);
        }

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="accessToken">The password token.</param>
        /// <param name="createGolfClubRequest">The create golf club request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateGolfClubResponse> CreateGolfClub(String accessToken,
                                                                 CreateGolfClubRequest createGolfClubRequest,
                                                                 CancellationToken cancellationToken)
        {
            return await this.GolfClubClient.CreateGolfClub(accessToken, createGolfClubRequest, cancellationToken);
        }

        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="accessToken">The password token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="createTournamentRequest">The create tournament request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateTournamentResponse> CreateTournament(String accessToken,
                                                                     Guid golfClubId,
                                                                     CreateTournamentRequest createTournamentRequest,
                                                                     CancellationToken cancellationToken)
        {
            return await this.TournamentClient.CreateTournament(accessToken, golfClubId, createTournamentRequest, cancellationToken);
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns></returns>
        public async Task<String> GetToken(TokenType tokenType,
                                           String clientId,
                                           String clientSecret,
                                           String userName = "",
                                           String password = "",
                                           List<String> scopes = null)
        {
            StringBuilder queryString = new StringBuilder();
            if (tokenType == TokenType.Client)
            {
                queryString.Append("grant_type=client_credentials");
                queryString.Append($"&client_id={clientId}");
                queryString.Append($"&client_secret={clientSecret}");
            }
            else if (tokenType == TokenType.Password)
            {
                queryString.Append("grant_type=password");
                queryString.Append($"&client_id={clientId}");
                queryString.Append($"&client_secret={clientSecret}");
                queryString.Append($"&username={userName}");

                queryString.Append($"&password={password}");

                if (scopes != null && scopes.Count > 0)
                {
                    String scopeString = "";
                    foreach (String scope in scopes) scopeString = $"{scopeString} {scope}";

                    queryString.Append($"&scope={scopeString}");
                }
            }

            String requestUri = $"{this.AddressResolver("SecurityService")}/connect/token";

            HttpResponseMessage httpResponse = await this.MakeHttpPost(requestUri, queryString.ToString(), mediaType:"application/x-www-form-urlencoded");

            TokenResponse token = await this.GetResponseObject<TokenResponse>(httpResponse);

            if (tokenType == TokenType.Password)
            {
                this.AccessTokens.Add(userName, token);
            }

            return token.AccessToken;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public String GetToken(String userName)
        {
            return this.AccessTokens.Single(a => a.Key == userName).Value.AccessToken;
        }

        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="recordMemberTournamentScoreRequest">The record member tournament score request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RecordPlayerScore(String accessToken,
                                            Guid playerId,
                                            Guid tournamentId,
                                            RecordPlayerTournamentScoreRequest recordMemberTournamentScoreRequest,
                                            CancellationToken cancellationToken)
        {
            await this.PlayerClient.RecordPlayerScore(accessToken, playerId, tournamentId, recordMemberTournamentScoreRequest, cancellationToken);
        }

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="registerClubAdministratorRequest">The register club administrator request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RegisterGolfClubAdministrator(RegisterClubAdministratorRequest registerClubAdministratorRequest,
                                                        CancellationToken cancellationToken)
        {
            await this.GolfClubClient.RegisterGolfClubAdministrator(registerClubAdministratorRequest, cancellationToken);
        }

        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="registerPlayerRequest">The register player request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest registerPlayerRequest,
                                                                 CancellationToken cancellationToken)
        {
            return await this.PlayerClient.RegisterPlayer(registerPlayerRequest, cancellationToken);
        }

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RequestClubMembership(String accessToken,
                                                Guid playerId,
                                                Guid golfClubId,
                                                CancellationToken cancellationToken)
        {
            await this.PlayerClient.RequestClubMembership(accessToken, playerId, golfClubId, cancellationToken);
        }

        /// <summary>
        /// Signs up player for tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task SignUpPlayerForTournament(String accessToken,
                                                    Guid playerId,
                                                    Guid tournamentId,
                                                    CancellationToken cancellationToken)
        {
            await this.PlayerClient.SignUpPlayerForTournament(accessToken, playerId, tournamentId, cancellationToken);
        }

        /// <summary>
        /// Gets the response object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage">The response message.</param>
        /// <returns></returns>
        private async Task<T> GetResponseObject<T>(HttpResponseMessage responseMessage)
        {
            T result = default;

            result = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));

            return result;
        }

        /// <summary>
        /// Makes the HTTP post.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="requestObject">The request object.</param>
        /// <param name="bearerToken">The bearer token.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> MakeHttpPost<T>(String requestUri,
                                                                T requestObject,
                                                                String bearerToken = "",
                                                                String mediaType = "application/json")
        {
            HttpResponseMessage result = null;
            StringContent httpContent = null;
            if (requestObject is String)
            {
                httpContent = new StringContent(requestObject.ToString(), Encoding.UTF8, mediaType);
            }
            else
            {
                String requestSerialised = JsonConvert.SerializeObject(requestObject);
                httpContent = new StringContent(requestSerialised, Encoding.UTF8, mediaType);
            }

            using(HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(bearerToken))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                result = await client.PostAsync(requestUri, httpContent, CancellationToken.None).ConfigureAwait(false);
            }

            return result;
        }

        #endregion
    }
}