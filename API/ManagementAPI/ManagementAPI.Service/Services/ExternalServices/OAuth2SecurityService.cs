namespace ManagementAPI.Service.Services
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using ExternalServices.DataTransferObjects;
    using Newtonsoft.Json;
    using Shared.General;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ManagementAPI.Service.Services.ISecurityService" />
    /// <seealso cref="Shared.General.ClientProxyBase" />
    /// <seealso cref="ISecurityService" />
    public class SecurityService : ClientProxyBase, ISecurityService
    {
        #region Methods

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<CreateRoleResponse> CreateRole(CreateRoleRequest request,
                                                         CancellationToken cancellationToken)
        {
            CreateRoleResponse response = null;

            String requestSerialised = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

            using(HttpClient client = new HttpClient())
            {
                String uri = $"{ConfigurationReader.GetBaseServerUri("SecurityService")}api/role";

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content, CancellationToken.None).ConfigureAwait(false);

                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                String responseContent = await this.HandleResponse(httpResponse, cancellationToken);

                response = JsonConvert.DeserializeObject<CreateRoleResponse>(responseContent);
            }

            return response;
        }

        /// <summary>
        /// Gets the name of the role by.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<GetRoleResponse> GetRoleByName(String roleName,
                                                         CancellationToken cancellationToken)
        {
            GetRoleResponse response = null;

            using(HttpClient client = new HttpClient())
            {
                String uri = $"{ConfigurationReader.GetBaseServerUri("SecurityService")}api/role?roleName={roleName}";

                HttpResponseMessage httpResponse = await client.GetAsync(uri, CancellationToken.None).ConfigureAwait(false);

                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                String responseContent = await this.HandleResponse(httpResponse, cancellationToken);

                response = JsonConvert.DeserializeObject<GetRoleResponse>(responseContent);
            }

            return response;
        }

        /// <summary>
        /// Registers the  user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request,
                                                             CancellationToken cancellationToken)
        {
            RegisterUserResponse response = null;

            String requestSerialised = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

            using(HttpClient client = new HttpClient())
            {
                String uri = $"{ConfigurationReader.GetBaseServerUri("SecurityService")}api/user";

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content, CancellationToken.None).ConfigureAwait(false);
                Logger.LogInformation($"Request message: {requestSerialised}");
                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                String responseContent = await this.HandleResponse(httpResponse, cancellationToken);

                response = JsonConvert.DeserializeObject<RegisterUserResponse>(responseContent);
            }

            return response;
        }

        #endregion
    }
}