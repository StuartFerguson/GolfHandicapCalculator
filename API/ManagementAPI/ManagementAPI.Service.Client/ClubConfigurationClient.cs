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
    public class ClubConfigurationClient : ClientProxyBase.ClientProxyBase, IClubConfigurationClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClubConfigurationClient" /> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public ClubConfigurationClient(Func<String, String> baseAddressResolver, HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }
        #endregion

        #region public async Task AddMeasuredCourseToClubConfiguration(String passwordToken, Guid clubConfigurationId, AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Adds the measured course to club configuration.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddMeasuredCourseToClubConfiguration(String passwordToken, Guid clubConfigurationId, AddMeasuredCourseToClubRequest request, CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/ClubConfiguration";

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
                Exception exception = new Exception($"Error getting a list of Clubs.", ex);

                throw exception;
            }
        }
        #endregion

        #region public async Task<CreateClubConfigurationResponse> CreateClubConfiguration(CreateClubConfigurationRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Creates the club configuration.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateClubConfigurationResponse> CreateClubConfiguration(CreateClubConfigurationRequest request, CancellationToken cancellationToken)
        {
            CreateClubConfigurationResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/ClubConfiguration";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                var httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                var httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating the new club configuration {request.Name}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task<List<GetClubConfigurationResponse>> GetClubConfigurationList(String passwordToken, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the club configuration list.
        /// </summary>
        /// <param name="passwordToken"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetClubConfigurationResponse>> GetClubConfigurationList(String passwordToken, CancellationToken cancellationToken)
        {
            List<GetClubConfigurationResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/ClubConfiguration";

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
                response = JsonConvert.DeserializeObject<List<GetClubConfigurationResponse>>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting a list of Clubs.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(String passwordToken, Guid clubConfigurationId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the pending membership requests.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<GetClubMembershipRequestResponse>> GetPendingMembershipRequests(String passwordToken, Guid clubConfigurationId, CancellationToken cancellationToken)
        {
            List<GetClubMembershipRequestResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/ClubConfiguration/{clubConfigurationId}/PendingMembershipRequests";

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
                Exception exception = new Exception($"Error getting a list of Clubs.", ex);

                throw exception;
            }

            return response;
        }
        #endregion

        #region public async Task<GetClubConfigurationResponse> GetSingleClubConfiguration(String passwordToken, Guid clubConfigurationId, CancellationToken cancellationToken)        
        /// <summary>
        /// Gets the single club configuration.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="clubConfigurationId">The club configuration identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetClubConfigurationResponse> GetSingleClubConfiguration(String passwordToken, Guid clubConfigurationId, CancellationToken cancellationToken)
        {
            GetClubConfigurationResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/ClubConfiguration/{clubConfigurationId}";

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
                response = JsonConvert.DeserializeObject<GetClubConfigurationResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting a single club {clubConfigurationId}.", ex);

                throw exception;
            }

            return response;
        }
        #endregion
    }
}