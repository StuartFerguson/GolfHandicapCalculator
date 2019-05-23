namespace ManagementAPI.Service.Client
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects;
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Client.ITournamentClient" />
    public class TournamentClient : ClientProxyBase, ITournamentClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        public String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TournamentClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        public TournamentClient(Func<String, String> baseAddressResolver,
                                HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task CancelTournament(String passwordToken,
                                           Guid tournamentId,
                                           CancelTournamentRequest request,
                                           CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/Cancel";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error cancelling tournament {tournamentId}.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Completes the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task CompleteTournament(String passwordToken,
                                             Guid tournamentId,
                                             CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/Complete";

            try
            {
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error completing tournament {tournamentId}.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateTournamentResponse> CreateTournament(String passwordToken,
                                                                     CreateTournamentRequest request,
                                                                     CancellationToken cancellationToken)
        {
            CreateTournamentResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/Tournament";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateTournamentResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating the new tournament {request.Name}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Produces the tournament result.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task ProduceTournamentResult(String passwordToken,
                                                  Guid tournamentId,
                                                  CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/ProduceResult";

            try
            {
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error producing result for tournament {tournamentId}.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RecordPlayerScore(String passwordToken,
                                            Guid tournamentId,
                                            RecordPlayerTournamentScoreRequest request,
                                            CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/RecordPlayerScore";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error recording tournament {tournamentId} score for player.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Signs up player for tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task SignUpPlayerForTournament(String passwordToken,
                                                    Guid tournamentId,
                                                    CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/SignUp";

            try
            {
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error signing player up for tournament {tournamentId}.", ex);

                throw exception;
            }
        }

        #endregion
    }
}