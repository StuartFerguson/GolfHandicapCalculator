namespace ManagementAPI.Service.IntegrationTests.Common
{
    using System.Net.Http;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public static class Helpers
    {
        #region Methods

        /// <summary>
        /// Creates the content of the string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestObject">The request object.</param>
        /// <returns></returns>
        public static StringContent CreateStringContent<T>(T requestObject)
        {
            return new StringContent(JsonConvert.SerializeObject(requestObject), Encoding.UTF8, "application/json");
        }

        #endregion
    }
}