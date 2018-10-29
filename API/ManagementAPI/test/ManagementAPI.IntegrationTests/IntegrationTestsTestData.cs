using System.Collections.Generic;
using System.Text;
using ManagementAPI.IntegrationTests.DataTransferObjects;

namespace ManagementAPI.IntegrationTests
{
    public class IntegrationTestsTestData
    {
        public static CreateClubConfigurationRequest CreateClubConfigurationRequest = new CreateClubConfigurationRequest
        {
            Name = "Name",
            AddressLine1 = "Address Line 1",
            EmailAddress = "1@2.com",
            PostalCode = "TE57 1NG",
            Town = "Test Town",
            Website = "www.website.com",
            Region = "Test Region",
            TelephoneNumber = "123456789",
            AddressLine2 = "Address Line 2"
        };
    }
}
