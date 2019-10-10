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
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Client.v2.IGolfClubClient" />
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

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "2.0");
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
        public async Task<AddMeasuredCourseToClubResponse> AddMeasuredCourseToGolfClub(String accessToken,
                                                                                       Guid golfClubId,
                                                                                       AddMeasuredCourseToClubRequest request,
                                                                                       CancellationToken cancellationToken)
        {
            AddMeasuredCourseToClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/measuredcourses";

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
                response = JsonConvert.DeserializeObject<AddMeasuredCourseToClubResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error Adding a measured course to Golf Club.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Adds the tournament division.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<AddTournamentDivisionToGolfClubResponse> AddTournamentDivision(String accessToken,
                                                                                         Guid golfClubId,
                                                                                         AddTournamentDivisionToGolfClubRequest request,
                                                                                         CancellationToken cancellationToken)
        {
            AddTournamentDivisionToGolfClubResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/tournamentdivisions";

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
                response = JsonConvert.DeserializeObject<AddTournamentDivisionToGolfClubResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error Adding a tournament divisionto Golf Club.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Cancels the tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CancelTournament(String accessToken,
                                           Guid golfClubId,
                                           Guid tournamentId,
                                           CancelTournamentRequest request,
                                           CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/tournaments/{tournamentId}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Create the patch request
                TournamentPatchRequest tournamentPatchRequest = new TournamentPatchRequest
                                                                {
                                                                    CancellationReason = request.CancellationReason,
                                                                    Status = TournamentStatusUpdate.Cancel
                                                                };

                String requestSerialised = JsonConvert.SerializeObject(tournamentPatchRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.Patch(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful there is no response object
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
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task CompleteTournament(String accessToken,
                                             Guid golfClubId,
                                             Guid tournamentId,
                                             CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/tournaments/{tournamentId}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Create the patch request
                TournamentPatchRequest tournamentPatchRequest = new TournamentPatchRequest
                                                                {
                                                                    Status = TournamentStatusUpdate.Complete
                                                                };

                String requestSerialised = JsonConvert.SerializeObject(tournamentPatchRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.Patch(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful there is no response object
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error completing tournament {tournamentId}.", ex);

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

            String requestUri = $"{this.BaseAddress}/api/golfclubs";

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
        public async Task<CreateMatchSecretaryResponse> CreateMatchSecretary(String accessToken,
                                                                             Guid golfClubId,
                                                                             CreateMatchSecretaryRequest request,
                                                                             CancellationToken cancellationToken)
        {
            CreateMatchSecretaryResponse response = null;
            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/users";

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
                response = JsonConvert.DeserializeObject<CreateMatchSecretaryResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error creating the new match secretary.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Creates the tournament.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateTournamentResponse> CreateTournament(String accessToken,
                                                                     Guid golfClubId,
                                                                     CreateTournamentRequest request,
                                                                     CancellationToken cancellationToken)
        {
            CreateTournamentResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/tournaments";

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
        /// Gets the single golf club.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetGolfClubResponse> GetGolfClub(String accessToken,
                                                           Guid golfClubId,
                                                           CancellationToken cancellationToken)
        {
            GetGolfClubResponse response = new GetGolfClubResponse();
            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}";

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
                response = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting golf club.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the golf club list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="includeMembers">if set to <c>true</c> [include members].</param>
        /// <param name="includeMeasuredCourses">if set to <c>true</c> [include measured courses].</param>
        /// <param name="includeUsers">if set to <c>true</c> [include users].</param>
        /// <returns></returns>
        public async Task<List<GetGolfClubResponse>> GetGolfClubList(String accessToken,
                                                                     CancellationToken cancellationToken,
                                                                     Boolean includeMembers = false,
                                                                     Boolean includeMeasuredCourses = false,
                                                                     Boolean includeUsers = false)
        {
            List<GetGolfClubResponse> response = new List<GetGolfClubResponse>();
            String requestUri =
                $"{this.BaseAddress}/api/golfclubs?includeMembers={includeMembers}&includeMeasuredCourses={includeMeasuredCourses}&includeUsers={includeUsers}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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
                Exception exception = new Exception("Error getting golf club list.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the golf club membership list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GolfClubMembershipDetailsResponse>> GetGolfClubMembershipList(String accessToken,
                                                                                             Guid golfClubId,
                                                                                             CancellationToken cancellationToken)
        {
            List<GolfClubMembershipDetailsResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}?includeMembers=true";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                GetGolfClubResponse getGolfClubResponse = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);

                response = getGolfClubResponse.GolfClubMembershipDetailsResponseList;
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
        /// Gets the golf club membership list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<TournamentResponse>> GetGolfClubTournamentList(String accessToken,
                                                                              Guid golfClubId,
                                                                              CancellationToken cancellationToken)
        {
            List<TournamentResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}?includeTournaments=true";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                GetGolfClubResponse getGolfClubResponse = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);

                response = getGolfClubResponse.Tournaments;
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting a golf club tournaments list.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the golf club user list.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<List<GolfClubUserResponse>> GetGolfClubUserList(String accessToken,
                                                                          Guid golfClubId,
                                                                          CancellationToken cancellationToken)
        {
            List<GolfClubUserResponse> response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}?includeUsers=true";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                GetGolfClubResponse getGolfClubResponse = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);

                response = getGolfClubResponse.Users;
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error getting a golf club users list.", ex);

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
        public async Task<List<MeasuredCourseListResponse>> GetMeasuredCourses(String accessToken,
                                                                               Guid golfClubId,
                                                                               CancellationToken cancellationToken)
        {
            List<MeasuredCourseListResponse> response = new List<MeasuredCourseListResponse>();
            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}?includeMeasuredCourses=true";

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
                GetGolfClubResponse getGolfClubResponse = JsonConvert.DeserializeObject<GetGolfClubResponse>(content);
                response = getGolfClubResponse.MeasuredCourses;
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
        /// Produces the tournament result.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="tournamentId">The tournament identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task ProduceTournamentResult(String accessToken,
                                                  Guid golfClubId,
                                                  Guid tournamentId,
                                                  CancellationToken cancellationToken)
        {
            String requestUri = $"{this.BaseAddress}/api/golfclubs/{golfClubId}/tournaments/{tournamentId}";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Create the patch request
                TournamentPatchRequest tournamentPatchRequest = new TournamentPatchRequest
                                                                {
                                                                    Status = TournamentStatusUpdate.ProduceResult
                                                                };

                String requestSerialised = JsonConvert.SerializeObject(tournamentPatchRequest);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.Patch(requestUri, httpContent, cancellationToken);

                // Process the response
                await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful there is no response object
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error producing result for tournament {tournamentId}.", ex);

                throw exception;
            }
        }

        /// <summary>
        /// Patches the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> Patch(String uri,
                                                      HttpContent content,
                                                      CancellationToken cancellationToken)
        {
            HttpMethod patchMethod = new HttpMethod("PATCH");

            HttpRequestMessage requestMessage = new HttpRequestMessage(patchMethod, uri);
            requestMessage.Content = content;

            HttpResponseMessage responseMessage = await this.HttpClient.SendAsync(requestMessage, cancellationToken);

            return responseMessage;
        }

        #endregion
    }
}