namespace ManagementAPI.Service.Client
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using ClientProxyBase;
    using DataTransferObjects.Responses;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ClientProxyBase.ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Client.IReportingClient" />
    public class ReportingClient : ClientProxyBase, IReportingClient
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
        /// Initializes a new instance of the <see cref="ReportingClient"/> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public ReportingClient(Func<String, String> baseAddressResolver,
                               HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the number of members report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersReportResponse> GetNumberOfMembersReport(String accessToken,
                                                                                     Guid golfClubId,
                                                                                     CancellationToken cancellationToken)
        {
            GetNumberOfMembersReportResponse response = null;
            String requestUri = $"{this.BaseAddress}/api/Reporting/GolfClub/{golfClubId}/numberofmembers";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetNumberOfMembersReportResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting number of members report for Golf Club {golfClubId}.", ex);

                throw exception;
            }

            return response;
        }

        /// <summary>
        /// Gets the number of members by handicap category report.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="golfClubId">The golf club identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetNumberOfMembersByHandicapCategoryReportResponse> GetNumberOfMembersByHandicapCategoryReport(String accessToken,
                                                                     Guid golfClubId,
                                                                     CancellationToken cancellationToken)
        {
            GetNumberOfMembersByHandicapCategoryReportResponse response = null;
            String requestUri = $"{this.BaseAddress}/api/Reporting/GolfClub/{golfClubId}/numberofmembersbyhandicapcategory";

            try
            {
                // Add the access token to the client headers
                this.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.GetAsync(requestUri, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<GetNumberOfMembersByHandicapCategoryReportResponse>(content);
            }
            catch (Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception($"Error getting number of members by handicap category report for Golf Club {golfClubId}.", ex);

                throw exception;
            }

            return response;
        }

        #endregion
    }
}