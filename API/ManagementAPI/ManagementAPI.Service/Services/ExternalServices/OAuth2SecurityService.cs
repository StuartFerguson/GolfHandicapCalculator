using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.CommandHandlers;
using Newtonsoft.Json;
using Shared.General;

namespace ManagementAPI.Service.Services
{
    using ExternalServices.DataTransferObjects;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Shared.General.ClientProxyBase" />
    /// <seealso cref="ManagementAPI.Service.Services.IOAuth2SecurityService" />
    public class OAuth2SecurityService : ClientProxyBase, IOAuth2SecurityService
    {
        #region public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)        
        /// <summary>
        /// Registers the  user.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            RegisterUserResponse response = null;

            String requestSerialised = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                String uri = $"{ConfigurationReader.GetBaseServerUri("OAuth2SecurityService")}api/user";
                
                HttpResponseMessage httpResponse = await client.PostAsync(uri, content, CancellationToken.None)
                    .ConfigureAwait(false);

                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                String responseContent = await HandleResponse(httpResponse, cancellationToken);

                response = JsonConvert.DeserializeObject<RegisterUserResponse>(responseContent);
            }

            return response;
        }

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

            using (HttpClient client = new HttpClient())
            {
                String uri = $"{ConfigurationReader.GetBaseServerUri("OAuth2SecurityService")}api/role";

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content, CancellationToken.None)
                                                               .ConfigureAwait(false);

                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                String responseContent = await HandleResponse(httpResponse, cancellationToken);

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
            
            using (HttpClient client = new HttpClient())
            {
                String uri = $"{ConfigurationReader.GetBaseServerUri($"OAuth2SecurityService")}api/role?roleName={roleName}";

                HttpResponseMessage httpResponse = await client.GetAsync(uri, CancellationToken.None)
                                                               .ConfigureAwait(false);

                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                String responseContent = await HandleResponse(httpResponse, cancellationToken);

                response = JsonConvert.DeserializeObject<GetRoleResponse>(responseContent);
            }

            return response;
        }

        #endregion
    }
}