using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Newtonsoft.Json;

namespace ManagementAPI.Service.Client
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Client.ITournamentClient" />
    public class TournamentClient : ClientProxyBase.ClientProxyBase, ITournamentClient
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
        public TournamentClient(Func<String, String> baseAddressResolver, HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }
        #endregion

        #region public async Task<CreateTournamentResponse> CreateTournament(String passwordToken, CreateTournamentRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateTournamentResponse> CreateTournament(String passwordToken, CreateTournamentRequest request, CancellationToken cancellationToken)
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
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateTournamentResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating the new tournament {request.Name}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task RecordPlayerScore(String passwordToken, Guid tournamentId, RecordMemberTournamentScoreRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Records the player score.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RecordPlayerScore(String passwordToken, Guid tournamentId, RecordMemberTournamentScoreRequest request, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/RecordMemberScore";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new Exception($"Error recording tournament {tournamentId} score for player {request.MemberId}.",
                        ex);

                throw exception;
            }
        }
        #endregion

        #region public async Task CompleteTournament(String passwordToken, Guid tournamentId, CancellationToken cancellationToken)        
        /// <summary>
        /// Completes the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task CompleteTournament(String passwordToken, Guid tournamentId, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/Complete";

            try
            {
                StringContent httpContent = new StringContent(String.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error completing tournament {tournamentId}.", ex);

                throw exception;
            }
        }
        #endregion

        #region public async Task CancelTournament(String passwordToken, Guid tournamentId, CancelTournamentRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task CancelTournament(String passwordToken, Guid tournamentId, CancelTournamentRequest request, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/Cancel";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error cancelling tournament {tournamentId}.", ex);

                throw exception;
            }
        }
        #endregion

        #region public async Task ProduceTournamentResult(String passwordToken, Guid tournamentId, CancellationToken cancellationToken)        
        /// <summary>
        /// Produces the tournament result.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task ProduceTournamentResult(String passwordToken, Guid tournamentId, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Tournament/{tournamentId}/ProduceResult";

            try
            {
                StringContent httpContent = new StringContent(String.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error producing result for tournament {tournamentId}.", ex);

                throw exception;
            }
        }
        #endregion
    }
}