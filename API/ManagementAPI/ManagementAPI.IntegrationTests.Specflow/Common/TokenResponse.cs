using System;
using Newtonsoft.Json;

namespace ManagementAPI.IntegrationTests.Specflow.Common
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public String AccessToken { get; set; }
    }
}