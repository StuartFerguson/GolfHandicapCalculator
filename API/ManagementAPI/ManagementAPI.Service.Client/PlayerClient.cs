namespace ManagementAPI.Service.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects;
    using Newtonsoft.Json;

    public class PlayerClient : ClientProxyBase, IPlayerClient
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
        public PlayerClient(Func<String, String> baseAddressResolver,
                            HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetPlayerDetailsResponse> GetPlayer(String passwordToken,
                                                              CancellationToken cancellationToken)
        {
            GetPlayerDetailsResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/Player";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetPlayerDetailsResponse>(content);
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
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<ClubMembershipResponse>> GetPlayerMemberships(String passwordToken,
                                                                             CancellationToken cancellationToken)
        {
            List<ClubMembershipResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/Player/Memberships";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<ClubMembershipResponse>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting memberships for player.", ex);

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

            String requestUri = $"{this.BaseAddress}/api/Player";

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
                Exception exception = new Exception($"Error creating the new player {request.FirstName} {request.LastName}.", ex);

                throw exception;
            }

            return response;
        }

        #endregion
    }
}