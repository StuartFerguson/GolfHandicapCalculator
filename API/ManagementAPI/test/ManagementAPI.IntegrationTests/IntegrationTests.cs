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
            using (StreamWriter sw = new StreamWriter("c:\\temp\\inttestslog"))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseURI);

                    sw.WriteLine($"request address is {BaseURI}/api/Values");

                    try
                    {
                        var response = await client.GetAsync("/api/Values", CancellationToken.None);

                        sw.WriteLine($"Status code is {response.StatusCode}");
                        response.EnsureSuccessStatusCode();
                    
                        var content = await response.Content.ReadAsStringAsync();
                        sw.WriteLine($"content is {response.StatusCode}");
                        var result = JsonConvert.DeserializeObject<String[]>(content);

                        Assert.NotEmpty(result);
                    }
                    catch (Exception e)
                    {
                        sw.WriteLine(e);
                    }

                    

                }
            }
        }
    }
}
