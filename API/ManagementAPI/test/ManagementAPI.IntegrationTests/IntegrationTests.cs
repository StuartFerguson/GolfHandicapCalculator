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
        public async Task ManagementAPI_CreateClubConfiguration_SuccessfulResponse()
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
        public async Task ManagementAPI_GetClubConfiguration_SuccessfulResponse()
        {
            // Construct the request 
            var request = IntegrationTestsTestData.CreateClubConfigurationRequest;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);

                String requestSerialised = JsonConvert.SerializeObject(request);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var createClubResponse = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None);

                createClubResponse.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await createClubResponse.Content.ReadAsStringAsync());

                var getClubResponse = await client.GetAsync($"/api/ClubConfiguration?clubId={responseData.ClubConfigurationId}", CancellationToken.None);

                getClubResponse.EnsureSuccessStatusCode();

                var getResponseData = JsonConvert.DeserializeObject<GetClubConfigurationResponse>(await getClubResponse.Content.ReadAsStringAsync());

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

        [Fact]
        public async Task ManagementAPI_AddMeasuredCourse_SuccessfulResponse()
        {
            // Construct the request 
            var createClubConfigurationRequest = IntegrationTestsTestData.CreateClubConfigurationRequest;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);

                String requestSerialised = JsonConvert.SerializeObject(createClubConfigurationRequest);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var createClubResponse = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None);

                createClubResponse.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await createClubResponse.Content.ReadAsStringAsync());

                var addMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
                addMeasuredCourseToClubRequest.ClubAggregateId = responseData.ClubConfigurationId;

                requestSerialised = JsonConvert.SerializeObject(addMeasuredCourseToClubRequest);
                httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var addMeasuredCourseResponse = await client.PutAsync("/api/ClubConfiguration", httpContent, CancellationToken.None);

                addMeasuredCourseResponse.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task ManagementAPI_CreateTournament_SuccessfulResponse()
        {
            // Construct the request 
            var createClubConfigurationRequest = IntegrationTestsTestData.CreateClubConfigurationRequest;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseURI);

                String requestSerialised = JsonConvert.SerializeObject(createClubConfigurationRequest);
                StringContent httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var createClubResponse = await client.PostAsync("/api/ClubConfiguration", httpContent, CancellationToken.None);

                createClubResponse.EnsureSuccessStatusCode();

                var responseData = JsonConvert.DeserializeObject<CreateClubConfigurationResponse>(await createClubResponse.Content.ReadAsStringAsync());

                var addMeasuredCourseToClubRequest = IntegrationTestsTestData.AddMeasuredCourseToClubRequest;
                addMeasuredCourseToClubRequest.ClubAggregateId = responseData.ClubConfigurationId;

                requestSerialised = JsonConvert.SerializeObject(addMeasuredCourseToClubRequest);
                httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var addMeasuredCourseResponse = await client.PutAsync("/api/ClubConfiguration", httpContent, CancellationToken.None);

                addMeasuredCourseResponse.EnsureSuccessStatusCode();

                var createTournamentRequest = IntegrationTestsTestData.CreateTournamentRequest;
                createTournamentRequest.ClubConfigurationId = responseData.ClubConfigurationId;
                createTournamentRequest.MeasuredCourseId = addMeasuredCourseToClubRequest.MeasuredCourseId;

                requestSerialised = JsonConvert.SerializeObject(createTournamentRequest);
                httpContent = new StringContent(requestSerialised, Encoding.UTF8, "application/json");

                var createTournamentResponse =
                    await client.PostAsync("/api/Tournament", httpContent, CancellationToken.None);
                
                createTournamentResponse.EnsureSuccessStatusCode();

                var responseData1 = JsonConvert.DeserializeObject<CreateTournamentResponse>(await createTournamentResponse.Content.ReadAsStringAsync());

                Assert.NotNull(responseData1);
                Assert.NotEqual(responseData1.TournamentId, Guid.Empty);

            }
        }
    }
}
