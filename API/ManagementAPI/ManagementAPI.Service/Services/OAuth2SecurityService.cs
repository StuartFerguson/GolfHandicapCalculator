using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.Service.CommandHandlers;
using ManagementAPI.Service.Services.DataTransferObjects;
using ManagementAPI.Service.Shared;
using Newtonsoft.Json;
using Shared.General;

namespace ManagementAPI.Service.Services
{
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
                
                var httpResponse = await client.PostAsync(uri, content, CancellationToken.None)
                    .ConfigureAwait(false);

                Logger.LogInformation($"Status Code: {httpResponse.StatusCode}");

                var responseContent = await HandleResponse(httpResponse, cancellationToken);

                response = JsonConvert.DeserializeObject<RegisterUserResponse>(responseContent);
            }

            return response;
        }
        #endregion
    }
}