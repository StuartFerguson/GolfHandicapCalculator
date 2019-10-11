namespace ManagementAPI.Service.Client.v2
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses.v2;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Client.v2.IPlayerClient" />
    public class PlayerClient : ClientProxyBase, IPlayerClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private readonly String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public PlayerClient(Func<String, String> baseAddressResolver,
                            HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");

            // Add the API version header
            if (this.HttpClient.DefaultRequestHeaders.Contains("api-version") == false)
            {
                this.HttpClient.DefaultRequestHeaders.Add("api-version", "2.0");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetPlayerResponse> GetPlayer(String accessToken,
                                                       Guid playerId,
                                                       CancellationToken cancellationToken)
        {
            GetPlayerResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/players/{playerId}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetPlayerResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting player details.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the player memberships.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<ClubMembershipResponse>> GetPlayerMemberships(String accessToken,
                                                                             Guid playerId,
                                                                             CancellationToken cancellationToken)
        {
            List<ClubMembershipResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/players/{playerId}?includeMemberships=true";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                GetPlayerResponse playerResponse = JsonConvert.DeserializeObject<GetPlayerResponse>(content);

                response = playerResponse.ClubMemberships;
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting player memberships details.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the tournaments signed up for.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<SignedUpTournamentResponse>> GetTournamentsSignedUpFor(String accessToken,
                                                                                      Guid playerId,
                                                                                      CancellationToken cancellationToken)
        {
            List<SignedUpTournamentResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/players/{playerId}?includeTournamentSignups=true";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                GetPlayerResponse playerResponse = JsonConvert.DeserializeObject<GetPlayerResponse>(content);

                response = playerResponse.SignedUpTournaments;
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting player tournament signup details.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest request,
                                                                 CancellationToken cancellationToken)
        {
            RegisterPlayerResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/players";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<RegisterPlayerResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating the new player {request.GivenName} {request.FamilyName}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RequestClubMembershipResponse> RequestClubMembership(String accessToken,
                                                                               Guid playerId,
                                                                               Guid golfClubId,
                                                                               CancellationToken cancellationToken)
        {
            RequestClubMembershipResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/players/{playerId}";

            try
            {
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<RequestClubMembershipResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error requesting Golf Club membership.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Signs up player for tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task SignInForTournament(String accessToken,
                                                    Guid playerId,
                                                    Guid tournamentId,
                                                    CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/players/{playerId}/tournaments/{tournamentId}";

            try
            {
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error signing player up for tournament {tournamentId}.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task RecordPlayerScore(String accessToken,
                                            Guid playerId,
                                            Guid tournamentId,
                                            RecordPlayerTournamentScoreRequest request,
                                            CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/players/{playerId}/tournaments/{tournamentId}/scores";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error recording tournament {tournamentId} score for player.", ex);

                throw exception;
            }
        }

        #endregion
    }
}