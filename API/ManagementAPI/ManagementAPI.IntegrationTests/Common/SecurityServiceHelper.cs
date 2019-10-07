using System;
using System.Collections.Generic;
using System.Text;

namespace ManagementAPI.IntegrationTests.Common
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public static class SecurityServiceHelper
    {
        public static async Task CreateSecurityRole(String baseUri, String roleName, CancellationToken cancellationToken)
        {
            var request = new { RoleName = roleName };

            String requestSerialised = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

            using(HttpClient client = new HttpClient())
            {
                String uri = $"{baseUri}/api/role";

                HttpResponseMessage httpResponse = await client.PostAsync(uri, content, cancellationToken).ConfigureAwait(false);

                if (httpResponse.IsSuccessStatusCode == false)
                {
                    throw new Exception($"Unable to create role {roleName}");
                }
            }
        }
    }
}
