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
    using DataTransferObjects.Requests;
    using DataTransferObjects.Responses;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Client.IGolfClubClient" />
    public class GolfClubClient : ClientProxyBase, IGolfClubClient
    {
        #region Fields

        /// <summary>
        /// The last request status code
        /// </summary>
        internal HttpResponseMessage LastRequesthHttpResponseMessage;

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
        public GolfClubClient(Func<String, String> baseAddressResolver,
                              HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the measured course to golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddMeasuredCourseToGolfClub(String accessToken,
                                                      Guid golfClubId,
                                                      AddMeasuredCourseToClubRequest request,
                                                      CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/AddMeasuredCourse";

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
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error Adding a measured course to Golf Club.", ex);

                throw exception;
            }
        }

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
            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/AddTournamentDivision";

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
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error Adding a tournament divisionto Golf Club.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateGolfClubResponse> CreateGolfClub(String accessToken,
                                                                 CreateGolfClubRequest request,
                                                                 CancellationToken cancellationToken)
        {
            CreateGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<CreateGolfClubResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error creating the new golf club {request.Name}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Creates the match secretary.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task CreateMatchSecretary(String accessToken,
                                               Guid golfClubId,
                                               CreateMatchSecretaryRequest request,
                                               CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/CreateMatchSecretary";

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
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error creating the new match secretary.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Gets the golf club membership list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubMembershipDetailsResponse>> GetGolfClubMembershipList(String accessToken,
                                                                                                Guid golfClubId,
                                                                                                CancellationToken cancellationToken)
        {
            List<GetGolfClubMembershipDetailsResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/MembersList";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<GetGolfClubMembershipDetailsResponse>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting a golf club members list.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetGolfClubUserListResponse> GetGolfClubUserList(String accessToken,
                                                                           Guid golfClubId,
                                                                           CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/Users";
            GetGolfClubUserListResponse response = null;

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetGolfClubUserListResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error requesting Golf Club User List.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the measured courses.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetMeasuredCourseListResponse> GetMeasuredCourses(String accessToken,
                                                                            Guid golfClubId,
                                                                            CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/MeasuredCourses";
            GetMeasuredCourseListResponse response = new GetMeasuredCourseListResponse();

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetMeasuredCourseListResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error requesting Golf Club measured courses.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the single golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetGolfClubResponse> GetSingleGolfClub(String accessToken,
                                                                 Guid golfClubId,
                                                                 CancellationToken cancellationToken)
        {
            GetGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting a single golf club.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RegisterGolfClubAdministrator(RegisterClubAdministratorRequest request,
                                                        CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/RegisterGolfClubAdministrator";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error registering a Golf Club Administrator.", ex);

                throw exception;
            }
        }

        #endregion
    }
}