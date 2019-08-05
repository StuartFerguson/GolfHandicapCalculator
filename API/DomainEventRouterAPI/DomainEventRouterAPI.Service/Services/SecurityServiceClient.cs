namespace DomainEventRouterAPI.Service.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DomainEventRouterAPI.Service.Services.ISecurityServiceClient" />
    public class SecurityServiceClient : ISecurityServiceClient
    {
        #region Fields

        /// <summary>
        /// The base address resolver
        /// </summary>
        private readonly Func<String, String> BaseAddressResolver;

        /// <summary>
        /// The HTTP client
        /// </summary>
        private readonly HttpClient HttpClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityServiceClient"/> class.
        /// </summary>
        /// <param name="baseAddressResolver">The base address resolver.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public SecurityServiceClient(Func<String, String> baseAddressResolver,
                                     HttpClient httpClient)
        {
            this.BaseAddressResolver = baseAddressResolver;
            this.HttpClient = httpClient;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the client token.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="scopes">The scopes.</param>
        /// <returns></returns>
        public async Task<String> GetClientToken(String clientId,
                                                 String clientSecret,
                                                 CancellationToken cancellationToken,
                                                 List<String> scopes = null)
        {
            String result = null;

            StringBuilder queryString = new StringBuilder();
            queryString.Append("grant_type=client_credentials");
            queryString.Append($"&client_id={clientId}");
            queryString.Append($"&client_secret={clientSecret}");

            String requestUri = $"{this.BaseAddressResolver("SecurityService")}/connect/token";

            StringContent content = new StringContent(queryString.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage httpResponse = await this.HttpClient.PostAsync(requestUri, content, cancellationToken);

            if (httpResponse.IsSuccessStatusCode)
            {
                TokenResponse token = await this.GetResponseObject<TokenResponse>(httpResponse);
                result = token.AccessToken;
            }

            return result;
        }

        /// <summary>
        /// Gets the response object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseMessage">The response message.</param>
        /// <returns></returns>
        private async Task<T> GetResponseObject<T>(HttpResponseMessage responseMessage)
        {
            T result = default;

            result = JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));

            return result;
        }

        #endregion
    }
}