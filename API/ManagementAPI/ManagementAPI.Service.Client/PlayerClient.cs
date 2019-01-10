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
    public class PlayerClient : ClientProxyBase.ClientProxyBase, IPlayerClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        public String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public PlayerClient(Func<String, String> baseAddressResolver, HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }
        #endregion

        #region public async Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Registers the player.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterPlayerResponse> RegisterPlayer(RegisterPlayerRequest request, CancellationToken cancellationToken)
        {
            RegisterPlayerResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/Player";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                var httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<RegisterPlayerResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception =
                    new Exception($"Error creating the new player {request.FirstName} {request.LastName}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region Task RequestClubMembership(String passwordToken, Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubId">The club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RequestClubMembership(String passwordToken, Guid clubId, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Player/ClubMembershipRequest/{clubId}";

            try
            {
                var httpContent = new StringContent(String.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error requesting club {clubId} membership for player.",
                    ex);

                throw exception;
            }
        }
        #endregion

        #region Task ApproveClubMembershipRequest(String passwordToken, Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task ApproveClubMembershipRequest(String passwordToken, Guid playerId, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Player/{playerId}/ClubMembershipRequest/Approve";

            try
            {
                var httpContent = new StringContent(String.Empty, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error approving membership request for player {playerId}.",
                    ex);

                throw exception;
            }
        }
        #endregion

        #region Task RejectClubMembershipRequest(String passwordToken, Guid clubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RejectClubMembershipRequest(String passwordToken, Guid playerId, RejectMembershipRequestRequest request, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/Player/{playerId}/ClubMembershipRequest/Reject";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                var httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error rejecting membership request for player {playerId}.",
                    ex);

                throw exception;
            }
        }
        #endregion
    }
}