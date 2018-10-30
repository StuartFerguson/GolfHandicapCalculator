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

        [Fact]
        public async Task ManagementAPI_GetClubConfiguration_SuccessfulReponse()
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

                var getResponse = await client.GetAsync($"/api/ClubConfiguration?clubId={responseData.ClubConfigurationId}", CancellationToken.None);

                getResponse.EnsureSuccessStatusCode();

                var getResponseData = JsonConvert.DeserializeObject<GetClubConfigurationResponse>(await getResponse.Content.ReadAsStringAsync());

                Assert.NotNull(getResponseData);
                Assert.Equal(getResponseData.Id, responseData.ClubConfigurationId);
                Assert.Equal(getResponseData.Name, request.Name);
                Assert.Equal(getResponseData.AddressLine1, request.AddressLine1);
                Assert.Equal(getResponseData.AddressLine2, request.AddressLine2);
                Assert.Equal(getResponseData.Town, request.Town);
                Assert.Equal(getResponseData.Region, request.Region);
                Assert.Equal(getResponseData.PostalCode, request.PostalCode);
                Assert.Equal(getResponseData.TelephoneNumber, request.TelephoneNumber);
                Assert.Equal(getResponseData.Website, request.Website);
                Assert.Equal(getResponseData.EmailAddress, request.EmailAddress);
            }
        }
    }
}
