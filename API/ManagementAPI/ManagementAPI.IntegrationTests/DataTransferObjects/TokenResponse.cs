using System;
using Newtonsoft.Json;

namespace ManagementAPI.IntegrationTests.DataTransferObjects
{
    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public String AccessToken { get; set; }
    }
}