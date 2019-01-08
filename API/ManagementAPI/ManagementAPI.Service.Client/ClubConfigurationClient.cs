using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.DataTransferObjects;
using Newtonsoft.Json;

namespace ManagementAPI.Service.Client
{
    public class GolfClubClient : ClientProxyBase.ClientProxyBase, IGolfClubClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public GolfClubClient(Func<String, String> baseAddressResolver, HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }
        #endregion

        #region public async Task AddMeasuredCourseToGolfClub(String passwordToken, Guid golfClubId, AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddMeasuredCourseToGolfClub(String passwordToken, Guid golfClubId, AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolClub";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                var httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PutAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error Adding a measured course to Golf Club {golfClubId}.", ex);

                throw exception;
            }
        }
        #endregion

        #region public async Task<CreateGolfClubResponse> CreateGolfClub(CreateGolfClubRequest request, CancellationToken cancellationToken)                
        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateGolfClubResponse> CreateGolfClub(CreateGolfClubRequest request, CancellationToken cancellationToken)
        {
            CreateGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                var httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateGolfClubResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating the new golf club {request.Name}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task<List<GetGolfClubResponse>> GetGolfClubList(String passwordToken, CancellationToken cancellationToken)                
        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubResponse>> GetGolfClubList(String passwordToken, CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<GetGolfClubResponse>>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting a list of Golf Clubs.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(String passwordToken, Guid golfClubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the pending membership requests.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(String passwordToken, Guid golfClubId, CancellationToken cancellationToken)
        {
            List<GetClubMembershipRequestResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/PendingMembershipRequests";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<GetClubMembershipRequestResponse>>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting a list of Pending Membership Requests for Golf Club {golfClubId}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task<GetGolfClubResponse> GetSingleGolfClub(String passwordToken, Guid golfClubId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the single golf club.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetGolfClubResponse> GetSingleGolfClub(String passwordToken, Guid golfClubId, CancellationToken cancellationToken)
        {
            GetGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                var httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting a single golf club {golfClubId}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion
    }
}