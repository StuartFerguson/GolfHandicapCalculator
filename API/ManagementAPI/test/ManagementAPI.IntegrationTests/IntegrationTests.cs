using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ManagementAPI.IntegrationTests.DataTransferObjects;
using Newtonsoft.Json;
using Xunit;

namespace ManagementAPI.IntegrationTests
{
    public class IntegrationTests
    {
        public const String BaseURI = "http://localhost:5000";

        /*[Fact]
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
        }*/

        [Fact]
        public async Task ManagementAPI_CreateClubConfiguration_SuccessfulReponse()
        {
            // Construct the request 
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);

                String requestSerialised = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None);

                response.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await response.Content.ReadAsStringAsync());

                Assert.NotNull(responseData);
                Assert.NotEqual(responseData.ClubConfigurationId, Guid.Empty);
            }
        }
    }
}
