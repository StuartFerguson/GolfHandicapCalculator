namespace DomainEventRouterAPI.Service.Services
{
    using System;
    using Newtonsoft.Json;

    public class TokenResponse
    {
        #region Properties

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty("access_token")]
        public String AccessToken { get; set; }

        #endregion
    }
}