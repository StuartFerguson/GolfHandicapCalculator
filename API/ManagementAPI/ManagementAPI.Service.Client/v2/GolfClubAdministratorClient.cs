namespace ManagementAPI.Service.Client.v2
{
    using System;
    using System.Net.Http;
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
    /// <seealso cref="ManagementAPI.Service.Client.v2.IGolfClubAdministratorClient" />
    public class GolfClubAdministratorClient : ClientProxyBase, IGolfClubAdministratorClient
    {
        #region Fields

        /// <summary>
        /// The base address
        /// </summary>
        private readonly String BaseAddress;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GolfClubAdministratorClient"/> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public GolfClubAdministratorClient(Func<String, String> baseAddressResolver,
                                           HttpClient httpClient) : base(httpClient)
        {
            this.BaseAddress = baseAddressResolver("ManagementAPI");

            // Add the API version header
            this.HttpClient.DefaultRequestHeaders.Add("api-version", "2.0");
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers the golf club administrator.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterClubAdministratorResponse> RegisterGolfClubAdministrator(RegisterClubAdministratorRequest request,
                                                                                           CancellationToken cancellationToken)
        {
            RegisterClubAdministratorResponse response = null;

            String requestUri = $"{this.BaseAddress}/api/golfclubadministrators";

            try
            {
                String requestSerialised = JsonConvert.SerializeObject(request);

                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                // Make the Http Call here
                HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, httpContent, cancellationToken);

                // Process the response
                String content = await this.HandleResponse(httpResponse, cancellationToken);

                // call was successful so now deserialise the body to the response object
                response = JsonConvert.DeserializeObject<RegisterClubAdministratorResponse>(content);
            }
            catch(Exception ex)
            {
                // An exception has occurred, add some additional information to the message
                Exception exception = new Exception("Error registering a Golf Club Administrator.", ex);

                throw exception;
            }

            return response;
        }

        #endregion
    }
}