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

    public class GolfClubClient : ClientProxyBase, IGolfClubClient
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
        /// <param name="passwordToken">The password token.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task AddMeasuredCourseToGolfClub(String passwordToken,
                                                      AddMeasuredCourseToClubRequest request,
                                                      CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/AddMeasuredCourse";

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
                Exception exception = new Exception("Error Adding a measured course to Golf Club.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Creates the golf club.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateGolfClubResponse> CreateGolfClub(String passwordToken,
                                                                 CreateGolfClubRequest request,
                                                                 CancellationToken cancellationToken)
        {
            CreateGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/Create";

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
        /// Gets the golf club list.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubResponse>> GetGolfClubList(String passwordToken,
                                                                     CancellationToken cancellationToken)
        {
            List<GetGolfClubResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/List";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<GetGolfClubResponse>>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting a list of Golf Clubs.", ex);

                throw exception;
            }

            return response;
        }

        public async Task<List<GolfClubMembershipDetails>> GetGolfClubMembershipList(String passwordToken,
                                                                                     CancellationToken cancellationToken)
        {
            List<GolfClubMembershipDetails> response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub/MembersList";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<List<GolfClubMembershipDetails>>(content);
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
        /// Gets the single golf club.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetGolfClubResponse> GetSingleGolfClub(String passwordToken,
                                                                 CancellationToken cancellationToken)
        {
            GetGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/GolfClub";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

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

        /// <summary>
        /// Requests the club membership.
        /// </summary>
        /// <param name="passwordToken">The password token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task RequestClubMembership(String passwordToken,
                                                Guid golfClubId,
                                                CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/GolfClub/{golfClubId}/RequestClubMembership";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", passwordToken);

                // Make the Http Call here
                StringContent httpContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful, no response data to deserialise
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error requesting Golf Club membership.", ex);

                throw exception;
            }
        }

        #endregion
    }
}