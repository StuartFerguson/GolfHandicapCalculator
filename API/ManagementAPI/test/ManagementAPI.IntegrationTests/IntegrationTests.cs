using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace ManagementAPI.IntegrationTests
{
    public class IntegrationTests
    {
        public const String BaseURI = "http://localhost:5000";

        [Fact]
        public async Task ManagementAPI_Values_GetValues()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);

                var response = await client.GetAsync("/api/Values", CancellationToken.None);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<String[]>(content);

                Assert.NotEmpty(result);
            }
        }
    }
}
